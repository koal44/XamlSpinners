#pragma once
#include "pch.h"
#include "App.h"
#include <iostream>
#include "Factory.h"

// Entry point of the application.
int WINAPI WinMain(
    _In_ HINSTANCE hInstance,
    _In_opt_ HINSTANCE hPrevInstance,
    _In_ LPSTR lpCmdLine,
    _In_ int nCmdShow
)
{
    log() << "Starting application" << "\n";
    HeapSetInformation(NULL, HeapEnableTerminationOnCorruption, NULL, 0);

    if (SUCCEEDED(CoInitialize(NULL)))
    {
        {
            App app;
            app.Initialize();
            app.RunMessageLoop();
        }
        CoUninitialize();
    }

    return 0;
}

App::App() :
    m_hwnd(NULL),
    m_pDirect2dFactory(NULL),
    m_pRenderTarget(NULL),
    m_pLightSlateGrayBrush(NULL),
    m_pCornflowerBlueBrush(NULL),
    m_pConicGradient(NULL)
{}

App::~App()
{
    SafeRelease(&m_pDirect2dFactory);
    SafeRelease(&m_pRenderTarget);
    SafeRelease(&m_pLightSlateGrayBrush);
    SafeRelease(&m_pCornflowerBlueBrush);
    SafeRelease(&m_pConicGradient);
}

void App::RunMessageLoop()
{
    MSG msg;

    while (GetMessage(&msg, NULL, 0, 0))
    {
        TranslateMessage(&msg);
        DispatchMessage(&msg);
    }
}

void App::Initialize()
{
    // Register the window class.
    WNDCLASSEX wcex = { sizeof(WNDCLASSEX) };
    wcex.style = CS_HREDRAW | CS_VREDRAW;
    wcex.lpfnWndProc = App::WndProc;
    wcex.cbClsExtra = 0;
    wcex.cbWndExtra = sizeof(LONG_PTR);
    wcex.hInstance = HINST_THISCOMPONENT;
    wcex.hbrBackground = NULL;
    wcex.lpszMenuName = NULL;
    wcex.hCursor = LoadCursor(NULL, IDI_APPLICATION);
    wcex.lpszClassName = L"D2DDemoApp";

    RegisterClassEx(&wcex);

    m_hwnd = CreateWindow(
        L"D2DDemoApp",
        L"Direct2D demo application",
        WS_OVERLAPPEDWINDOW,
        CW_USEDEFAULT,
        CW_USEDEFAULT,
        0,
        0,
        NULL,
        NULL,
        HINST_THISCOMPONENT,
        this);

    if (m_hwnd)
    {
        // Because the SetWindowPos function takes its size in pixels, we
        // obtain the window's DPI, and use it to scale the window size.
        float dpi = static_cast<float>(GetDpiForWindow(m_hwnd));

        SetWindowPos(
            m_hwnd,
            NULL,
            NULL,
            NULL,
            static_cast<int>(ceil(640.f * dpi / 96.f)),
            static_cast<int>(ceil(480.f * dpi / 96.f)),
            SWP_NOMOVE);
        ShowWindow(m_hwnd, SW_SHOWNORMAL);
        UpdateWindow(m_hwnd);
    }
}



HRESULT App::CreateDeviceIndependentResources()
{
    HRESULT hr = S_OK;

    // Create a Direct2D factory.
    hr = D2D1CreateFactory(D2D1_FACTORY_TYPE_SINGLE_THREADED, &m_pDirect2dFactory);

    // Register the ConicGradientEffectD2D1 class with the Direct2D factory, 
    if (SUCCEEDED(hr))
    {
        hr = ConicGradientEffectD2D1::Register(m_pDirect2dFactory);
    }

    return hr;
}

HRESULT App::InitializeDirectX()
{
    HRESULT hr = S_OK;

    // Create device-dependent resources
    RECT rc;
    GetClientRect(m_hwnd, &rc);
    D2D1_SIZE_U size = D2D1::SizeU(rc.right - rc.left, rc.bottom - rc.top);

    hr = m_pDirect2dFactory->CreateHwndRenderTarget(
        D2D1::RenderTargetProperties(),
        D2D1::HwndRenderTargetProperties(m_hwnd, size),
        &m_pRenderTarget
    );
    if (FAILED(hr)) {
        log() << "Failed to create render target: " << std::hex << hr << "\n";
        return hr;
    }

    hr = m_pRenderTarget->CreateSolidColorBrush(
        D2D1::ColorF(D2D1::ColorF::LightSlateGray),
        &m_pLightSlateGrayBrush
    );
    if (FAILED(hr)) {
        log() << "Failed to create gray brush: " << std::hex << hr << "\n";
        return hr;
    }

    hr = m_pRenderTarget->CreateSolidColorBrush(
        D2D1::ColorF(D2D1::ColorF::CornflowerBlue),
        &m_pCornflowerBlueBrush
    );
    if (FAILED(hr)) {
        log() << "Failed to create blue brush: " << std::hex << hr << "\n";
        return hr;
    }

    return S_OK;
}


