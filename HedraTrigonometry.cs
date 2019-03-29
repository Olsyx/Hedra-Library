/*
* Hedra's partial class responsible for all trigonometrical calculations.
*
* @author  Olsyx (Olatz Castaño)
* @source https://github.com/Darkatom/Hedra-Library
* @since   Unity 2017.1.0p4
* @last    Unity 2017.2.0f3
*/

using System.Collections;
using System.Collections.Generic;
using HedraLibrary.Shapes;
using HedraLibrary.Shapes.Polygons;
using UnityEngine;
using System.Linq;

namespace HedraLibrary {
    public static partial class Hedra {
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
        /// Given the vector AB, and a point P, returns the distance from A to the intersecting point X, where PX is perpendicular to AB.
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
            float sign = HardClamp(Vector2.Dot(AP.normalized, AB.normalized), -1, 1);
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

        /// <summary>
        /// Calculates the intersection of the lines or segments AB and CD.
        /// </summary>
        /// <param name="A">First point of Line 1.</param>
        /// <param name="B">Second point of Line 1.</param>
        /// <param name="C">First point of Line 2.</param>
        /// <param name="D">Second point of Line 2.</param>
        /// <param name="onSegment">If true, point must be contained in both segments.</param>
        /// <returns>The intersection point of two lines or segments.(NaN, NaN) will be returned 
        /// if they are parallel or the intersection point is not contained on both segments, 
        /// should On Segment be true.</returns>
        public static Vector2 IntersectionPoint(Vector2 A, Vector2 B, Vector2 C, Vector2 D, bool onSegment) {
            if (onSegment) {
                Segment2D L1 = new Segment2D(A, B);
                Segment2D L2 = new Segment2D(C, D);
                return L1.IntersectionPoint(L2);
            } else {
                Line2D L1 = new Line2D(A, B);
                Line2D L2 = new Line2D(C, D);
                return L1.IntersectionPoint(L2);
            }
        }

        /// <summary>
        /// Calculates the intersection point of the line or segment AB with a Collider.
        /// </summary>
        /// <param name="collider"></param>
        /// <param name="pointA">First point.</param>
        /// <param name="pointB">Second point.</param>
        /// <param name="onSegment">If true, point must be contained by the colliding segments (both collider edges and AB).</param>
        /// <returns>The intersection points of a line or segment with the edges of a collider.</returns>
        public static List<Vector2> IntersectionPoints(Collider2D collider, Vector2 pointA, Vector2 pointB, bool onSegment) {
            if (onSegment) {
                Segment2D segment = new Segment2D(pointA, pointB);
                return IntersectionPoints(collider, segment);
            } else {
                Line2D line = new Segment2D(pointA, pointB);
                return IntersectionPoints(collider, line);
            }
        }
        
        /// <summary>
        /// Calculates the intersection point of the segment AB with a Collider.
        /// </summary>
        /// <param name="collider"></param>
        /// <param name="pointA">First point.</param>
        /// <param name="pointB">Second point.</param>
        /// <returns>The intersection points of a segment with the edges of a collider.</returns>
        public static List<Vector2> IntersectionPoints(Collider2D collider, Segment2D segment) {
            Polygon polygon = PolygonManager.Create2D(collider);
            return polygon.IntersectionPoints(segment);
        }

        /// <summary>
        /// Calculates the intersection point of the line AB with a Collider.
        /// </summary>
        /// <param name="collider"></param>
        /// <param name="pointA">First point.</param>
        /// <param name="pointB">Second point.</param>
        /// <returns>The intersection points of a line with the edges of a collider.</returns>
        public static List<Vector2> IntersectionPoints(Collider2D collider, Line2D line) {
            Polygon polygon = PolygonManager.Create2D(collider);
            return polygon.IntersectionPoints(line);
        }

        /// <summary>
        /// Sorts a list of points around a center.
        /// </summary>
        /// <param name="center"></param>
        /// <param name="points"></param>
        /// <param name="clockwise"></param>
        /// <returns>Returns a sorted list of points around a center.</returns>
        public static List<Vector2> SortAroundCenter(Vector2 center, List<Vector2> points, bool clockwise) {
            List<Vector2> sortedPoints = null;
            sortedPoints = Hedra.Copy(points);

            sortedPoints.Sort((v1, v2) => { 
                float angle1 = Hedra.Angle(center, center + Vector2.up, v1);
                float angle2 = Hedra.Angle(center, center + Vector2.up, v2);

                if (angle1 > angle2) {
                    return 1;
                } else if (angle1 == angle2) {
                    return 0;
                } else {
                    return -1;
                }
                
            });

            return sortedPoints;
        }

        /// <summary>
        /// Sorts a list of points around a center.
        /// </summary>
        /// <param name="center"></param>
        /// <param name="points"></param>
        /// <param name="clockwise"></param>
        /// <returns>Returns a sorted list of points around a center.</returns>
        public static Vector2[] SortAroundCenter(Vector2 center, Vector2[] points, bool clockwise) {
            List<Vector2> sortedPoints = null;
            sortedPoints = Hedra.Copy(points.ToList());

            sortedPoints.Sort((v1, v2) => {
                float angle1 = Hedra.Angle(center, center + Vector2.left, v1);
                float angle2 = Hedra.Angle(center, center + Vector2.left, v2);

                if (angle1 > angle2) {
                    return 1;
                } else if (angle1 == angle2) {
                    return 0;
                } else {
                    return -1;
                }

            });

            return sortedPoints.ToArray();
        }

    }
}