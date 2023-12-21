#pragma once
#include "pch.h"

#include <d2d1_1.h>
#include <d2d1effectauthor.h>
#include <d2d1effecthelpers.h>

// {BAE3B462-A937-4FFF-B85E-8CEE23E2CC30}
DEFINE_GUID(CLSID_SimpleEffect, 0xbae3b462, 0xa937, 0x4fff, 0xb8, 0x5e, 0x8c, 0xee, 0x23, 0xe2, 0xcc, 0x30);


class SimpleEffect final :
    public ID2D1EffectImpl,
    public ID2D1DrawTransform
{
public:
    // ID2D1EffectImpl
    IFACEMETHODIMP Initialize(
        ID2D1EffectContext* pContextInternal,
        ID2D1TransformGraph* pTransformGraph);
    IFACEMETHODIMP PrepareForRender(
        D2D1_CHANGE_TYPE changeType);
    IFACEMETHODIMP SetGraph(
        ID2D1TransformGraph* pGraph);

    // ID2D1Transform
    IFACEMETHODIMP MapInputRectsToOutputRect(
        const D2D1_RECT_L* pInputRects,
        const D2D1_RECT_L* pInputOpaqueSubRects,
        UINT32 inputRectCount,
        D2D1_RECT_L* pOutputRect,
        D2D1_RECT_L* pOutputOpaqueSubRect);
    IFACEMETHODIMP MapOutputRectToInputRects(
        const D2D1_RECT_L* pOutputRect,
        D2D1_RECT_L* pInputRects,
        UINT32 inputRectCount) const;
    IFACEMETHODIMP MapInvalidRect(
        UINT32 inputIndex,
        D2D1_RECT_L invalidInputRect,
        D2D1_RECT_L* pInvalidOutputRect) const;

    // ID2D1TransformNode
    IFACEMETHODIMP_(UINT32) GetInputCount() const { return 1; }

    // ID2D1DrawTransform
    IFACEMETHODIMP SetDrawInfo(ID2D1DrawInfo* pDrawInfo);

    // ID2D Factory Registration
    static HRESULT Register(ID2D1Factory1* aFactory);
    static void Unregister(ID2D1Factory1* aFactory);

    // IUnknown - COM calls.
    IFACEMETHODIMP_(ULONG) AddRef();
    IFACEMETHODIMP_(ULONG) Release();
    IFACEMETHODIMP QueryInterface(REFIID riid, void** ppOutput);

    // COM Calls
    static HRESULT __stdcall CreateEffect(IUnknown** aEffectImpl);

private:
    SimpleEffect();

    uint32_t mRefCount;
    ComPtr<ID2D1ResourceTexture> mResourceTexture;
    ComPtr<ID2D1EffectContext> mEffectContext;
    ComPtr<ID2D1DrawInfo> mDrawInfo;
};
