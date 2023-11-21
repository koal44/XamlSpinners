using System;
using System.Windows;
using System.Windows.Media;


////// wrong eff'n class, man!

namespace ProjectionCanvas
{
    public class Matrix3
    {
        private enum RotationAxis
        {
            X, // Rotation around X axis.
            Y, // Rotation around Y axis.
            Z  // Rotation around Z axis.
        }

        // Composite matrix.
        private float[][] _mainMatrix;

        // Default translation for chart area cube ( without composition ).
        private float _translateX;

        // Default translation for chart area cube ( without composition ).
        private float _translateY;

        // Default translation for chart area cube ( without composition ).
        private float _translateZ;

        // The value, which is used to rescale chart area.
        private float _scale;

        // The value used for Isometric Shift.
        private float _shiftX;

        // The value used for Isometric Shift.
        private float _shiftY;

        // Perspective value.
        internal float _perspective;

        // Isometric projection.
        private bool _rightAngleAxis;

        // The value, which is used for perspective.
        private float _perspectiveFactor = float.NaN;

        // The value, which is used to set projection plane.
        private float _perspectiveZ;

        // X Angle.
        private float _angleX;

        // Y Angle.
        private float _angleY;

        // Gets the X Angle.
        internal float AngleX => _angleX;

        // Gets the Y Angle.
        internal float AngleY => _angleY;

        // Get perspective value.
        internal float Perspective => _perspective;

        readonly Point3[] _lightVectors = new Point3[7];

        // LightStyle Style
        LightStyle _lightStyle;

        public Matrix3()
        {
        }

        public bool IsInitialized() => _mainMatrix != null;

        /// <summary>
        /// Initialize Matrix 3D. This method calculates how much a chart area 
        /// cube has to be resized to fit Inner Plotting Area rectangle. Order 
        /// of operation is following: Translation for X and Y axes, Rotation 
        /// by X-axis, Rotation by Y-axis and same scaling for all axes. All 
        /// other elements, which belongs to this chart area cube (Data points, 
        /// grid lines etc.) has to follow same order. Translation and rotation 
        /// form composite matrix mainMatrix. Scale has to be allied separately.
        /// </summary>
        internal void Initialize(Rect innerPlotRectangle, float depth, float angleX, float angleY, float perspective, bool rightAngleAxis)
        {
            // Initialization for mainMatrix
            Reset();

            // Remember non-composite translation
            _translateX = (float)(innerPlotRectangle.X + innerPlotRectangle.Width / 2);
            _translateY = (float)(innerPlotRectangle.Y + innerPlotRectangle.Height / 2);
            _translateZ = depth / 2f;
            float width = (float)(innerPlotRectangle.Width);
            float height = (float)(innerPlotRectangle.Height);
            this._perspective = perspective;
            this._rightAngleAxis = rightAngleAxis;

            // Remember Angles
            this._angleX = angleX;
            this._angleY = angleY;

            // Change Degrees to radians.
            angleX = angleX / 180f * (float)Math.PI;
            angleY = angleY / 180f * (float)Math.PI;

            // Set points for 3D Bar which represents 3D Chart Area Cube.
            Point3[] points = CreateBoxCornerPoints(width, height, depth);

            // Translate Chart Area Cube WITH CENTER OF ROTATION - COMPOSITE TRANSLATION.
            Translate(_translateX, _translateY, 0);

            // Non Isometric projection
            if (!rightAngleAxis)
            {
                // Rotate Chart Area Cube by X axis. 
                Rotate(angleX, RotationAxis.X);

                // Rotate Chart Area Cube by Y axis. 
                Rotate(angleY, RotationAxis.Y);
            }
            else
            {
                if (this._angleY >= 45)
                {
                    // Rotate Chart Area Cube by Y axis. 
                    Rotate(Math.PI / 2, RotationAxis.Y);
                }
                else if (this._angleY <= -45)
                {
                    // Rotate Chart Area Cube by Y axis. 
                    Rotate(-Math.PI / 2, RotationAxis.Y);
                }
            }

            // Apply composed transformation ( Translation and rotation ).
            GetValues(points);

            float maxZ = float.MinValue;

            if (perspective != 0f || rightAngleAxis)
            {
                // Find projection plane
                foreach (Point3 point in points)
                {
                    if (point.Z > maxZ)
                        maxZ = point.Z;
                }

                // Set Projection plane
                _perspectiveZ = maxZ;
            }

            if (perspective != 0f)
            {
                _perspectiveFactor = perspective / 2000f;

                // Apply perspective
                ApplyPerspective(points);
            }

            // Isometric projection is active
            if (rightAngleAxis)
            {
                RightAngleProjection(points);

                float minX = 0f;
                float minY = 0f;
                float maxX = 0f;
                float maxY = 0f;

                // Point loop
                foreach (Point3 point in points)
                {
                    if (point.X - _translateX < 0f && Math.Abs(point.X - _translateX) > minX)
                        minX = Math.Abs(point.X - _translateX);

                    if (point.X - _translateX >= 0f && Math.Abs(point.X - _translateX) > maxX)
                        maxX = Math.Abs(point.X - _translateX);

                    if (point.Y - _translateY < 0f && Math.Abs(point.Y - _translateY) > minY)
                        minY = Math.Abs(point.Y - _translateY);

                    if (point.Y - _translateY >= 0f && Math.Abs(point.Y - _translateY) > maxY)
                        maxY = Math.Abs(point.Y - _translateY);
                }

                _shiftX = (maxX - minX) / 2f;
                _shiftY = (maxY - minY) / 2f;
                RightAngleShift(points);
            }

            // This code searches for value, which will be used for scaling.
            float maxXScale = float.MinValue;
            float maxYScale = float.MinValue;

            foreach (Point3 point in points)
            {
                // Find maximum relative distance for X axis.
                // Relative distance is (distance from the center of plotting area 
                // position) / (distance from the edge of rectangle to 
                // the center of the rectangle).
                if (maxXScale < Math.Abs(point.X - _translateX) / width * 2)
                    maxXScale = Math.Abs(point.X - _translateX) / width * 2;

                // Find maximum relative distance for Y axis.
                if (maxYScale < Math.Abs(point.Y - _translateY) / height * 2)
                    maxYScale = Math.Abs(point.Y - _translateY) / height * 2;
            }

            // Remember scale factor
            _scale = (maxYScale > maxXScale) ? maxYScale : maxXScale;

            // Apply scaling
            Scale(points);

        }

