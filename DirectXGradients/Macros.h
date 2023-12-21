#pragma once

// SIMPLE_PROP: Creates public getter and setter methods for a private field.
#define SIMPLE_PROP(type, name)                    \
    public:                                        \
        HRESULT Set##name(type a##name) {          \
            m##name = a##name;                     \
            return S_OK;                           \
        }                                          \
        type Get##name() const { return m##name; } \
                                                   \
    private:                                       \
        type m##name;

// TEXTW: Converts a string into a wide-character string.
#define TEXTW(x) L##x

// XML: Creates a single string from multiple lines of text.
#define XML(X) \
    TEXTW(#X)

// ASSERT: Outputs a debug message if the specified condition is false.
#ifndef ASSERT
#if defined( DEBUG ) || defined( _DEBUG )
#define ASSERT(b) do {if (!(b)) {OutputDebugStringA("Assert: " #b "\n");}} while(0)
#else
#define ASSERT(b)
#endif
#endif

// HEX: Converts an HRESULT into a string.
#define HEX(hr) "0x" << std::hex << hr << std::dec

// SafeAddRef: Increments the reference count of a COM object.
template<class Interface>
inline void SafeAddRef(
    Interface* pInterfaceToRef)
{
    if (pInterfaceToRef != NULL)
    {
        pInterfaceToRef->AddRef();
    }
}

// SafeRelease: Releases a COM object and sets the pointer to NULL.
template<class Interface>
inline void SafeRelease(
    Interface** ppInterfaceToRelease)
{
    if (*ppInterfaceToRelease != NULL)
    {
        (*ppInterfaceToRelease)->Release();
        (*ppInterfaceToRelease) = NULL;
    }
}

// SafeDelete: Deletes a pointer and sets the pointer to NULL.
template<class Type>
inline void SafeDelete(
    Type** ppTypeToDelete)
{
    if (*ppTypeToDelete != NULL)
    {
        delete (*ppTypeToDelete);
        (*ppTypeToDelete) = NULL;
    }
}

// SafeDeleteArray: Deletes an array and sets the pointer to NULL.
template<class Type>
inline void SafeDeleteArray(
    Type** ppTypeToDelete)
{
    if (*ppTypeToDelete != NULL)
    {
        delete[](*ppTypeToDelete);
        (*ppTypeToDelete) = NULL;
    }
}

