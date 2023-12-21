#pragma once

#include "pch.h"

#include <cstdint>
#include <memory>
#include <mutex>
#include "Types.h"
#include "Factory.h"
#include <iostream>
#include "ConicGradientEffectD2D1.h"
#include "dxgi1_3.h"

static uint32_t mDeviceSeq = 0;
ComPtr<ID3D11Device> Factory::mD3D11Device;
ComPtr<ID2D1Device> Factory::mD2D1Device;
ComPtr<ID2D1DeviceContext> Factory::mMTDC;
ComPtr<ID2D1Factory1> Factory::mFactory;
ComPtr<ID3D11DeviceContext> Factory::mD3D11DeviceContext;


// D2D1CreateFactoryFunc: Typedef for a pointer to a function with a specific 
// signature, used for creating a Direct2D factory. This function pointer can 
// be used to dynamically load the 'D2D1CreateFactory' function from the Direct2D DLL.
typedef HRESULT(WINAPI* D2D1CreateFactoryFunc)(
    D2D1_FACTORY_TYPE factoryType,
    REFIID iid,
    CONST D2D1_FACTORY_OPTIONS* pFactoryOptions,
    void** factory
);

Factory::Factory() :
    mInitState(InitState::Uninitialized),
    mFormat(SurfaceFormat::B8G8R8A8),
    mUsedCommandListsSincePurge(0),
    mTransform(Matrix()),
    mTransformDirty(false),
    mSize(D2D1::SizeU(0, 0)),
    mCommandList(nullptr),
    mBitmap(nullptr),
    mCpuBitmap(nullptr),
    mSurface(nullptr),
    mDC(nullptr),
    m_d3dFeatureLevel(D3D_FEATURE_LEVEL_9_1)
{}

ComPtr<ID2D1Factory1> Factory::factory() {

    if (mFactory) {
        return mFactory;
    }

    ComPtr<ID2D1Factory> factory;
    D2D1CreateFactoryFunc createD2DFactory;
    HMODULE d2dModule = LoadLibraryW(L"d2d1.dll");
    if (!d2dModule) {
        log() << "Failed to load d2d1.dll" << "\n";
        return nullptr;
    }

    createD2DFactory = (D2D1CreateFactoryFunc)GetProcAddress(d2dModule, "D2D1CreateFactory");

    if (!createD2DFactory) {
        log() << "Failed to locate D2D1CreateFactory function." << "\n";
        return nullptr;
    }

    D2D1_FACTORY_OPTIONS options{};
    // options.debugLevel = D2D1_DEBUG_LEVEL_INFORMATION;

    HRESULT hr = createD2DFactory(
        D2D1_FACTORY_TYPE_MULTI_THREADED,
        __uuidof(ID2D1Factory),
        &options,
        &factory);

    if (FAILED(hr) || !factory) {
        log() << "Failed to create a D2D1 content device: " << HEX(hr) << "\n";
        return nullptr;
    }

    ComPtr<ID2D1Factory1> factory1;
    hr = factory->QueryInterface(
        __uuidof(ID2D1Factory1),
        &factory1);
    if (FAILED(hr) || !factory1) {
        log() << "Failed to create a D2D1.1 content device: " << HEX(hr) << "\n";
        return nullptr;
    }

    mFactory = factory1;

    ConicGradientEffectD2D1::Register(mFactory.Get());

    return mFactory;
}

ComPtr<ID2D1Factory1> D2DFactory() { return Factory::factory(); }
bool Factory::SupportsD2D1() { return !!D2DFactory(); }

