#pragma once
#include "pch.h"
#include "SimpleEffect.h"
#include "Types.h"
#include "Matrix.h"
#include <d2d1effecthelpers.h>
#include <iostream>
#include <filesystem>

static const PCWSTR xmlDescription =
XML(
    <?xml version='1.0'?>
    <Effect>
        <Property name='DisplayName' type='string' value='SimpleEffect'/>
        <Property name='Author' type='string' value='koal'/>
        <Property name='Category' type='string' value='Pattern effects'/>
        <Property name='Description' type='string' value='This effect is used to render conic gradients.'/>
        <Inputs />
        <Property name='StopCollection' type='iunknown'>
            <Property name='DisplayName' type='string' value='Gradient stop collection'/>
        </Property>
        <Property name='Center' type='vector2'>
            <Property name='DisplayName' type='string' value='Gradient center'/>
        </Property>
        <Property name='Angle' type='vector2'>
            <Property name='DisplayName' type='string' value='Gradient angle'/>
        </Property>
    </Effect>
);

// {BE7AB141-43EF-4599-AE99-BDD221555802}
static const GUID CLSID_SimpleEffect = { 0xbe7ab141, 0x43ef, 0x4599, 0xae, 0x99, 0xbd, 0xd2, 0x21, 0x55, 0x58, 0x2 };

// {D839B853-14CC-4B19-80CF-C4647691956C}
static const GUID GUID_SimplePixeShader = { 0xd839b853, 0x14cc, 0x4b19, { 0x80, 0xcf, 0xc4, 0x64, 0x76, 0x91, 0x95, 0x6c } };

std::vector<BYTE> SimpleEffect::m_pixelShader;


//static inline std::wstring GetDllPath() {
//    std::vector<wchar_t> dllPath(MAX_PATH);
//    HMODULE hm = NULL;
//
//    if (GetModuleHandleEx(
//        GET_MODULE_HANDLE_EX_FLAG_FROM_ADDRESS | GET_MODULE_HANDLE_EX_FLAG_UNCHANGED_REFCOUNT,
//        (LPCWSTR)&GetDllPath,
//        &hm))
//    {
//        GetModuleFileNameW(hm, &dllPath[0], static_cast<DWORD>(dllPath.size()));
//    }
//
//    std::wstring path(dllPath.begin(), dllPath.end());
//    // Extract the directory
//    size_t lastBackslash = path.find_last_of(L"\\");
//    if (lastBackslash != std::wstring::npos) {
//        path = path.substr(0, lastBackslash);
//    }
//
//    return path;
//}

//static inline std::wstring Trim(const std::wstring& str) {
//    size_t first = str.find_first_not_of(L' ');
//    if (std::wstring::npos == first) {
//        return L"";
//    }
//    size_t last = str.find_last_not_of(L' ');
//    return str.substr(first, (last - first + 1));
//}


//HRESULT SimpleEffect::LoadCompiledShader(std::wstring binpath)
//{
//    //std::wstring path = GetDllPath();
//    //log() << "DLL path: " << path << "\n";
//
//    std::wstring filename = L"ConicGradient.cso";
//    //path += L"\\" + filename;
//    //log() << "Shader path: " << path << "\n";
//
//    std::ifstream file(filename, std::ios::binary | std::ios::ate);
//    if (!file)
//    {
//        log() << "Couldn't find: " << filename << "\n";
//
//        filename = binpath + L"Resources\\" + filename;
//        //filename = binpath + filename;
//
//        std::ifstream file(filename, std::ios::binary | std::ios::ate);
//        if (!file)
//        {
//            log() << "Couldn't find: " << filename << "\n";
//            return E_FAIL;
//        }
//    }
//
//
//
//    size_t fileSize = static_cast<size_t>(file.tellg());
//    log() << "File size: " << fileSize << "\n";
//
//    if (fileSize == 0) {
//        log() << "File is empty: " << filename << "\n";
//        file.close();
//        return E_FAIL;
//    }
//
//    std::vector<BYTE> buffer(fileSize);
//
//    file.seekg(0, std::ios::beg);
//    file.read(reinterpret_cast<char*>(buffer.data()), fileSize);
//
//    if (file.fail()) {
//        log() << "Failed to read the entire file: " << filename << "\n";
//        file.close();
//        return E_FAIL;
//    }
//
//    file.close();
//
//    log() << "Buffer size after reading: " << buffer.size() << "\n";
//
//    if (buffer.empty()) {
//        log() << "Buffer is empty after reading file: " << filename << "\n";
//        return E_FAIL;
//    }
//
//    m_pixelShader = buffer;
//
//    log() << "Shader file loaded successfully.\n";
//    return S_OK;
//
//    //size_t fileSize = static_cast<size_t>(file.tellg());
//    //std::vector<BYTE> buffer(fileSize);
//
//    //file.seekg(0, std::ios::beg);
//    //file.read(reinterpret_cast<char*>(buffer.data()), fileSize);
//    //file.close();
//
//    //// if buffer is empty then return error
//    //if (buffer.empty())
//    //{
//    //    log() << "Failed to read shader file: " << filename << "\n";
//    //    return E_FAIL;
//    //}
//
//
//    //m_pixelShader = buffer;
//
//    //return S_OK;
//}