        /// <summary>
        /// Apply transformations on array of 3D Points. Order of operation is 
        /// following: Translation ( Set coordinate system for 0:100 to -50:50 
        /// Center of rotation is always 0), Composite Translation for X and Y 
        /// axes ( Moving center of rotation ), Rotation by X-axis, Rotation 
        /// by Y-axis, perspective and same scaling for all axes.
        /// </summary>
        /// <param name="points">3D Points array.</param>
        public void TransformPoints(Point3[] points)
        {
            TransformPoints(points, true);
        }

        /// <summary>
        /// Apply transformations on array od 3D Points. Order of operation is 
        /// following: Translation ( Set coordinate system for 0:100 to -50:50 
        /// Center of rotation is always 0), Composite Translation for X and Y 
        /// axes ( Moving center of rotation ), Rotation by X-axis, Rotation 
        /// by Y-axis, perspective and same scaling for all axes.
        /// </summary>
        /// <param name="points">3D Points array.</param>
        /// <param name="withPerspective">Applay Perspective</param>
        private void TransformPoints(Point3[] points, bool withPerspective)
        {
            // Matrix is not initialized.
            if (_mainMatrix == null)
                throw new InvalidOperationException("Matrix3 not initialized");

            // Translate point. CENTER OF ROTATION is 0 and that center is in 
            // the middle of chart area 3D CUBE. Translate method cannot 
            // be used because composite translation WILL MOVE 
            // CENTER OF ROTATION.
            for (int i = 0; i < points.Length; i++)
            {
                points[i].X += _translateX;
                points[i].Y += _translateY;
                points[i].Z += _translateZ;
            }

            // Transform points using composite mainMatrix. (Translation of points together with 
            // Center of rotation and rotations by X and Y axes).
            GetValues(points);

            // Apply perspective
            if (_perspective != 0f && withPerspective)
            {
                ApplyPerspective(points);
            }

            // RightAngle Projection
            if (_rightAngleAxis)
            {
                RightAngleProjection(points);
                RightAngleShift(points);
            }

            // Scales data points. Scaling has to be performed SEPARATELY from 
            // composite matrix. If scale is used with composite matrix after 
            // rotation, scaling will deform object.
            Scale(points);
        }