bool Factory::SetDirect3D11Device(ID3D11Device* aDevice) {

    // D2DFactory already takes the device lock, so we get the factory before
    // entering the lock scope.
    ComPtr<ID2D1Factory1> factory = D2DFactory();

    mD3D11Device = aDevice;

    if (mD2D1Device) {
        mD2D1Device = nullptr;
        mMTDC = nullptr;
    }

    if (!aDevice) {
        return true;
    }

    ComPtr<IDXGIDevice> device;
    aDevice->QueryInterface((IDXGIDevice**)&device);

    ComPtr<ID2D1Device> d2dDevice;
    HRESULT hr = factory->CreateDevice(device.Get(), &d2dDevice);
    if (FAILED(hr)) {
        log() << "[D2D1] Failed to create gfx factory's D2D1 device, code: " << HEX(hr) << "\n";
        mD3D11Device = nullptr;
        return false;
    }

    mDeviceSeq++;
    mD2D1Device = d2dDevice;
    return true;
}

ComPtr<ID3D11Device> Factory::GetDirect3D11Device() {
    return mD3D11Device;
}

ComPtr<ID2D1Device> Factory::GetD2D1Device(uint32_t* aOutSeqNo) {
    return mD2D1Device;
}

bool Factory::HasD2D1Device() { return !!GetD2D1Device(); }

ComPtr<ID2D1DeviceContext> Factory::GetD2DDeviceContext() {
    ComPtr<ID2D1DeviceContext>* ptr;

    ptr = &mMTDC;

    if (*ptr) {
        return *ptr;
    }

    ComPtr<ID2D1Device> device = GetD2D1Device();

    if (!device) {
        return nullptr;
    }

    ComPtr<ID2D1DeviceContext> dc;
    HRESULT hr = device->CreateDeviceContext(
        D2D1_DEVICE_CONTEXT_OPTIONS_ENABLE_MULTITHREADED_OPTIMIZATIONS,
        &dc);

    if (FAILED(hr)) {
        log() << "Failed to create global device context" << HEX(hr) << "\n";
        return nullptr;
    }

    *ptr = dc;

    return *ptr;
}

void Factory::D2DCleanup() {
    if (mD2D1Device) {
        mD2D1Device = nullptr;
    }
    if (mFactory) {
        ConicGradientEffectD2D1::Unregister(mFactory.Get());
        mFactory = nullptr;
    }
}

bool Factory::Init(const D2D1_SIZE_U& aSize, SurfaceFormat aFormat)
{
    ComPtr<ID2D1Device> device = Factory::GetD2D1Device(&mDeviceSeq);
    if (!device) {
        log() << "[D2D1.1] Failed to obtain a device for Factory::Init(IntSize, SurfaceFormat).\n";
        return false;
    }

    mFormat = aFormat;
    mSize = aSize;

    return true;
}

void Factory::FinalizeDrawing(const Pattern& aPattern) {

    if (aPattern.GetType() != PatternType::CONIC_GRADIENT) {
        log() << "Pattern type not supported" << "\n";
        return;
    }
    const ConicGradientPattern* pat = static_cast<const ConicGradientPattern*>(&aPattern);

    mDC->SetTarget(mBitmap.Get());
    if (!mCommandList) {
        log() << "Invalid D21.1 command list on finalize" << "\n";
        return;
    }
    mCommandList->Close();

    ComPtr<ID2D1CommandList> source = mCommandList;
    mCommandList = nullptr;

    mDC->SetTransform(D2D1::IdentityMatrix());

    ComPtr<ID2D1Effect> conicGradientEffect;
    HRESULT hr = mDC->CreateEffect(CLSID_ConicGradientEffect, conicGradientEffect.ReleaseAndGetAddressOf());
    if (FAILED(hr) || !conicGradientEffect) {
        log() << "Failed to create conic gradient effect. Code: " << HEX(hr) << "\n";
        return;
    }

    //conicGradientEffect->SetValue(CONIC_PROP_STOP_COLLECTION, static_cast<const GradientStops*>(pat->mStops.Get())->mStopCollection);
    conicGradientEffect->SetValue(CONIC_PROP_STOP_COLLECTION, pat->mStops.Get());
    conicGradientEffect->SetValue(CONIC_PROP_CENTER, D2D1::Vector2F(pat->mCenter.x, pat->mCenter.y));
    conicGradientEffect->SetValue(CONIC_PROP_ANGLE, pat->mAngle);
    conicGradientEffect->SetValue(CONIC_PROP_START_OFFSET, pat->mStartOffset);
    conicGradientEffect->SetValue(CONIC_PROP_END_OFFSET, pat->mEndOffset);
    conicGradientEffect->SetValue(CONIC_PROP_TRANSFORM, D2DMatrix(pat->mMatrix * mTransform));
    //conicGradientEffect->SetInput(0, source.Get());

    mDC->DrawImage(
        conicGradientEffect.Get(),
        D2D1_INTERPOLATION_MODE_NEAREST_NEIGHBOR,
        D2DCompositionMode(CompositionOp::OP_OVER));

    hr = mDC->EndDraw();
    if (FAILED(hr)) {
        log() << "Failed to end draw. Code: " << HEX(hr) << "\n";
    }

    return;
}

