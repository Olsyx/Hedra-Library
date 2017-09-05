/*
 * Hedra's partial class responsible for all geometrical calculations.
 *
 * @author  Olsyx (Olatz Castaño)
 * @source https://github.com/Darkatom/Hedra-Library
 * @since   Unity 2017.1.0p4
 */

using Hedras.Objects;
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
        public static Vector3 GetPoint(Vector3 A, Vector3 B, float distance) {
            Vector3 AB = B - A;
            return A + AB.normalized * distance;
        }

        /// <summary>
        /// Returns a point at given degrees and distance from origin around the normal of a geometrical plane.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="radius"></param>
        /// <param name="degrees"></param>
        /// <returns></returns>
        public static Vector3 GetPoint(Vector3 origin, Vector3 normal, float radius, float degrees) {
            GeometricalPlane plane = new GeometricalPlane(normal);
            return plane.GetPoint(origin, radius, degrees);
        }
               

        /// <summary>
        /// Returns the points obtained from subdividing the AB vector, including both A and B.
        /// </summary>
        /// <param name="A">Origin of the vector.</param>
        /// <param name="B">End of the vector.</param>
        /// <param name="subdivisions">Number of subdivisions.</param>
        /// <returns></returns>
        public static List<Vector3> Subdivide(Vector3 A, Vector3 B, int subdivisions) {
            List<Vector3> points = new List<Vector3>();
            points.Add(A);
            points.Add(B);

            if (subdivisions <= 1) {
                return points;
            }

            Vector3 AB = B - A;
            float distance = AB.magnitude / subdivisions;
            for (int i = 1; i < subdivisions; i++) {
                Vector3 point = GetPoint(A, B, distance * i);
                points.Add(point);
            }

            return points;
        }

        /// <summary>
        /// Returns the angle between points A and B from a provided origin of coordenates.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="pointA"></param>
        /// <param name="pointB"></param>
        /// <returns></returns>
        public static float Angle(Vector3 origin, Vector3 pointA, Vector3 pointB) {
            Vector3 A = pointA - origin;
            Vector3 B = pointB - origin;

            return Angle(A, B);
        }

        /// <summary>
        /// Returns the angle between vectors A and B.
        /// https://www.gamedev.net/forums/topic/509720-angle-between-two-3d-points/
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static float Angle(Vector3 A, Vector3 B) {
            float dotAB = Vector3.Dot(A, B);
            float lengthsCombined = A.magnitude * B.magnitude;

            float angle = Mathf.Acos(dotAB / lengthsCombined);
            return angle * Mathf.Rad2Deg;
        }

        /// <summary>
        /// Area of the triangle formed by the points A, B and C.
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <returns></returns>
        public static float Area(Vector3 A, Vector3 B, Vector3 C) {
            // Heron's formula
            Vector3 CB = B - C;
            Vector3 CA = A - C;

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
        public static float PerpendicularDistance(Vector3 A, Vector3 B, Vector3 P) {
            float a = (B - P).magnitude;

            Vector3 BP = P - B;
            Vector3 BA = A - B;
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
        public static float PerpendicularBase(Vector3 A, Vector3 B, Vector3 P) {
            // Pythagoras theorem: h2 = a2 + b2
            Vector3 AP = P - A;
            Vector3 AB = B - A;

            float hypotenuse = AP.magnitude;
            float perpendicularDistance = PerpendicularDistance(A, B, P); // Triangle height on P

            float baseIntersectionLength = Mathf.Sqrt(Mathf.Pow(hypotenuse, 2) - Mathf.Pow(perpendicularDistance, 2));
            float sign = Mathf.Clamp(Vector3.Dot(AP, AB), -1, 1);

            return baseIntersectionLength * sign;
        }
        
        /// <summary>
        /// Returns the point of intersection of the perpendicular from point P to vector AB.
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="P"></param>
        /// <returns></returns>
        public static Vector3 PerpendicularPoint(Vector3 A, Vector3 B, Vector3 P) {
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
        public static float ProjectionValue(Vector3 A, Vector3 B, Vector3 P) {
            Vector3 AB = B - A;
            Vector3 AP = P - A;
            return Vector3.Dot(AB, AP);
        }

        /// <summary>
        /// Returns the closest point to P on the segment AB. If P is not projectable over AB, the closest end is returned.
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="P"></param>
        /// <returns></returns>
        public static Vector3 ClosestPointOnSegment(Vector3 A, Vector3 B, Vector3 P) {
            Vector3 intersectionPoint = PerpendicularPoint(A, B, P);

            if (ProjectionValue(A, B, intersectionPoint) < 0f) {
                return A;
            }

            if (ProjectionValue(B, A, intersectionPoint) < 0f) {
                return B;
            }

            return intersectionPoint;
        }


    }