        /// <summary>
        /// This method adjusts a position of 3D Chart Area cube. This 
        /// method will translate chart for better use of the inner 
        /// plotting area. Center of rotation is shifted for 
        /// right Angle projection.
        /// </summary>
        /// <param name="points">3D Points array.</param>
        private void RightAngleShift(Point3[] points)
        {
            for (int i = 0; i < points.Length; i++)
            {
                points[i].X += _shiftX;
                points[i].Y += _shiftY;
            }
        }

        /// <summary>
        /// Method used to calculate right Angle projection.
        /// </summary>
        /// <param name="points">3D points array.</param>
        private void RightAngleProjection(Point3[] points)
        {
            float coorectionAngle = 45f;

            float xFactor = this._angleX / 45;

            float yFactor;

            if (this._angleY >= 45)
            {
                yFactor = (this._angleY - 90) / coorectionAngle;
            }
            else if (this._angleY <= -45)
            {
                yFactor = (this._angleY + 90) / coorectionAngle;
            }
            else
            {
                yFactor = this._angleY / coorectionAngle;
            }

            // Projection formula
            // perspectiveZ - Position of perspective plain.
            // Perspective Factor - Intensity of projection.
            for (int i = 0; i < points.Length; i++)
            {
                var point = points[i];
                points[i].X = point.X + (_perspectiveZ - point.Z) * yFactor;
                points[i].Y = point.Y - (_perspectiveZ - point.Z) * xFactor;
            }
        }

        /// <summary>
        /// Method is used for Planar Geometric projection. 
        /// </summary>
        /// <param name="points">3D Points array.</param>
        private void ApplyPerspective(Point3[] points)
        {
            // Projection formula
            // perspectiveZ - Position of perspective plain.
            // perspectiveFactor - Intensity of projection.
            for (int i = 0; i < points.Length; i++)
            {
                var point = points[i];
                points[i].X = _translateX + (point.X - _translateX) / (1 + (_perspectiveZ - point.Z) * _perspectiveFactor);
                points[i].Y = _translateY + (point.Y - _translateY) / (1 + (_perspectiveZ - point.Z) * _perspectiveFactor);
            }
        }

        /// <summary>
        /// Scales data points. Scaling has to be performed SEPARATELY from 
        /// composite matrix. If scale is used with composite matrix after 
        /// rotation, scaling will deform object.
        /// </summary>
        /// <param name="points">3D Points array.</param>
        private void Scale(Point3[] points)
        {
            for (int i = 0; i < points.Length; i++)
            {
                var point = points[i];
                points[i].X = _translateX + (point.X - _translateX) / _scale;
                points[i].Y = _translateY + (point.Y - _translateY) / _scale;
            }
        }

        /// <summary>
        /// Prepend to this Matrix object a translation. This method is used 
        /// only if CENTER OF ROTATION HAS TO BE MOVED.
        /// </summary>
        /// <param name="dx">Translate in x axis direction.</param>
        /// <param name="dy">Translate in y axis direction.</param>
        /// <param name="dz">Translate in z axis direction.</param>
        private void Translate(float dx, float dy, float dz)
        {
            float[][] translationMatrix = new float[4][];
            translationMatrix[0] = new float[4];
            translationMatrix[1] = new float[4];
            translationMatrix[2] = new float[4];
            translationMatrix[3] = new float[4];

            // Matrix initialization
            // Row loop
            for (int row = 0; row < 4; row++)
            {
                // Column loop
                for (int column = 0; column < 4; column++)
                {
                    // For initialization: Diagonal matrix elements are equal to one 
                    // and all other elements are equal to zero.
                    if (row == column)
                    {
                        translationMatrix[row][column] = 1f;
                    }
                    else
                    {
                        translationMatrix[row][column] = 0f;
                    }
                }
            }

            // Set translation values to the matrix
            translationMatrix[0][3] = dx;
            translationMatrix[1][3] = dy;
            translationMatrix[2][3] = dz;

            // Translate main Matrix
            Multiply(translationMatrix, MatrixOrder.Prepend, true);

        }