bool Factory::Init(ID3D11Texture2D* aTexture, SurfaceFormat aFormat)
{
    ComPtr<ID2D1Device> device = Factory::GetD2D1Device(&mDeviceSeq);
    if (!device) {
        log() << "[D2D1.1] Failed to obtain a device for DrawTargetD2D1::Init(ID3D11Texture2D*, SurfaceFormat)." << "\n";
        return false;
    }

    aTexture->QueryInterface(__uuidof(IDXGISurface),
        (void**)((IDXGISurface**)mSurface.ReleaseAndGetAddressOf()));
    if (!mSurface) {
        log() << "[D2D1.1] Failed to obtain a DXGI surface." << "\n";
        return false;
    }

    mFormat = aFormat;

    D3D11_TEXTURE2D_DESC desc;
    aTexture->GetDesc(&desc);
    mSize.width = desc.Width;
    mSize.height = desc.Height;

    return true;
}


bool Factory::EnsureInitialized() {

    if (mInitState != InitState::Uninitialized) {
        return mInitState == InitState::Success;
    }

    // Don't retry.
    mInitState = InitState::Failure;

    HRESULT hr;

    ComPtr<ID2D1Device> device = Factory::GetD2D1Device(&mDeviceSeq);
    if (!device) {
        log() << "[D2D1.1] Failed to obtain a device for Factory::EnsureInitialized()" << "\n";
        return false;
    }

    hr = device->CreateDeviceContext(
        D2D1_DEVICE_CONTEXT_OPTIONS_ENABLE_MULTITHREADED_OPTIMIZATIONS,
        mDC.ReleaseAndGetAddressOf());

    if (FAILED(hr)) {
        log() << "[D2D1.1] 2Failed to create a DeviceContext, code: " << HEX(hr) << "\n";
        return false;
    }

    if (!mSurface) {
        if (mDC->GetMaximumBitmapSize() < UINT32(mSize.width)
            || mDC->GetMaximumBitmapSize() < UINT32(mSize.height))
        {
            // This is 'ok', so don't assert
            log() << "[D2D1.1] Attempt to use unsupported surface size "
                << "mSize.width: " << mSize.width << ", mSize.height: " << mSize.height << "\n";
            return false;
        }

        D2D1_BITMAP_PROPERTIES1 props{};
        props.dpiX = 96;
        props.dpiY = 96;
        props.pixelFormat = D2DPixelFormat(mFormat);
        props.colorContext = nullptr;
        props.bitmapOptions = D2D1_BITMAP_OPTIONS_TARGET;
        //props.bitmapOptions = D2D1_BITMAP_OPTIONS_CPU_READ | D2D1_BITMAP_OPTIONS_TARGET; // | D2D1_BITMAP_OPTIONS_CANNOT_DRAW;
        hr = mDC->CreateBitmap(mSize, nullptr, 0, props,
            (ID2D1Bitmap1**)mBitmap.ReleaseAndGetAddressOf());

        if (FAILED(hr)) {
            log() << "ID2D1DeviceContext::CreateBitmap (render) failure " << HEX(hr) << "\n";
            return false;
        }


        props.bitmapOptions = D2D1_BITMAP_OPTIONS_CPU_READ | D2D1_BITMAP_OPTIONS_CANNOT_DRAW;
        hr = mDC->CreateBitmap(mSize, nullptr, 0, props,
            (ID2D1Bitmap1**)mCpuBitmap.ReleaseAndGetAddressOf());

        if (FAILED(hr)) {
            log() << "ID2D1DeviceContext::CreateBitmap (cpu) failure " << HEX(hr) << "\n";

            log() << "ID2D1DeviceContext::CreateBitmap (cpu) failure " << HEX(hr) << "\n";
            log() << "Bitmap Size: Width = " << mSize.width << ", Height = " << mSize.height << "\n";
            log() << "Bitmap DPI: X = " << props.dpiX << ", Y = " << props.dpiY << "\n";
            log() << "Bitmap Pixel Format: Format = " << props.pixelFormat.format
                << ", AlphaMode = " << props.pixelFormat.alphaMode << "\n";
            log() << "Bitmap Options: " << props.bitmapOptions << "\n";
            log() << "Maximum Bitmap Size Supported: " << mDC->GetMaximumBitmapSize() << "\n";

            return false;
        }

    }
    else {
        D2D1_BITMAP_PROPERTIES1 props{};
        props.dpiX = 96;
        props.dpiY = 96;
        props.pixelFormat = D2DPixelFormat(mFormat);
        props.colorContext = nullptr;
        props.bitmapOptions = D2D1_BITMAP_OPTIONS_TARGET;
        hr = mDC->CreateBitmapFromDxgiSurface(
            mSurface.Get(), props, (ID2D1Bitmap1**)mBitmap.ReleaseAndGetAddressOf());

        if (FAILED(hr)) {
            log() << "[D2D1.1] CreateBitmapFromDxgiSurface failure Code: " << HEX(hr) << "\n";
            return false;
        }
    }

    mDC->SetTarget(mBitmap.Get());

    /*hr = mDC->CreateSolidColorBrush(D2D1::ColorF(0, 0), mSolidColorBrush.ReleaseAndGetAddressOf());
    if (FAILED(hr)) {
        log() << "[D2D1.1] Failure creating solid color brush (I2)." << HEX(hr) << "\n";
        return false;
    }*/

    mDC->BeginDraw();

    if (!mSurface) {
        mDC->Clear();
    }

    mInitState = InitState::Success;

    return true;
}