//HRESULT SimpleEffect::LoadCompiledShaderFromResource()
//{
//    HMODULE hModule = GetModuleHandle(NULL);
//    HRSRC hRes = FindResource(hModule, MAKEINTRESOURCE(IDR_SHADER1), L"Shader");
//    if (!hRes) return E_FAIL;
//
//    DWORD size = SizeofResource(hModule, hRes);
//    HGLOBAL hMem = LoadResource(hModule, hRes);
//    if (!hMem) return E_FAIL;
//
//    void* pShaderResourceData = LockResource(hMem);
//    if (!pShaderResourceData) return E_FAIL;
//
//    m_pixelShader.assign((BYTE*)pShaderResourceData, (BYTE*)pShaderResourceData + size);
//
//    return S_OK;
//}


SimpleEffect::SimpleEffect() :
    mRefCount(0),
    mDrawInfo(0),
    mEffectContext(0),
    mStopCollection(0),
    mCenter(D2D1::Vector2F(0, 0)),
    mAngle(0)
{}

//IFACEMETHODIMP SimpleEffect::Initialize(
//    ID2D1EffectContext* pContextInternal,
//    ID2D1TransformGraph* pTransformGraph
//)
//{
//    HRESULT hr;
//
//    try
//    {
//        /*hr = pContextInternal->LoadPixelShader(
//            GUID_SimplePixeShader,
//            m_pixelShader.data(),
//            m_pixelShader.size()
//        );*/
//
//        hr = pContextInternal->LoadPixelShader(
//            GUID_SimplePixeShader,
//            SimplePixelShader,
//            sizeof(SimplePixelShader)
//        );
//
//        if (FAILED(hr)) {
//            return hr;
//        }
//    }
//    catch (const std::runtime_error& e)
//    {
//        log() << "Failed to load compiled shader: " << e.what() << "\n";
//        return E_FAIL;
//    }
//
//    hr = pTransformGraph->SetSingleTransformNode(this);
//    if (FAILED(hr)) {
//        return hr;
//    }
//
//    mEffectContext = pContextInternal;
//
//    return S_OK;
//}

//IFACEMETHODIMP SimpleEffect::PrepareForRender(
//    D2D1_CHANGE_TYPE changeType)
//{
//    if (changeType == D2D1_CHANGE_TYPE_NONE) {
//        return S_OK;
//    }
//
//    /*if (!mStopCollection) {
//        log() << "No stop collection set.\n";
//        return E_FAIL;
//    }*/
//
//    HRESULT hr = mDrawInfo->SetPixelShader(GUID_SimplePixeShader);
//
//    if (FAILED(hr)) {
//        log() << "Failed to set pixel shader: " << HEX(hr) << "\n";
//        return hr;
//    }
//
//    /*ComPtr<ID2D1ResourceTexture> tex = CreateGradientTexture();
//    hr = mDrawInfo->SetResourceTexture(1, tex.Get());
//
//    if (FAILED(hr)) {
//        log() << "Failed to set resource texture: " << HEX(hr) << "\n";
//        return hr;
//    }*/
//
//    /*struct PSConstantBuffer {
//        float center[2];
//        float angle;
//    };
//
//    PSConstantBuffer buffer = {
//        {mCenter.x, mCenter.y},
//        mAngle,
//    };
//
//    hr = mDrawInfo->SetPixelShaderConstantBuffer((BYTE*)&buffer, sizeof(buffer));
//
//    if (FAILED(hr)) {
//        log() << "Failed to set pixel shader constant buffer: " << HEX(hr) << "\n";
//        return hr;
//    }*/
//
//    return S_OK;
//}

//IFACEMETHODIMP SimpleEffect::SetGraph(
//    ID2D1TransformGraph* pGraph)
//{
//    return pGraph->SetSingleTransformNode(this);
//}

