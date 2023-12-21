#pragma once
#include "pch.h"

#include <memory>
#include <d2d1_1.h>
#include <d3d11.h>
#include <mutex>
#include "Pattern.h"


class Factory {
    /*
     * Attempts to create and install a D2D1 device from the supplied Direct3D11
     * device. Returns true on success, or false on failure and leaves the
     * D2D1/Direct3D11 devices unset.
     */
    static bool SetDirect3D11Device(ID3D11Device* aDevice);
    static ComPtr<ID3D11Device> GetDirect3D11Device();
    static ComPtr<ID2D1Device> GetD2D1Device(uint32_t* aOutSeqNo = nullptr);
    static bool HasD2D1Device();
    static bool SupportsD2D1();
    static ComPtr<ID2D1DeviceContext> GetD2DDeviceContext();
    static void D2DCleanup();

public:
    Factory();
    static ComPtr<ID2D1Factory1> factory();

    bool Init(const D2D1_SIZE_U& aSize, SurfaceFormat aFormat);
    bool Init(ID3D11Texture2D* aTexture, SurfaceFormat aFormat);

    // formerly private
    bool PrepareForDrawing(CompositionOp aOp, const Pattern& aPattern);
    ComPtr<ID2D1GradientStopCollection1> CreateGradientStops(GradientStop* rawStops, uint32_t aNumStops, ExtendMode aExtendMode) const;
    HRESULT CreateDeviceResources();
    void FinalizeDrawing(const Pattern& aPattern);

    ComPtr<ID2D1Factory1> GetD2DFactory() const { return mFactory; }
    ComPtr<ID2D1DeviceContext> GetD2DDeviceContext2() const { return mDC; }
    ComPtr<ID2D1Bitmap1> GetBitmap() const { return mBitmap; }
    ComPtr<ID2D1Bitmap1> GetCpuBitmap() const { return mCpuBitmap; }

    ComPtr<IDXGIAdapter1> GetDXGIAdapterLocked();

private:
    static ComPtr<ID2D1Device> mD2D1Device;
    static ComPtr<ID3D11Device> mD3D11Device;
    //static ComPtr<IDXGIDevice> mDXGIDevice;
    static ComPtr<ID3D11DeviceContext> mD3D11DeviceContext;
    static ComPtr<ID2D1DeviceContext> mMTDC;
    static ComPtr<ID2D1Factory1> mFactory;
    static ComPtr<IDXGIAdapter1> mAdapter;

    mutable ComPtr<ID2D1DeviceContext> mDC;
    bool EnsureInitialized();
    void FlushTransformToDC();

    enum class InitState { Uninitialized, Success, Failure };
    InitState mInitState;
    D2D1_SIZE_U mSize;
    SurfaceFormat mFormat;
    Matrix mTransform;

    ComPtr<ID2D1Bitmap1> mBitmap;
    ComPtr<ID2D1Bitmap1> mCpuBitmap;
    ComPtr<ID2D1CommandList> mCommandList;
    ComPtr<IDXGISurface> mSurface;


    uint32_t mUsedCommandListsSincePurge;
    bool mTransformDirty : 1;

    D3D_FEATURE_LEVEL m_d3dFeatureLevel;

protected:
    friend class DrawTargetD2D1;
};
