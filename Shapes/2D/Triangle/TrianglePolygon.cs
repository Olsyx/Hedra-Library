using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HedraLibrary.Components {
    public partial class Triangle : Polygon {

        public override Vector2 ClosestPointTo(Line2D line) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the closest point of this triangle to a segment.
        /// </summary>
        /// <param name="point"></param>
        /// <returns>The closest point of this triangle to a point.</returns>
        public override Vector2 ClosestPointTo(Segment2D segment) {
            return ClosestPointTo(segment.MiddlePoint);
        }

        /// <summary>
        /// Returns the closest point of this triangle to another box.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>The closest point of this triangle to another box.</returns>
        public override Vector2 ClosestPointTo(Polygon other) {
            return ClosestPointTo(other.Center);
        }

        public override Vector2 FurthestPointFrom(Line2D line) {
            throw new NotImplementedException();
        }

        public override Vector2 FurthestPointFrom(Segment2D segment) {
            throw new NotImplementedException();
        }

        public override Vector2 FurthestPointFrom(Polygon other) {
            throw new NotImplementedException();
        }

        public override List<Vector2> PerpendicularPointsTo(Vector2 point) {
            throw new NotImplementedException();
        }

        public override Vector2 ClosestPerpendicularPointTo(Vector2 point) {
            throw new NotImplementedException();
        }

        public override Vector2 ClosestVertexTo(Vector2 point) {
            throw new NotImplementedException();
        }

        public override Vector2 ClosestVertexTo(Line2D line) {
            throw new NotImplementedException();
        }

        public override Vector2 ClosestVertexTo(Segment2D segment) {
            throw new NotImplementedException();
        }

        public override Vector2 ClosestVertexTo(Polygon other) {
            throw new NotImplementedException();
        }

        public override Vector2 FurthestVertexFrom(Vector2 point) {
            throw new NotImplementedException();
        }

        public override Vector2 FurthestVertexFrom(Line2D line) {
            throw new NotImplementedException();
        }

        public override Vector2 FurthestVertexFrom(Segment2D segment) {
            throw new NotImplementedException();
        }

        public override Vector2 FurthestVertexFrom(Polygon other) {
            throw new NotImplementedException();
        }

        public override List<Vector2> VerticesInside(Polygon other) {
            throw new NotImplementedException();
        }

        public override Vector2 DeepestVertexIn(Polygon other) {
            throw new NotImplementedException();
        }

        public override Segment2D ClosestEdgeTo(Vector2 point) {
            throw new NotImplementedException();
        }

        public override Segment2D ClosestEdgeTo(Line2D line) {
            throw new NotImplementedException();
        }

        public override Segment2D ClosestEdgeTo(Segment2D segment) {
            throw new NotImplementedException();
        }

        public override Segment2D ClosestEdgeTo(Polygon other) {
            throw new NotImplementedException();
        }

        public override Segment2D FurthestEdgeFrom(Vector2 point) {
            throw new NotImplementedException();
        }

        public override Segment2D FurthestEdgeFrom(Line2D line) {
            throw new NotImplementedException();
        }

        public override Segment2D FurthestEdgeFrom(Segment2D segment) {
            throw new NotImplementedException();
        }

        public override Segment2D FurthestEdgeFrom(Polygon other) {
            throw new NotImplementedException();
        }

        public override Segment2D EdgeFacingTowards(Vector2 point) {
            throw new NotImplementedException();
        }

        public override Segment2D EdgeFacingTowards(Line2D line) {
            throw new NotImplementedException();
        }

        public override Segment2D EdgeFacingTowards(Segment2D edge) {
            throw new NotImplementedException();
        }

        public override Segment2D EdgeFacingTowards(Polygon other) {
            throw new NotImplementedException();
        }

        public override Segment2D HiddenEdgeFrom(Vector2 point) {
            throw new NotImplementedException();
        }

        public override Segment2D HiddenEdgeFrom(Line2D line) {
            throw new NotImplementedException();
        }

        public override Segment2D HiddenEdgeFrom(Segment2D edge) {
            throw new NotImplementedException();
        }

        public override Segment2D HiddenEdgeFrom(Polygon other) {
            throw new NotImplementedException();
        }

        public override bool Contains(Vector2 point) {
            throw new NotImplementedException();
        }

        public override bool ContainsAll(Vector2[] points) {
            throw new NotImplementedException();
        }

        public override bool ContainsAll(List<Vector2> points) {
            throw new NotImplementedException();
        }

        public override bool Contains(Segment2D segment) {
            throw new NotImplementedException();
        }

        public override bool Intersects(Line2D line) {
            throw new NotImplementedException();
        }

        public override bool Intersects(Segment2D line) {
            throw new NotImplementedException();
        }

        public override List<Vector2> IntersectionPoints(Line2D line) {
            throw new NotImplementedException();
        }

        public override List<Vector2> IntersectionPoints(Segment2D line) {
            throw new NotImplementedException();
        }

        public override Vector2 ClosestPointTo(Vector2 point) {
            throw new NotImplementedException();
        }

        public override Vector2 FurthestPointFrom(Vector2 point) {
            throw new NotImplementedException();
        }

    }
}
