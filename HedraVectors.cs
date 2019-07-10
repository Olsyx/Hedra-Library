/*
* Hedra's partial class responsible for all basic spatial vector calculations.
*
* @author  Olsyx (Olatz Castaño)
* @source  https://github.com/Olsyx/Hedra-Library
* @since   Unity 2017.1.0p4
* @last    Unity 2018.3.0f2
*/

using System.Collections;
using System.Collections.Generic;
using HedraLibrary.Components;
using HedraLibrary.Shapes;
using UnityEngine;

namespace HedraLibrary {
    public static partial class Hedra {
        
        /// <summary>
        /// Returns a Random Vector in the [-1, 1] value range.
        /// </summary>
        /// <returns></returns>
        public static Vector3 RandomVector() {
            return RandomVector(-1000f, 1000f) / 1000f; 
        }

        /// <summary>
        /// Returns a Random Vector in the given value range.
        /// </summary>
        /// <returns></returns>
        public static Vector3 RandomVector(float minValue, float maxValue) {
            Vector3 vector;
            vector.x = UnityEngine.Random.Range(minValue, maxValue);
            vector.y = UnityEngine.Random.Range(minValue, maxValue);
            vector.z = UnityEngine.Random.Range(minValue, maxValue);
            return vector;
        }

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
        /// Returns a point at given degrees and distance from origin around the normal of a geometrical plane.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="radius"></param>
        /// <param name="degrees"></param>
        /// <returns></returns>
        public static Vector3 GetPoint(Vector3 origin, Vector3 normal, float radius, float degrees) {
            Components.Plane plane = new Components.Plane(normal);
            return plane.GetPoint(origin, radius, degrees);
        }

        /// <summary>
        /// Returns the center point of the AB vector.
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static Vector2 MidPoint(Vector2 A, Vector2 B) {
            Vector2 direction = B - A;
            return new Vector2(A.x + direction.x / 2, A.y + direction.y / 2);
        }

        /// <summary>
        /// Returns the center point of the AB vector.
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static Vector3 MidPoint(Vector3 A, Vector3 B) {
            Vector3 direction = B - A;
            return new Vector3(A.x + direction.x / 2, A.y + direction.y / 2);
        }

        /// <summary>
        /// Returns the bisector of vectors A and B.
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static Vector2 Bisector(Vector2 A, Vector2 B) {
            Vector2 midPoint = MidPoint(A, B);
            return midPoint;
        } 
        
        /// <summary>
        /// Returns the bisector of vectors A and B from origin O.
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static Vector2 Bisector(Vector2 O, Vector2 A, Vector2 B) {
            Vector2 midPoint = MidPoint(A, B);
            return midPoint - O;
        } 
        
        /// <summary>
          /// Returns the bisector of vectors A and B.
          /// </summary>
          /// <param name="A"></param>
          /// <param name="B"></param>
          /// <returns></returns>
        public static Vector3 Bisector(Vector3 A, Vector3 B) {
            Vector3 midPoint = MidPoint(A, B);
            return midPoint;
        }

        /// <summary>
        /// Returns the bisector of vectors A and B from origin O.
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static Vector3 Bisector(Vector3 O, Vector3 A, Vector3 B) {
            Vector3 midPoint = MidPoint(A, B);
            return midPoint - O;
        }

        /// <summary>
        /// Returns the points obtained from subdividing the AB vector, including both A and B.
        /// </summary>
        /// <param name="A">Origin of the vector.</param>
        /// <param name="B">End of the vector.</param>
        /// <param name="divisions">Number of whole parts of the vector after subdivision.</param>
        /// <returns></returns>
        public static List<Vector2> Subdivide(Vector2 A, Vector2 B, int divisions) {
            List<Vector2> points = new List<Vector2>();
            points.Add(A);
            points.Add(B);

            if (divisions <= 1) {
                return points;
            }

            Vector2 AB = B - A;
            float distance = AB.magnitude / divisions;
            for (int i = 1; i < divisions; i++) {
                Vector2 point = GetPoint(A, B, distance * i);
                points.Add(point);
            }

            return points;
        }
        
        /// <summary>
        /// Returns the points obtained from subdividing the AB vector, including both A and B.
        /// </summary>
        /// <param name="A">Origin of the vector.</param>
        /// <param name="B">End of the vector.</param>
        /// <param name="divisions">Number of subdivisions.</param>
        /// <returns></returns>
        public static List<Vector3> Subdivide(Vector3 A, Vector3 B, int divisions) {
            List<Vector3> points = new List<Vector3>();
            points.Add(A);
            points.Add(B);

            if (divisions <= 1) {
                return points;
            }

            Vector3 AB = B - A;
            float distance = AB.magnitude / divisions;
            for (int i = 1; i < divisions; i++) {
                Vector3 point = GetPoint(A, B, distance * i);
                points.Add(point);
            }

            return points;
        }

        /// <summary>
        /// Returns the angle degrees between points A and B from a provided origin of coordenates.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static float Angle(Vector2 origin, Vector2 A, Vector2 B) {
            Vector2 pointA = A - origin;
            Vector2 pointB = B - origin;

            return Angle(pointA, pointB);
        }

        /// <summary>
        /// Returns the angle degrees between vectors A and B.
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static float Angle(Vector2 A, Vector2 B) {
            float angle = Mathf.Atan2(B.y, B.x) - Mathf.Atan2(A.y, A.x);
            return angle * Mathf.Rad2Deg;
        }

        /// <summary>
        /// Returns the angle degrees between points A and B from a provided origin of coordenates.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static float Angle(Vector3 origin, Vector3 A, Vector3 B) {
            Vector3 pointA = A - origin;
            Vector3 pointB = B - origin;

            return Angle(pointA, pointB);
        }

        /// <summary>
        /// Returns the angle degrees between vectors A and B.
        /// https://www.gamedev.net/forums/topic/509720-angle-between-two-3d-points/
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static float Angle(Vector3 A, Vector3 B) {
            Vector3 a = A.normalized;
            Vector3 b = B.normalized;

            Vector3 perpendicular = Vector3.Cross(a, b).normalized;
            Vector3 normal = Hedra.Abs(perpendicular);

            // atan2((Vb x Va).Vn, Va.Vb) - https://stackoverflow.com/questions/5188561/signed-angle-between-two-3d-vectors-with-same-origin-within-the-same-plane
            return Mathf.Atan2(Vector3.Dot(perpendicular, normal), Vector3.Dot(a, b)) * Mathf.Rad2Deg;
        }

        public static float Angle(Segment2D a, Segment2D b) {
            if (a.Intersects(b)) {
                return Angle(a.IntersectionPoint(b), a.Vector, b.Vector);
            }

            Vector2 origin = a.ToLine().IntersectionPoint(b.ToLine());
            return Angle(origin, a.Vector, b.Vector);
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

    }
}