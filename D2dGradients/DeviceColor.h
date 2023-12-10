#pragma once
typedef float Float;

struct DeviceColor {
public:
    Float r, g, b, a;

    DeviceColor() : r(0.0f), g(0.0f), b(0.0f), a(0.0f) {}
    DeviceColor(Float aR, Float aG, Float aB, Float aA)
        : r(aR), g(aG), b(aB), a(aA) {}
    DeviceColor(Float aR, Float aG, Float aB) : r(aR), g(aG), b(aB), a(1.0f) {}
};