IFACEMETHODIMP_(ULONG) SimpleEffect::AddRef()
{
    //log() << "AddRef called. New count: " << mRefCount + 1 << "\n";
    return ++mRefCount;
}

//IFACEMETHODIMP_(ULONG) SimpleEffect::Release()
//{
//    if (!--mRefCount) {
//        delete this;
//        return 0;
//    }
//    return mRefCount;
//}

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

//IFACEMETHODIMP SimpleEffect::MapInputRectsToOutputRect(
//    const D2D1_RECT_L* pInputRects,
//    const D2D1_RECT_L* pInputOpaqueSubRects,
//    UINT32 inputRectCount,
//    D2D1_RECT_L* pOutputRect,
//    D2D1_RECT_L* pOutputOpaqueSubRect)
//{
//    if (inputRectCount != 1) {
//        return E_INVALIDARG;
//    }
//
//    *pOutputRect = *pInputRects;
//    *pOutputOpaqueSubRect = *pInputOpaqueSubRects;
//    return S_OK;
//}

//IFACEMETHODIMP SimpleEffect::MapOutputRectToInputRects(
//    const D2D1_RECT_L* pOutputRect,
//    D2D1_RECT_L* pInputRects,
//    UINT32 inputRectCount) const
//{
//    if (inputRectCount != 1) {
//        return E_INVALIDARG;
//    }
//
//    *pInputRects = *pOutputRect;
//    return S_OK;
//}

//IFACEMETHODIMP SimpleEffect::MapInvalidRect(
//    UINT32 inputIndex,
//    D2D1_RECT_L invalidInputRect,
//    D2D1_RECT_L* pInvalidOutputRect) const
//{
//    ASSERT(inputIndex == 0);
//
//    *pInvalidOutputRect = invalidInputRect;
//    return S_OK;
//}

//IFACEMETHODIMP SimpleEffect::SetDrawInfo(
//    ID2D1DrawInfo* pDrawInfo)
//{
//    mDrawInfo = pDrawInfo;
//    return S_OK;
//}

//HRESULT SimpleEffect::Register(
//    ID2D1Factory1* aFactory)
//{
//    //D2D1_PROPERTY_BINDING bindings[] = {
//    //    D2D1_VALUE_TYPE_BINDING(
//    //        L"StopCollection",
//    //        &SimpleEffect::SetStopCollection,
//    //        &SimpleEffect::GetStopCollection),
//    //    D2D1_VALUE_TYPE_BINDING(
//    //        L"Center",
//    //        &SimpleEffect::SetCenter,
//    //        &SimpleEffect::GetCenter),
//    //    D2D1_VALUE_TYPE_BINDING(
//    //        L"Angle", &SimpleEffect::SetAngle,
//    //        &SimpleEffect::GetAngle),
//    //};
//    HRESULT hr = aFactory->RegisterEffectFromString(
//        CLSID_SimpleEffect,
//        kXmlDescription,
//        nullptr,
//        0,
//        //bindings,
//        //ARRAYSIZE(bindings),
//        CreateEffect);
//
//    if (FAILED(hr)) {
//        log() << "Failed to register conic gradient effect." << HEX(hr) << "\n";
//    }
//    return hr;
//}

void SimpleEffect::Unregister(
    ID2D1Factory1* aFactory)
{
    aFactory->UnregisterEffect(CLSID_SimpleEffect);
}

//HRESULT __stdcall SimpleEffect::CreateEffect(
//    IUnknown** aEffectImpl)
//{
//    *aEffectImpl = static_cast<ID2D1EffectImpl*>(new SimpleEffect());
//    (*aEffectImpl)->AddRef();
//
//    return S_OK;
//}

HRESULT SimpleEffect::SetStopCollection(
    IUnknown* aStopCollection)
{
    if (SUCCEEDED(aStopCollection->QueryInterface(
        (ID2D1GradientStopCollection**)mStopCollection.ReleaseAndGetAddressOf()))) {
        return S_OK;
    }

    return E_INVALIDARG;
}


