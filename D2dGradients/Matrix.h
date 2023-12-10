#pragma once
#include <d2d1.h>
#include <ostream>

class Matrix {

public:
    float _11, _12;
    float _21, _22;
    float _31, _32;

    Matrix() : _11(1.0f), _12(0), _21(0), _22(1.0f), _31(0), _32(0) {}
    Matrix(float a11, float a12, float a21, float a22, float a31, float a32)
        : _11(a11), _12(a12), _21(a21), _22(a22), _31(a31), _32(a32) {}

    float Determinant() const { return _11 * _22 - _12 * _21; }

    bool Invert() {
        // Compute co-factors.
        float A = _22;
        float B = -_21;
        float C = _21 * _32 - _22 * _31;
        float D = -_12;
        float E = _11;
        float F = _31 * _12 - _11 * _32;

        float det = Determinant();

        if (!det) {
            return false;
        }

        float inv_det = 1 / det;

        _11 = inv_det * A;
        _12 = inv_det * D;
        _21 = inv_det * B;
        _22 = inv_det * E;
        _31 = inv_det * C;
        _32 = inv_det * F;

        return true;
    }

};

static inline Matrix ToMatrix(const D2D1_MATRIX_3X2_F& aTransform) {
    return Matrix(aTransform._11, aTransform._12, aTransform._21, aTransform._22,
        aTransform._31, aTransform._32);
}

//class Matrix {
//public:
//    float _11, _12;
//    float _21, _22;
//    float _31, _32;
//
//    Matrix(float m11, float m12, float m21, float m22, float m31, float m32)
//        : _11(m11), _12(m12), _21(m21), _22(m22), _31(m31), _32(m32) {}
//
//    static inline Matrix ToMatrix(const D2D1_MATRIX_3X2_F& aTransform) {
//        return Matrix(aTransform._11, aTransform._12, aTransform._21, aTransform._22,
//            aTransform._31, aTransform._32);
//    }
//};