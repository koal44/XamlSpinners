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

// HINST_THISCOMPONENT: Gets the instance handle of the current module.
#ifndef HINST_THISCOMPONENT
    EXTERN_C IMAGE_DOS_HEADER __ImageBase;
    #define HINST_THISCOMPONENT ((HINSTANCE)&__ImageBase)
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




#define MOZ_ARG_1(a1, ...) a1
#define MOZ_ARG_2(a1, a2, ...) a2
#define MOZ_ARG_3(a1, a2, a3, ...) a3
#define MOZ_ARG_4(a1, a2, a3, a4, ...) a4
#define MOZ_ARG_5(a1, a2, a3, a4, a5, ...) a5
#define MOZ_ARG_6(a1, a2, a3, a4, a5, a6, ...) a6
#define MOZ_ARG_7(a1, a2, a3, a4, a5, a6, a7, ...) a7
#define MOZ_ARG_8(a1, a2, a3, a4, a5, a6, a7, a8, ...) a8
#define MOZ_ARG_9(a1, a2, a3, a4, a5, a6, a7, a8, a9, ...) a9

#define MOZ_MACROARGS_ARG_COUNT_HELPER3(                                       \
    a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16, \
    a17, a18, a19, a20, a21, a22, a23, a24, a25, a26, a27, a28, a29, a30, a31, \
    a32, a33, a34, a35, a36, a37, a38, a39, a40, a41, a42, a43, a44, a45, a46, \
    a47, a48, a49, a50, a51, ...)                                              \
  a51

#define MOZ_MACROARGS_ARG_COUNT_HELPER2(aArgs) \
  MOZ_MACROARGS_ARG_COUNT_HELPER3 aArgs

#define MOZ_MACROARGS_ARG_COUNT_HELPER(...)                                    \
  (_, ##__VA_ARGS__, 50, 49, 48, 47, 46, 45, 44, 43, 42, 41, 40, 39, 38, 37,   \
   36, 35, 34, 33, 32, 31, 30, 29, 28, 27, 26, 25, 24, 23, 22, 21, 20, 19, 18, \
   17, 16, 15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0)

#define MOZ_ARG_COUNT(...) \
  MOZ_MACROARGS_ARG_COUNT_HELPER2(MOZ_MACROARGS_ARG_COUNT_HELPER(__VA_ARGS__))

#define MOZ_CONCAT2(x, y) x##y
#define MOZ_CONCAT(x, y) MOZ_CONCAT2(x, y)

#define MOZ_PASTE_PREFIX_AND_ARG_COUNT_GLUE(a, b) a b

#define MOZ_PASTE_PREFIX_AND_ARG_COUNT(aPrefix, ...) \
MOZ_PASTE_PREFIX_AND_ARG_COUNT_GLUE(MOZ_CONCAT, \
    (aPrefix, MOZ_ARG_COUNT(__VA_ARGS__)))

#define MOZ_FOR_EACH_EXPAND_HELPER(...) __VA_ARGS__

#define MOZ_FOR_EACH_HELPER_GLUE(a, b) a b

#define MOZ_FOR_EACH_HELPER(aMacro, aFixedArgs, aArgs) \
MOZ_FOR_EACH_HELPER_GLUE(\
    aMacro, (MOZ_FOR_EACH_EXPAND_HELPER aFixedArgs MOZ_ARG_1 aArgs))

#define MOZ_ARGS_AFTER_1(a1, ...) __VA_ARGS__
#define MOZ_ARGS_AFTER_2(a1, a2, ...) __VA_ARGS__

#define MOZ_FOR_EACH_0(m, s, fa, a)
#define MOZ_FOR_EACH_1(m, s, fa, a) MOZ_FOR_EACH_HELPER(m, fa, a)
#define MOZ_FOR_EACH_2(m, s, fa, a) \
  MOZ_FOR_EACH_HELPER(m, fa, a)     \
  MOZ_FOR_EACH_EXPAND_HELPER s MOZ_FOR_EACH_1(m, s, fa, (MOZ_ARGS_AFTER_1 a))
#define MOZ_FOR_EACH_3(m, s, fa, a) \
  MOZ_FOR_EACH_HELPER(m, fa, a)     \
  MOZ_FOR_EACH_EXPAND_HELPER s MOZ_FOR_EACH_2(m, s, fa, (MOZ_ARGS_AFTER_1 a))
#define MOZ_FOR_EACH_4(m, s, fa, a) \
  MOZ_FOR_EACH_HELPER(m, fa, a)     \
  MOZ_FOR_EACH_EXPAND_HELPER s MOZ_FOR_EACH_3(m, s, fa, (MOZ_ARGS_AFTER_1 a))
#define MOZ_FOR_EACH_5(m, s, fa, a) \
  MOZ_FOR_EACH_HELPER(m, fa, a)     \
  MOZ_FOR_EACH_EXPAND_HELPER s MOZ_FOR_EACH_4(m, s, fa, (MOZ_ARGS_AFTER_1 a))