static inline bool IsPatternSupportedByD2D(
    const Pattern& aPattern, CompositionOp aOp = CompositionOp::OP_OVER) {
    if (aOp == CompositionOp::OP_CLEAR) {
        return true;
    }

    if (aPattern.GetType() == PatternType::CONIC_GRADIENT) {
        return false;
    }

    if (aPattern.GetType() != PatternType::RADIAL_GRADIENT) {
        return true;
    }

    const RadialGradientPattern* pat =
        static_cast<const RadialGradientPattern*>(&aPattern);

    if (pat->mRadius1 != 0) {
        return false;
    }

    POINT2F diff = POINT2F{
        pat->mCenter2.x - pat->mCenter1.x,
        pat->mCenter2.y - pat->mCenter1.y };

    if (sqrt(diff.x * diff.x + diff.y * diff.y) >= pat->mRadius2) {
        // Inner point lies outside the circle.
        return false;
    }

    return true;
}

static inline D2D1_PRIMITIVE_BLEND D2DPrimitiveBlendMode(CompositionOp aOp) {
    switch (aOp) {
    case CompositionOp::OP_OVER:
        return D2D1_PRIMITIVE_BLEND_SOURCE_OVER;
    // D2D1_PRIMITIVE_BLEND_COPY should leave pixels out of the source's
    // bounds unchanged, but doesn't- breaking unbounded ops.
    // D2D1_PRIMITIVE_BLEND_MIN doesn't quite work like darken either, as it
    // accounts for the source alpha.
    //
    // case CompositionOp::OP_SOURCE:
    //   return D2D1_PRIMITIVE_BLEND_COPY;
    // case CompositionOp::OP_DARKEN:
    //   return D2D1_PRIMITIVE_BLEND_MIN;
    case CompositionOp::OP_ADD:
        return D2D1_PRIMITIVE_BLEND_ADD;
    default:
        return D2D1_PRIMITIVE_BLEND_SOURCE_OVER;
    }
}


