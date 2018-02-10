using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HedraLibrary.Components {
    public partial class Rectangle : Polygon {
        public override Vector2 ClosestPointTo(Line2D line) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the closest point of this box to a segment.
        /// </summary>
        /// <param name="point"></param>
        /// <returns>The closest point of this box to a point.</returns>
        public override Vector2 ClosestPointTo(Segment2D segment) {
            return ClosestPointTo(segment.MiddlePoint);
        }

        /// <summary>
        /// Returns the closest point of this box to another box.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>The closest point of this box to another box.</returns>
        public override Vector2 ClosestPointTo(Polygon other) {
            return ClosestPointTo(other.Center);
        }

        public override Vector2 FurthestPointFrom(Line2D line) {
            throw new NotImplementedException();
        }

        public override Vector2 FurthestPointFrom(Segment2D segment) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the furthest point of this box to another box.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>The furthest point of this box to another box.</returns>
        public override Vector2 FurthestPointFrom(Polygon other) {
            return FurthestPointFrom(other.Center);
        }

        public override Vector2 ClosestVertexTo(Line2D line) {
            throw new NotImplementedException();
        }

        public override Vector2 ClosestVertexTo(Segment2D segment) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the closest Vertex of this box to another box.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>The closest Vertex of this box to another box.</returns>
        public override Vector2 ClosestVertexTo(Polygon other) {
            return ClosestVertexTo(other.Center);
        }

        /// <summary>
        /// Returns the closest Vertex of this box to a point.
        /// </summary>
        /// <param name="point"></param>
        /// <returns>The closest Vertex of this box to a point.</returns>
        public override Vector2 ClosestVertexTo(Vector2 point) {
            return Hedra.GetClosestPoint(point, Vertices.ToList());
        }

        /// <summary>
        /// Returns the Vertices of this box inside another box.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override List<Vector2> VerticesInside(Polygon other) {
            List<Vector2> containedVertices = new List<Vector2>();
            for (int i = 0; i < Vertices.Length; i++) {
                if (other.Contains(Vertices[i])) {
                    containedVertices.Add(Vertices[i]);
                }
            }
            return containedVertices;
        }

        /// <summary>
        /// Returns the deepest Vertex of this box in another box.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>The closest Vertex of this box to another box.</returns>
        public override Vector2 DeepestVertexIn(Polygon other) {
            List<Vector2> containedPoints = VerticesInside(other);

            Vector2 Vertex = new Vector2(float.NaN, float.NaN);
            float greatestDistance = float.MinValue;
            for (int i = 0; i < containedPoints.Count; i++) {
                float distance = Vector2.Distance(containedPoints[i], other.ClosestPerpendicularPointTo(containedPoints[i]));
                if (distance >= greatestDistance) {
                    Vertex = containedPoints[i];
                    greatestDistance = distance;
                }
            }

            return Vertex;
        }

        /// <summary>
        /// Returns the furthest Vertex of this box to a point.
        /// </summary>
        /// <param name="point"></param>
        /// <returns>The furthest Vertex of this box to a point.</returns>
        public override Vector2 FurthestVertexFrom(Vector2 point) {
            return Hedra.GetFurthestPoint(point, Vertices.ToList());
        }

        public override Vector2 FurthestVertexFrom(Line2D line) {
            throw new NotImplementedException();
        }

        public override Vector2 FurthestVertexFrom(Segment2D segment) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the furthest Vertex of this box to another box.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>The furthest Vertex of this box to another box.</returns>
        public override Vector2 FurthestVertexFrom(Polygon other) {
            return FurthestVertexFrom(other.Center);
        }

        /// <summary>
        /// Returns the closest edge of this box to a point.
        /// </summary>
        /// <param name="point"></param>
        /// <returns>The closest edge of this box to a point.</returns>
        public override Segment2D ClosestEdgeTo(Vector2 point) {
            List<Vector2> perpendicularPoints = PerpendicularPointsTo(point);
            Vector2 closestPoint = Hedra.GetClosestPoint(point, perpendicularPoints);
            int closestEdgeIndex = perpendicularPoints.IndexOf(closestPoint);
            return Edges[closestEdgeIndex];
        }

        public override Segment2D ClosestEdgeTo(Line2D line) {
            throw new NotImplementedException();
        }

        public override Segment2D ClosestEdgeTo(Segment2D segment) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the closest edge of this box to another box.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>The closest edge of this box to another box.</returns>
        public override Segment2D ClosestEdgeTo(Polygon other) {
            return ClosestEdgeTo(other.Center);
        }

        /// <summary>
        /// Returns the furthest edge of this box to a point.
        /// </summary>
        /// <param name="point"></param>
        /// <returns>The furthest edge of this box to a point.</returns>
        public override Segment2D FurthestEdgeFrom(Vector2 point) {
            List<Vector2> perpendicularPoints = PerpendicularPointsTo(point);
            Vector2 closestPoint = Hedra.GetFurthestPoint(point, perpendicularPoints);
            int closestEdgeIndex = perpendicularPoints.IndexOf(closestPoint);
            return Edges[closestEdgeIndex];
        }

        public override Segment2D FurthestEdgeFrom(Line2D line) {
            throw new NotImplementedException();
        }

        public override Segment2D FurthestEdgeFrom(Segment2D segment) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the furthest edge of this box to another box.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>The furthest edge of this box to another box.</returns>
        public override Segment2D FurthestEdgeFrom(Polygon other) {
            return FurthestEdgeFrom(other.Center);
        }

        /// <summary>
        /// Returns the edge of this box facing a point.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public override Segment2D EdgeFacingTowards(Vector2 point) {
            Vector2 closestPoint = ClosestPointTo(point);

            int i = 0;
            while (i < Edges.Length && !Edges[i].Contains(closestPoint)) {
                i++;
            }

            return i < Edges.Length ? Edges[i] : null;
        }

        public override Segment2D EdgeFacingTowards(Line2D line) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the edge of this box facing another edge (even if technically, it isn't the closest one).
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public override Segment2D EdgeFacingTowards(Segment2D edge) {
            Vector2 closestPoint = ClosestPointTo(edge);

            int i = 0;
            while (i < Edges.Length && !Edges[i].Contains(closestPoint)) {
                i++;
            }

            return i < Edges.Length ? Edges[i] : null;
        }

        /// <summary>
        /// Returns the edge of this box facing another box (even if technically, it isn't the closest one).
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public override Segment2D EdgeFacingTowards(Polygon box) {

           /* List<Vector2> edgeCenters = new List<Vector2> { Edges[0].MiddlePoint, Edges[1].MiddlePoint, Edges[2].MiddlePoint, Edges[3].MiddlePoint };
            Vector2 closestCenter = Hedra.GetClosestPoint(box.Center, edgeCenters);
            return Edges[edgeCenters.IndexOf(closestCenter)];
            */
            Vector2 closestPoint = ClosestPointTo(box.ClosestVertexTo(this));

            int i = 0;
            while (i < Edges.Length && !Edges[i].Contains(closestPoint)) {
                i++;
            }

            return i < Edges.Length ? Edges[i] : null;
        }

        /// <summary>
        /// Returns the edge hidden from the view perspective of a point towards this box.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public override Segment2D HiddenEdgeFrom(Vector2 point) {
            Vector2 furthestPoint = FurthestPointFrom(point);

            int i = 0;
            while (i < Edges.Length && !Edges[i].Contains(furthestPoint)) {
                i++;
            }

            return i < Edges.Length ? Edges[i] : null;
        }

        public override Segment2D HiddenEdgeFrom(Line2D line) {
            throw new NotImplementedException();
        }

        public override Segment2D HiddenEdgeFrom(Segment2D edge) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the edge hidden from the view perspective of a box towards this box.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public override Segment2D HiddenEdgeFrom(Polygon box) {
            Vector2 furthestPoint = FurthestPointFrom(box);

            int i = 0;
            while (i < Edges.Length && !Edges[i].Contains(furthestPoint)) {
                i++;
            }

            return i < Edges.Length ? Edges[i] : null;
        }

    }
}