ComPtr<ID2D1ResourceTexture> SimpleEffect::CreateGradientTexture()
{
    std::vector<D2D1_GRADIENT_STOP> rawStops;
    rawStops.resize(mStopCollection->GetGradientStopCount());
    mStopCollection->GetGradientStops(&rawStops.front(), (UINT)rawStops.size());

    std::vector<unsigned char> textureData(4096 * 4);
    unsigned char* texData = &textureData.front();

    float prevColorPos = 0;
    float nextColorPos = rawStops[0].position;
    D2D1_COLOR_F prevColor = rawStops[0].color;
    D2D1_COLOR_F nextColor = prevColor;
    uint32_t stopPosition = 1;

    for (int i = 0; i < 4096; i++) {
        float pos = float(i) / 4095;

        while (pos > nextColorPos) {
            prevColor = nextColor;
            prevColorPos = nextColorPos;
            if (rawStops.size() > stopPosition) {
                nextColor = rawStops[stopPosition].color;
                nextColorPos = rawStops[stopPosition++].position;
            }
            else {
                nextColorPos = 1.0f;
            }
        }

        float interp;

        if (nextColorPos != prevColorPos) {
            interp = (pos - prevColorPos) / (nextColorPos - prevColorPos);
        }
        else {
            interp = 0;
        }

        DeviceColor newColor(
            prevColor.r + (nextColor.r - prevColor.r) * interp,
            prevColor.g + (nextColor.g - prevColor.g) * interp,
            prevColor.b + (nextColor.b - prevColor.b) * interp,
            prevColor.a + (nextColor.a - prevColor.a) * interp);

        // Note D2D expects RGBA here!!
        texData[i * 4 + 0] = (unsigned char)(255.0f * newColor.r);
        texData[i * 4 + 1] = (unsigned char)(255.0f * newColor.g);
        texData[i * 4 + 2] = (unsigned char)(255.0f * newColor.b);
        texData[i * 4 + 3] = (unsigned char)(255.0f * newColor.a);
    }


    UINT32 width = 4096;
    UINT32 stride = 4096 * 4;
    D2D1_RESOURCE_TEXTURE_PROPERTIES props{};
    // Older shader models do not support 1D textures. So just use a width x 1 texture.
    props.dimensions = 2;
    UINT32 dims[] = { width, 1 };
    props.extents = dims;
    props.channelDepth = D2D1_CHANNEL_DEPTH_4;
    props.bufferPrecision = D2D1_BUFFER_PRECISION_8BPC_UNORM;
    props.filter = D2D1_FILTER_MIN_MAG_MIP_LINEAR;
    D2D1_EXTEND_MODE extendMode[] = {
        mStopCollection->GetExtendMode(),
        mStopCollection->GetExtendMode()
    };
    props.extendModes = extendMode;

    ComPtr<ID2D1ResourceTexture> tex;
    HRESULT hr = mEffectContext->CreateResourceTexture(
        nullptr,
        &props,
        &textureData.front(),
        &stride,
        4096 * 4,
        tex.ReleaseAndGetAddressOf()
    );
    if (FAILED(hr)) {
        log() << "Failed to create resource texture: " << HEX(hr) << "\n";
    }

    return tex;
}

HRESULT SimpleEffect::RegisterEffect(_In_ ID2D1Factory1* pFactory)
{
    HRESULT hr = S_OK;

    D2D1_PROPERTY_BINDING bindings[] = {
        D2D1_VALUE_TYPE_BINDING(
            L"StopCollection",
            &SimpleEffect::SetStopCollection,
            &SimpleEffect::GetStopCollection),
        D2D1_VALUE_TYPE_BINDING(
            L"Center",
            &SimpleEffect::SetCenter,
            &SimpleEffect::GetCenter),
        D2D1_VALUE_TYPE_BINDING(
            L"Angle",
            &SimpleEffect::SetAngle,
            &SimpleEffect::GetAngle)
    };

    hr = pFactory->RegisterEffectFromString(
        CLSID_SimpleEffect,
        xmlDescription,
        bindings,
        ARRAYSIZE(bindings),
        SimpleEffect::Create
    );

    return hr;
            
}

//HRESULT __stdcall
HRESULT STDMETHODCALLTYPE SimpleEffect::Create(_Outptr_ IUnknown** ppEffectImpl)
{
    *ppEffectImpl = static_cast<ID2D1EffectImpl*>(new SimpleEffect());
    if (!*ppEffectImpl) return E_OUTOFMEMORY;
    (*ppEffectImpl)->AddRef();

    return S_OK;
}


HRESULT SimpleEffect::Initialize(
    _In_ ID2D1EffectContext* pEffectContext,
    _In_ ID2D1TransformGraph* pTransformGraph
)
{
    mEffectContext = pEffectContext;

    HRESULT hr = S_OK;

    // Load the compiled pixel shader, and associate it with GUID 
    /*hr = pEffectContext->LoadPixelShader(
        GUID_SimplePixeShader,
        m_pixelShader.data(),
        (UINT)m_pixelShader.size());*/

    hr = pEffectContext->LoadPixelShader(
        GUID_SimplePixeShader,
        ConicGradientPS,
        sizeof(ConicGradientPS));
    if (FAILED(hr)) {
        log() << "Failed to load pixel shader: " << HEX(hr) << "\n";
        return hr;
    }

    hr = pTransformGraph->SetSingleTransformNode(this);
    if (FAILED(hr)) {
        log() << "Failed to set single transform node: " << HEX(hr) << "\n";
        return hr;
    }

    return hr;
}

