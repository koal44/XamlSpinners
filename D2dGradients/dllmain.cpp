// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"
#include "Factory.h"
#include "macros.h"
#include <d2d1_1.h>
#include <d2d1_2.h>
#include <d2d1effects.h> // for CLSID_D2D1Flood
//#include <d3d11.h>
//#include <d2d1.h>
#include "ConicGradientEffectD2D1.h"

BOOL APIENTRY DllMain(HMODULE hModule,
    DWORD  ul_reason_for_call,
    LPVOID lpReserved
)
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    }
    return TRUE;
}

extern "C" {
    __declspec(dllexport) const char* HelloWorld()
    {
        return "Hello World from C++ DLL";
    }
}

extern "C" {
    __declspec(dllexport) void HelloWorld2()
    {
        OutputDebugStringA("Hello World from C++ DLL\n");
    }
}


extern "C" __declspec(dllexport) HRESULT GetBitmap2(BYTE** outBitmap, int* width, int* height, int* stride)
//extern "C" HRESULT __stdcall GetBitmap(BYTE** outBitmap, int* width, int* height, int* stride)
{
    log() << "GetBitmap" << "\n";
    HRESULT hr = S_OK;

    Factory factory = Factory();
    factory.factory();
    factory.CreateDeviceResources();
    if (!factory.Init(D2D1_SIZE_U{ 100, 100 }, SurfaceFormat::B8G8R8A8))
    {
        log() << "Failed to initialize factory" << "\n";
        return E_FAIL;
    }

    GradientStop stops[3];
    stops[0].offset = 0.0f;
    stops[0].color = DeviceColor(1.0f, 0.0f, 0.0f, 1.0f);
    stops[1].offset = 0.5f;
    stops[1].color = DeviceColor(0.0f, 1.0f, 0.0f, 1.0f);
    stops[2].offset = 1.0f;
    stops[2].color = DeviceColor(0.0f, 0.0f, 1.0f, 1.0f);
    ComPtr<ID2D1GradientStopCollection1> stopCollection;
    stopCollection = factory.CreateGradientStops(stops, 3, ExtendMode::CLAMP); // 3 stops

    if (!stopCollection)
    {
        log() << "Failed to create gradient stop collection. Code: " << HEX(hr) << "\n";
        return hr;
    }

    ConicGradientPattern pat = ConicGradientPattern(POINT2F{ 0.5f, 0.5f }, 0.0f, 0.0f, 0.5f, stopCollection.Get());
    factory.PrepareForDrawing(CompositionOp::OP_OVER, pat);
    factory.FinalizeDrawing(pat);
    /*hr = factory.GetD2DDeviceContext2()->EndDraw();
    if (FAILED(hr))
    {
        log() << "Failed to end draw" << "\n";
        return hr;
    }*/

    ID2D1Bitmap1* bitmap = factory.GetBitmap().Get();

    if (!bitmap)
    {
        log() << "bitmap is null" << "\n";
        return E_FAIL;
    }

    ID2D1Bitmap1* cpuBitmap = factory.GetCpuBitmap().Get();
    if (!cpuBitmap)
    {
        log() << "cpuBitmap is null" << "\n";
        return E_FAIL;
    }

    hr = cpuBitmap->CopyFromBitmap(NULL, bitmap, NULL);
    if (FAILED(hr))
    {
        log() << "Failed to copy from bitmap into cpuBitmap. Code: " << HEX(hr) << "\n";
        return hr;
    }

    // Lock the bitmap and access pixels
    D2D1_MAPPED_RECT mappedRect;
    hr = cpuBitmap->Map(D2D1_MAP_OPTIONS_READ, &mappedRect);
    if (FAILED(hr))
    {
        log() << "Failed to map bitmap. Code: " << HEX(hr) << "\n";
        return hr;
    }

    // Allocate memory for the byte array
    int size = mappedRect.pitch * bitmap->GetSize().height;
    *outBitmap = new BYTE[size];

    // Copy pixels
    memcpy(*outBitmap, mappedRect.bits, size);

    // Set width, height, and stride
    *width = bitmap->GetSize().width;
    *height = bitmap->GetSize().height;
    *stride = mappedRect.pitch;

    // Unlock the bitmap
    hr = cpuBitmap->Unmap();
    if (FAILED(hr))
    {
        log() << "Failed to unmap bitmap. Code: " << HEX(hr) << "\n";
        return hr;
    }

    return hr;
}

