#include "pch.h"
#include "DirectXResources.h"
#include <dxgi.h>
#include "Macros.h"
#include <d3d11.h>
#include <d2d1.h>
#include <d2d1_1.h>
#include <d2d1_2.h>
#include "Pattern.h"
#include <vector>
#include "SimpleEffect.h"


ComPtr<IDXGIFactory1> DirectXResources::m_dxgiFactory = nullptr;
ComPtr<IDXGIAdapter1> DirectXResources::m_dxgiAdapter = nullptr;
ComPtr<ID3D11Device> DirectXResources::m_d3dDevice = nullptr;
ComPtr<ID3D11DeviceContext> DirectXResources::m_d3dContext = nullptr;
ComPtr<IDXGIDevice> DirectXResources::m_dxgiDevice = nullptr;
ComPtr<ID2D1Factory1> DirectXResources::m_d2dFactory = nullptr;
ComPtr<ID2D1Device> DirectXResources::m_d2dDevice = nullptr;
D3D_FEATURE_LEVEL DirectXResources::m_featureLevel = D3D_FEATURE_LEVEL_9_1;

DirectXResources::DirectXResources() :
    m_d2dContext(nullptr)
{ }

DirectXResources::~DirectXResources()
{ }

HRESULT DirectXResources::CreateDeviceResources()
{
    HRESULT hr = S_OK;

    UINT creationFlags = D3D11_CREATE_DEVICE_BGRA_SUPPORT;

    // Create DXGI Factory for enumerating adapters
    hr = CreateDXGIFactory1(__uuidof(IDXGIFactory1), (void**)(&m_dxgiFactory));
    if (FAILED(hr))
    {
        log() << "Failed to create DXGI factory. Code: " << HEX(hr) << "\n";
        return hr;
    }

    // Enumerate adapters
    UINT adapterIndex = 0;
    while (m_dxgiFactory->EnumAdapters1(adapterIndex, &m_dxgiAdapter) != DXGI_ERROR_NOT_FOUND)
    {
        DXGI_ADAPTER_DESC1 desc;
        hr = m_dxgiAdapter->GetDesc1(&desc);
        if (SUCCEEDED(hr))
        {
            //log() << "Adapter " << adapterIndex << ": " << desc.Description << "\n";
        }
        else
        {
            log() << "Failed to get adapter description. Code: " << HEX(hr) << "\n";
        }
        adapterIndex++;
    }

    // Get the most powerful adapter
    hr = GetMostPowerfulAdapter();
    if (FAILED(hr))
    {
        log() << "Failed to get most powerful adapter. Code: " << HEX(hr) << "\n";
        return hr;
    }


    // Create the Direct3D Device and Context


    D3D_FEATURE_LEVEL featureLevels[] = {
         D3D_FEATURE_LEVEL_11_1,
         D3D_FEATURE_LEVEL_11_0,
         D3D_FEATURE_LEVEL_10_1,
         D3D_FEATURE_LEVEL_10_0,
         D3D_FEATURE_LEVEL_9_3,
         D3D_FEATURE_LEVEL_9_2,
         D3D_FEATURE_LEVEL_9_1
    };

    hr = D3D11CreateDevice(
        m_dxgiAdapter.Get(),		// Specify nullptr to use the default adapter.
        D3D_DRIVER_TYPE_UNKNOWN,	// Create a device using the hardware graphics driver. D3D_DRIVER_TYPE_HARDWARE,
        0,							// Should be 0 unless the driver is D3D_DRIVER_TYPE_SOFTWARE.
        creationFlags,		        // Set debug and Direct2D compatibility flags.
        featureLevels,				// List of feature levels this app can support.
        ARRAYSIZE(featureLevels),	// Size of the list above.
        D3D11_SDK_VERSION,			// Always set this to D3D11_SDK_VERSION.
        &m_d3dDevice,				// Returns the Direct3D device created.
        &m_featureLevel,			// Returns feature level of device created.
        &m_d3dContext				// Returns the device immediate context.
    );

    if (FAILED(hr))
    {
        log() << "Failed to create D3D11 device. Code: " << HEX(hr) << "\n";
    }

    if (FAILED(hr))
    {
        hr = D3D11CreateDevice(
            nullptr,
            D3D_DRIVER_TYPE_WARP, // Create a WARP device instead of a hardware device.
            0,
            creationFlags,
            featureLevels,
            ARRAYSIZE(featureLevels),
            D3D11_SDK_VERSION,
            &m_d3dDevice,
            &m_featureLevel,
            &m_d3dContext
        );
        if (FAILED(hr))
        {
            log() << "Failed to create D3D11 device. Code: " << HEX(hr) << "\n";
            return hr;
        }
    }

    hr = m_d3dDevice.As(&m_d3dDevice);
    if (FAILED(hr))
    {
        log() << "Failed to get device as ID3D11Device. Code: " << HEX(hr) << "\n";
        return hr;
    }

    hr = m_d3dContext.As(&m_d3dContext);
    if (FAILED(hr))
    {
        log() << "Failed to get device context as ID3D11DeviceContext. Code: " << HEX(hr) << "\n";
        return hr;
    }

    // Create the Direct2D Device
    hr = m_d3dDevice.As(&m_dxgiDevice);
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

    hr = D2D1CreateFactory(
        D2D1_FACTORY_TYPE_SINGLE_THREADED,
        __uuidof(ID2D1Factory2),
        &options,
        &m_d2dFactory
    );
    if (FAILED(hr))
    {
        log() << "Failed to create D2D1 factory. Code: " << HEX(hr) << "\n";
        return hr;
    }

    // Create the Direct2D Device
    hr = m_d2dFactory->CreateDevice(m_dxgiDevice.Get(), &m_d2dDevice);
    if (FAILED(hr))
    {
        log() << "Failed to create D2D1 device. Code: " << HEX(hr) << "\n";
        return hr;
    }

    return hr;
}



