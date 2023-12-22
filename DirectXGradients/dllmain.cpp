// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"
#include "DirectXResources.h"
#include "Macros.h"
#include "Types.h"
#include "Pattern.h"
#include "SimpleEffect.h"
#include <mutex>
#include <filesystem>

BOOL APIENTRY DllMain(
    HMODULE hModule,
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

static std::mutex lockMutex;

//static inline std::wstring ConvertToWideString(const char* str) {
//    if (str == nullptr) return std::wstring();
//
//    // Calculate the size of the wide string (including null terminator)
//    int count = MultiByteToWideChar(CP_ACP, 0, str, -1, nullptr, 0);
//
//    // Create a buffer of the correct size minus the null terminator
//    std::wstring wstr(count - 1, L'\0');
//
//    // Perform the conversion
//    MultiByteToWideChar(CP_ACP, 0, str, -1, &wstr[0], count);
//
//    return wstr;
//}


extern "C" __declspec(dllexport) HRESULT GetConicGradientBitmap(
    const char* binPath,
    int width,
    int height,
    float angleOffset,
    BYTE * *outBitmap,
    int* stride
)
{
    std::lock_guard<std::mutex> lock(lockMutex);
    HRESULT hr = S_OK;

    // test char* binpath

    log() << "Binary path: '" << binPath << "'\n";
    log() << "Current directory: " << std::filesystem::current_path() << "\n";

    log() << "DirectXResources::CreateDeviceResources()\n";
    DirectXResources dx = DirectXResources();
    hr = DirectXResources::CreateDeviceResources();
    if (FAILED(hr))
    {
        log() << "Failed to create device resources. Code: " << HEX(hr) << "\n";
        return hr;
    }

    /*std::wstring wBinPath = ConvertToWideString(binPath);
    log() << "wBinPath: '" << wBinPath << "'\n";
    hr = SimpleEffect::LoadCompiledShader(wBinPath);
    if (FAILED(hr)) {
        log() << "Failed to load compiled shader: " << HEX(hr) << "\n";
        return hr;
    }*/

    log () << "DirectXResources::RegisterEffects()\n";
    hr = SimpleEffect::RegisterEffect(DirectXResources::GetD2DFactory().Get());
    if (FAILED(hr))
    {
        log() << "Failed to register effects. Code: " << HEX(hr) << "\n";
        return hr;
    }

    // Get the device context
    log () << "dx.CreateLocalDeviceResources()\n";
    hr = dx.CreateLocalDeviceResources();
    if (FAILED(hr))
    {
        log() << "Failed to create local device resources. Code: " << HEX(hr) << "\n";
        return hr;
    }

    ComPtr<ID2D1DeviceContext> d2dContext = dx.GetD2DDeviceContext();

    // create the gradient pattern
    log () << "dx.CreateGradientStops()\n";
    ComPtr<ID2D1GradientStopCollection1> stopCollection;
    hr = dx.CreateGradientStops(stopCollection);
    if (FAILED(hr))
    {
        log() << "Failed to create gradient stops. Code: " << HEX(hr) << "\n";
        return hr;
    }

    // Create the ConicGradient pattern
    log() << "ConicGradientPattern\n";
    ConicGradientPattern pat = ConicGradientPattern(POINT2F{ (float)width/2.0f, (float)height/2.0f }, angleOffset, 0.0f, 0.5f, stopCollection.Get());

    // Create the Bitmaps
    DXGI_FORMAT format = DXGI_FORMAT_B8G8R8A8_UNORM;
    D2D1_ALPHA_MODE alphaMode = D2D1_ALPHA_MODE_IGNORE;
    D2D1_SIZE_U bitmapSize = { (UINT)width, (UINT)height };

    ComPtr<ID2D1Bitmap1> gpuBitmap;
    D2D1_BITMAP_PROPERTIES1 gpuProps{};
    gpuProps.dpiX = 96;
    gpuProps.dpiY = 96;
    gpuProps.pixelFormat = D2D1::PixelFormat(format, alphaMode);
    gpuProps.colorContext = nullptr;
    gpuProps.bitmapOptions = D2D1_BITMAP_OPTIONS_TARGET;
    log() << "d2dContext->CreateBitmap (gpu)\n";
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
    log() << "d2dContext->CreateBitmap (cpu)\n";
    hr = d2dContext->CreateBitmap(bitmapSize, nullptr, 0, cpuProps, cpuBitmap.ReleaseAndGetAddressOf());
    if (FAILED(hr)) {
        log() << "ID2D1DeviceContext::CreateBitmap (cpu) failure. Code: " << HEX(hr) << "\n";
        return hr;
    }

    // Create and draw the conic gradient effect
    d2dContext->SetTarget(gpuBitmap.Get());
    d2dContext->BeginDraw();
    d2dContext->Clear(D2D1::ColorF(D2D1::ColorF::Black));

    log() << "d2dContext->CreateEffect\n";
    ComPtr<ID2D1Effect> simpleEffect;
    hr = d2dContext->CreateEffect(CLSID_SimpleEffect, simpleEffect.ReleaseAndGetAddressOf());
    if (FAILED(hr) || !simpleEffect) {
        log() << "Failed to create conic gradient effect. Code: " << HEX(hr) << "\n";
        return hr;
    }

    simpleEffect->SetValue(CONIC_PROP_STOP_COLLECTION, pat.mStops.Get());
    simpleEffect->SetValue(CONIC_PROP_CENTER, D2D1::Vector2F(pat.mCenter.x, pat.mCenter.y));
    simpleEffect->SetValue(CONIC_PROP_ANGLE, pat.mAngle);

    d2dContext->DrawImage(simpleEffect.Get());

    //End draw
    log() << "d2dContext->EndDraw\n";
    hr = d2dContext->EndDraw();
    if (FAILED(hr)) {
        log() << "Failed to end draw. Code: " << HEX(hr) << "\n";
        return hr;
    }

    // Copy the bitmap to the CPU bitmap
    log() << "cpuBitmap->CopyFromBitmap\n";
    hr = cpuBitmap->CopyFromBitmap(NULL, gpuBitmap.Get(), NULL);
    if (FAILED(hr))
    {
        log() << "Failed to copy from bitmap into cpuBitmap. Code: " << HEX(hr) << "\n";
        return hr;
    }

    D2D1_MAPPED_RECT mappedRect;
    log() << "cpuBitmap->Map\n";
    hr = cpuBitmap->Map(D2D1_MAP_OPTIONS_READ, &mappedRect);
    if (FAILED(hr))
    {
        log() << "Failed to map bitmap. Code: " << HEX(hr) << "\n";
        return hr;
    }

    // return bitmap data
    int size = (int)(mappedRect.pitch * cpuBitmap->GetSize().height);
    *outBitmap = new BYTE[size];
    memcpy(*outBitmap, mappedRect.bits, size);

    *stride = mappedRect.pitch;
    //log() << "width: " << cpuBitmap->GetSize().width << "\n";
    //log() << "height: " << cpuBitmap->GetSize().height << "\n";
    //log() << "stride: " << mappedRect.pitch << "\n";

    log() << "cpuBitmap->Unmap\n";
    hr = cpuBitmap->Unmap();
    if (FAILED(hr))
    {
        log() << "Failed to unmap bitmap. Code: " << HEX(hr) << "\n";
        return hr;
    }

    // Cleanup
    log() << "Cleanup start\n";
    //log() << "cpuBitmap.Reset\n";
    cpuBitmap.Reset();
    //log() << "gpuBitmap.Reset\n";
    gpuBitmap.Reset();
    //log() << "stopCollection.Reset\n";
    stopCollection.Reset();
    //log() << "pat.mStops.Reset\n";
    pat.mStops.Reset();
    //log() << "d2dContext.Reset\n";
    d2dContext.Reset();
    //log() << "end of export method\n";

    //DirectXResources::CleanupDeviceResources();
    return hr;
}

extern "C" __declspec(dllexport) void FreeBitmap(BYTE** bitmap) {
    if (bitmap != nullptr && *bitmap != nullptr) {
        delete[] * bitmap;
        *bitmap = nullptr;
    }
    else {
        log() << "Error!\nFreeBitmap(): bitmap was nullptr\n";
    }
}


extern "C" __declspec(dllexport) void CleanupDirectXResources() {
    DirectXResources::CleanupDeviceResources();
}