static inline bool D2DSupportsPrimitiveBlendMode(CompositionOp aOp) {
    switch (aOp) {
    case CompositionOp::OP_OVER:
    // case CompositionOp::OP_SOURCE:
    // case CompositionOp::OP_DARKEN:
    case CompositionOp::OP_ADD:
        return true;
    default:
        return false;
    }
}


bool Factory::PrepareForDrawing(CompositionOp aOp, const Pattern& aPattern) {

    if (!EnsureInitialized()) {
        return false;
    }

    bool patternSupported = IsPatternSupportedByD2D(aPattern, aOp);
    if (D2DSupportsPrimitiveBlendMode(aOp) && patternSupported) {
        // It's important to do this before FlushTransformToDC! As this will cause
        // the transform to become dirty.

        FlushTransformToDC();

        if (aOp != CompositionOp::OP_OVER) {
            mDC->SetPrimitiveBlend(D2DPrimitiveBlendMode(aOp));
        }

        return true;
    }

    HRESULT hr = mDC->CreateCommandList(mCommandList.ReleaseAndGetAddressOf());
    mDC->SetTarget(mCommandList.Get());
    mUsedCommandListsSincePurge++;

    // This is where we should have a valid command list.  If we don't, something
    // is wrong, and it's likely an OOM.
    if (!mCommandList) {
        log() << "Invalid D2D1.1 command list on creation "
            << mUsedCommandListsSincePurge << ", " << HEX(hr) << "\n";
    }

    FlushTransformToDC();

    return true;
}

void Factory::FlushTransformToDC() {
    if (mTransformDirty) {
        mDC->SetTransform(D2DMatrix(mTransform));
        mTransformDirty = false;
    }
}


ComPtr<ID2D1GradientStopCollection1> Factory::CreateGradientStops(
    GradientStop* rawStops,
    uint32_t aNumStops,
    ExtendMode aExtendMode) const
{
    if (aNumStops == 0) {
        log() << ": Failed to create GradientStopCollection with no stops." << "\n";
        return nullptr;
    }

    //D2D1_GRADIENT_STOP* stops = new D2D1_GRADIENT_STOP[aNumStops];
    std::unique_ptr<D2D1_GRADIENT_STOP[]> stops(new D2D1_GRADIENT_STOP[aNumStops]);

    for (uint32_t i = 0; i < aNumStops; i++) {
        stops[i].position = rawStops[i].offset;
        stops[i].color = D2DColor(rawStops[i].color);
    }

    ComPtr<ID2D1GradientStopCollection1> stopCollection;

    ComPtr<ID2D1DeviceContext> dc = Factory::GetD2DDeviceContext();

    if (!dc) {
        return nullptr;
    }

    HRESULT hr = dc->CreateGradientStopCollection(
        stops.get(), aNumStops, D2D1_COLOR_SPACE_SRGB, D2D1_COLOR_SPACE_SRGB,
        D2D1_BUFFER_PRECISION_8BPC_UNORM, D2DExtend(aExtendMode, Axis::BOTH),
        D2D1_COLOR_INTERPOLATION_MODE_PREMULTIPLIED,
        stopCollection.ReleaseAndGetAddressOf());
    //delete[] stops;

    if (FAILED(hr)) {
        log() << "Failed to create GradientStopCollection. Code: " << HEX(hr) << "\n";
        return nullptr;
    }

    ComPtr<ID3D11Device> device = Factory::GetDirect3D11Device();
    return stopCollection;
    //return MakeAndAddRef<GradientStopsD2D>(stopCollection, device);
}

//void Factory::InitializeD2D() {
//
//    // Verify that Direct2D device creation succeeded.
//    ComPtr<ID3D11Device> contentDevice = dm->GetContentDevice();
//    if (!Factory::SetDirect3D11Device(contentDevice)) {
//        d2d1.SetFailed(FeatureStatus::Failed, "Failed to create a Direct2D device",
//            "FEATURE_FAILURE_D2D_CREATE_FAILED"_ns);
//        return;
//    }
//}

