#pragma once

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

    Matrix operator*(const Matrix& aMatrix) const {
        Matrix resultMatrix;

        resultMatrix._11 = this->_11 * aMatrix._11 + this->_12 * aMatrix._21;
        resultMatrix._12 = this->_11 * aMatrix._12 + this->_12 * aMatrix._22;
        resultMatrix._21 = this->_21 * aMatrix._11 + this->_22 * aMatrix._21;
        resultMatrix._22 = this->_21 * aMatrix._12 + this->_22 * aMatrix._22;
        resultMatrix._31 =
            this->_31 * aMatrix._11 + this->_32 * aMatrix._21 + aMatrix._31;
        resultMatrix._32 =
            this->_31 * aMatrix._12 + this->_32 * aMatrix._22 + aMatrix._32;

        return resultMatrix;
    }

    bool ExactlyEquals(const Matrix& o) const {
        return _11 == o._11 && _12 == o._12 && _21 == o._21 && _22 == o._22 &&
            _31 == o._31 && _32 == o._32;
    }

};



static inline Matrix ToMatrix(const D2D1_MATRIX_3X2_F& aTransform) {
    return Matrix(aTransform._11, aTransform._12, aTransform._21, aTransform._22,
        aTransform._31, aTransform._32);
}

static inline D2D1_MATRIX_3X2_F D2DMatrix(const Matrix& aTransform) {
    return D2D1::Matrix3x2F(aTransform._11, aTransform._12, aTransform._21,
        aTransform._22, aTransform._31, aTransform._32);
}
