#include "pch.h"
#include "SimpleEffect.h"
#include "macros.h"
#include <vector>

static const PCWSTR kXmlDescription =
XML(
    <?xml version='1.0'?>
    <Effect>
        <Property name='DisplayName' type='string' value='SimpleEffect'/>
        <Property name='Author' type='string' value='koal'/>
        <Property name='Category' type='string' value='Simple category'/>
        <Property name='Description' type='string' value='Simple Description'/>
    </Effect>
);

// {BAE3B462-A937-4FFF-B85E-8CEE23E2CC30}
static const GUID CLSID_SimpleEffect = { 0xbae3b462, 0xa937, 0x4fff, 0xb8, 0x5e, 0x8c, 0xee, 0x23, 0xe2, 0xcc, 0x30 };

SimpleEffect::SimpleEffect() :
    mRefCount(0),
    mResourceTexture(0),
    mDrawInfo(0),
    mEffectContext(0)
{}

IFACEMETHODIMP SimpleEffect::Initialize(
    ID2D1EffectContext* pContext,
    ID2D1TransformGraph* pTransformGraph)
{
    // Define the checkerboard pattern
    const UINT32 checkerboardSize = 2;
    std::vector<UINT32> checkerboardData(checkerboardSize * checkerboardSize);

    // Fill in the data (black and white squares)
    checkerboardData[0] = 0xFFFFFFFF; // White
    checkerboardData[1] = 0xFF000000; // Black
    checkerboardData[2] = 0xFF000000; // Black
    checkerboardData[3] = 0xFFFFFFFF; // White

    // Define the texture properties
    D2D1_RESOURCE_TEXTURE_PROPERTIES resourceTextureProperties = {};
    resourceTextureProperties.dimensions = 2;
    UINT32 dims[] = { checkerboardSize, checkerboardSize };
    //UINT32 dims[] = { checkerboardSize, 1 };
    resourceTextureProperties.extents = dims;
    resourceTextureProperties.bufferPrecision = D2D1_BUFFER_PRECISION_8BPC_UNORM;
    resourceTextureProperties.channelDepth = D2D1_CHANNEL_DEPTH_1;
    resourceTextureProperties.filter = D2D1_FILTER_MIN_MAG_MIP_POINT;

    // Calculate dataSize and strides
    UINT32 dataSize = static_cast<UINT32>(checkerboardData.size() * sizeof(UINT32));
    UINT32 strides[1] = { checkerboardSize * sizeof(UINT32) };

    // Create the resource texture
    HRESULT hr = pContext->CreateResourceTexture(
        nullptr, // resource ID
        &resourceTextureProperties,
        reinterpret_cast<const BYTE*>(checkerboardData.data()),
        strides,
        dataSize,
        mResourceTexture.ReleaseAndGetAddressOf()
    );

    if (FAILED(hr)) {
        log() << "Failed to create resource texture" << HEX(hr) << "\n";
    }

    return hr;
}

IFACEMETHODIMP SimpleEffect::PrepareForRender(
    D2D1_CHANGE_TYPE changeType)
{
    HRESULT hr = S_OK;

    if (changeType == D2D1_CHANGE_TYPE_NONE) {
        return S_OK;
    }

    // Create the effect
    hr = mDrawInfo->SetResourceTexture(1, mResourceTexture.Get());

    if (FAILED(hr)) {
        log() << "Failed to set resource texture" << HEX(hr) << "\n";
        return hr;
    }

    return hr;
}

IFACEMETHODIMP SimpleEffect::SetGraph(
    ID2D1TransformGraph* pGraph)
{
    return pGraph->SetSingleTransformNode(this);
}

IFACEMETHODIMP_(ULONG) SimpleEffect::AddRef()
{
    return ++mRefCount;
}

IFACEMETHODIMP_(ULONG) SimpleEffect::Release()
{
    if (!--mRefCount) {
        delete this;
        return 0;
    }
    return mRefCount;
}

IFACEMETHODIMP SimpleEffect::QueryInterface(
    REFIID riid,
    void** aPtr)
{
    if (!aPtr) {
        return E_POINTER;
    }

    if (riid == IID_IUnknown) {
        *aPtr = static_cast<IUnknown*>(static_cast<ID2D1EffectImpl*>(this));
    }
    else if (riid == IID_ID2D1EffectImpl) {
        *aPtr = static_cast<ID2D1EffectImpl*>(this);
    }
    else if (riid == IID_ID2D1DrawTransform) {
        *aPtr = static_cast<ID2D1DrawTransform*>(this);
    }
    else if (riid == IID_ID2D1Transform) {
        *aPtr = static_cast<ID2D1Transform*>(this);
    }
    else if (riid == IID_ID2D1TransformNode) {
        *aPtr = static_cast<ID2D1TransformNode*>(this);
    }
    else {
        return E_NOINTERFACE;
    }

    static_cast<IUnknown*>(*aPtr)->AddRef();
    return S_OK;
}

IFACEMETHODIMP SimpleEffect::MapInputRectsToOutputRect(
    const D2D1_RECT_L* pInputRects,
    const D2D1_RECT_L* pInputOpaqueSubRects,
    UINT32 inputRectCount,
    D2D1_RECT_L* pOutputRect,
    D2D1_RECT_L* pOutputOpaqueSubRect)
{
    if (inputRectCount != 1) {
        return E_INVALIDARG;
    }

    *pOutputRect = *pInputRects;
    *pOutputOpaqueSubRect = *pInputOpaqueSubRects;
    return S_OK;
}

IFACEMETHODIMP SimpleEffect::MapOutputRectToInputRects(
    const D2D1_RECT_L* pOutputRect,
    D2D1_RECT_L* pInputRects,
    UINT32 inputRectCount) const
{
    if (inputRectCount != 1) {
        return E_INVALIDARG;
    }

    *pInputRects = *pOutputRect;
    return S_OK;
}

IFACEMETHODIMP SimpleEffect::MapInvalidRect(
    UINT32 inputIndex,
    D2D1_RECT_L invalidInputRect,
    D2D1_RECT_L* pInvalidOutputRect) const
{
    ASSERT(inputIndex == 0);

    *pInvalidOutputRect = invalidInputRect;
    return S_OK;
}

IFACEMETHODIMP SimpleEffect::SetDrawInfo(
    ID2D1DrawInfo* pDrawInfo)
{
    mDrawInfo = pDrawInfo;
    return S_OK;
}

HRESULT SimpleEffect::Register(
    ID2D1Factory1* aFactory)
{
    HRESULT hr = aFactory->RegisterEffectFromString(
        CLSID_SimpleEffect,
        kXmlDescription,
        nullptr,
        0,
        CreateEffect);

    if (FAILED(hr)) {
        log() << "Failed to register SimpleEffect." << HEX(hr) << "\n";
    }
    return hr;
}

void SimpleEffect::Unregister(
    ID2D1Factory1* aFactory)
{
    aFactory->UnregisterEffect(CLSID_SimpleEffect);
}

HRESULT __stdcall SimpleEffect::CreateEffect(
    IUnknown** aEffectImpl)
{
    *aEffectImpl = static_cast<ID2D1EffectImpl*>(new SimpleEffect());
    (*aEffectImpl)->AddRef();

    return S_OK;
}