inline bool SdkLayersAvailable()
{
    HRESULT hr = D3D11CreateDevice(
        nullptr,
        D3D_DRIVER_TYPE_NULL,       // There is no need to create a real hardware device.
        0,
        D3D11_CREATE_DEVICE_DEBUG,  // Check for the SDK layers.
        nullptr,                    // Any feature level will do.
        0,
        D3D11_SDK_VERSION,          // Always set this to D3D11_SDK_VERSION for Windows Store apps.
        nullptr,                    // No need to keep the D3D device reference.
        nullptr,                    // No need to know the feature level.
        nullptr                     // No need to keep the D3D device context reference.
    );

    return SUCCEEDED(hr);
}

HRESULT Factory::CreateDeviceResources()
{
    // This flag adds support for surfaces with a different color channel ordering
    // than the API default. It is required for compatibility with Direct2D.
    UINT creationFlags = D3D11_CREATE_DEVICE_BGRA_SUPPORT;

#if defined(_DEBUG)
    if (SdkLayersAvailable())
    {
        // If the project is in a debug build, enable debugging via SDK Layers with this flag.
        creationFlags |= D3D11_CREATE_DEVICE_DEBUG;
    }
#endif

    // This array defines the set of DirectX hardware feature levels this app will support.
    // Note the ordering should be preserved.
    // Don't forget to declare your application's minimum required feature level in its
    // description.  All applications are assumed to support 9.1 unless otherwise stated.
    D3D_FEATURE_LEVEL featureLevels[] =
    {
        D3D_FEATURE_LEVEL_11_1,
        D3D_FEATURE_LEVEL_11_0,
        D3D_FEATURE_LEVEL_10_1,
        D3D_FEATURE_LEVEL_10_0,
        D3D_FEATURE_LEVEL_9_3,
        D3D_FEATURE_LEVEL_9_2,
        D3D_FEATURE_LEVEL_9_1
    };

    ComPtr<ID3D11Device> device;
    ComPtr<ID3D11DeviceContext> context;

    HRESULT hr = D3D11CreateDevice(
        nullptr,					// Specify nullptr to use the default adapter.
        D3D_DRIVER_TYPE_HARDWARE,	// Create a device using the hardware graphics driver.
        0,							// Should be 0 unless the driver is D3D_DRIVER_TYPE_SOFTWARE.
        creationFlags,				// Set debug and Direct2D compatibility flags.
        featureLevels,				// List of feature levels this app can support.
        ARRAYSIZE(featureLevels),	// Size of the list above.
        D3D11_SDK_VERSION,			// Always set this to D3D11_SDK_VERSION for Windows Store apps.
        &device,					// Returns the Direct3D device created.
        &m_d3dFeatureLevel,			// Returns feature level of device created.
        &context					// Returns the device immediate context.
    );

    if (FAILED(hr))
    {
        // If the initialization fails, fall back to the WARP device.
        // For more information on WARP, see: 
        // http://go.microsoft.com/fwlink/?LinkId=286690
        hr = D3D11CreateDevice(
            nullptr,
            D3D_DRIVER_TYPE_WARP, // Create a WARP device instead of a hardware device.
            0,
            creationFlags,
            featureLevels,
            ARRAYSIZE(featureLevels),
            D3D11_SDK_VERSION,
            &device,
            &m_d3dFeatureLevel,
            &context
        );
        if (FAILED(hr))
        {
            log() << "Failed to D3D11CreateDevice" << HEX(hr) << "\n";
        }
    }

    // Store pointers to the Direct3D 11.1 API device and immediate context.
    hr = device.As(&mD3D11Device);
    if (FAILED(hr))
    {
        log() << "Failed to get device as ID3D11Device" << HEX(hr) << "\n";
        return hr;
    }

    hr = context.As(&mD3D11DeviceContext);
    if (FAILED(hr))
    {
        log() << "Failed to get context as ID3D11DeviceContext" << HEX(hr) << "\n";
        return hr;
    }

    
    // Create the Direct2D device object and a corresponding context.
    ComPtr<IDXGIDevice3> dxgiDevice;
    hr = mD3D11Device.As(&dxgiDevice);
    if (FAILED(hr))
    {
        log() << "Failed to get device as IDXGIDevice3" << HEX(hr) << "\n";
        return hr;
    }

    hr = mFactory->CreateDevice(dxgiDevice.Get(), &mD2D1Device);
    if (FAILED(hr))
    {
        log() << "Failed to create ID2D1Device" << HEX(hr) << "\n";
        return hr;
    }

    hr = mD2D1Device->CreateDeviceContext(
        D2D1_DEVICE_CONTEXT_OPTIONS_NONE,
        &mMTDC
    );

    if (FAILED(hr))
    {
        log() << "Failed to create ID2D1DeviceContext" << HEX(hr) << "\n";
        return hr;
    }
    

    if (!Factory::SetDirect3D11Device(mD3D11Device.Get())) {
        log() << "Failed to SetDirect3D11Device" << "\n";
        return E_FAIL;;
    }

    return S_OK;
}


