/*
* Hedra's partial class responsible for all geometrical calculations.
*
* @author  Olsyx (Olatz Castaño)
* @source  https://github.com/Olsyx/Hedra-Library
* @since   Unity 2017.1.0p4
* @last    Unity 2018.3.0f2
*/

using System.Collections;
using System.Collections.Generic;
using HedraLibrary.Components;
using UnityEngine;

namespace HedraLibrary {
    public static partial class Hedra {

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
            float sign = Vector3.Dot(AP, AB) < 0 ? -1 : 1;

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

}