#pragma once
#include "pch.h"
#include "Macros.h"
#include <d2d1_1.h>
#include <d2d1effectauthor.h>
#include <vector>
#include "ConicShader.h"

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

    IFACEMETHODIMP_(UINT32) GetInputCount() const;

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


    static HRESULT LoadCompiledShader(std::wstring binpath = L"");
    //static HRESULT LoadCompiledShaderFromResource();
    // Properties
    static std::vector<BYTE> m_pixelShader;

    static HRESULT RegisterEffect(_In_ ID2D1Factory1* pFactory);
private:
    ComPtr<ID2D1ResourceTexture> CreateGradientTexture();

    static HRESULT STDMETHODCALLTYPE Create(_Outptr_ IUnknown** ppEffectImpl);


    SimpleEffect();

    uint32_t mRefCount;
    ComPtr<ID2D1GradientStopCollection> mStopCollection;
    ComPtr<ID2D1EffectContext> mEffectContext;
    ComPtr<ID2D1DrawInfo> mDrawInfo;
    SIMPLE_PROP(D2D1_VECTOR_2F, Center)
    SIMPLE_PROP(FLOAT, Angle)
};
