#pragma once
#include "pch.h"
#include "Macros.h"
#include <d2d1_1.h>
#include <d2d1effectauthor.h>
#include <vector>

// {BE7AB141-43EF-4599-AE99-BDD221555802}
DEFINE_GUID(CLSID_SimpleEffect, 0xbe7ab141, 0x43ef, 0x4599, 0xae, 0x99, 0xbd, 0xd2, 0x21, 0x55, 0x58, 0x2);

enum {
    CONIC_PROP_STOP_COLLECTION = 0,
    CONIC_PROP_CENTER,
    CONIC_PROP_ANGLE,
};

class SimpleEffect final
    : public ID2D1EffectImpl
    , public ID2D1DrawTransform
{
public:
    // ID2D1EffectImpl
    IFACEMETHODIMP Initialize(
        _In_ ID2D1EffectContext* pContextInternal,
        _In_ ID2D1TransformGraph* pTransformGraph);
    IFACEMETHODIMP PrepareForRender(
        D2D1_CHANGE_TYPE changeType);
    IFACEMETHODIMP SetGraph(
        ID2D1TransformGraph* pGraph);

    UINT32 GetInputCount() const;

    // ID2D1Transform
    IFACEMETHODIMP MapInputRectsToOutputRect(
        _In_reads_(inputRectCount) const D2D1_RECT_L* pInputRects,
        _In_reads_(inputRectCount) const D2D1_RECT_L* pInputOpaqueSubRects,
        UINT32 inputRectCount,
        _Out_ D2D1_RECT_L* pOutputRect,
        _Out_ D2D1_RECT_L* pOutputOpaqueSubRect);
    IFACEMETHODIMP MapOutputRectToInputRects(
        _In_ const D2D1_RECT_L* pOutputRect,
        _Out_writes_(inputRectCount) D2D1_RECT_L* pInputRects,
        UINT32 inputRectCount) const;
    IFACEMETHODIMP MapInvalidRect(
        UINT32 inputIndex,
        D2D1_RECT_L invalidInputRect,
        _Out_ D2D1_RECT_L* pInvalidOutputRect) const;

    // ID2D1TransformNode
    //IFACEMETHODIMP_(UINT32) GetInputCount() const { return 1; }

    // ID2D1DrawTransform
    IFACEMETHODIMP SetDrawInfo(_In_ ID2D1DrawInfo* pDrawInfo);

    // ID2D Factory Registration
    //static HRESULT Register(ID2D1Factory1* aFactory);
    static void Unregister(ID2D1Factory1* aFactory);

    // IUnknown - COM calls.
    IFACEMETHODIMP_(ULONG) AddRef();
    IFACEMETHODIMP_(ULONG) Release();
    IFACEMETHODIMP QueryInterface(REFIID riid, void** ppOutput);

    // COM Calls
    //static HRESULT __stdcall CreateEffect(IUnknown** aEffectImpl);
    HRESULT SetStopCollection(IUnknown* aStopCollection);
    IUnknown* GetStopCollection() const { return mStopCollection.Get(); }


    static HRESULT LoadCompiledShader();
    //static HRESULT LoadCompiledShaderFromResource();
    // Properties
    static std::vector<BYTE> m_pixelShader;

    static HRESULT RegisterEffect(_In_ ID2D1Factory1* pFactory);
private:
    ComPtr<ID2D1ResourceTexture> CreateGradientTexture();

    static HRESULT Create(_Outptr_ IUnknown** ppEffectImpl);


    SimpleEffect();

    uint32_t mRefCount;
    ComPtr<ID2D1GradientStopCollection> mStopCollection;
    ComPtr<ID2D1EffectContext> mEffectContext;
    ComPtr<ID2D1DrawInfo> mDrawInfo;
    SIMPLE_PROP(D2D1_VECTOR_2F, Center)
    SIMPLE_PROP(FLOAT, Angle)
};

const BYTE SimplePixelShader[] = {
68, 88, 66, 67, 72, 112, 215, 6, 94, 109, 74, 2, 89, 120, 214, 72, 133, 201, 113, 163, 1, 0, 0, 0, 100, 2, 0, 0, 6, 0, 0, 0, 56, 0, 0, 0, 144, 0, 0, 0, 236, 0, 0, 0, 104, 1, 0, 0, 180, 1, 0, 0, 48, 2, 0, 0, 65, 111, 110, 57, 80, 0, 0, 0, 80, 0, 0, 0, 0, 2, 255, 255, 44, 0, 0, 0, 36, 0, 0, 0, 0, 0, 36, 0, 0, 0, 36, 0, 0, 0, 36, 0, 0, 0, 36, 0, 0, 0, 36, 0, 1, 2, 255, 255, 81, 0, 0, 5, 0, 0, 15, 160, 0, 0, 128, 63, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 128, 63, 1, 0, 0, 2, 0, 8, 15, 128, 0, 0, 228, 160, 255, 255, 0, 0, 83, 72, 68, 82, 84, 0, 0, 0, 64, 0, 0, 0, 21, 0, 0, 0, 101, 0, 0, 3, 242, 32, 16, 0, 0, 0, 0, 0, 104, 0, 0, 2, 1, 0, 0, 0, 54, 0, 0, 8, 242, 0, 16, 0, 0, 0, 0, 0, 2, 64, 0, 0, 0, 0, 128, 63, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 128, 63, 54, 0, 0, 5, 242, 32, 16, 0, 0, 0, 0, 0, 70, 14, 16, 0, 0, 0, 0, 0, 62, 0, 0, 1, 83, 84, 65, 84, 116, 0, 0, 0, 3, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 82, 68, 69, 70, 68, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 28, 0, 0, 0, 0, 4, 255, 255, 4, 1, 0, 0, 28, 0, 0, 0, 77, 105, 99, 114, 111, 115, 111, 102, 116, 32, 40, 82, 41, 32, 72, 76, 83, 76, 32, 83, 104, 97, 100, 101, 114, 32, 67, 111, 109, 112, 105, 108, 101, 114, 32, 49, 48, 46, 49, 0, 73, 83, 71, 78, 116, 0, 0, 0, 3, 0, 0, 0, 8, 0, 0, 0, 80, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 15, 0, 0, 0, 92, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 1, 0, 0, 0, 15, 0, 0, 0, 107, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 2, 0, 0, 0, 15, 0, 0, 0, 83, 86, 95, 80, 79, 83, 73, 84, 73, 79, 78, 0, 83, 67, 69, 78, 69, 95, 80, 79, 83, 73, 84, 73, 79, 78, 0, 84, 69, 88, 67, 79, 79, 82, 68, 0, 79, 83, 71, 78, 44, 0, 0, 0, 1, 0, 0, 0, 8, 0, 0, 0, 32, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 15, 0, 0, 0, 83, 86, 95, 84, 97, 114, 103, 101, 116, 0, 171, 171
};