HRESULT DirectXResources::CreateLocalDeviceResources()
{
    HRESULT hr = S_OK;

    //// Create the Direct2D Device
    //hr = m_d3dDevice.As(&m_dxgiDevice);
    //if (FAILED(hr))
    //{
    //    log() << "Failed to get device as IDXGIDevice. Code: " << HEX(hr) << "\n";
    //    return hr;
    //}

    //// Create the Direct2D Factory
    //D2D1_FACTORY_OPTIONS options{};

    // Create the Direct2D Device Context
    hr = m_d2dDevice->CreateDeviceContext(D2D1_DEVICE_CONTEXT_OPTIONS_NONE, &m_d2dContext);
    if (FAILED(hr))
    {
        log() << "Failed to create D2D1 device context. Code: " << HEX(hr) << "\n";
        return hr;
    }

    return hr;
}


HRESULT DirectXResources::CreateGradientStops(ComPtr<ID2D1GradientStopCollection1>& outStopCollection) {
    HRESULT hr = S_OK;

    // Create the gradient pattern
    UINT32 numStops = 3;
    GradientStop stops[3];
    ExtendMode extendMode = ExtendMode::CLAMP;

    // Define gradient stops
    stops[0].offset = 0.0f;
    stops[0].color = DeviceColor(1.0f, 0.0f, 0.0f, 1.0f);
    stops[1].offset = 0.5f;
    stops[1].color = DeviceColor(0.0f, 1.0f, 0.0f, 1.0f);
    stops[2].offset = 1.0f;
    stops[2].color = DeviceColor(0.0f, 0.0f, 1.0f, 1.0f);

    std::unique_ptr<D2D1_GRADIENT_STOP[]> d2dStops(new D2D1_GRADIENT_STOP[numStops]);

    // Convert to Direct2D gradient stops
    for (uint32_t i = 0; i < numStops; i++) {
        d2dStops[i].position = stops[i].offset;
        d2dStops[i].color = D2DColor(stops[i].color);
    }

    ComPtr<ID2D1GradientStopCollection1> stopCollection;
    hr = m_d2dContext->CreateGradientStopCollection(
        d2dStops.get(),
        numStops,
        D2D1_COLOR_SPACE_SRGB,
        D2D1_COLOR_SPACE_SRGB,
        D2D1_BUFFER_PRECISION_8BPC_UNORM,
        D2DExtend(extendMode, Axis::BOTH),
        D2D1_COLOR_INTERPOLATION_MODE_PREMULTIPLIED,
        stopCollection.ReleaseAndGetAddressOf()
    );

    if (FAILED(hr)) {
        log() << "Failed to create gradient stop collection. Code: " << HEX(hr) << "\n";
        return hr;
    }

    outStopCollection = stopCollection;

    return hr;
}