        /// <summary>
        /// This method initialize and set default values for mainMatrix ( there is no rotation and translation )
        /// </summary>
        private void Reset()
        {
            // First element is row and second element is column !!!
            _mainMatrix = new float[4][];
            _mainMatrix[0] = new float[4];
            _mainMatrix[1] = new float[4];
            _mainMatrix[2] = new float[4];
            _mainMatrix[3] = new float[4];

            // Matrix initialization
            // Row loop
            for (int row = 0; row < 4; row++)
            {
                // Column loop
                for (int column = 0; column < 4; column++)
                {
                    // For initialization: Diagonal matrix elements are equal to one 
                    // and all other elements are equal to zero.
                    if (row == column)
                    {
                        _mainMatrix[row][column] = 1f;
                    }
                    else
                    {
                        _mainMatrix[row][column] = 0f;
                    }
                }
            }
        }


        /// <summary>
        /// Multiplies this Matrix object by the matrix specified in the 
        /// matrix parameter, and in the order specified in the order parameter.
        /// </summary>
        /// <param name="mulMatrix">The Matrix object by which this Matrix object is to be multiplied.</param>
        /// <param name="order">The MatrixOrder enumeration that represents the order of the multiplication. If the specified order is MatrixOrder.Prepend, this Matrix object is multiplied by the specified matrix in a prepended order. If the specified order is MatrixOrder.Append, this Matrix object is multiplied by the specified matrix in an appended order.</param>
        /// <param name="setMainMatrix">Set main matrix to be result of multiplication</param>
        /// <returns>Matrix multiplication result.</returns>
        private float[][] Multiply(float[][] mulMatrix, MatrixOrder order, bool setMainMatrix)
        {
            // A matrix which is result of matrix multiplication
            // of mulMatrix and mainMatrix
            float[][] resultMatrix = new float[4][];
            resultMatrix[0] = new float[4];
            resultMatrix[1] = new float[4];
            resultMatrix[2] = new float[4];
            resultMatrix[3] = new float[4];

            // Row loop
            for (int row = 0; row < 4; row++)
            {
                // Column loop
                for (int column = 0; column < 4; column++)
                {
                    // Initialize element
                    resultMatrix[row][column] = 0f;
                    for (int sumIndx = 0; sumIndx < 4; sumIndx++)
                    {
                        // Find matrix element
                        if (order == MatrixOrder.Prepend)
                        {
                            // Order of matrix multiplication
                            resultMatrix[row][column] += _mainMatrix[row][sumIndx] * mulMatrix[sumIndx][column];
                        }
                        else
                        {
                            // Order of matrix multiplication
                            resultMatrix[row][column] += mulMatrix[row][sumIndx] * _mainMatrix[sumIndx][column];
                        }
                    }
                }
            }

            // Set result matrix to be main matrix
            if (setMainMatrix)
            {
                _mainMatrix = resultMatrix;
            }

            return resultMatrix;
        }


        /// <summary>
        /// Multiplies this Matrix object by the Vector specified in the 
        /// vector parameter.
        /// </summary>
        /// <param name="mulVector">The vector object by which this Matrix object is to be multiplied.</param>
        /// <param name="resultVector">Vector which is result of matrix and vector multiplication.</param>
        private void MultiplyVector(float[] mulVector, ref float[] resultVector)
        {
            // Row loop
            for (int row = 0; row < 3; row++)
            {
                // Initialize element
                resultVector[row] = 0f;

                // Column loop
                for (int column = 0; column < 4; column++)
                {
                    // Find matrix element
                    resultVector[row] += _mainMatrix[row][column] * mulVector[column];
                }
            }
        }

