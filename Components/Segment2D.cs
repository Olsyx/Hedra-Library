using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HedraLibrary.Components {

    public class Segment2D : Line2D {

        public Vector2 Center { get; protected set; }

        public Segment2D(Vector2 pointA, Vector2 pointB) : base(pointA, pointB) {
            Center = Hedra.MidPoint(pointA, pointB);
        }
        

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
        /// Returns true if the point is contained in this segment.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public override bool Contains(Vector2 point) {
            float value = Vector2.Distance(PointA, point) + Vector2.Distance(point, PointB) - Vector2.Distance(PointA, PointB);
            return -EPSILON < value && value < EPSILON;
        }
        

        /// <summary>
        /// Returns the intersection point of this segment with a perpendicular line casted from the given point, 
        /// or the closest if the intersection would not be on segment.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public override Vector2 PerpendicularPoint(Vector2 point) {
            return Hedra.ClosestPointOnSegment(PointA, PointB, point);
        }

        public Vector2[] ToArray() {
            return new Vector2[]{PointA, PointB};
        }

        public override string ToString() {
            return "[" + PointA + ", " + PointB + "]";
        }
    }
}