#define MOZ_FOR_EACH_6(m, s, fa, a) \
  MOZ_FOR_EACH_HELPER(m, fa, a)     \
  MOZ_FOR_EACH_EXPAND_HELPER s MOZ_FOR_EACH_5(m, s, fa, (MOZ_ARGS_AFTER_1 a))
#define MOZ_FOR_EACH_7(m, s, fa, a) \
  MOZ_FOR_EACH_HELPER(m, fa, a)     \
  MOZ_FOR_EACH_EXPAND_HELPER s MOZ_FOR_EACH_6(m, s, fa, (MOZ_ARGS_AFTER_1 a))
#define MOZ_FOR_EACH_8(m, s, fa, a) \
  MOZ_FOR_EACH_HELPER(m, fa, a)     \
  MOZ_FOR_EACH_EXPAND_HELPER s MOZ_FOR_EACH_7(m, s, fa, (MOZ_ARGS_AFTER_1 a))
#define MOZ_FOR_EACH_9(m, s, fa, a) \
  MOZ_FOR_EACH_HELPER(m, fa, a)     \
  MOZ_FOR_EACH_EXPAND_HELPER s MOZ_FOR_EACH_8(m, s, fa, (MOZ_ARGS_AFTER_1 a))
#define MOZ_FOR_EACH_10(m, s, fa, a) \
  MOZ_FOR_EACH_HELPER(m, fa, a)      \
  MOZ_FOR_EACH_EXPAND_HELPER s MOZ_FOR_EACH_9(m, s, fa, (MOZ_ARGS_AFTER_1 a))
#define MOZ_FOR_EACH_11(m, s, fa, a) \
  MOZ_FOR_EACH_HELPER(m, fa, a)      \
  MOZ_FOR_EACH_EXPAND_HELPER s MOZ_FOR_EACH_10(m, s, fa, (MOZ_ARGS_AFTER_1 a))
#define MOZ_FOR_EACH_12(m, s, fa, a) \
  MOZ_FOR_EACH_HELPER(m, fa, a)      \
  MOZ_FOR_EACH_EXPAND_HELPER s MOZ_FOR_EACH_11(m, s, fa, (MOZ_ARGS_AFTER_1 a))
#define MOZ_FOR_EACH_13(m, s, fa, a) \
  MOZ_FOR_EACH_HELPER(m, fa, a)      \
  MOZ_FOR_EACH_EXPAND_HELPER s MOZ_FOR_EACH_12(m, s, fa, (MOZ_ARGS_AFTER_1 a))
#define MOZ_FOR_EACH_14(m, s, fa, a) \
  MOZ_FOR_EACH_HELPER(m, fa, a)      \
  MOZ_FOR_EACH_EXPAND_HELPER s MOZ_FOR_EACH_13(m, s, fa, (MOZ_ARGS_AFTER_1 a))

#define MOZ_FOR_EACH_GLUE(a, b) a b

#define MOZ_FOR_EACH_SEPARATED(aMacro, aSeparator, aFixedArgs, aArgs)     \
  MOZ_FOR_EACH_GLUE(MOZ_PASTE_PREFIX_AND_ARG_COUNT(                       \
                        MOZ_FOR_EACH_, MOZ_FOR_EACH_EXPAND_HELPER aArgs), \
                    (aMacro, aSeparator, aFixedArgs, aArgs))

#define MOZ_FOR_EACH(aMacro, aFixedArgs, aArgs) \
  MOZ_FOR_EACH_SEPARATED(aMacro, (), aFixedArgs, aArgs)

#define MOZ_ASSERT_ENUMERATOR_HAS_NO_INITIALIZER(aEnumName, aEnumeratorDecl) \
  static_assert(                                                             \
      int(aEnumName::aEnumeratorDecl) <=                                     \
          (int(aEnumName::aEnumeratorDecl) | 1),                             \
      "MOZ_DEFINE_ENUM does not allow enumerators to have initializers");

#define MOZ_UNWRAP_ARGS(...) __VA_ARGS__

#define MOZ_DEFINE_ENUM_IMPL(aEnumName, aClassSpec, aBaseSpec, aEnumerators) \
  enum aClassSpec aEnumName aBaseSpec{MOZ_UNWRAP_ARGS aEnumerators};         \
  constexpr size_t k##aEnumName##Count = MOZ_ARG_COUNT aEnumerators;         \
  constexpr aEnumName kHighest##aEnumName =                                  \
      aEnumName(k##aEnumName##Count - 1);                                    \
  MOZ_FOR_EACH(MOZ_ASSERT_ENUMERATOR_HAS_NO_INITIALIZER, (aEnumName, ),      \
               aEnumerators)

#define MOZ_DEFINE_ENUM_CLASS_WITH_BASE(aEnumName, aBaseName, aEnumerators) \
  MOZ_DEFINE_ENUM_IMPL(aEnumName, class, : aBaseName, aEnumerators)