        /// <summary>
        /// Prepend to this Matrix object a clockwise rotation, around the axis and by the specified angle.
        /// </summary>
        /// <param name="angle">Angle to rotate</param>
        /// <param name="axis">Axis used for rotation</param>
        private void Rotate(double angle, RotationAxis axis)
        {
            float[][] rotationMatrix = new float[4][];
            rotationMatrix[0] = new float[4];
            rotationMatrix[1] = new float[4];
            rotationMatrix[2] = new float[4];
            rotationMatrix[3] = new float[4];

            // Change angle direction
            angle = -1f * angle;

            // Matrix initialization
            // Row loop
            for (int row = 0; row < 4; row++)
            {
                // Column loop
                for (int column = 0; column < 4; column++)
                {
                    // For initialization: Diagonal matrix elements are equal to one 
                    // and all other elements are equal to zero.
                    if (row == column)
                    {
                        rotationMatrix[row][column] = 1f;
                    }
                    else
                    {
                        rotationMatrix[row][column] = 0f;
                    }
                }
            }

            // Rotation about axis
            switch (axis)
            {
                // Rotation about X axis
                case RotationAxis.X:
                    rotationMatrix[1][1] = (float)Math.Cos(angle);
                    rotationMatrix[1][2] = (float)-Math.Sin(angle);
                    rotationMatrix[2][1] = (float)Math.Sin(angle);
                    rotationMatrix[2][2] = (float)Math.Cos(angle);
                    break;

                // Rotation about Y axis
                case RotationAxis.Y:
                    rotationMatrix[0][0] = (float)Math.Cos(angle);
                    rotationMatrix[0][2] = (float)Math.Sin(angle);
                    rotationMatrix[2][0] = (float)-Math.Sin(angle);
                    rotationMatrix[2][2] = (float)Math.Cos(angle);
                    break;

                // Rotation about Z axis
                case RotationAxis.Z:
                    rotationMatrix[0][0] = (float)Math.Cos(angle);
                    rotationMatrix[0][1] = (float)-Math.Sin(angle);
                    rotationMatrix[1][0] = (float)Math.Sin(angle);
                    rotationMatrix[1][1] = (float)Math.Cos(angle);
                    break;

            }

            // Rotate Main matrix
            Multiply(rotationMatrix, MatrixOrder.Prepend, true);

        }

        /// <summary>
        /// Returns transformed x and y values from x, y and z values 
        /// and composed main matrix values (All rotations, 
        /// translations and scaling).
        /// </summary>
        /// <param name="points">Array of 3D points.</param>
        private void GetValues(Point3[] points)
        {
            // Create one dimensional matrix (vector)
            float[] inputVector = new float[4];

            // A vector which is result of matrix and vector multiplication
            float[] resultVector = new float[4];

            for (int i = 0; i < points.Length; i++)
            {
                // Fill input vector with x, y and z coordinates
                inputVector[0] = points[i].X;
                inputVector[1] = points[i].Y;
                inputVector[2] = points[i].Z;
                inputVector[3] = 1;

                // Apply 3D transformations.
                MultiplyVector(inputVector, ref resultVector);

                // Return x and y coordinates.
                points[i].X = resultVector[0];
                points[i].Y = resultVector[1];
                points[i].Z = resultVector[2];
            }
        }


        /// <summary>
        /// Set points for 3D Bar which represents 3D Chart Area.
        /// </summary>
        /// <param name="dx">Width of the bar 3D.</param>
        /// <param name="dy">Height of the bar 3D.</param>
        /// <param name="dz">Depth of the bar 3D.</param>
        /// <returns>Collection of Points 3D.</returns>
        private static Point3[] CreateBoxCornerPoints(float dx, float dy, float dz)
        {
            Point3[] points = new Point3[8];

            // 3D Bar side: Front
            points[0] = new Point3(-dx / 2, -dy / 2, dz / 2);
            points[1] = new Point3(dx / 2, -dy / 2, dz / 2);
            points[2] = new Point3(dx / 2, dy / 2, dz / 2);
            points[3] = new Point3(-dx / 2, dy / 2, dz / 2);

            // 3D Bar side: Back
            points[4] = new Point3(-dx / 2, -dy / 2, -dz / 2);
            points[5] = new Point3(dx / 2, -dy / 2, -dz / 2);
            points[6] = new Point3(dx / 2, dy / 2, -dz / 2);
            points[7] = new Point3(-dx / 2, dy / 2, -dz / 2);

            return points;
        }


        #region Lighting Methods

