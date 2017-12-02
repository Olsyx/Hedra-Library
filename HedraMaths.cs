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
        /// Hard clamps the value to the nearest limit.
        /// </summary>
        /// <param name="value">Value to clamp.</param>
        /// <param name="upperLimit"></param>
        /// <param name="bottomLimit"></param>
        /// <returns></returns>
        public static int HardClamp(int value, int bottomLimit, int upperLimit) {
            return (int)HardClamp(value, bottomLimit, upperLimit);
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
            return (int)HardClamp(value, bottomLimit, centerLimit, upperLimit);
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

    }
}