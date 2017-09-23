/*
 * Hedra's partial class responsible for all trigonometrical calculations.
 *
 * @author  Olsyx (Olatz Castaño)
 * @source https://github.com/Darkatom/Hedra-Library
 * @since   Unity 2017.1.0p4
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class Hedra {
        
        /// <summary>
        /// Returns a point P on vector AB at given distance from A.
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public static Vector2 GetPoint(Vector2 A, Vector2 B, float distance) {
            Vector2 AB = B - A;
            return A + AB.normalized * distance;
        }

        /// <summary>
        /// Returns a point in the circle of radius R at D degrees.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="radius"></param>
        /// <param name="degrees"></param>
        /// <returns></returns>
        public static Vector2 GetPoint(Vector2 origin, float radius, float degrees) {
            float angle = degrees * Mathf.Deg2Rad;
            Vector2 point = origin;
            point.x = origin.x + radius * Mathf.Cos(angle);
            point.y = origin.y + radius * Mathf.Sin(angle);
            return point;
        }

        /// <summary>
        /// Returns a point in the circle of radius R at D degrees.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="radius"></param>
        /// <param name="degrees"></param>
        /// <returns></returns>
        public static Vector2 GetPoint(Vector2 origin, float horizontalRadius, float verticalRadius, float degrees) {
            float angle = degrees * Mathf.Deg2Rad;
            Vector2 point = origin;
            point.x = origin.x + horizontalRadius * Mathf.Cos(angle);
            point.y = origin.y + verticalRadius * Mathf.Sin(angle);
            return point;
        }

        /// <summary>
        /// Returns the points obtained from subdividing the AB vector, including both A and B.
        /// </summary>
        /// <param name="A">Origin of the vector.</param>
        /// <param name="B">End of the vector.</param>
        /// <param name="subdivisions">Number of subdivisions.</param>
        /// <returns></returns>
        public static List<Vector2> Subdivide(Vector2 A, Vector2 B, int subdivisions) {
            List<Vector2> points = new List<Vector2>();
            points.Add(A);
            points.Add(B);

            if (subdivisions <= 1) {
                return points;
            }

            Vector2 AB = B - A;
            float distance = AB.magnitude / subdivisions;
            for (int i = 1; i < subdivisions; i++) {
                Vector2 point = GetPoint(A, B, distance * i);
                points.Add(point);
            }

            return points;
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
        public static float ClampAngle(float degrees) {
            float correctAngle = degrees % 360f;

            if (correctAngle < 0) {
                correctAngle = correctAngle + 360f;
            }

            return correctAngle;
        }

        /// <summary>
        /// Returns the angle between points A and B from a provided origin of coordenates.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="pointA"></param>
        /// <param name="pointB"></param>
        /// <returns></returns>
        public static float Angle(Vector2 origin, Vector2 pointA, Vector2 pointB) {
            Vector2 A = pointA - origin;
            Vector2 B = pointB - origin;

            return Angle(A, B);
        }

        /// <summary>
        /// Returns the angle between vectors A and B.
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static float Angle(Vector2 A, Vector2 B) {
            float angle = Mathf.Atan2(A.y, A.x) - Mathf.Atan2(B.y, B.x);
            return angle * Mathf.Rad2Deg;
        }

        /// <summary>
        /// Rotates point P by the given degrees around origin.
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="degrees"></param>
        /// <returns></returns>
        public static Vector2 Rotate(Vector2 origin, Vector2 P, float degrees) {
            Vector2 vector = P - origin;
            return origin + Rotate(vector, degrees);
        }

        /// <summary>
        /// Rotates a vector by the given degrees.
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="degrees"></param>
        /// <returns></returns>
        public static Vector2 Rotate(Vector2 vector, float degrees) {
            float angle = degrees * Mathf.Deg2Rad;
            Vector2 result = vector;
            result.x = vector.x * Mathf.Cos(angle) - vector.y * Mathf.Sin(angle);
            result.y = vector.x * Mathf.Sin(angle) + vector.y * Mathf.Cos(angle);
            return result;
        }
        
        /// <summary>
        /// Area of the triangle formed by the points A, B and C.
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <returns></returns>
        public static float Area(Vector2 A, Vector2 B, Vector2 C) {
            // Heron's formula
            Vector2 CB = B - C;
            Vector2 CA = A - C;

            float angleC = Angle(CB, CA) * Mathf.Deg2Rad;

            float a = (B - C).magnitude;
            float b = CA.magnitude;

            float area = 0.5f * a * b * Mathf.Sin(angleC);
            return area;
        }
        
        /// <summary>
        /// Returns the unsigned shortest distance from point P to vector AB. Also called triangle height.
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="P"></param>
        /// <returns></returns>
        public static float PerpendicularDistance(Vector2 A, Vector2 B, Vector2 P) {
            float a = (B - P).magnitude;

            Vector2 BP = P - B;
            Vector2 BA = A - B;
            float angleB = Angle(BP, BA) * Mathf.Deg2Rad;

            float height = a * Mathf.Sin(angleB);
            return Mathf.Abs(height);
        }

        /// <summary>
        /// Returns the distance from A to the intersecting point of P over vector AB.
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="P"></param>
        /// <returns></returns>
        public static float PerpendicularBase(Vector2 A, Vector2 B, Vector2 P) {
            // Pythagoras theorem: h2 = a2 + b2
            Vector2 AP = P - A;
            Vector2 AB = B - A;

            float hypotenuse = AP.magnitude;
            float perpendicularDistance = PerpendicularDistance(A, B, P); // Triangle height on P

            float baseIntersectionLength = Mathf.Sqrt(Mathf.Pow(hypotenuse, 2) - Mathf.Pow(perpendicularDistance, 2));
            float sign = Mathf.Clamp(Vector2.Dot(AP, AB), -1, 1);

            return baseIntersectionLength * sign;
        }

        /// <summary>
        /// Returns the point of intersection of the perpendicular from point P to vector AB.
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="P"></param>
        /// <returns></returns>
        public static Vector2 PerpendicularPoint(Vector2 A, Vector2 B, Vector2 P) {
            float triangleBase = PerpendicularBase(A, B, P);
            return GetPoint(A, B, triangleBase);
        }

        /// <summary>
        /// Returns the projection value of P on the vector AB.
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="P">param>
        /// <param name="reverse"></returns>
        public static float ProjectionValue(Vector2 A, Vector2 B, Vector2 P) {
            Vector2 AB = B - A;
            Vector2 AP = P - A;
            return Vector2.Dot(AB, AP);
        }
              
        /// <summary>
        /// Returns the closest point to P on the segment AB. If P is not projectable over AB, the closest end is returned.
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="P"></param>
        /// <returns></returns>
        public static Vector2 ClosestPointOnSegment(Vector2 A, Vector2 B, Vector2 P) {
            Vector2 intersectionPoint = PerpendicularPoint(A, B, P);

            if (ProjectionValue(A, B, intersectionPoint) < 0f) {
                return A;
            }

            if (ProjectionValue(B, A, intersectionPoint) < 0f) {
                return B;
            }

            return intersectionPoint;
        }
        
    }