        /// <summary>
        /// Initial Lighting. Use matrix transformation only once 
        /// for Normal vectors.
        /// </summary>
        /// <param name="lightStyle">LightStyle Style</param>
        internal void InitLight(LightStyle lightStyle)
        {
            // Set LightStyle Style
            this._lightStyle = lightStyle;

            // Center of rotation
            _lightVectors[0] = new Point3(0f, 0f, 0f);

            // Front side normal Vector.
            _lightVectors[1] = new Point3(0f, 0f, 1f);

            // Back side normal Vector.
            _lightVectors[2] = new Point3(0f, 0f, -1f);

            // Left side normal Vector.
            _lightVectors[3] = new Point3(-1f, 0f, 0f);

            // Right side normal Vector.
            _lightVectors[4] = new Point3(1f, 0f, 0f);

            // Top side normal Vector.
            _lightVectors[5] = new Point3(0f, -1f, 0f);

            // Bottom side normal Vector.
            _lightVectors[6] = new Point3(0f, 1f, 0f);

            // Apply matrix transformations
            TransformPoints(_lightVectors, false);

            // ********************************************************
            // LightStyle Vector and normal vectors have to have same center. 
            // Shift Normal vectors.
            // ********************************************************

            // Front Side shift
            _lightVectors[1].X -= _lightVectors[0].X;
            _lightVectors[1].Y -= _lightVectors[0].Y;
            _lightVectors[1].Z -= _lightVectors[0].Z;

            // Back Side shift
            _lightVectors[2].X -= _lightVectors[0].X;
            _lightVectors[2].Y -= _lightVectors[0].Y;
            _lightVectors[2].Z -= _lightVectors[0].Z;

            // Left Side shift
            _lightVectors[3].X -= _lightVectors[0].X;
            _lightVectors[3].Y -= _lightVectors[0].Y;
            _lightVectors[3].Z -= _lightVectors[0].Z;

            // Right Side shift
            _lightVectors[4].X -= _lightVectors[0].X;
            _lightVectors[4].Y -= _lightVectors[0].Y;
            _lightVectors[4].Z -= _lightVectors[0].Z;

            // Top Side shift
            _lightVectors[5].X -= _lightVectors[0].X;
            _lightVectors[5].Y -= _lightVectors[0].Y;
            _lightVectors[5].Z -= _lightVectors[0].Z;

            // Bottom Side shift
            _lightVectors[6].X -= _lightVectors[0].X;
            _lightVectors[6].Y -= _lightVectors[0].Y;
            _lightVectors[6].Z -= _lightVectors[0].Z;

        }

