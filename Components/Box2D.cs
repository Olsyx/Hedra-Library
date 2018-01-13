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

            if (collider.GetType() == typeof(BoxCollider2D)) {
                size = ((BoxCollider2D)collider).size;
            } else {
                size = collider.bounds.size;
            }

            rotation = owner.rotation.eulerAngles.z;
            Area = size.x * size.y;
            Center = collider.bounds.center;
        }

        public Box2D(BoxCollider2D collider) {
            Transform owner = collider.gameObject.transform;

            size = collider.size;
            rotation = owner.rotation.eulerAngles.z;
            Area = size.x * size.y;
            Center = collider.bounds.center;
        }
        void StoreCorners() {

            Vector2 halfSize = size / 2f;
            Corners = new Vector2[4];
            Corners[0] = Hedra.Rotate(Center, new Vector2(Center.x - halfSize.x, Center.y + halfSize.y), Rotation);
            Corners[1] = Hedra.Rotate(Center, new Vector2(Center.x + halfSize.x, Center.y + halfSize.y), Rotation);
            Corners[2] = Hedra.Rotate(Center, new Vector2(Center.x + halfSize.x, Center.y - halfSize.y), Rotation);
            Corners[3] = Hedra.Rotate(Center, new Vector2(Center.x - halfSize.x, Center.y - halfSize.y), Rotation);
        }

        void StoreEdges() {
            Edges = new Segment2D[4];
            Edges[0] = new Segment2D(Corners[0], Corners[1]);
            Edges[1] = new Segment2D(Corners[1], Corners[2]);
            Edges[2] = new Segment2D(Corners[2], Corners[3]);
            Edges[3] = new Segment2D(Corners[3], Corners[0]);
        }

        public bool Intersects(Vector2 A, Vector2 B, bool onSegment) {
            if (onSegment) {
                Segment2D segment = new Segment2D(A, B);
                return Intersects(segment);
            } else {
                Line2D line = new Segment2D(A, B);
                return Intersects(line);
            }
        }

        public bool Intersects(Segment2D segment) {
            return Intersects((Line2D)segment);
        }
        
        public bool Intersects(Line2D line) {
            List<Vector2> intersectionPoints = IntersectionPoints(line);
            return intersectionPoints != null && intersectionPoints.Count > 0;
        }

        /// <summary>
        /// Calculates the intersection point of the line or segment AB with this box.
        /// </summary>
        /// <param name="A">First point.</param>
        /// <param name="B">Second point.</param>
        /// <param name="onSegment">If true, point must be contained by the colliding segments (both collider edges and AB).</param>
        /// <returns>The intersection points of a line or segment with the edges of this box.</returns>
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
        /// Calculates the intersection point of a segment with this box.
        /// </summary>
        /// <param name="segment"></param>
        /// <returns>The intersection points of a segment with this box.</returns>
        public List<Vector2> IntersectionPoints(Segment2D segment) {
            List<Vector2> intersections = new List<Vector2>();

            Vector2 point;
            for (int i = 0; i < Edges.Length; i++) {
                point = segment.IntersectionPoint(Edges[i]);
                if (!float.IsNaN(point.x) && !float.IsInfinity(point.x) &&
                    !float.IsNaN(point.y) && !float.IsInfinity(point.y)) {
                    intersections.Add(point);
                }
            }

            return intersections;
        }

        /// <summary>
        /// Calculates the intersection point of a line with this box.
        /// </summary>
        /// <param name="line"></param>
        /// <returns>The intersection points of a line with the edges of this box.</returns>
        public List<Vector2> IntersectionPoints(Line2D line) {
            List<Vector2> intersections = new List<Vector2>();

            Vector2 point;
            for (int i = 0; i < Edges.Length; i++) {
                point = line.IntersectionPoint(Edges[i]);
                if (!float.IsNaN(point.x) && !float.IsInfinity(point.x) && 
                    !float.IsNaN(point.y) && !float.IsInfinity(point.y)) {
                    intersections.Add(point);
                }
            }

            return intersections;
        }
        
        /// <summary>
        /// Returns the points of this box colliding with another box.
        /// </summary>
        /// <param name="box">The box to check.</param>
        /// <returns>The points of this box colliding with another box.</returns>
        public List<Vector2> IntersectionPoints(Box2D box) {
            List<Vector2> intersections = new List<Vector2>();
            for (int i = 0; i < Edges.Length; i++) {
                intersections = intersections.Union(box.IntersectionPoints(Edges[i])).ToList();
            }
            return intersections;
        }
        
        /// <summary>
        /// Returns the edges of this box colliding with the line or segment formed by the points AB.
        /// </summary>
        /// <param name="A">First point.</param>
        /// <param name="B">Second point.</param>
        /// <param name="onSegment">If true, point must be contained by the colliding segments (both collider edges and AB).</param>
        /// <returns>The edges of this box colliding with a line or segment.</returns>
        public List<Segment2D> IntersectingEdges(Vector2 A, Vector2 B, bool onSegment) {
            if (onSegment) {
                Segment2D segment = new Segment2D(A, B);
                return IntersectingEdges(segment);
            } else {
                Line2D line = new Segment2D(A, B);
                return IntersectingEdges(line);
            }
        }
        
        /// <summary>
        /// Returns the edges of this box colliding with a segment.
        /// </summary>
        /// <param name="segment">The segment to check.</param>
        /// <returns>The edges of this box colliding with a segment.</returns>
        public List<Segment2D> IntersectingEdges(Segment2D segment) {
            List<Segment2D> intersections = new List<Segment2D>();

            for (int i = 0; i < Edges.Length; i++) {
                if (Edges[i].Intersects(segment)) {
                    intersections.Add(Edges[i]);
                }
            }

            return intersections;
        }
        
        /// <summary>
        /// Returns the edges of this box colliding with a line.
        /// </summary>
        /// <param name="line">The line to check.</param>
        /// <returns>The edges of this box colliding with a line.</returns>
        public List<Segment2D> IntersectingEdges(Line2D line) {
            List<Segment2D> intersections = new List<Segment2D>();
            
            for (int i = 0; i < Edges.Length; i++) {
                if (Edges[i].Intersects(line)) {
                    intersections.Add(Edges[i]);
                }
            }

            return intersections;
        }
        
        /// <summary>
        /// Returns the edges of this box colliding with another box.
        /// </summary>
        /// <param name="box">The box to check.</param>
        /// <returns>The edges of this box colliding with another box.</returns>
        public List<Segment2D> IntersectingEdges(Box2D box) {
            List<Segment2D> intersections = new List<Segment2D>();
            for (int i = 0; i < box.Edges.Length; i++) {
                intersections = intersections.Union(IntersectingEdges(box.Edges[i])).ToList();
            }
            return intersections;
        }

        /// <summary>
        /// Check whether a point is part of one of this box's edges or not.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool ContainsInAnyEdge(Vector2 point) {
            int i = 0;
            while (i < Edges.Length && !Edges[i].Contains(point)) {
                Debug.Log("Edge " + i + " does not contain " + point);
                i++;
            }
            Debug.Log("Edge " + i + " contains " + point);
            return i < Edges.Length;
        }

        /// <summary>
        /// Check whether a point is part of one of this box's edges, is at least at margin distance, or not.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool ContainsInAnyEdge(Vector2 point, float margin) {
            int i = 0;
            while (i < Edges.Length && !Edges[i].Contains(point, margin)) {
                Debug.Log("Edge " + i + " does not contain " + point);
                i++;
            }
            Debug.Log("Edge " + i + " contains " + point);
            return i < Edges.Length;
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
        /// Returns the closest corner of this box to another box.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>The closest corner of this box to another box.</returns>
        public Vector2 ClosestCornerTo(Box2D other) {
            return ClosestCornerTo(other.Center);
        }

        /// <summary>
        /// Returns the closest corner of this box to a point.
        /// </summary>
        /// <param name="point"></param>
        /// <returns>The closest corner of this box to a point.</returns>
        public Vector2 ClosestCornerTo(Vector2 point) {
            return Hedra.GetClosestPoint(point, Corners.ToList());
        }

        public List<Vector2> CornersInside(Box2D other) {
            List<Vector2> containedCorners = new List<Vector2>();
            for (int i = 0; i < Corners.Length; i++) {
                if (other.Contains(Corners[i])) {
                    containedCorners.Add(Corners[i]);
                }
            }
            return containedCorners;
        }

        /// <summary>
        /// Returns the closest corner of this box to another box.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>The closest corner of this box to another box.</returns>
        public Vector2 DeepestCornerIn(Box2D other) {
            List<Vector2> containedPoints = CornersInside(other);

            Vector2 corner = new Vector2(float.NaN, float.NaN);
            float greatestDistance = float.MinValue;
            for (int i = 0; i < containedPoints.Count; i++) {
                float distance = Vector2.Distance(containedPoints[i], other.ClosestPointTo(containedPoints[i]));
                if (distance >= greatestDistance) {
                    corner = containedPoints[i];
                    greatestDistance = distance;
                }
            }

            return corner;
        }
        
        /// <summary>
        /// Returns the furthest corner of this box to another box.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>The furthest corner of this box to another box.</returns>
        public Vector2 FurthestCornerFrom(Box2D other) {
            return FurthestCornerFrom(other.Center);
        }

        /// <summary>
        /// Returns the furthest corner of this box to a point.
        /// </summary>
        /// <param name="point"></param>
        /// <returns>The furthest corner of this box to a point.</returns>
        public Vector2 FurthestCornerFrom(Vector2 point) {
            return Hedra.GetFurthestPoint(point, Corners.ToList());
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
                Gizmos.color = Color.white;
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