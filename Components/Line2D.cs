using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HedraLibrary.Components {
    public struct SlopeInterceptEquation {

        // Slope-Intercept line equation y = mx + b.
        public float M;
        public float B;
        public SlopeInterceptEquation(float m, float b) {
            M = m;
            B = b;
        }

        public SlopeInterceptEquation(Vector2 pointA, Vector2 pointB) {
            Vector2 direction = pointB - pointA;
            if (direction.y == 0) {
                M = 0;
            } else {
                M = direction.y / direction.x;
            }
            B = pointA.y - M * pointA.x;
        }

        /// <summary>
        /// Calculate the X with a given Y, so that (X,Y) is on the line.
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        public float GetX(float y) {
            return (y - B) / M;
        }

        /// <summary>
        /// Calculate the Y with a given X, so that (X,Y) is on the line.
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        public float GetY(float x) {
            return M * x + B;
        }

        public override string ToString() {
            return "y = " + M.ToString("0.00") + "x + " + B.ToString("0.00");
        }
    }

    public struct LineEquation {
        // Common line equation A * x + B * y + C = 0
        public float A;
        public float B;
        public float C;
        public LineEquation(float a, float b, float c) {
            A = a;
            B = b;
            C = c;
        }

        public LineEquation(Vector2 pointA, Vector2 pointB) {
            A = -(pointB.y - pointA.y);
            B = pointB.x - pointA.x;
            C = -(A * pointA.x + B * pointA.y);
        }

        /// <summary>
        /// Calculate the X with a given Y, so that (X,Y) is on the line.
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        public float GetX(float y) {
            return (-C - B * y) / A;
        }

        /// <summary>
        /// Calculate the Y with a given X, so that (X,Y) is on the line.
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        public float GetY(float x) {
            return (-C - A * x) / B;
        }

        /// <summary>
        /// Returns the situation of the point regarding this line or segment.
        /// </summary>
        /// <param name="point"></param>
        /// <returns> > 0, left; 0, on the line; < 0, right.</returns>
        public float GetPointSituation(Vector2 point) {
            return GetPointSituation(point.x, point.y);
        }

        /// <summary>
        /// Returns the situation of the point regarding this line or segment.
        /// </summary>
        /// <param name="point"></param>
        /// <returns> > 0, left; 0, on the line; < 0, right.</returns>
        public float GetPointSituation(float x, float y) {
            return A * x + B * y + C;
        }

        public override string ToString() {
            return A.ToString("0.00") + "*x + " + B.ToString("0.00") + "*y + " + C.ToString("0.00") + " = 0";
        }
    }

    public class Line2D {
        protected const float EPSILON = 0.001f;   // Constant for float precise operations

        // Line points
        public Vector2 PointA { get; protected set; }
        public Vector2 PointB { get; protected set; }
        public Vector2 Direction { get; protected set; }

        public LineEquation Equation { get; protected set; }
        public SlopeInterceptEquation SlopeEquation { get; protected set; }

        public Line2D(Vector2 pointA, Vector2 pointB) {
            this.PointA = pointA;
            this.PointB = pointB;
            Direction = this.PointB - this.PointA;
            Equation = new LineEquation(PointA, PointB);
            SlopeEquation = new SlopeInterceptEquation(PointA, PointB);
        }

        public virtual bool Intersects(Vector2 A, Vector2 B, bool onSegment) {
            if (onSegment) {
                Segment2D segment = new Segment2D(A, B);
                return Intersects(segment);
            } else {
                Line2D line = new Segment2D(A, B);
                return Intersects(line);
            }
        }


        public virtual bool Intersects(Segment2D segment) {
            Vector2 point = IntersectionPoint(segment);
            return !point.IsNaN() && !point.IsInfinity();
        }
        
        public virtual bool Intersects(Line2D line) {
            Vector2 point = IntersectionPoint(line);
            return !point.IsNaN() && !point.IsInfinity();
        }

        /// <summary>
        /// Calculates the intersection point of two lines.
        /// </summary>
        /// <param name="other">The line to intersect with this one.</param>
        /// <param name="segment">Segment intersection; the point must be contained in the line segment. If not contained, the returned Vector has int.MaxValue.</param>
        /// <returns></returns>
        public virtual Vector2 IntersectionPoint(Vector2 pointA, Vector2 pointB) {
            Line2D line = new Line2D(pointA, pointB);
            return this.IntersectionPoint(line);
        }

        /// <summary>
        /// Calculates the intersection point of two lines.
        /// </summary>
        /// <param name="other">The line to intersect with this one.</param>
        /// <param name="segment">Segment intersection; the point must be contained in the line segment. If not contained, the returned Vector has int.MaxValue.</param>
        /// <returns></returns>
        public virtual Vector2 IntersectionPoint(Line2D other) {
            float x, y;

            if ((SlopeEquation.M == 0 && other.SlopeEquation.M == 0) || (float.IsNaN(SlopeEquation.M) && float.IsNaN(other.SlopeEquation.M))) { // Horizontal && Vertical
                return new Vector2(float.NaN, float.NaN);

            } else if (float.IsNaN(SlopeEquation.M) || float.IsInfinity(SlopeEquation.M)) {
                x = PointA.x;
                y = other.SlopeEquation.GetY(x);
            } else if (float.IsNaN(other.SlopeEquation.M) || float.IsInfinity(other.SlopeEquation.M)) {
                x = other.PointA.x;
                y = SlopeEquation.GetY(x);
            } else {
                x = (other.SlopeEquation.B - SlopeEquation.B) / (SlopeEquation.M - other.SlopeEquation.M);
                y = SlopeEquation.GetY(x);
            }

            Vector2 point = new Vector2(x, y);
            return point;
        }

        /// <summary>
        /// Returns true if the point is contained in this line.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public virtual bool Contains(Vector2 point) {
            Vector2 AP = point - PointA;
            float cross = AP.x * Direction.y - AP.y * Direction.x;
            return (cross == 0);
        }

        /// <summary>
        /// Returns true if the point is contained in this segment or as close as a given margin.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public virtual bool Contains(Vector2 point, float margin) {
            float distance = Vector2.Distance(PerpendicularPoint(point), point);

            return Contains(point) || distance <= margin;
        }


        /// <summary>
        /// Returns the situation of the point regarding this line or segment.
        /// </summary>
        /// <param name="point"></param>
        /// <returns> > 0, left; 0, on the line; < 0, right.</returns>
        public virtual float GetPointSituation(Vector2 point) {
            return Equation.GetPointSituation(point);
        }

        /// <summary>
        /// Returns the intersection point of this line with a perpendicular line casted from the given point.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public virtual Vector2 PerpendicularPoint(Vector2 point) {
            return Hedra.PerpendicularPoint(PointA, PointB, point);
        }

        public override string ToString() {
            return Equation.ToString() + " | " + SlopeEquation.ToString();
        }
    }
}