//void gfxWindowsPlatform::InitializeD2D() {
//    ScopedGfxFeatureReporter d2d1_1("D2D1.1");
//
//    FeatureState& d2d1 = gfxConfig::GetFeature(Feature::DIRECT2D);
//
//    DeviceManagerDx* dm = DeviceManagerDx::Get();
//
//    // We don't know this value ahead of time, but the user can force-override
//    // it, so we use Disable instead of SetFailed.
//    if (dm->IsWARP()) {
//        d2d1.Disable(FeatureStatus::Blocked,
//            "Direct2D is not compatible with Direct3D11 WARP",
//            "FEATURE_FAILURE_D2D_WARP_BLOCK"_ns);
//    }
//
//    // If we pass all the initial checks, we can proceed to runtime decisions.
//    if (!d2d1.IsEnabled()) {
//        return;
//    }
//
//    if (!Factory::SupportsD2D1()) {
//        d2d1.SetFailed(FeatureStatus::Unavailable,
//            "Failed to acquire a Direct2D 1.1 factory",
//            "FEATURE_FAILURE_D2D_FACTORY"_ns);
//        return;
//    }
//
//    if (!dm->GetContentDevice()) {
//        d2d1.SetFailed(FeatureStatus::Failed,
//            "Failed to acquire a Direct3D 11 content device",
//            "FEATURE_FAILURE_D2D_DEVICE"_ns);
//        return;
//    }
//
//    if (!dm->TextureSharingWorks()) {
//        d2d1.SetFailed(FeatureStatus::Failed,
//            "Direct3D11 device does not support texture sharing",
//            "FEATURE_FAILURE_D2D_TXT_SHARING"_ns);
//        return;
//    }
//
//    // Using Direct2D depends on DWrite support.
//    if (!DWriteEnabled() && !InitDWriteSupport()) {
//        d2d1.SetFailed(FeatureStatus::Failed,
//            "Failed to initialize DirectWrite support",
//            "FEATURE_FAILURE_D2D_DWRITE"_ns);
//        return;
//    }
//
//    // Verify that Direct2D device creation succeeded.
//    RefPtr<ID3D11Device> contentDevice = dm->GetContentDevice();
//    if (!Factory::SetDirect3D11Device(contentDevice)) {
//        d2d1.SetFailed(FeatureStatus::Failed, "Failed to create a Direct2D device",
//            "FEATURE_FAILURE_D2D_CREATE_FAILED"_ns);
//        return;
//    }
//
//    MOZ_ASSERT(d2d1.IsEnabled());
//    d2d1_1.SetSuccessful();
//}