void App::DiscardDeviceResources()
{
    SafeRelease(&m_pRenderTarget);
    SafeRelease(&m_pLightSlateGrayBrush);
    SafeRelease(&m_pCornflowerBlueBrush);
}

LRESULT CALLBACK App::WndProc(HWND hwnd, UINT message, WPARAM wParam, LPARAM lParam)
{
    LRESULT result = 0;

    if (message == WM_CREATE)
    {
        LPCREATESTRUCT pcs = (LPCREATESTRUCT)lParam;
        App* pDemoApp = (App*)pcs->lpCreateParams;

        ::SetWindowLongPtrW(
            hwnd,
            GWLP_USERDATA,
            reinterpret_cast<LONG_PTR>(pDemoApp)
        );

        result = 1;
    }
    else
    {
        App* pDemoApp = reinterpret_cast<App*>(
            static_cast<LONG_PTR>(
                ::GetWindowLongPtrW(
                    hwnd,
                    GWLP_USERDATA)
                )
            );

        bool wasHandled = false;

        if (pDemoApp)
        {
            switch (message)
            {
            case WM_SIZE:
            {
                UINT width = LOWORD(lParam);
                UINT height = HIWORD(lParam);
                pDemoApp->OnResize(width, height);
            }
            result = 0;
            wasHandled = true;
            break;

            case WM_DISPLAYCHANGE:
            {
                InvalidateRect(hwnd, NULL, FALSE);
            }
            result = 0;
            wasHandled = true;
            break;

            case WM_PAINT:
            {
                pDemoApp->OnRender();
                ValidateRect(hwnd, NULL);
            }
            result = 0;
            wasHandled = true;
            break;

            case WM_DESTROY:
            {
                PostQuitMessage(0);
            }
            result = 1;
            wasHandled = true;
            break;
            }
        }

        if (!wasHandled)
        {
            result = DefWindowProc(hwnd, message, wParam, lParam);
        }
    }

    return result;
}

HRESULT App::OnRender()
{
    HRESULT hr = S_OK;

    Factory factory = Factory();
    //ComPtr<ID2D1Factory1> fac = factory.factory();
    factory.factory();
    factory.CreateDeviceResources();
    if (!factory.Init(D2D1_SIZE_U{ 100, 100 }, SurfaceFormat::B8G8R8A8))
    {
        log() << "Failed to initialize factory" << "\n";
        return E_FAIL;
    }

    RECT rc;
    GetClientRect(m_hwnd, &rc);
    D2D1_SIZE_U size = D2D1::SizeU(rc.right - rc.left, rc.bottom - rc.top);

    hr = factory.GetD2DFactory()->CreateHwndRenderTarget(
        D2D1::RenderTargetProperties(),
        D2D1::HwndRenderTargetProperties(m_hwnd, size),
        &m_pRenderTarget
    );
    if (FAILED(hr)) {
        log() << "Failed to create render target: " << std::hex << hr << "\n";
        return hr;
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
        log() << "Failed to create gradient stop collection" << "\n";
        return hr;
    }

    ConicGradientPattern pat = ConicGradientPattern(POINT2F{ 0.5f, 0.5f }, 0.0f, 0.0f, 0.5f, stopCollection.Get());
    factory.PrepareForDrawing(CompositionOp::OP_OVER, pat);
    factory.FinalizeDrawing(pat);

    // Begin drawing
    m_pRenderTarget->BeginDraw();
    m_pRenderTarget->SetTransform(D2D1::Matrix3x2F::Identity());
    m_pRenderTarget->Clear(D2D1::ColorF(D2D1::ColorF::Orange));

    // Draw the bitmap produced by Factory
    if (factory.GetBitmap()) {
        ID2D1Bitmap1* bitmap = factory.GetBitmap().Get();

        // Get the size of the bitmap
        D2D1_SIZE_F size = bitmap->GetSize();
        log() << "Bitmap Size: Width = " << size.width << ", Height = " << size.height << "\n";

        // Get the pixel format of the bitmap
        D2D1_PIXEL_FORMAT pixelFormat = bitmap->GetPixelFormat();
        log() << "Bitmap Pixel Format: "
            << HEX(pixelFormat.format)
            << " with alpha mode "
            << pixelFormat.alphaMode << "\n";



        ID2D1Bitmap1* compatibleBitmap = nullptr;
        D2D1_BITMAP_PROPERTIES1 bitmapProperties = {};
        bitmap->GetDpi(&bitmapProperties.dpiX, &bitmapProperties.dpiY);
        bitmapProperties.pixelFormat = bitmap->GetPixelFormat();
        bitmapProperties.bitmapOptions = D2D1_BITMAP_OPTIONS_TARGET;

        D2D1_SIZE_U bitmapSize = bitmap->GetPixelSize();
        // CreateBitmap(size, srcData, pitch, &bitmapProperties, bitmap);
        UINT32 pitch = bitmapSize.width * 4;
        //hr = m_pRenderTarget->CreateBitmap(bitmapSize, pitch, &bitmapProperties, &compatibleBitmap);

        if (FAILED(hr)) {
            log() << "Failed to create compatible bitmap: " << HEX(hr) << "\n";
            return hr;
        }

        //m_pRenderTarget->BeginDraw();
        //m_pRenderTarget->DrawBitmap(compatibleBitmap);
        //m_pRenderTarget->EndDraw();

        D2D1_RECT_F destinationRect = D2D1::RectF(0.0f, 0.0f, 10.0f, 10.0f);  // Example rectangle
        m_pRenderTarget->DrawBitmap(bitmap, D2D1::RectF(0.0f, 0.0f, 10.0f, 10.0f));
        
    }

    hr = m_pRenderTarget->CreateSolidColorBrush(
        D2D1::ColorF(D2D1::ColorF::CornflowerBlue),
        &m_pCornflowerBlueBrush
    );
    if (FAILED(hr)) {
        log() << "Failed to create blue brush: " << std::hex << hr << "\n";
        return hr;
    }

    D2D1_SIZE_F rtSize = m_pRenderTarget->GetSize();

    int width = static_cast<int>(rtSize.width);
    int height = static_cast<int>(rtSize.height);

    D2D1_RECT_F rectangle1 = D2D1::RectF(
        rtSize.width / 2 - 50.0f,
        rtSize.height / 2 - 50.0f,
        rtSize.width / 2 + 50.0f,
        rtSize.height / 2 + 50.0f
    );

    // Draw the outline of a rectangle.
    m_pRenderTarget->DrawRectangle(&rectangle1, m_pCornflowerBlueBrush);

    // End the drawing session
    hr = m_pRenderTarget->EndDraw();
    if (hr == D2DERR_RECREATE_TARGET) {
        DiscardDeviceResources();
    }

    if (FAILED(hr)) {
        log() << "Failed to end draw: " << HEX(hr) << "\n";
        return hr;
    }

    return hr;
}