        /// <summary>
        /// Return intensity of lightStyle for 3D Cube. There are tree types of lights: None, 
        /// Simplistic and Realistic. None Style have same lightStyle intensity on 
        /// all polygons. Normal vector doesn’t have influence on this type 
        /// of lighting. Simplistic style have lightStyle source, which is 
        /// rotated together with scene. Realistic lighting have fixed lightStyle 
        /// source and intensity of lightStyle is change when scene is rotated.
        /// </summary>
        /// <param name="surfaceColor">Color used for polygons without lighting</param>
        /// <param name="front">Color corrected with intensity of lightStyle for Front side of the 3D Rectangle</param>
        /// <param name="back">Color corrected with intensity of lightStyle for Back side of the 3D Rectangle</param>
        /// <param name="left">Color corrected with intensity of lightStyle for Left side of the 3D Rectangle</param>
        /// <param name="right">Color corrected with intensity of lightStyle for Right side of the 3D Rectangle</param>
        /// <param name="top">Color corrected with intensity of lightStyle for Top side of the 3D Rectangle</param>
        /// <param name="bottom">Color corrected with intensity of lightStyle for Bottom side of the 3D Rectangle</param>
        internal void GetLight(Color surfaceColor, out Color front, out Color back, out Color left, out Color right, out Color top, out Color bottom)
        {
            switch (_lightStyle)
            {
                // LightStyle style is None
                case LightStyle.None:
                    {
                        front = surfaceColor;
                        left = surfaceColor;
                        top = surfaceColor;
                        back = surfaceColor;
                        right = surfaceColor;
                        bottom = surfaceColor;
                        break;
                    }
                // LightStyle style is Simplistic
                case LightStyle.Simplistic:
                    {
                        front = surfaceColor;
                        left = Utils.GetGradientColor(surfaceColor, Colors.Black, 0.25);
                        top = Utils.GetGradientColor(surfaceColor, Colors.Black, 0.15);
                        back = surfaceColor;
                        right = Utils.GetGradientColor(surfaceColor, Colors.Black, 0.25);
                        bottom = Utils.GetGradientColor(surfaceColor, Colors.Black, 0.15);
                        break;
                    }
                // LightStyle style is Realistic
                default:
                    {

                        // For Right Axis angle Realistic lightStyle should be different
                        if (_rightAngleAxis)
                        {
                            // LightStyle source Vector
                            var lightSource = new Point3(0f, 0f, -1f);
                            var rightPRpoints = new Point3[1];
                            rightPRpoints[0] = lightSource;
                            RightAngleProjection(rightPRpoints);

                            // ******************************************************************
                            // Color correction. Angle between Normal vector of polygon and 
                            // vector of lightStyle source is used.
                            // ******************************************************************
                            if (this._angleY >= 45 || this._angleY <= -45)
                            {
                                front = Utils.GetGradientColor(surfaceColor, Colors.Black, lightSource.GetAngle(_lightVectors[1]) / Math.PI);

                                back = Utils.GetGradientColor(surfaceColor, Colors.Black, lightSource.GetAngle(_lightVectors[2]) / Math.PI);

                                left = Utils.GetGradientColor(surfaceColor, Colors.Black, 0);

                                right = Utils.GetGradientColor(surfaceColor, Colors.Black, 0);
                            }
                            else
                            {
                                front = Utils.GetGradientColor(surfaceColor, Colors.Black, 0);

                                back = Utils.GetGradientColor(surfaceColor, Colors.Black, 1);

                                left = Utils.GetGradientColor(surfaceColor, Colors.Black, lightSource.GetAngle(_lightVectors[3]) / Math.PI);

                                right = Utils.GetGradientColor(surfaceColor, Colors.Black, lightSource.GetAngle(_lightVectors[4]) / Math.PI);
                            }

                            top = Utils.GetGradientColor(surfaceColor, Colors.Black, lightSource.GetAngle(_lightVectors[5]) / Math.PI);

                            bottom = Utils.GetGradientColor(surfaceColor, Colors.Black, lightSource.GetAngle(_lightVectors[6]) / Math.PI);
                        }
                        else
                        {
                            // LightStyle source Vector
                            var lightSource = new Point3(0f, 0f, 1f);

                            // ******************************************************************
                            // Color correction. Angle between Normal vector of polygon and 
                            // vector of lightStyle source is used.
                            // ******************************************************************
                            front = GetBrightGradientColor(surfaceColor, lightSource.GetAngle(_lightVectors[1]) / Math.PI);

                            back = GetBrightGradientColor(surfaceColor, lightSource.GetAngle(_lightVectors[2]) / Math.PI);

                            left = GetBrightGradientColor(surfaceColor, lightSource.GetAngle(_lightVectors[3]) / Math.PI);

                            right = GetBrightGradientColor(surfaceColor, lightSource.GetAngle(_lightVectors[4]) / Math.PI);

                            top = GetBrightGradientColor(surfaceColor, lightSource.GetAngle(_lightVectors[5]) / Math.PI);

                            bottom = GetBrightGradientColor(surfaceColor, lightSource.GetAngle(_lightVectors[6]) / Math.PI);
                        }

                        break;
                    }
            }
        }


        /// <summary>
        /// This method creates gradien color with brightnes.
        /// </summary>
        /// <param name="beginColor">Start color for gradient.</param>
        /// <param name="position">Position used between Start and end color.</param>
        /// <returns>Calculated Gradient color from gradient position</returns>
        private static Color GetBrightGradientColor(Color beginColor, double position)
        {
            position *= 2;
            double brightness = 0.5;
            if (position < brightness)
            {
                return Utils.GetGradientColor(Color.FromArgb(beginColor.A, 255, 255, 255), beginColor, 1 - brightness + position);
            }
            else if (-brightness + position < 1)
            {
                return Utils.GetGradientColor(beginColor, Colors.Black, -brightness + position);
            }
            else
            {
                return Color.FromArgb(beginColor.A, 0, 0, 0);
            }
        }


        #endregion

    }

    public enum MatrixOrder
    {
        Prepend,
        Append
    }

    public enum LightStyle
    {
        None,
        Simplistic,
        Realistic
    }
}