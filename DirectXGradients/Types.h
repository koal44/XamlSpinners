#pragma once

#include "pch.h"
//#include <ostream>
//#include <d3d11.h>
#include <d2d1_1.h>

typedef D2D1_POINT_2F POINT2F;

enum class SurfaceFormat : int8_t {
    // The following values are named to reflect layout of colors in memory, from
    // lowest byte to highest byte. The 32-bit value layout depends on machine
    // endianness.
    //               in-memory            32-bit LE value   32-bit BE value
    B8G8R8A8,  // [BB, GG, RR, AA]     0xAARRGGBB        0xBBGGRRAA
    B8G8R8X8,  // [BB, GG, RR, 00]     0x00RRGGBB        0xBBGGRR00
    R8G8B8A8,  // [RR, GG, BB, AA]     0xAABBGGRR        0xRRGGBBAA
    R8G8B8X8,  // [RR, GG, BB, 00]     0x00BBGGRR        0xRRGGBB00
    A8R8G8B8,  // [AA, RR, GG, BB]     0xBBGGRRAA        0xAARRGGBB
    X8R8G8B8,  // [00, RR, GG, BB]     0xBBGGRR00        0x00RRGGBB

    R8G8B8,
    B8G8R8,
    HSV,
    Lab,

    UNKNOWN,
};

static inline DXGI_FORMAT DXGIFormat(SurfaceFormat aFormat) {
    switch (aFormat) {
    case SurfaceFormat::B8G8R8A8:
        return DXGI_FORMAT_B8G8R8A8_UNORM;
    case SurfaceFormat::B8G8R8X8:
        return DXGI_FORMAT_B8G8R8A8_UNORM;
    default:
        return DXGI_FORMAT_UNKNOWN;
    }
}

static inline D2D1_ALPHA_MODE D2DAlphaModeForFormat(SurfaceFormat aFormat) {
    switch (aFormat) {
    case SurfaceFormat::B8G8R8X8:
        return D2D1_ALPHA_MODE_IGNORE;
    default:
        return D2D1_ALPHA_MODE_PREMULTIPLIED;
    }
}

static inline D2D1_PIXEL_FORMAT D2DPixelFormat(SurfaceFormat aFormat) {
    return D2D1::PixelFormat(DXGIFormat(aFormat), D2DAlphaModeForFormat(aFormat));
}

/* Color is stored in non-premultiplied form in device color space */
struct DeviceColor {
public:
    DeviceColor() : r(0.0f), g(0.0f), b(0.0f), a(0.0f) {}
    DeviceColor(FLOAT aR, FLOAT aG, FLOAT aB, FLOAT aA)
        : r(aR), g(aG), b(aB), a(aA) {}
    DeviceColor(FLOAT aR, FLOAT aG, FLOAT aB) : r(aR), g(aG), b(aB), a(1.0f) {}

    /* The following Mask* variants are helpers used to make it clear when a
     * particular color is being used for masking purposes. These masks should
     * never be colored managed. */
    static DeviceColor Mask(float aC, float aA) {
        return DeviceColor(aC, aC, aC, aA);
    }

    static DeviceColor MaskWhite(float aA) { return Mask(1.f, aA); }

    static DeviceColor MaskBlack(float aA) { return Mask(0.f, aA); }

    static DeviceColor MaskOpaqueWhite() { return MaskWhite(1.f); }

    static DeviceColor MaskOpaqueBlack() { return MaskBlack(1.f); }

    static DeviceColor FromU8(uint8_t aR, uint8_t aG, uint8_t aB, uint8_t aA) {
        return DeviceColor(float(aR) / 255.f, float(aG) / 255.f, float(aB) / 255.f,
            float(aA) / 255.f);
    }

    static DeviceColor FromABGR(uint32_t aColor) {
        DeviceColor newColor(((aColor >> 0) & 0xff) * (1.0f / 255.0f),
            ((aColor >> 8) & 0xff) * (1.0f / 255.0f),
            ((aColor >> 16) & 0xff) * (1.0f / 255.0f),
            ((aColor >> 24) & 0xff) * (1.0f / 255.0f));

        return newColor;
    }

    // The "Unusual" prefix is to avoid unintentionally using this function when
    // FromABGR(), which is much more common, is needed.
    static DeviceColor UnusualFromARGB(uint32_t aColor) {
        DeviceColor newColor(((aColor >> 16) & 0xff) * (1.0f / 255.0f),
            ((aColor >> 8) & 0xff) * (1.0f / 255.0f),
            ((aColor >> 0) & 0xff) * (1.0f / 255.0f),
            ((aColor >> 24) & 0xff) * (1.0f / 255.0f));

        return newColor;
    }

    uint32_t ToABGR() const {
        return uint32_t(r * 255.0f) | uint32_t(g * 255.0f) << 8 |
            uint32_t(b * 255.0f) << 16 | uint32_t(a * 255.0f) << 24;
    }

