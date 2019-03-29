using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HedraLibrary.Shapes {

    public class Segment2D : Line2D {

        public Vector2 MiddlePoint { get; protected set; }

        public Segment2D(Vector2 pointA, Vector2 pointB) : base(pointA, pointB) {
            MiddlePoint = Hedra.MidPoint(pointA, pointB);
        }

        public Segment2D(Vector2[] points) : base(points[0], points[1]) {
            MiddlePoint = Hedra.MidPoint(points[0], points[1]);
        }

        public Segment2D(Segment2D original) : base(original) {
            MiddlePoint = original.MiddlePoint;
        }

        #region Control
        public static Segment2D operator *(Segment2D segment, float value) {
            Vector2 resultB = segment.PointA + segment.Vector * value;
            return new Segment2D(segment.PointA, resultB);
        }

        public Segment2D Scale (float value) {
            Vector2 resultA = this.PointB + (this.PointA - this.PointB) * value;
            Vector2 resultB = this.PointA + (this.PointB - this.PointA) * value;

            return new Segment2D(resultA, resultB);
        }

        public override void Translate(Vector2 direction) {
            base.Translate(direction);
            MiddlePoint = Hedra.MidPoint(PointA, PointB);
        }
        #endregion

        /// <summary>
        /// Returns true if the point is contained in this segment.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public override bool Contains(Vector2 point) {
            if (point == PointA || point == PointB) {
                return true;
            }

            float value = Vector2.Distance(PointA, point) + Vector2.Distance(point, PointB) - Vector2.Distance(PointA, PointB);
            value = Hedra.Truncate(value, 3);
            return -EPSILON <= value && value <= EPSILON;
        }

        /// <summary>
        /// Returns the closest end point (Point A or B) of the segment to a point.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public Vector2 ClosestEndPoint(Vector2 point) {
            if (Vector2.Distance(point, PointA) > Vector2.Distance(point, PointB)) {
                return PointB;
            }

            return PointA;
        }

        /// <summary>
        /// Returns the closest end point (Point A or B) of the segment to another segment.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public Vector2 ClosestEndPoint(Segment2D other) {
            float closestA = other.PerpendicularDistance(PointA);
            float closestB = other.PerpendicularDistance(PointB);

            return closestA <= closestB ? PointA : PointB;
        }

        /// <summary>
        /// Returns the furthest end point (Point A or B) of the segment to a point.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public Vector2 FurthestEndPoint(Vector2 point) {
            if (Vector2.Distance(point, PointA) > Vector2.Distance(point, PointB)) {
                return PointA;
            }

            return PointB;
        }
        
        /// <summary>
        /// Returns the closest end point (Point A or B) of the segment to another segment.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public Vector2 FurthestEndPoint(Segment2D other) {
            float closestA = other.PerpendicularDistance(PointA);
            float closestB = other.PerpendicularDistance(PointB);

            return closestA >= closestB ? PointA : PointB;
        }

        /// <summary>
        /// Calculates the intersection point of this segment with the segment AB.
        /// </summary>
        /// <param name="other">The line to intersect with this one.</param>
        /// /// <returns></returns>
        public override Vector2 IntersectionPoint(Vector2 pointA, Vector2 pointB) {
            Segment2D segment = new Segment2D(pointA, pointB);
            return this.IntersectionPoint(segment);
        }

        /// <summary>
        /// Calculates the intersection point of a line and a segment.
        /// </summary>
        /// <param name="other">The line to intersect with this one.</param>
        /// /// <returns></returns>
        public override Vector2 IntersectionPoint(Line2D other) {
            Vector2 point = base.IntersectionPoint(other);
            if (float.IsNaN(point.x) || float.IsInfinity(point.x)) {
                return new Vector2(float.NaN, float.NaN);
            }

            if (Contains(point)) {
                return point;
            }

            return new Vector2(float.NaN, float.NaN);
        }

        /// <summary>
        /// Calculates the intersection point of two segments
        /// </summary>
        /// <param name="other">The line to intersect with this one.</param>
        /// <param name="segment">Segment intersection; the point must be contained in the line segment. If not contained, the returned Vector has int.MaxValue.</param>
        /// <returns></returns>
        public virtual Vector2 IntersectionPoint(Segment2D other) {
            Vector2 point = base.IntersectionPoint(other);
            if (float.IsNaN(point.x) || float.IsInfinity(point.x)) {
                return new Vector2(float.NaN, float.NaN);
            }

            if (Contains(point) && other.Contains(point)) {
                return point;
            }

            return new Vector2(float.NaN, float.NaN);
        }

        /// <summary>
        /// Returns the intersection point of this segment with a perpendicular line casted from the given point, 
        /// or the closest if the intersection would not be on segment.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public override Vector2 PerpendicularPoint(Vector2 point) {
            float triangleBase = PerpendicularBase(point);
            Vector2 intersectionPoint = GetPoint(triangleBase);

            Vector2 AP = intersectionPoint - PointA;
            if (Vector2.Dot(Vector, AP) < 0f) {
                return PointA;
            }

            Vector2 BP = intersectionPoint - PointB;
            if (Vector2.Dot(-Vector, BP) < 0f) {
                return PointB;
            }

            return intersectionPoint;
        }
        
        public Line2D ToLine() {
            return new Line2D(PointA, PointB);
        }
        
        public Vector2[] ToArray() {
            return new Vector2[]{PointA, PointB};
        }

        public override string ToString() {
            return "[" + PointA + ", " + PointB + "]";
        }
        
        public override void DrawGizmos(bool drawPoints, float size = 0.2f) {
            Gizmos.DrawLine(PointA, PointB);
            if (drawPoints) {
                Gizmos.DrawWireSphere(PointA, size);
                Gizmos.DrawWireSphere(PointB, size);
                Gizmos.DrawWireSphere(MiddlePoint, size);
            }
        }

        public virtual void DrawVector(float angle = 35, float size = 0.2f, bool oppositeDirection = false) {
            Gizmos.DrawLine(PointA, PointB);

            Vector2 origin = oppositeDirection ? PointA : PointB;
            Vector2 vector = oppositeDirection ? Vector.normalized : -Vector.normalized;
            Vector2 flapPoint = origin + vector * size;

            Vector2 flapA = Hedra.Rotate(origin, flapPoint, angle);
            Vector2 flapB = Hedra.Rotate(origin, flapPoint, -angle);
            
            Gizmos.DrawLine(origin, flapA);
            Gizmos.DrawLine(origin, flapB);
        }
    }
}