extern "C" __declspec(dllexport) HRESULT GetBitmap(BYTE * *outBitmap, int* width, int* height, int* stride)
{
    HRESULT hr = S_OK;


    // Create DXGI Factory for enumerating adapters
    ComPtr<IDXGIFactory1> dxgiFactory;
    hr = CreateDXGIFactory1(__uuidof(IDXGIFactory1), (void**)(&dxgiFactory));
    if (FAILED(hr))
    {
        log() << "Failed to create DXGI factory. Code: " << HEX(hr) << "\n";
        return hr;
    }

    // Enumerate adapters
    UINT adapterIndex = 0;
    ComPtr<IDXGIAdapter1> adapter;
    while (dxgiFactory->EnumAdapters1(adapterIndex, &adapter) != DXGI_ERROR_NOT_FOUND)
    {
        DXGI_ADAPTER_DESC1 desc;
        hr = adapter->GetDesc1(&desc);
        if (SUCCEEDED(hr))
        {
            log() << "Adapter " << adapterIndex << ": " << desc.Description << "\n";
        }
        adapterIndex++;
    }





    // Initialize the COM Library
    /*HRESULT hr = CoInitializeEx(nullptr, COINITBASE_MULTITHREADED);
    if (FAILED(hr))
    {
        log() << "Failed to initialize COM Library" << "\n";
        return hr;
    }*/

    // Create the Direct3D Device and Context
    UINT createDeviceFlags = D3D11_CREATE_DEVICE_BGRA_SUPPORT;
#ifdef _DEBUG
    //createDeviceFlags |= D3D11_CREATE_DEVICE_DEBUG;
#endif

    D3D_FEATURE_LEVEL featureLevels[] = {
       /* D3D_FEATURE_LEVEL_11_1,
        D3D_FEATURE_LEVEL_11_0,
        D3D_FEATURE_LEVEL_10_1,
        D3D_FEATURE_LEVEL_10_0,*/
        D3D_FEATURE_LEVEL_9_3,
        D3D_FEATURE_LEVEL_9_2,
        D3D_FEATURE_LEVEL_9_1
    };

    ComPtr<ID3D11Device> d3dDevice;
    D3D_FEATURE_LEVEL featureLevel;
    ComPtr<ID3D11DeviceContext> d3dContext;

    hr = D3D11CreateDevice(
        nullptr,					// Specify nullptr to use the default adapter.
        D3D_DRIVER_TYPE_HARDWARE,	// Create a device using the hardware graphics driver.
        0,							// Should be 0 unless the driver is D3D_DRIVER_TYPE_SOFTWARE.
        createDeviceFlags,		    // Set debug and Direct2D compatibility flags.
        featureLevels,				// List of feature levels this app can support.
        ARRAYSIZE(featureLevels),	// Size of the list above.
        D3D11_SDK_VERSION,			// Always set this to D3D11_SDK_VERSION for Windows Store apps.
        &d3dDevice,					// Returns the Direct3D device created.
        &featureLevel,			    // Returns feature level of device created.
        &d3dContext					// Returns the device immediate context.
    );

    if (FAILED(hr))
    {
        log() << "Failed to create D3D11 device" << "\n";
        return hr;
    }

    hr = d3dDevice.As(&d3dDevice);
    if (FAILED(hr))
    {
        log() << "Failed to get device as ID3D11Device. Code: " << HEX(hr) << "\n";
        return hr;
    }

    hr = d3dContext.As(&d3dContext);
    if (FAILED(hr))
    {
        log() << "Failed to get device context as ID3D11DeviceContext. Code: " << HEX(hr) << "\n";
        return hr;
    }

    // Create the Direct2D Device
    ComPtr<IDXGIDevice> dxgiDevice;
    hr = d3dDevice.As(&dxgiDevice);
    if (FAILED(hr))
    {
        log() << "Failed to get device as IDXGIDevice. Code: " << HEX(hr) << "\n";
        return hr;
    }

    // Create the Direct2D Factory
    D2D1_FACTORY_OPTIONS options{};
#if defined(_DEBUG)
    options.debugLevel = D2D1_DEBUG_LEVEL_INFORMATION;
#endif

    ComPtr<ID2D1Factory1> d2dFactory;
    hr = D2D1CreateFactory(
        D2D1_FACTORY_TYPE_SINGLE_THREADED,
        __uuidof(ID2D1Factory2),
        &options,
        &d2dFactory
    );
    if (FAILED(hr))
    {
        log() << "Failed to create D2D1 factory. Code: " << HEX(hr) << "\n";
        return hr;
    }

    // Create the Direct2D Device
    ComPtr<ID2D1Device> d2dDevice;
    hr = d2dFactory->CreateDevice(dxgiDevice.Get(), &d2dDevice);
    if (FAILED(hr))
    {
        log() << "Failed to create D2D1 device. Code: " << HEX(hr) << "\n";
        return hr;
    }

    // Create the Direct2D Device Context
    ComPtr<ID2D1DeviceContext> d2dContext;
    hr = d2dDevice->CreateDeviceContext(D2D1_DEVICE_CONTEXT_OPTIONS_NONE, &d2dContext);
    if (FAILED(hr))
    {
        log() << "Failed to create D2D1 device context. Code: " << HEX(hr) << "\n";
        return hr;
    }

    //// Create the ID2D1CommandList
    //ComPtr<ID2D1CommandList> d2dCommandList;
    //hr = d2dContext->CreateCommandList(d2dCommandList.ReleaseAndGetAddressOf());
    //if (FAILED(hr))
    //{
    //    log() << "Failed to create D2D1 command list. Code: " << HEX(hr) << "\n";
    //    return hr;
    //}
    //d2dContext->SetTarget(d2dCommandList.Get());

    
    // Register the Custom Effect
    hr = ConicGradientEffectD2D1::Register(d2dFactory.Get());
    if (FAILED(hr))
    {
        log() << "Failed to register conic gradient effect. Code: " << HEX(hr) << "\n";
        return hr;
    }
    

    // Create the Bitmaps
    DXGI_FORMAT format = DXGI_FORMAT_B8G8R8A8_UNORM;
    D2D1_ALPHA_MODE alphaMode = D2D1_ALPHA_MODE_IGNORE;
    D2D1_SIZE_U bitmapSize = { 100, 100 };

    ComPtr<ID2D1Bitmap1> gpuBitmap;
    D2D1_BITMAP_PROPERTIES1 gpuProps{};
    gpuProps.dpiX = 96;
    gpuProps.dpiY = 96;
    gpuProps.pixelFormat = D2D1::PixelFormat(format, alphaMode);
    gpuProps.colorContext = nullptr;
    gpuProps.bitmapOptions = D2D1_BITMAP_OPTIONS_TARGET;
    hr = d2dContext->CreateBitmap(bitmapSize, nullptr, 0, gpuProps, gpuBitmap.ReleaseAndGetAddressOf());
    if (FAILED(hr)) {
        log() << "ID2D1DeviceContext::CreateBitmap (render) failure. Code: " << HEX(hr) << "\n";
        return hr;
    }

    ComPtr<ID2D1Bitmap1> cpuBitmap;
    D2D1_BITMAP_PROPERTIES1 cpuProps{};
    cpuProps.dpiX = 96;
    cpuProps.dpiY = 96;
    cpuProps.pixelFormat = D2D1::PixelFormat(format, alphaMode);
    cpuProps.colorContext = nullptr;
    cpuProps.bitmapOptions = D2D1_BITMAP_OPTIONS_CPU_READ | D2D1_BITMAP_OPTIONS_CANNOT_DRAW;
    hr = d2dContext->CreateBitmap(bitmapSize, nullptr, 0, cpuProps, cpuBitmap.ReleaseAndGetAddressOf());
    if (FAILED(hr)) {
        log() << "ID2D1DeviceContext::CreateBitmap (cpu) failure. Code: " << HEX(hr) << "\n";
        return hr;
    }

    /*ComPtr<ID2D1Bitmap> tmpBitmap;
    D2D1_BITMAP_PROPERTIES tmpProps = D2D1::BitmapProperties(D2DPixelFormat(SurfaceFormat::B8G8R8A8));
    hr = d2dContext->CreateBitmap(bitmapSize, tmpProps, tmpBitmap.ReleaseAndGetAddressOf());
    if (FAILED(hr)) {
        log() << "ID2D1DeviceContext::CreateBitmap (tmp) failure. Code: " << HEX(hr) << "\n";
        return hr;
    }*/

    d2dContext->SetTarget(gpuBitmap.Get());
    // Define clipping rectangle
    //D2D1_RECT_F clipRect = D2D1::RectF(0, 0, 100, 100);
    // Apply clipping
    //d2dContext->PushAxisAlignedClip(&clipRect, D2D1_ANTIALIAS_MODE_ALIASED);
    d2dContext->BeginDraw();
    d2dContext->Clear();

    /*
    // create the gradient pattern
    UINT32 numStops = 3;
    GradientStop stops[3];
    ExtendMode extendMode = ExtendMode::CLAMP;

    stops[0].offset = 0.0f;
    stops[0].color = DeviceColor(1.0f, 0.0f, 0.0f, 1.0f);
    stops[1].offset = 0.5f;
    stops[1].color = DeviceColor(0.0f, 1.0f, 0.0f, 1.0f);
    stops[2].offset = 1.0f;
    stops[2].color = DeviceColor(0.0f, 0.0f, 1.0f, 1.0f);
    //stopCollection = d2dFactory.CreateGradientStops(stops, 3, ExtendMode::CLAMP); // 3 stops
    std::unique_ptr<D2D1_GRADIENT_STOP[]> d2dStops(new D2D1_GRADIENT_STOP[numStops]);

    for (uint32_t i = 0; i < numStops; i++) {
        d2dStops[i].position = stops[i].offset;
        d2dStops[i].color = D2DColor(stops[i].color);
    }

    ComPtr<ID2D1GradientStopCollection1> stopCollection;
    hr = d2dContext->CreateGradientStopCollection(
        d2dStops.get(),
        numStops,
        D2D1_COLOR_SPACE_SRGB,
        D2D1_COLOR_SPACE_SRGB,
        D2D1_BUFFER_PRECISION_8BPC_UNORM,
        D2DExtend(extendMode, Axis::BOTH),
        D2D1_COLOR_INTERPOLATION_MODE_PREMULTIPLIED,
        stopCollection.ReleaseAndGetAddressOf()
    );

    if (FAILED(hr) || !stopCollection) {
        log() << "Failed to create gradient stop collection. Code: " << HEX(hr) << "\n";
        return hr;
    }

    ConicGradientPattern pat = ConicGradientPattern(POINT2F{ 0.5f, 0.5f }, 0.0f, 0.0f, 0.5f, stopCollection.Get());

    //
    ComPtr<ID2D1Effect> conicGradientEffect;
    hr = d2dContext->CreateEffect(CLSID_ConicGradientEffect, conicGradientEffect.ReleaseAndGetAddressOf());
    if (FAILED(hr) || !conicGradientEffect) {
        log() << "Failed to create conic gradient effect. Code: " << HEX(hr) << "\n";
        return hr;
    }

    conicGradientEffect->SetValue(CONIC_PROP_STOP_COLLECTION, pat.mStops.Get());
    conicGradientEffect->SetValue(CONIC_PROP_CENTER, D2D1::Vector2F(pat.mCenter.x, pat.mCenter.y));
    conicGradientEffect->SetValue(CONIC_PROP_ANGLE, pat.mAngle);
    conicGradientEffect->SetValue(CONIC_PROP_START_OFFSET, pat.mStartOffset);
    conicGradientEffect->SetValue(CONIC_PROP_END_OFFSET, pat.mEndOffset);
    conicGradientEffect->SetValue(CONIC_PROP_TRANSFORM, D2DMatrix(pat.mMatrix));
    conicGradientEffect->SetInput(0, tmpBitmap.Get());

    d2dContext->DrawImage(
        conicGradientEffect.Get(),
        D2D1_INTERPOLATION_MODE_NEAREST_NEIGHBOR,
        D2DCompositionMode(CompositionOp::OP_OVER)
    );
    */


    
    // Create and apply a flood effect to fill with a solid color
    ComPtr<ID2D1Effect> floodEffect;
    hr = d2dContext->CreateEffect(CLSID_D2D1Flood, &floodEffect);
    //hr = d2dContext->CreateEffect(CLSID_D2D1Flood, &floodEffect);
    if (FAILED(hr)) {
        log() << "Failed to create flood effect" << HEX(hr) << "\n";
        return hr;
    }

    // Set the color for the flood effect
    D2D1_VECTOR_4F color = D2D1::Vector4F(1.0f, 0.0f, 0.0f, 1.0f); // RGBA: Red
    floodEffect->SetValue(D2D1_FLOOD_PROP_COLOR, color);

    // Draw the effect
    d2dContext->DrawImage(floodEffect.Get());
    

    hr = d2dContext->EndDraw();
    if (FAILED(hr)) {
        log() << "Failed to end draw. Code: " << HEX(hr) << "\n";
        return hr;
    }

    // Copy the bitmap to the CPU bitmap
    hr = cpuBitmap->CopyFromBitmap(NULL, gpuBitmap.Get(), NULL);
    if (FAILED(hr))
    {
        log() << "Failed to copy from bitmap into cpuBitmap. Code: " << HEX(hr) << "\n";
        return hr;
    }

    D2D1_MAPPED_RECT mappedRect;
    hr = cpuBitmap->Map(D2D1_MAP_OPTIONS_READ, &mappedRect);
    if (FAILED(hr))
    {
        log() << "Failed to map bitmap. Code: " << HEX(hr) << "\n";
        return hr;
    }

    // return bitmap data
    int size = mappedRect.pitch * gpuBitmap->GetSize().height;
    *outBitmap = new BYTE[size];
    memcpy(*outBitmap, mappedRect.bits, size);

    *width = gpuBitmap->GetSize().width;
    *height = gpuBitmap->GetSize().height;
    *stride = mappedRect.pitch;

    hr = cpuBitmap->Unmap();
    if (FAILED(hr))
    {
        log() << "Failed to unmap bitmap. Code: " << HEX(hr) << "\n";
        return hr;
    }

    return hr;
}