ComPtr<IDXGIAdapter1> Factory::GetDXGIAdapterLocked()
{
    if (mAdapter) {
        return mAdapter;
    }

    HRESULT hr = S_OK;

    typedef HRESULT(WINAPI* CreateDXGIFactory1Func)(REFIID riid, void** ppFactory);

    ComPtr<IDXGIFactory1> dxgiFactory;
    CreateDXGIFactory1Func createDXGIFactory1;
    HMODULE dxgiModule = LoadLibraryW(L"dxgi.dll");

    if (!dxgiModule) {
        log() << "Failed to load dxgi.dll" << "\n";
        return nullptr;
    }

    createDXGIFactory1 = (CreateDXGIFactory1Func)GetProcAddress(dxgiModule, "CreateDXGIFactory1");

    if (!createDXGIFactory1) {
        log() << "Failed to locate CreateDXGIFactory1 function." << "\n";
        return nullptr;
    }

    typedef HRESULT(WINAPI* CreateDXGIFactory2Func)(UINT Flags, REFIID riid, void** ppFactory);

    CreateDXGIFactory2Func createDXGIFactory2 = nullptr;
    createDXGIFactory2 = (CreateDXGIFactory2Func)GetProcAddress(dxgiModule, "CreateDXGIFactory2");

    // Attempt to create IDXGIFactory2 for additional features, like debug layer
#if defined(_DEBUG)
    if (createDXGIFactory2) {
        hr = createDXGIFactory2(
            DXGI_CREATE_FACTORY_DEBUG,
            __uuidof(IDXGIFactory2),
            reinterpret_cast<void**>(dxgiFactory.GetAddressOf()));
        if (FAILED(hr)) {
            log() << "Failed to create DXGI factory 2 with debug layer. Code: " << hr << "\n";
            return nullptr;
        }
    }
    else {
        log() << "fCreateDXGIFactory2 not loaded, cannot create debug IDXGIFactory2." << "\n";
    }
#endif

    // If IDXGIFactory2 is not created or debug layer is not needed, fall back to IDXGIFactory1
    if (!dxgiFactory) {
        hr = createDXGIFactory1(__uuidof(IDXGIFactory1), reinterpret_cast<void**>(dxgiFactory.GetAddressOf()));
        if (FAILED(hr)) {
            // This seems to happen with some people running the iZ3D driver.
            log() << "Failed to create DXGI factory 1. Code: " << hr << "\n";
            return nullptr;
        }
    }


    if (!mDeviceStatus) {
        // If we haven't created a device yet, and have no existing device status,
        // then this must be the compositor device. Pick the first adapter we can.
        if (FAILED(dxgiFactory->EnumAdapters1(0, getter_AddRefs(mAdapter)))) {
            return nullptr;
        }
    }
    else {
        // In the UI and GPU process, we clear mDeviceStatus on device reset, so we
        // should never reach here. Furthermore, the UI process does not create
        // devices when using a GPU process.
        //
        // So, this should only ever get called on the content process or RDD
        // process
        MOZ_ASSERT(XRE_IsContentProcess() || XRE_IsRDDProcess());

        // In the child process, we search for the adapter that matches the parent
        // process. The first adapter can be mismatched on dual-GPU systems.
        for (UINT index = 0;; index++) {
            RefPtr<IDXGIAdapter1> adapter;
            if (FAILED(dxgiFactory->EnumAdapters1(index, getter_AddRefs(adapter)))) {
                break;
            }

            const DxgiAdapterDesc& preferred = mDeviceStatus->adapter();

            DXGI_ADAPTER_DESC desc;
            if (SUCCEEDED(adapter->GetDesc(&desc)) &&
                desc.AdapterLuid.HighPart == preferred.AdapterLuid.HighPart &&
                desc.AdapterLuid.LowPart == preferred.AdapterLuid.LowPart &&
                desc.VendorId == preferred.VendorId &&
                desc.DeviceId == preferred.DeviceId) {
                mAdapter = adapter.forget();
                break;
            }
        }
    }

    if (!mAdapter) {
        return nullptr;
    }

    // We leak this module everywhere, we might as well do so here as well.
    dxgiModule.disown();
    return mAdapter;
}