HRESULT SimpleEffect::PrepareForRender(D2D1_CHANGE_TYPE changeType)
{
    HRESULT hr = S_OK;

    if (!mStopCollection) {
        log() << "No stop collection set.\n";
        return E_FAIL;
    }

   /* hr = mDrawInfo->SetPixelShader(GUID_SimplePixeShader);
    if (FAILED(hr)) {
        log() << "Failed to set pixel shader: " << HEX(hr) << "\n";
        return hr;
    }*/

    ComPtr<ID2D1ResourceTexture> tex = CreateGradientTexture();
    hr = mDrawInfo->SetResourceTexture(0, tex.Get());

    if (FAILED(hr)) {
        log() << "Failed to set resource texture: " << HEX(hr) << "\n";
        return hr;
    }

    struct PSConstantBuffer {
        float center[2];
        float angle;
    };

    PSConstantBuffer buffer = {
        {mCenter.x, mCenter.y},
        mAngle,
    };

    hr = mDrawInfo->SetPixelShaderConstantBuffer((BYTE*)&buffer, sizeof(buffer));

    if (FAILED(hr)) {
        log() << "Failed to set pixel shader constant buffer: " << HEX(hr) << "\n";
        return hr;
    }

    return hr;
}

IFACEMETHODIMP SimpleEffect::SetGraph(
    ID2D1TransformGraph* pGraph)
{
    // Not implemented

    HRESULT hr = pGraph->SetSingleTransformNode(this);
    if (FAILED(hr)) {
        log() << "Failed to set single transform node: " << HEX(hr) << "\n";
        return hr;
    }
    return hr;
}

// ID2D1TransformNode method
IFACEMETHODIMP_(UINT32) SimpleEffect::GetInputCount() const
{
    // No input bitmaps, so...
    return 0;
}

// ID2D1Transform methods.
HRESULT SimpleEffect::MapOutputRectToInputRects(
    _In_ const D2D1_RECT_L* pOutputRect,
    _Out_writes_(inputRectCount) D2D1_RECT_L* pInputRects,
    UINT32 inputRectCount
) const
{
    if (inputRectCount != 0)
    {
        return E_INVALIDARG;
    }
    return S_OK;
}

HRESULT SimpleEffect::MapInputRectsToOutputRect(
    _In_reads_(inputRectCount) CONST D2D1_RECT_L* pInputRects,
    _In_reads_(inputRectCount) CONST D2D1_RECT_L* pInputOpaqueSubRects,
    UINT32 inputRectCount,
    _Out_ D2D1_RECT_L* pOutputRect,
    _Out_ D2D1_RECT_L* pOutputOpaqueSubRect
)
{
    if (inputRectCount != 0)
    {
        return E_INVALIDARG;
    }
    pOutputRect[0] = D2D1::RectL(LONG_MIN, LONG_MIN, LONG_MAX, LONG_MAX);
    pOutputOpaqueSubRect[0] = D2D1::RectL();
    return S_OK;
}

IFACEMETHODIMP SimpleEffect::MapInvalidRect(
    UINT32 inputIndex,
    D2D1_RECT_L invalidInputRect,
    _Out_ D2D1_RECT_L* pInvalidOutputRect) const
{
    ASSERT(inputIndex == 0);

    *pInvalidOutputRect = invalidInputRect;
    return S_OK;
}

// ID2D1DrawTransform method
HRESULT SimpleEffect::SetDrawInfo(_In_ ID2D1DrawInfo* pDrawInfo)
{
    mDrawInfo = pDrawInfo;

    HRESULT hr = S_OK;

    // Set pixel shader
    hr = pDrawInfo->SetPixelShader(GUID_SimplePixeShader);
    if (FAILED(hr)) {
        log() << "Failed to set pixel shader: " << HEX(hr) << "\n";
        return hr;
    }

    return hr;
}

ULONG STDMETHODCALLTYPE SimpleEffect::Release()
{
    mRefCount--;
    //log() << "Release called. New count: " << mRefCount << "\n";
    LONG newCount = mRefCount;

    if (mRefCount == 0) {
        //log() << "Reference count reached zero. Object should be deleted here.\n";
        delete this;
    }

    return newCount;
}