    // The "Unusual" prefix is to avoid unintentionally using this function when
    // ToABGR(), which is much more common, is needed.
    uint32_t UnusualToARGB() const {
        return uint32_t(b * 255.0f) | uint32_t(g * 255.0f) << 8 |
            uint32_t(r * 255.0f) << 16 | uint32_t(a * 255.0f) << 24;
    }

    bool operator==(const DeviceColor& aColor) const {
        return r == aColor.r && g == aColor.g && b == aColor.b && a == aColor.a;
    }

    bool operator!=(const DeviceColor& aColor) const {
        return !(*this == aColor);
    }

    friend std::ostream& operator<<(std::ostream& aOut, const DeviceColor& aColor) {
        aOut << "DeviceColor(" << aColor.r << ", " << aColor.g << ", " << aColor.b
            << ", " << aColor.a << ")";
        return aOut;
    }

    FLOAT r, g, b, a;
};

static inline D2D1_COLOR_F D2DColor(const DeviceColor& aColor) {
    return D2D1::ColorF(aColor.r, aColor.g, aColor.b, aColor.a);
}

struct GradientStop {
    bool operator<(const GradientStop& aOther) const {
        return offset < aOther.offset;
    }

    FLOAT offset = 0.0f;
    DeviceColor color;
};

enum class CompositionOp : int8_t {
    OP_CLEAR,
    OP_OVER,
    OP_ADD,
    OP_ATOP,
    OP_OUT,
    OP_IN,
    OP_SOURCE,
    OP_DEST_IN,
    OP_DEST_OUT,
    OP_DEST_OVER,
    OP_DEST_ATOP,
    OP_XOR,
    OP_MULTIPLY,
    OP_SCREEN,
    OP_OVERLAY,
    OP_DARKEN,
    OP_LIGHTEN,
    OP_COLOR_DODGE,
    OP_COLOR_BURN,
    OP_HARD_LIGHT,
    OP_SOFT_LIGHT,
    OP_DIFFERENCE,
    OP_EXCLUSION,
    OP_HUE,
    OP_SATURATION,
    OP_COLOR,
    OP_LUMINOSITY,
    OP_COUNT
};

static inline D2D1_COMPOSITE_MODE D2DCompositionMode(CompositionOp aOp) {
    switch (aOp) {
    case CompositionOp::OP_OVER:
        return D2D1_COMPOSITE_MODE_SOURCE_OVER;
    case CompositionOp::OP_ADD:
        return D2D1_COMPOSITE_MODE_PLUS;
    case CompositionOp::OP_ATOP:
        return D2D1_COMPOSITE_MODE_SOURCE_ATOP;
    case CompositionOp::OP_OUT:
        return D2D1_COMPOSITE_MODE_SOURCE_OUT;
    case CompositionOp::OP_IN:
        return D2D1_COMPOSITE_MODE_SOURCE_IN;
    case CompositionOp::OP_SOURCE:
        return D2D1_COMPOSITE_MODE_SOURCE_COPY;
    case CompositionOp::OP_DEST_IN:
        return D2D1_COMPOSITE_MODE_DESTINATION_IN;
    case CompositionOp::OP_DEST_OUT:
        return D2D1_COMPOSITE_MODE_DESTINATION_OUT;
    case CompositionOp::OP_DEST_OVER:
        return D2D1_COMPOSITE_MODE_DESTINATION_OVER;
    case CompositionOp::OP_DEST_ATOP:
        return D2D1_COMPOSITE_MODE_DESTINATION_ATOP;
    case CompositionOp::OP_XOR:
        return D2D1_COMPOSITE_MODE_XOR;
    case CompositionOp::OP_CLEAR:
        return D2D1_COMPOSITE_MODE_DESTINATION_OUT;
    default:
        return D2D1_COMPOSITE_MODE_SOURCE_OVER;
    }
}

enum class Axis : int8_t { X_AXIS, Y_AXIS, BOTH };

enum class ExtendMode : int8_t {
    CLAMP,     // Do not repeat
    REPEAT,    // Repeat in both axis
    REPEAT_X,  // Only X axis
    REPEAT_Y,  // Only Y axis
    REFLECT    // Mirror the image
};

static inline D2D1_EXTEND_MODE D2DExtend(ExtendMode aExtendMode, Axis aAxis) {
    D2D1_EXTEND_MODE extend;
    switch (aExtendMode) {
    case ExtendMode::REPEAT:
        extend = D2D1_EXTEND_MODE_WRAP;
        break;
    case ExtendMode::REPEAT_X: {
        extend = aAxis == Axis::X_AXIS
            ? D2D1_EXTEND_MODE_WRAP
            : D2D1_EXTEND_MODE_CLAMP;
        break;
    }
    case ExtendMode::REPEAT_Y: {
        extend = aAxis == Axis::Y_AXIS
            ? D2D1_EXTEND_MODE_WRAP
            : D2D1_EXTEND_MODE_CLAMP;
        break;
    }
    case ExtendMode::REFLECT:
        extend = D2D1_EXTEND_MODE_MIRROR;
        break;
    default:
        extend = D2D1_EXTEND_MODE_CLAMP;
    }

    return extend;
}