HRESULT App::OnRender2()
{
    HRESULT hr = S_OK;

    // Create device-independent resources
    hr = D2D1CreateFactory(D2D1_FACTORY_TYPE_SINGLE_THREADED, &m_pDirect2dFactory);
    if (FAILED(hr)) {
        log() << "Failed to create D2D factory: " << std::hex << hr << "\n";
        return hr;
    }

    hr = ConicGradientEffectD2D1::Register(m_pDirect2dFactory);
    if (FAILED(hr)) {
        log() << "Failed to register ConicGradientEffectD2D1: " << std::hex << hr << "\n";
        return hr;
    }

    if (SUCCEEDED(hr))
    {
        m_pRenderTarget->BeginDraw();
        m_pRenderTarget->SetTransform(D2D1::Matrix3x2F::Identity());
        m_pRenderTarget->Clear(D2D1::ColorF(D2D1::ColorF::White));

        D2D1_SIZE_F rtSize = m_pRenderTarget->GetSize();

        int width = static_cast<int>(rtSize.width);
        int height = static_cast<int>(rtSize.height);

        for (int x = 0; x < width; x += 10)
        {
            m_pRenderTarget->DrawLine(
                D2D1::Point2F(static_cast<FLOAT>(x), 0.0f),
                D2D1::Point2F(static_cast<FLOAT>(x), rtSize.height),
                m_pLightSlateGrayBrush,
                0.5f
            );
        }

        for (int y = 0; y < height; y += 10)
        {
            m_pRenderTarget->DrawLine(
                D2D1::Point2F(0.0f, static_cast<FLOAT>(y)),
                D2D1::Point2F(rtSize.width, static_cast<FLOAT>(y)),
                m_pLightSlateGrayBrush,
                0.5f
            );
        }

        D2D1_RECT_F rectangle1 = D2D1::RectF(
            rtSize.width / 2 - 50.0f,
            rtSize.height / 2 - 50.0f,
            rtSize.width / 2 + 50.0f,
            rtSize.height / 2 + 50.0f
        );

        D2D1_RECT_F rectangle2 = D2D1::RectF(
            rtSize.width / 2 - 100.0f,
            rtSize.height / 2 - 100.0f,
            rtSize.width / 2 + 100.0f,
            rtSize.height / 2 + 100.0f
        );

        // Draw a filled rectangle.
        m_pRenderTarget->FillRectangle(&rectangle1, m_pLightSlateGrayBrush);

        // Draw the outline of a rectangle.
        m_pRenderTarget->DrawRectangle(&rectangle2, m_pCornflowerBlueBrush);

        hr = m_pRenderTarget->EndDraw();

        if (hr == D2DERR_RECREATE_TARGET)
        {
            hr = S_OK;
            DiscardDeviceResources();
        }
    }

    return hr;
}

void App::OnResize(UINT width, UINT height)
{
    if (m_pRenderTarget)
    {
        m_pRenderTarget->Resize(D2D1::SizeU(width, height));
    }
}
