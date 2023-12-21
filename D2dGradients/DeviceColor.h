#pragma once
#include "pch.h"

struct DeviceColor {
public:
    FLOAT r, g, b, a;

    DeviceColor() : r(0.0f), g(0.0f), b(0.0f), a(0.0f) {}
    DeviceColor(FLOAT aR, FLOAT aG, FLOAT aB, FLOAT aA)
        : r(aR), g(aG), b(aB), a(aA) {}
    DeviceColor(FLOAT aR, FLOAT aG, FLOAT aB) : r(aR), g(aG), b(aB), a(1.0f) {}
};
