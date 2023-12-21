#pragma once
#include <d3d11.h>
#include <d2d1_1.h>
//#include "ConicGradientEffect.h"
#include "SimpleEffect.h"


class DirectXResources
{
    public:
    DirectXResources();
    ~DirectXResources();

    static ComPtr<IDXGIFactory1> GetDXGIFactory() { return m_dxgiFactory; }
    static ComPtr<IDXGIAdapter1> GetDXGIAdapter() { return m_dxgiAdapter; }
    static ComPtr<ID3D11Device> GetD3DDevice() { return m_d3dDevice; }
    static ComPtr<ID3D11DeviceContext> GetD3DDeviceContext() { return m_d3dContext; }
    static ComPtr<IDXGIDevice> GetDXGIDevice() { return m_dxgiDevice; }
    static ComPtr<ID2D1Factory1> GetD2DFactory() { return m_d2dFactory; }
    static ComPtr<ID2D1Device> GetD2DDevice() { return m_d2dDevice; }
    static D3D_FEATURE_LEVEL m_featureLevel;

    static void CleanupDeviceResources() {
        if (m_d2dFactory) {
            m_d2dFactory->UnregisterEffect(CLSID_SimpleEffect);
            m_d2dFactory.Reset();
        }
        if (m_dxgiFactory) {
            m_dxgiFactory.Reset();
        }
        if (m_dxgiAdapter) {
            m_dxgiAdapter.Reset();
        }
        if (m_d3dDevice) {
            m_d3dDevice.Reset();
        }
        if (m_d3dContext) {
            m_d3dContext.Reset();
        }
        if (m_dxgiDevice) {
            m_dxgiDevice.Reset();
        }
        if (m_d2dDevice) {
            m_d2dDevice.Reset();
        }
    }

    static HRESULT CreateDeviceResources();
    static HRESULT GetMostPowerfulAdapter();
    //static HRESULT RegisterEffects();

    // instance methods
    ComPtr<ID2D1DeviceContext> GetD2DDeviceContext() { return m_d2dContext; }

    HRESULT CreateGradientStops(ComPtr<ID2D1GradientStopCollection1>& outStopCollection);
    HRESULT CreateLocalDeviceResources();

private:
    class AutoCleanup {
    public:
        ~AutoCleanup() {
            DirectXResources::CleanupDeviceResources();
        }
    };
    // Static member declarations
    static ComPtr<IDXGIFactory1> m_dxgiFactory;
    static ComPtr<IDXGIAdapter1> m_dxgiAdapter;
    static ComPtr<ID3D11Device> m_d3dDevice;
    static ComPtr<ID3D11DeviceContext> m_d3dContext;
    static ComPtr<IDXGIDevice> m_dxgiDevice;
    static ComPtr<ID2D1Factory1> m_d2dFactory;
    static ComPtr<ID2D1Device> m_d2dDevice;
    static AutoCleanup sAutoCleanup; // needs to be last static member


    // instance member declarations
    ComPtr<ID2D1DeviceContext> m_d2dContext;

};


