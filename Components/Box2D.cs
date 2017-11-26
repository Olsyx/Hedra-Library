using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HedraLibrary.Components {
    public class Box2D {
        private Vector2 center;
        public Vector2 Center {
            get { return center; }
            set {
                center = value;
                StoreCorners();
                StoreEdges();
            }
        }

        private float rotation;
        public float Rotation {
            get { return rotation; }
            set {
                rotation = value;
                StoreCorners();
                StoreEdges();
            }
        }

        private Vector2 size;
        public Vector2 Size {
            get { return size; }
            set {
                size = value;
                Area = size.x * size.y;
                StoreCorners();
                StoreEdges();
            }
        }

        public float Area { get; protected set; }
        public Vector2[] Corners { get; protected set; }
        public Segment2D[] Edges { get; protected set; }

        public Box2D(Vector2 position, Vector2 size, float rotation) {
            this.rotation = rotation;
            this.size = size;
            Area = size.x * size.y;
            Center = position;
        }

        public Box2D(Collider2D collider) {
            Transform owner = collider.gameObject.transform;

            rotation = owner.rotation.eulerAngles.z;
            size = collider.bounds.size;
            Area = size.x * size.y;
            Center = (Vector2)owner.position + collider.offset;
        }

        public Box2D(BoxCollider2D collider) {
            Transform owner = collider.gameObject.transform;

            rotation = owner.rotation.eulerAngles.z;
            size = collider.size;
            Area = size.x * size.y;
            Center = (Vector2)owner.position + collider.offset;
        }

        void StoreCorners() {
            Corners = new Vector2[4];
            Corners[0] = Hedra.Rotate(Center, new Vector2(Center.x - Size.x / 2, Center.y + Size.y / 2), Rotation);
            Corners[1] = Hedra.Rotate(Center, new Vector2(Center.x + Size.x / 2, Center.y + Size.y / 2), Rotation);
            Corners[2] = Hedra.Rotate(Center, new Vector2(Center.x + Size.x / 2, Center.y - Size.y / 2), Rotation);
            Corners[3] = Hedra.Rotate(Center, new Vector2(Center.x - Size.x / 2, Center.y - Size.y / 2), Rotation);
        }

        void StoreEdges() {
            Edges = new Segment2D[4];
            Edges[0] = new Segment2D(Corners[0], Corners[1]);
            Edges[1] = new Segment2D(Corners[1], Corners[2]);
            Edges[2] = new Segment2D(Corners[2], Corners[3]);
            Edges[3] = new Segment2D(Corners[3], Corners[0]);
        }
        
        /// <summary>
        /// Calculates the intersection point of the line or segment AB with this box.
        /// </summary>
        /// <param name="A">First point.</param>
        /// <param name="B">Second point.</param>
        /// <param name="onSegment">If true, point must be contained by the colliding segments (both collider edges and AB).</param>
        /// <returns>The intersection points of a line or segment with the edges of a collider.</returns>
        public List<Vector2> IntersectionPoints(Vector2 A, Vector2 B, bool onSegment) {
            if (onSegment) {
                Segment2D segment = new Segment2D(A, B);
                return IntersectionPoints(segment);
            } else {
                Line2D line = new Segment2D(A, B);
                return IntersectionPoints(line);
            }
        }
        
        /// <summary>
        /// Calculates the intersection point of the segment AB with this box.
        /// </summary>
        /// <param name="segment"></param>
        /// <returns>The intersection points of a segment with the edges of a collider.</returns>
        public List<Vector2> IntersectionPoints(Segment2D segment) {
            return IntersectionPoints((Line2D) segment);
        }

        /// <summary>
        /// Calculates the intersection point of the line AB with this box.
        /// </summary>
        /// <param name="line"></param>
        /// <returns>The intersection points of a line with the edges of a collider.</returns>
        public List<Vector2> IntersectionPoints(Line2D line) {
            List<Vector2> intersections = new List<Vector2>();

            Vector2 point;
            for (int i = 0; i < Corners.Length - 1; i++) {
                point = line.IntersectionPoint(Corners[i], Corners[i + 1]);
                if (point.x != float.NaN && point.y != float.NaN) {
                    intersections.Add(point);
                }
            }

            point = line.IntersectionPoint(Corners[Corners.Length - 1], Corners[0]);
            if (point.x != float.NaN && point.y != float.NaN) {
                intersections.Add(point);
            }

            return intersections;
        }

        /// <summary>
        /// Returns the closest point of this box to another box.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>The closest point of this box to another box.</returns>
        public Vector2 ClosestPointTo(Box2D other) {
            return ClosestPointTo(other.Center);
        }

        /// <summary>
        /// Returns the closest point of this box to a point.
        /// </summary>
        /// <param name="point"></param>
        /// <returns>The closest point of this box to a point.</returns>
        public Vector2 ClosestPointTo(Vector2 point) {
            List<Vector2> perpendicularPoints = PerpendicularPointsTo(point);
            return Hedra.GetClosestPoint(point, perpendicularPoints);
        }

        /// <summary>
        /// Returns the furthest point of this box to another box.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>The furthest point of this box to another box.</returns>
        public Vector2 FurthestPointFrom(Box2D other) {
            return FurthestPointFrom(other.Center);
        }

        /// <summary>
        /// Returns the furthest point of this box to a point.
        /// </summary>
        /// <param name="point"></param>
        /// <returns>The furthest point of this box to another point.</returns>
        public Vector2 FurthestPointFrom(Vector2 point) {
            List<Vector2> perpendicularPoints = PerpendicularPointsTo(point);
            return Hedra.GetFurthestPoint(point, perpendicularPoints);
        }

        /// <summary>
        /// Returns the closest edge of this box to another box.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>The closest edge of this box to another box.</returns>
        public Segment2D ClosestEdgeTo(Box2D other) {
            return ClosestEdgeTo(other.Center);
        }

        /// <summary>
        /// Returns the closest edge of this box to a point.
        /// </summary>
        /// <param name="point"></param>
        /// <returns>The closest edge of this box to a point.</returns>
        public Segment2D ClosestEdgeTo(Vector2 point) {
            List<Vector2> perpendicularPoints = PerpendicularPointsTo(point);
            Vector2 closestPoint = Hedra.GetClosestPoint(point, perpendicularPoints);
            int closestEdgeIndex = perpendicularPoints.IndexOf(closestPoint);
            return Edges[closestEdgeIndex];
        }

        /// <summary>
        /// Returns the furthest edge of this box to another box.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>The furthest edge of this box to another box.</returns>
        public Segment2D FurthestEdgeFrom(Box2D other) {
            return FurthestEdgeFrom(other.Center);
        }

        /// <summary>
        /// Returns the furthest edge of this box to a point.
        /// </summary>
        /// <param name="point"></param>
        /// <returns>The furthest edge of this box to a point.</returns>
        public Segment2D FurthestEdgeFrom(Vector2 point) {
            List<Vector2> perpendicularPoints = PerpendicularPointsTo(point);
            Vector2 closestPoint = Hedra.GetFurthestPoint(point, perpendicularPoints);
            int closestEdgeIndex = perpendicularPoints.IndexOf(closestPoint);
            return Edges[closestEdgeIndex];
        }

        /// <summary>
        /// Returns each edge's perpendicular point to a given point.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public List<Vector2> PerpendicularPointsTo(Vector2 point) {
            List<Vector2> perpendicularPoints = new List<Vector2>();
            
            for (int i = 0; i < Edges.Length; i++) {
                perpendicularPoints.Add(Edges[i].PerpendicularPoint(point));
            }

            return perpendicularPoints;
        }

        /// <summary>
        /// Returns true if this box contains the given point.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool Contains(Vector2 point) {
            int i = Edges.Length - 1;   // Corners are stored in clockwise direction.

            while (i >= 0 && Edges[i].GetPointSituation(point) < 0) {
                i--;
            }

            return i < 0;
        }

        /// <summary>
        /// Returns true if this box contains the given segment.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool Contains(Segment2D segment) {
            return Contains(segment.PointA) && Contains(segment.PointB);
        }

        /// <summary>
        /// Returns true if this box contains all the given points.
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public bool Contains(Vector2[] points) {
            int i = 0;
            while (i < points.Length && Contains(points[i])) {
                i++;
            }
            return i >= points.Length;
        }


        public void DrawGizmos(bool drawCorners, float size = 0.2f) {
            Gizmos.color = Color.green;
            for (int i = 1; i < Corners.Length; i++) {
                Gizmos.DrawLine(Corners[i - 1], Corners[i]);
            }
            Gizmos.DrawLine(Corners[0], Corners[Corners.Length - 1]);

            if (!drawCorners) {
                return;
            }
            Gizmos.color = Color.yellow;
            for (int i = 0; i < Corners.Length; i++) {
                Gizmos.DrawWireSphere(Corners[i], size);
            }

            Gizmos.color = Color.white;
            float step = size / Corners.Length;
            for (int i = 0; i < Corners.Length; i++) {
                Gizmos.DrawWireSphere(Corners[i], step * (i+1));
            }
        }
    }
}