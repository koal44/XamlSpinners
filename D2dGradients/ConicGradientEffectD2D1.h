#include <d2d1_1.h>
#include <d2d1effectauthor.h>
#include <d2d1effecthelpers.h>

#include <cstdint>

//#include "2D.h"
//#include "mozilla/Attributes.h"

 // {fa4e3246-be57-4052-8c8b-881cc3633287}
//DEFINE_GUID(CLSID_ConicGradientEffect, 0xfa4e3246, 0xbe57, 0x4052, 0x8c, 0x8b,
//    0x88, 0x1c, 0xc3, 0x63, 0x32, 0x87);

// {463B1810-1A08-4786-BEF0-97EC8E8C25C8}
DEFINE_GUID(CLSID_ConicGradientEffect,
    0x463b1810, 0x1a08, 0x4786, 0xbe, 0xf0, 0x97, 0xec, 0x8e, 0x8c, 0x25, 0xc8);


// Macro to keep our class nice and clean.
#define SIMPLE_PROP(type, name)              \
 public:                                     \
  HRESULT Set##name(type a##name) {          \
    m##name = a##name;                       \
    return S_OK;                             \
  }                                          \
  type Get##name() const { return m##name; } \
                                             \
 private:                                    \
  type m##name;

namespace mozilla {
    namespace gfx {

        enum {
            CONIC_PROP_STOP_COLLECTION = 0,
            CONIC_PROP_CENTER,
            CONIC_PROP_ANGLE,
            CONIC_PROP_START_OFFSET,
            CONIC_PROP_END_OFFSET,
            CONIC_PROP_TRANSFORM
        };

        class ConicGradientEffectD2D1 final : public ID2D1EffectImpl,
            public ID2D1DrawTransform {
        public:
            // ID2D1EffectImpl
            IFACEMETHODIMP Initialize(ID2D1EffectContext* pContextInternal,
                ID2D1TransformGraph* pTransformGraph);
            IFACEMETHODIMP PrepareForRender(D2D1_CHANGE_TYPE changeType);
            IFACEMETHODIMP SetGraph(ID2D1TransformGraph* pGraph);

            // IUnknown
            IFACEMETHODIMP_(ULONG) AddRef();
            IFACEMETHODIMP_(ULONG) Release();
            IFACEMETHODIMP QueryInterface(REFIID riid, void** ppOutput);

            // ID2D1Transform
            IFACEMETHODIMP MapInputRectsToOutputRect(
                const D2D1_RECT_L* pInputRects, const D2D1_RECT_L* pInputOpaqueSubRects,
                UINT32 inputRectCount, D2D1_RECT_L* pOutputRect,
                D2D1_RECT_L* pOutputOpaqueSubRect);
            IFACEMETHODIMP MapOutputRectToInputRects(const D2D1_RECT_L* pOutputRect,
                D2D1_RECT_L* pInputRects,
                UINT32 inputRectCount) const;
            IFACEMETHODIMP MapInvalidRect(UINT32 inputIndex, D2D1_RECT_L invalidInputRect,
                D2D1_RECT_L* pInvalidOutputRect) const;

            // ID2D1TransformNode
            IFACEMETHODIMP_(UINT32) GetInputCount() const { return 1; }

            // ID2D1DrawTransform
            IFACEMETHODIMP SetDrawInfo(ID2D1DrawInfo* pDrawInfo);

            static HRESULT Register(ID2D1Factory1* aFactory);
            static void Unregister(ID2D1Factory1* aFactory);
            static HRESULT __stdcall CreateEffect(IUnknown** aEffectImpl);

            HRESULT SetStopCollection(IUnknown* aStopCollection);
            IUnknown* GetStopCollection() const { return mStopCollection; }

        private:
            ID2D1ResourceTexture* CreateGradientTexture();

            ConicGradientEffectD2D1();

            uint32_t mRefCount;
            ID2D1GradientStopCollection* mStopCollection;
            ID2D1EffectContext* mEffectContext;
            ID2D1DrawInfo* mDrawInfo;
            SIMPLE_PROP(D2D1_VECTOR_2F, Center);
            SIMPLE_PROP(FLOAT, Angle);
            SIMPLE_PROP(FLOAT, StartOffset);
            SIMPLE_PROP(FLOAT, EndOffset);
            SIMPLE_PROP(D2D_MATRIX_3X2_F, Transform);
        };

    }  // namespace gfx
}  // namespace mozilla
#undef SIMPLE_PROP