//HRESULT DirectXResources::RegisterEffects()
//{
//    HRESULT hr = S_OK;
//
//    //// Register the ConicGradient Effect
//    //hr = ConicGradientEffect::Register(m_d2dFactory.Get());
//    //if (FAILED(hr))
//    //{
//    //    log() << "Failed to register ConicGradientEffect. Code: " << HEX(hr) << "\n";
//    //    return hr;
//    //}
//
//    hr = SimpleEffect::Register(m_d2dFactory.Get());
//    if (FAILED(hr))
//    {
//        log() << "Failed to register ConicGradientEffect. Code: " << HEX(hr) << "\n";
//        return hr;
//    }
//
//    return hr;
//}


HRESULT DirectXResources::GetMostPowerfulAdapter() //ComPtr<IDXGIAdapter1>& outAdapter
{
    HRESULT hr = S_OK;
    std::vector<ComPtr<IDXGIAdapter1>> adapters;
    ComPtr<IDXGIAdapter1> currentAdapter;

    UINT adapterIndex = 0;
    while (m_dxgiFactory->EnumAdapters1(adapterIndex, &currentAdapter) != DXGI_ERROR_NOT_FOUND) {
        adapters.push_back(currentAdapter);
        adapterIndex++;
    }

    // NVIDIA, AMD, Intel, and Microsoft Vendor IDs
    const UINT NVIDIA_VID = 0x000010de;
    const UINT AMD_VID = 0x00001002;
    const UINT INTEL_VID = 0x00008086;
    const UINT MS_VID = 0x00001414;

    std::vector<ComPtr<IDXGIAdapter1>> discreteAdapters;
    ComPtr<IDXGIAdapter1> intelAdapter;
    ComPtr<IDXGIAdapter1> microsoftAdapter;

    for (auto& adapter : adapters) {
        DXGI_ADAPTER_DESC1 desc;
        hr = adapter->GetDesc1(&desc);
        if (FAILED(hr)) {
            log() << "Failed to get adapter description. Code: " << HEX(hr) << "\n";
            continue;
        }

        if (desc.VendorId == INTEL_VID) {
            intelAdapter = adapter;
        }
        else if (desc.VendorId == MS_VID) {
            microsoftAdapter = adapter;
        }
        else {
            discreteAdapters.push_back(adapter);
        }
    }

    if (!discreteAdapters.empty()) {
        m_dxgiAdapter = discreteAdapters.front();
    }
    else if (intelAdapter) {
        m_dxgiAdapter = intelAdapter;
    }
    else if (microsoftAdapter) {
        m_dxgiAdapter = microsoftAdapter;
    }
    else {
        log() << "DXGI adapter enumeration failed to find a GPU." << "\n";
        return E_FAIL;
    }

    // print the adapter description
    DXGI_ADAPTER_DESC1 desc;
    hr = m_dxgiAdapter->GetDesc1(&desc);
    if (SUCCEEDED(hr))
    {
        //log() << "Most powerful adapter: " << desc.Description << "\n";
    }
    else
    {
        log() << "Failed to get adapter description. Code: " << HEX(hr) << "\n";
    }

    return hr;
}

