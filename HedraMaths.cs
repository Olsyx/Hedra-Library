/*
 * Hedra's mathematical partial class. Holds typical mathematical functions,
 * such as proportions, equation solving, etc.
 * 
 * @author  Olsyx (Olatz Castaño)
 * @source https://github.com/Darkatom/Hedra-Library
 * @since   Unity 2017.1.0p4
 * @last    Unity 2017.2.0f3
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HedraLibrary {
    public static partial class Hedra {
        /// <summary>
        /// Rounds a number to a number of decimal places.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        public static float RoundDecimals(float value, int digits) {
            float mult = Mathf.Pow(10.0f, (float)digits);
            return Mathf.Round(value * mult) / mult;
        }
        
        /// <summary>
        /// Rounds a Vector coordinates to a number of decimal places.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        public static Vector2 Round(Vector2 value, int digits) {
            return new Vector2(RoundDecimals(value.x, digits), RoundDecimals(value.y, digits));
        }

        /// <summary>
        /// Rounds a Vector coordinates to a number of decimal places.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        public static Vector3 Round(Vector3 value, int digits) {
            return new Vector3(RoundDecimals(value.x, digits), RoundDecimals(value.y, digits), RoundDecimals(value.z, digits));
        }
        /// <summary>
        /// Truncates a number to a number of decimal places.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="digits"></param>
        /// <returns></returns>

        public static float Truncate(float value, int digits) {
            float mult = Mathf.Pow(10.0f, (float)digits);
            return ((int)(value * mult)) / mult;
        }

        /// <summary>
        /// Truncates a Vector to a number of decimal places.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        public static Vector2 Truncate(Vector2 value, int digits) {
            return new Vector2(Truncate(value.x, digits), Truncate(value.y, digits));
        }

        /// <summary>
        /// Truncates a Vector to a number of decimal places.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        public static Vector3 Truncate(Vector3 value, int digits) {
            return new Vector3(Truncate(value.x, digits), Truncate(value.y, digits), Truncate(value.z, digits));
        }

        /// <summary>
        /// Returns the same vector, its components turned to absolute values.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Vector2 Abs(Vector2 target) {
            target.x = Mathf.Abs(target.x);
            target.y = Mathf.Abs(target.y);
            return target;
        }

        /// <summary>
        /// Returns the same vector, its components turned to absolute values.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Vector3 Abs(Vector3 target) {
            target.x = Mathf.Abs(target.x);
            target.y = Mathf.Abs(target.y);
            target.z = Mathf.Abs(target.z);
            return target;
        }

        /// <summary>
        /// Returns the same vector, each component casted to Integer.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Vector2 ParseInt(Vector2 target) {
            target.x = (int)target.x;
            target.y = (int)target.y;
            return target;
        }

        /// <summary>
        /// Returns the same vector, each component casted to Integer.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Vector3 ParseInt(Vector3 target) {
            target.x = (int)target.x;
            target.y = (int)target.y;
            target.z = (int)target.z;
            return target;
        }
        
        /// <summary>
        /// Clamps the value to range bounds.
        /// </summary>
        /// <param name="value">Value to clamp.</param>
        /// <param name="upperLimit"></param>
        /// <param name="bottomLimit"></param>
        /// <returns></returns>
        public static int Clamp(int value, int bottomLimit, int upperLimit) {
            return Mathf.Max(Mathf.Min(value, upperLimit), bottomLimit);
        }

        /// <summary>
        /// Clamps the value to range bounds.
        /// </summary>
        /// <param name="value">Value to clamp.</param>
        /// <param name="upperLimit"></param>
        /// <param name="bottomLimit"></param>
        /// <returns></returns>
        public static float Clamp(float value, float bottomLimit, float upperLimit) {
            return Mathf.Max(Mathf.Min(value, upperLimit), bottomLimit);
        }

        /// <summary>
        /// Hard clamps the value to the nearest limit.
        /// </summary>
        /// <param name="value">Value to clamp.</param>
        /// <param name="upperLimit"></param>
        /// <param name="bottomLimit"></param>
        /// <returns></returns>
        public static int HardClamp(int value, int bottomLimit, int upperLimit) {
            return (int)HardClamp((float)value, bottomLimit, upperLimit);
        }

        /// <summary>
        /// Hard clamps the value to the nearest limit.
        /// </summary>
        /// <param name="value">Value to clamp.</param>
        /// <param name="upperLimit"></param>
        /// <param name="bottomLimit"></param>
        /// <returns></returns>
        public static float HardClamp(float value, float bottomLimit, float upperLimit) {
            float center = (upperLimit + bottomLimit) / 2f; 
            if (value > center) {
                return upperLimit;
            } else {
                return bottomLimit;
            }
        }

        /// <summary>
        /// Hard clamps the value to the nearest limit.
        /// </summary>
        /// <param name="value">Value to clamp.</param>
        /// <param name="upperLimit"></param>
        /// <param name="centerLimit"></param>
        /// <param name="bottomLimit"></param>
        /// <returns></returns>
        public static int HardClamp(int value, int bottomLimit, int centerLimit, int upperLimit) {
            return (int)HardClamp((float)value, bottomLimit, centerLimit, upperLimit);
        }

        /// <summary>
        /// Hard clamps the value to the nearest limit.
        /// </summary>
        /// <param name="value">Value to clamp.</param>
        /// <param name="upperLimit"></param>
        /// <param name="centerLimit"></param>
        /// <param name="bottomLimit"></param>
        /// <returns></returns>
        public static float HardClamp(float value, float bottomLimit, float centerLimit, float upperLimit) {
            if (value > upperLimit) {
                return upperLimit;
            } else if (value < bottomLimit) {
                return bottomLimit;
            }

            float upperMiddle = (upperLimit + centerLimit) / 2;
            float bottomMiddle = (bottomLimit + centerLimit) / 2;

            if (value >= upperMiddle) {
                return upperLimit;
            } else if (value <= bottomMiddle) {
                return bottomLimit;
            }

            return centerLimit;
        }

        /// <summary>
        /// Hard clamps the value to the nearest limit.
        /// </summary>
        /// <param name="value">Value to clamp.</param>
        /// <param name="upperLimit"></param>
        /// <param name="centerLimit"></param>
        /// <param name="bottomLimit"></param>
        /// <returns></returns>
        public static Vector2 HardClamp(Vector2 value, int bottomLimit, int centerLimit, int upperLimit) {
            Vector2 clampedValue = new Vector2(
                                        Hedra.HardClamp(value.x, bottomLimit, centerLimit, upperLimit),
                                        Hedra.HardClamp(value.y, bottomLimit, centerLimit, upperLimit)
                                    );
            return clampedValue;
        }

        /// <summary>
        /// Hard clamps the value to the nearest limit.
        /// </summary>
        /// <param name="value">Value to clamp.</param>
        /// <param name="upperLimit"></param>
        /// <param name="centerLimit"></param>
        /// <param name="bottomLimit"></param>
        /// <returns></returns>
        public static Vector3 HardClamp(Vector3 value, float bottomLimit, float centerLimit, float upperLimit) {
            Vector3 clampedValue = new Vector3(
                                        Hedra.HardClamp(value.x, bottomLimit, centerLimit, upperLimit),
                                        Hedra.HardClamp(value.y, bottomLimit, centerLimit, upperLimit),
                                        Hedra.HardClamp(value.z, bottomLimit, centerLimit, upperLimit)
                                    );
            return clampedValue;
        }

        /// <summary>
        /// Clamps a given angle in degrees into the range [0, 360]
        /// </summary>
        /// <param name="degrees"></param>
        /// <returns></returns>
        public static float ClampAngle(float degrees) {
            float correctAngle = degrees % 360f;

            if (correctAngle < 0) {
                correctAngle = correctAngle + 360f;
            }

            return correctAngle;
        }

        /// <summary>
        /// Clamps a given angle in degrees into the range [0, 360]
        /// </summary>
        /// <param name="degrees"></param>
        /// <returns></returns>
        public static Vector3 ClampAngle(Vector3 angular) {
            return new Vector3(ClampAngle(angular.x),
                               ClampAngle(angular.y),
                               ClampAngle(angular.z));
        }

        /// <summary>
        /// Turns negative angles in degrees into the range [0, 360]. Angles higher than 360 are not clamped.
        /// </summary>
        /// <param name="degrees"></param>
        /// <returns></returns>
        public static float ClampNegativeAngle(float degrees) {
            if (degrees >= 0f) {
                return degrees;
            }

            return ClampAngle(degrees);
        }

        /// <summary>
        /// Clamps a given angle in degrees into the range [0, 360]
        /// </summary>
        /// <param name="degrees"></param>
        /// <returns></returns>
        public static Vector3 ClampNegativeAngle(Vector3 angular) {
            return new Vector3(ClampNegativeAngle(angular.x),
                               ClampNegativeAngle(angular.y),
                               ClampNegativeAngle(angular.z));
        }

        /// <summary>
        /// Calculates the inverse proportion of a value in a target range related to a given value in another range.
        /// </summary>
        /// <param name="value">Value in the original range.</param>
        /// <param name="rangeMinimumValue">Original range minimum value.</param>
        /// <param name="rangeMaximumValue">Original range maximum value.</param>
        /// <param name="targetMinimumValue">Target range minimum value. The result's minimum value.</param>
        /// <param name="targetMaximumValue">Target range maximum value. The result's maximum value.</param>
        /// <returns>A value comprised in a target range in inverse proportion to the given value in its original range.</returns>
        public static int InverseProportion(int value, int rangeMinimumValue, int rangeMaximumValue, int targetMinimumValue, int targetMaximumValue) {
            /* x = value | r = rangeMinimumValue | R = rangeMaximumValue
            * t = targetMinimumValue | T = targetMaximumValue
            * y = targetValue
            *
            * F(y) = t + (1 - (x-r)/(R-r)) * (T - t)
            * Use https://www.desmos.com/calculator to see the equation graphically. */

            int valueOffset = value - rangeMinimumValue;
            int rangeOffset = rangeMaximumValue - rangeMinimumValue;
            int valueProportion = valueOffset / rangeOffset;

            int targetRangeOffset = targetMaximumValue - targetMinimumValue;

            int targetValue = targetMinimumValue + (1 - valueProportion) * targetRangeOffset;
            return targetValue;
        }

        /// <summary>
        /// Calculates the inverse proportion of a value in a target range related to a given value in another range.
        /// </summary>
        /// <param name="value">Value in the original range.</param>
        /// <param name="rangeMinimumValue">Original range minimum value.</param>
        /// <param name="rangeMaximumValue">Original range maximum value.</param>
        /// <param name="targetMinimumValue">Target range minimum value. The result's minimum value.</param>
        /// <param name="targetMaximumValue">Target range maximum value. The result's maximum value.</param>
        /// <returns>A value comprised in a target range in inverse proportion to the given value in its original range.</returns>
        public static float InverseProportion(float value, float rangeMinimumValue, float rangeMaximumValue, float targetMinimumValue, float targetMaximumValue) {
            /* x = value | r = rangeMinimumValue | R = rangeMaximumValue
            * t = targetMinimumValue | T = targetMaximumValue
            * y = targetValue
            *
            * F(y) = t + (1 - (x-r)/(R-r)) * (T - t)
            * Use https://www.desmos.com/calculator to see the equation graphically. */

            float valueOffset = value - rangeMinimumValue;
            float rangeOffset = rangeMaximumValue - rangeMinimumValue;
            float valueProportion = valueOffset / rangeOffset;

            float targetRangeOffset = targetMaximumValue - targetMinimumValue;

            float targetValue = targetMinimumValue + (1 - valueProportion) * targetRangeOffset;
            return targetValue;
        }

        /// <summary>
        /// Solves the quadratic equation of the form At^2 + Bt + C = 0.
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <returns>The two values derived of solving t.</returns>
        public static float[] QuadraticFormula(float A, float B, float C) {
            // Quadratic Equation Form => At^2 + Bt + C = 0
            // Solving t = (-B +- Sqrt(B^2 - 4AC))/2A

            if (A == 0) {
                return null;
            }

            float[] results;

            float discriminant = B * B - 4 * A * C;
            if (discriminant == 0) {
                results = new float[1];
                results[0] = -B / 2 / A;
                return results;
            }

            float squaredDiscriminant = Mathf.Sqrt(discriminant);

            float addedNumerator = -B + squaredDiscriminant;
            float substractedNumerator = -B - squaredDiscriminant;

            results = new float[2];
            results[0] = addedNumerator / 2 * A;
            results[1] = substractedNumerator / 2 * A;

            return results;
        }

        public static float[] CubicFormula(float A, float B, float C, float D) {
            const float epsilon = 1e-9f;
            // Quadratic Equation Form => Ax^3 + Bx^2 + Cx + D = 0
            // Solving x = https://math.vanderbilt.edu/schectex/courses/cubic/cubic.gif
            // Solution from: https://github.com/forrestthewoods/lib_fts/blob/master/code/fts_ballistic_trajectory.cs

            if (A == 0) {
                return QuadraticFormula(A, B, C);
            }

            float[] results;

            // To eliminate quadric term, substitute x = y - A/3  ===> (x^3 + px + q = 0)
            float sq_A = A * A;
            float p = (-sq_A / 3f + B) / 3f;
            float q = ((2f * A * sq_A) / 27f - (A * B) / 3f + C) / 2f;
            
            if (D > epsilon) {  // One solution
                results = new float[1];

                float sqrt_D = (float) System.Math.Sqrt(D);
                float u = (float)System.Math.Pow(sqrt_D - q, 1.0 / 3.0);
                float v = (float)-System.Math.Pow(sqrt_D + q, 1.0 / 3.0);

                results[0] = u + v;

            } else if (D < epsilon) {  // Three real solutions
                results = new float[3];
                
                float cb_p = p * p * p; // Cardano's formula
                float phi = (float)System.Math.Acos(-q / System.Math.Sqrt(-cb_p)) / 3f;
                float t = 2f * (float)System.Math.Sqrt(-p);

                results[0] = t * (float)System.Math.Cos(phi);
                results[1] = -t * (float)System.Math.Cos(phi + System.Math.PI / 3);
                results[2] = -t * (float)System.Math.Cos(phi - System.Math.PI / 3);
                
            } else if (q >= -epsilon && q <= epsilon) { // One triple solution
                results = new float[1];
                results[0] = 0f;

            } else { // One single and one double solution
                float u = (float)System.Math.Pow(-q, 1.0 / 3.0);
                results = new float[2];
                results[0] = 2 * u;
                results[1] = -u;
            }

            // Resubstitute
            float sub = A / 3f; 

            if (results.Length > 0) results[0] -= sub;
            if (results.Length > 1) results[1] -= sub;
            if (results.Length > 2) results[2] -= sub;
            
            return results;
        }

        public static float[] QuarticFormula(float A, float B, float C, float D, float E) {
            const float epsilon = 1e-9f;
            // Quartic Equation Form => Ax^4 + Bx^3 + Cx^2 + Dx + E = 0
            // Solution from: https://github.com/forrestthewoods/lib_fts/blob/master/code/fts_ballistic_trajectory.cs

            float[] results;

            // To eliminate cubic term, substitute x = y - A/4 ===> (x^4 + px^2 + qx + r = 0)
            float sq_A = A * A;
            float p = (-3.0f * sq_A) / 8f + B;
            float q = (sq_A * A) / 8f - (A * B) / 2f + C;
            float r = (-3.0f * sq_A * sq_A) / 256f + (sq_A * B) / 16f - (A * C) / 4f + D;

            if (r >= -epsilon && r <= epsilon) {
                // No absolute term: y(y^3 + py + q) = 0
                results = CubicFormula(1, 0, p, q);

            } else {
                float s0 = 0, s1 = 0, s2 = 0, s3 = 0;

                results = CubicFormula( (r * p) / 2f - (q * q) / 8f, 
                                        -r, 
                                        -p/2f,
                                        1
                                      );

                float z = results[0];
                s1 = results[1];
                s2 = results[2];

                float u = z * z - r;
                float v = 2 * z - p;

                if (u >= -epsilon && u <= epsilon) {
                    u = 0;
                } else if (u > 0) {
                    u = Mathf.Sqrt(u);
                } else {
                    return null;
                }

                if (v >= -epsilon && v <= epsilon) {
                    v = 0;
                } else if (u > 0) {
                    v = Mathf.Sqrt(v);
                } else {
                    return null;
                }

                results = QuadraticFormula ( 1,
                                             q < 0 ? -v : v,
                                             z - u
                                           );

                float c0 = 1;
                float c1 = q < 0 ? v : v;
                float c2 = z + u;
                
                if (results.Length == 0) {
                    results = QuadraticFormula(c0, c1, c2);
                    s0 = results[0];
                    s1 = results[1];
                }

                if (results.Length == 1) {
                    results = QuadraticFormula(c0, c1, c2);
                    s1 = results[0];
                    s2 = results[1];
                }

                if (results.Length == 2) {
                    results = QuadraticFormula(c0, c1, c2);
                    s2 = results[0];
                    s3 = results[1];
                }

                results = new float[4] { s0, s1, s2, s3 };
            }

            // Resubstitute
            float sub = A / 4f;

            if (results.Length > 0) results[0] -= sub;
            if (results.Length > 1) results[1] -= sub;
            if (results.Length > 2) results[2] -= sub;
            if (results.Length > 3) results[3] -= sub;

            return results;
        }
    }
}