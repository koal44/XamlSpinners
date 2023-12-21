#pragma once
#include "pch.h"
#include "Matrix.h"
#include "Types.h"

enum class PatternType : int8_t {
    COLOR, SURFACE, LINEAR_GRADIENT, RADIAL_GRADIENT, CONIC_GRADIENT
};
constexpr size_t kPatternTypeCount = 5;
constexpr PatternType kHighestPatternType = PatternType(kPatternTypeCount - 1);

inline bool operator==(const D2D1_POINT_2F& lhs, const D2D1_POINT_2F& rhs) {
    // Use an epsilon value for comparison to account for floating point precision issues
    const float epsilon = 1e-6f;
    return fabs(lhs.x - rhs.x) < epsilon && fabs(lhs.y - rhs.y) < epsilon;
}


class Pattern {
public:
    virtual ~Pattern() = default;

    virtual PatternType GetType() const = 0;

    virtual bool operator==(const Pattern& aOther) const = 0;

    bool operator!=(const Pattern& aOther) const { return !(*this == aOther); }

protected:
    Pattern() = default;
};

class ColorPattern : public Pattern {
public:
    // Explicit because consumers should generally use ToDeviceColor when
    // creating a ColorPattern.
    explicit ColorPattern(const DeviceColor& aColor) : mColor(aColor) {}

    PatternType GetType() const override { return PatternType::COLOR; }

    bool operator==(const Pattern& aOther) const override {
        if (aOther.GetType() != PatternType::COLOR) {
            return false;
        }
        const ColorPattern& other = static_cast<const ColorPattern&>(aOther);
        return mColor == other.mColor;
    }

    DeviceColor mColor;
};

/**
 * This class is used for Linear Gradient Patterns, the gradient stops are
 * stored in a separate object and are backend dependent. This class itself
 * may be used on the stack.
 */
class LinearGradientPattern : public Pattern {
public:
    /// For constructor parameter description, see member data documentation.
    LinearGradientPattern(
        const POINT2F& aBegin,
        const POINT2F& aEnd,
        ID2D1GradientStopCollection* aStops,
        const Matrix& aMatrix = Matrix()
    ) :
        mBegin(aBegin),
        mEnd(aEnd),
        mStops(std::move(aStops)),
        mMatrix(aMatrix) {}

    PatternType GetType() const override { return PatternType::LINEAR_GRADIENT; }

    bool operator==(const Pattern& aOther) const override {
        if (aOther.GetType() != PatternType::LINEAR_GRADIENT) {
            return false;
        }
        const LinearGradientPattern& otherPattern = static_cast<const LinearGradientPattern&>(aOther);
        return *this == otherPattern;
    }

    bool operator==(const LinearGradientPattern& aOther) const {
        return mBegin == aOther.mBegin && mEnd == aOther.mEnd &&
            mStops == aOther.mStops && mMatrix.ExactlyEquals(aOther.mMatrix);
    }

    POINT2F mBegin;              //!< Start of the linear gradient
    POINT2F mEnd;                /**< End of the linear gradient - NOTE: In the case
                                    of a zero length gradient it will act as the
                                    color of the last stop. */
    ComPtr<ID2D1GradientStopCollection> mStops;
    /*ComPtr<GradientStops> mStops; *< GradientStops object for this gradient, this
                                    should match the backend type of the draw
                                    target this pattern will be used with. */
    Matrix mMatrix;            /**< A matrix that transforms the pattern into
                                    user space */
};


/**
 * This class is used for Radial Gradient Patterns, the gradient stops are
 * stored in a separate object and are backend dependent. This class itself
 * may be used on the stack.
 */
class RadialGradientPattern : public Pattern {
public:
    /// For constructor parameter description, see member data documentation.
    RadialGradientPattern(
        const POINT2F& aCenter1,
        const POINT2F& aCenter2,
        FLOAT aRadius1,
        FLOAT aRadius2,
        ID2D1GradientStopCollection* aStops,
        const Matrix& aMatrix = Matrix()
    ) :
        mCenter1(aCenter1),
        mCenter2(aCenter2),
        mRadius1(aRadius1),
        mRadius2(aRadius2),
        mStops(std::move(aStops)),
        mMatrix(aMatrix) {}


    bool operator==(const RadialGradientPattern& aOther) const {
        return mCenter1 == aOther.mCenter1 &&
            mCenter2 == aOther.mCenter2 &&
            mRadius1 == aOther.mRadius1 &&
            mRadius2 == aOther.mRadius2 &&
            mStops == aOther.mStops &&
            mMatrix.ExactlyEquals(aOther.mMatrix);
    }

    bool operator==(const Pattern& aOther) const override {
        if (aOther.GetType() != PatternType::RADIAL_GRADIENT) {
            return false;
        }
        const RadialGradientPattern& otherPattern = static_cast<const RadialGradientPattern&>(aOther);
        return *this == otherPattern;
    }

    POINT2F mCenter1;            //!< Center of the inner (focal) circle.
    POINT2F mCenter2;            //!< Center of the outer circle.
    FLOAT mRadius1;            //!< Radius of the inner (focal) circle.
    FLOAT mRadius2;            //!< Radius of the outer circle.
    ComPtr<ID2D1GradientStopCollection> mStops;
    /*ComPtr<GradientStops> mStops; *< GradientStops object for this gradient, this
                                    should match the backend type of the draw
                                  target this pattern will be used with. */
    Matrix mMatrix;  //!< A matrix that transforms the pattern into user space
};

/**
 * This class is used for Conic Gradient Patterns, the gradient stops are
 * stored in a separate object and are backend dependent. This class itself
 * may be used on the stack.
 */
class ConicGradientPattern : public Pattern {
public:
    ConicGradientPattern(
        const POINT2F& aCenter,
        FLOAT aAngle,
        FLOAT aStartOffset,
        FLOAT aEndOffset,
        ID2D1GradientStopCollection* aStops,
        const Matrix& aMatrix = Matrix()
    ) :
        mCenter(aCenter),
        mAngle(aAngle),
        mStartOffset(aStartOffset),
        mEndOffset(aEndOffset),
        mStops(std::move(aStops)),
        mMatrix(aMatrix)
    {}

    PatternType GetType() const override { return PatternType::CONIC_GRADIENT; }

    bool operator==(const ConicGradientPattern& aOther) const {
        return mCenter == aOther.mCenter &&
            mAngle == aOther.mAngle &&
            mStartOffset == aOther.mStartOffset &&
            mEndOffset == aOther.mEndOffset &&
            mStops == aOther.mStops &&
            mMatrix.ExactlyEquals(aOther.mMatrix);
    }

    bool operator==(const Pattern& aOther) const override {
        if (aOther.GetType() != PatternType::CONIC_GRADIENT) {
            return false;
        }
        const ConicGradientPattern& otherPattern = static_cast<const ConicGradientPattern&>(aOther);
        return *this == otherPattern;
    }

    POINT2F mCenter;             //!< Center of the gradient
    FLOAT mAngle;              //!< Start angle of gradient
    FLOAT mStartOffset;        // Offset of first stop
    FLOAT mEndOffset;          // Offset of last stop
    ComPtr<ID2D1GradientStopCollection> mStops;
    /*ComPtr<GradientStops> mStops; *< GradientStops object for this gradient, this
                                    should match the backend type of the draw
                                  target this pattern will be used with. */
    Matrix mMatrix;  //!< A matrix that transforms the pattern into user space
};
