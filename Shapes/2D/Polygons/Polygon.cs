using HedraLibrary;
using HedraLibrary.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HedraLibrary.Shapes.Polygons {
    public class ImpossibleCollisionException : Exception {
        public ImpossibleCollisionException() {
        }

        public ImpossibleCollisionException(string message)
            : base(message) {
        }

        public ImpossibleCollisionException(string message, Exception inner)
            : base(message, inner) {
        }
    }

    public abstract class Polygon {
        private const float EPSILON = 0.01f;

        protected Vector2 center;
        public Vector2 Center {
            get {
                return center;
            }
            set {
                MoveTo(value);
            }
        }

        protected float rotation;
        public float Rotation {
            get {
                return rotation;
            }
            set {
                Rotate(value - rotation);
            }
        }

        public float Area { get; protected set; }
        public Collider2D Collider { get; protected set; }
        public Vector2[] Vertices { get; protected set; }
        public Segment2D[] Edges { get; protected set; }
        public Line2D[] Normals { get; protected set; }

        #region Init
        protected virtual void Init() {
            CreateVertices();
            CreateEdges();
            CreateNormals();
            CalculateArea();
        }

        protected abstract void CreateVertices();

        protected virtual void CreateEdges() {
            if (Vertices.Length <= 0) {
                return;
            }

            Edges = new Segment2D[Vertices.Length];
            for (int i = 0; i < Edges.Length - 1; i++) {
                Edges[i] = new Segment2D(Vertices[i], Vertices[i + 1]);
            }
            Edges[Edges.Length - 1] = new Segment2D(Vertices[Vertices.Length - 1], Vertices[0]);
        }

        protected virtual void CreateNormals() {
            if (Edges.Length <= 0) {
                return;
            }

            Normals = new Segment2D[Edges.Length];
            for (int i = 0; i < Edges.Length; i++) {
                Vector2 middlePoint = Edges[i].MiddlePoint;

                // We look for the normal value furthest from the Center
                Vector2 normal = Hedra.FurthestObject(Edges[i].Normals.ToList(), 
                    (vector => {    return Vector2.Distance(middlePoint + vector, Center);  }));

                Normals[i] = new Segment2D(middlePoint, middlePoint + normal);
            }
        }

        protected abstract void CalculateArea();
        #endregion

        #region Control

        public void MoveTo(Vector2 position) {
            Translate(position - Center);
        }

        public virtual void Translate(Vector2 direction) {
            center += direction;

            for (int i = 0; i < Vertices.Length; i++) {
                Vertices[i] += direction;
            }

            for (int i = 0; i < Edges.Length; i++) {
                Edges[i].Translate(direction);
            }

            for (int i = 0; i < Normals.Length; i++) {
                Normals[i].Translate(direction);
            }
        }
        
        public virtual void Rotate(float degrees) {
            rotation += degrees;

            for (int i = 0; i < Vertices.Length; i++) {
                Vertices[i] = Hedra.Rotate(Center, Vertices[i], degrees);
            }

            for (int i = 0; i < Edges.Length; i++) {
                Edges[i].Rotate(Center, degrees);
            }

            for (int i = 0; i < Normals.Length; i++) {
                Normals[i].Rotate(Center, degrees);
            }
        }

        public void SortVertices(bool clockwise = true) {
            Vertices = Hedra.SortAroundCenter(Center, Vertices, clockwise);
        }

        public int GetVertexIndex(Vector2 vertex) {
            int i = 0;
            while (i < Vertices.Length && Vertices[i] != vertex) {
                i++;
            }

            return i < Vertices.Length ? i : -1;
        }

        public override bool Equals(object obj) {
            Polygon other = (Polygon)obj;

            if (Vertices.Length != other.Vertices.Length) {
                return false;
            }

            int i = 0;
            while (i < other.Vertices.Length && Vertices[i] == other.Vertices[i]) {
                i++;
            }
            return i >= Vertices.Length;
        }
        #endregion

        #region Consults
        /// <summary>
        /// Returns true if this polygon contains the given point.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public virtual bool Contains(Vector2 point) {
            int i = Edges.Length - 1;   // Corners are stored in clockwise direction.

            while (i >= 0 && Edges[i].GetPointSituation(point) < 0) {
                i--;
            }

            return i < 0;
        }

        /// <summary>
        /// Returns true if this polygon fully contains the given segment.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public virtual bool Contains(Segment2D segment) {
            return Contains(segment.PointA) && Contains(segment.PointB);
        }

        /// <summary>
        /// Returns true if this polygon fully contains a given polygon.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public virtual bool Contains(Polygon other) {
            return this.ContainsAll(other.Vertices);
        }

        /// <summary>
        /// Returns true if this polygon contains all the given points.
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public virtual bool ContainsAll(Vector2[] points) {
            int i = 0;
            while (i < points.Length && Contains(points[i])) {
                i++;
            }
            return i >= points.Length;
        }

        /// <summary>
        /// Returns true if this polygon contains all the given points.
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public virtual bool ContainsAll(List<Vector2> points) {
            int i = 0;
            while (i < points.Count && Contains(points[i])) {
                i++;
            }
            return i >= points.Count;
        }

        /// <summary>
        /// Check whether a point is part of one of this polygon's edges or not.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public virtual bool ContainsInEdge(Vector2 point) {
            int i = 0;
            while (i < Edges.Length && !Edges[i].Contains(point)) {
                i++;
            }

            return i < Edges.Length;
        }

        /// <summary>
        /// Check whether a point is part of one of this polygon's edges, is at least at margin distance, or not.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public virtual bool ContainsInEdge(Vector2 point, float margin) {
            int i = 0;
            while (i < Edges.Length && !Edges[i].Contains(point, margin)) {
                i++;
            }

            return i < Edges.Length;
        }

        public virtual bool IsVertex(Vector2 point) {
            int i = 0;
            while (i < Vertices.Length && !EqualVertices(point, Vertices[i])) {
                i++;
            }
            return i < Vertices.Length;
        }

        /// <summary>
        /// Returns true if at least one edge from the given polygon overlaps one edge of this polygon.
        /// For an edge to overlap, it must be parallel and contained into another edge.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public virtual bool AnyEdgeOverlaps(Polygon other) {
            int i = 0;
            int j = 0;

            while (i < Edges.Length && !Edges[i].Contains(other.Edges[j])) {
                j++;

                if (j >= other.Edges.Length) {
                    j = 0;
                    i++;
                }
            }

            return i < Edges.Length;
        }

        /// <summary>
        /// Returns true if all edges from the given polygon overlap this polygon's edges.
        /// For an edge to overlap, it must be parallel and contained into another edge.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public virtual bool AllEdgesOverlap(Polygon other) {
            int i = 0;
            int j = 0;

            while (i < Edges.Length && !Edges[i].Contains(other.Edges[j])) {
                j++;

                if (j >= other.Edges.Length) {
                    j = 0;
                    i++;
                }
            }

            return i >= Edges.Length;
        }

        /// <summary>
        /// Returns true if this polygon intersects the given line.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public virtual bool Intersects(Line2D line) {
            int i = 0;

            while (i < Edges.Length && !Edges[i].Intersects(line)) {
                i++;
            }

            return i < Edges.Length;
        }

        /// <summary>
        /// Returns true if this polygon intersects the given segment.
        /// </summary>
        /// <param name="segment"></param>
        /// <returns></returns>
        public virtual bool Intersects(Segment2D segment) {
            bool a = Contains(segment.PointA);
            bool b = Contains(segment.PointB);

            return (a && !b) || (!a && b);
        }

        /// <summary>
        /// Returns true if this polygon intersects the given polygon.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public virtual bool Intersects(Polygon other) {
            int i = 0;
            int j = 0;

            while (i < Edges.Length && !Edges[i].Intersects(other.Edges[j])) {
                j++;
                if (j >= other.Edges.Length) {
                    j = 0;
                    i++;
                }
            }

            return i < Edges.Length;
        }
        #endregion

        #region Intersections
        /// <summary>
        /// Calculates the intersection point of a line with this polygon.
        /// </summary>
        /// <param name="segment"></param>
        /// <returns>The intersection points of a segment with this polygon.</returns>
        public virtual List<Vector2> IntersectionPoints(Line2D line) {
            List<Vector2> intersections = new List<Vector2>();

            Vector2 point;
            for (int i = 0; i < Edges.Length; i++) {
                point = line.IntersectionPoint(Edges[i]);
                if (!point.IsNaN() && !point.IsInfinity()) {
                    intersections.Add(point);
                }
            }

            return intersections;
        }
    
        /// <summary>
        /// Calculates the intersection point of a segment with this polygon.
        /// </summary>
        /// <param name="segment"></param>
        /// <returns>The intersection points of a segment with this polygon.</returns>
        public virtual List<Vector2> IntersectionPoints(Segment2D segment) {
            List<Vector2> intersections = new List<Vector2>();

            Vector2 point;
            for (int i = 0; i < Edges.Length; i++) {
                point = segment.IntersectionPoint(Edges[i]);
                if (!point.IsNaN() && !point.IsInfinity()) {
                    intersections.Add(point);
                }
            }

            return intersections;
        }

        /// <summary>
        /// Returns the points of this polygon colliding with another polygon.
        /// </summary>
        /// <param name="polygon">The polygon to check.</param>
        /// <returns>The points of this polygon colliding with another polygon.</returns>
        public List<Vector2> IntersectionPoints(Polygon other, int precision = 4) {
            List<Vector2> intersections = new List<Vector2>();
            for (int i = 0; i < Edges.Length; i++) {
                intersections = intersections.Union(other.IntersectionPoints(Edges[i])).ToList();
            }
            intersections = RemoveVertexDuplicates(intersections, precision);
            return intersections;
        }

        /// <summary>
        /// Returns the edges of this polygon colliding with a line.
        /// </summary>
        /// <param name="line">The line to check.</param>
        /// <returns>The edges of this polygon colliding with a line.</returns>
        public virtual List<Segment2D> IntersectingEdges(Line2D line) {
            List<Segment2D> intersections = new List<Segment2D>();
            for (int i = 0; i < Edges.Length; i++) {
                if (Edges[i].Intersects(line)) {
                    intersections.Add(Edges[i]);
                }
            }
            return intersections;
        }

        /// <summary>
        /// Returns the edges of this polygon colliding with a segment.
        /// </summary>
        /// <param name="segment">The segment to check.</param>
        /// <returns>The edges of this polygon colliding with a segment.</returns>
        public virtual List<Segment2D> IntersectingEdges(Segment2D segment) {
            List<Segment2D> intersections = new List<Segment2D>();
            for (int i = 0; i < Edges.Length; i++) {
                if (Edges[i].Intersects(segment)) {
                    intersections.Add(Edges[i]);
                }
            }

            return intersections;
        }

        /// <summary>
        /// Returns the edges of this polygon colliding with another polygon.
        /// </summary>
        /// <param name="polygon">The polygon to check.</param>
        /// <returns>The edges of this polygon colliding with another polygon.</returns>
        public virtual List<Segment2D> IntersectingEdges(Polygon other) {
            List<Segment2D> intersections = new List<Segment2D>();
            for (int i = 0; i < Edges.Length; i++) {
                if (other.Intersects(Edges[i])) {
                    intersections.Add(Edges[i]);
                }
            }
            return intersections;
        }

        /// <summary>
        /// Returns the edges overlapping between the given polygon and this polygon.
        /// For an edge to overlap, it must be parallel and contained into another edge.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>The edges of this polygon that overlap the given polygon's edges.</returns>
        public virtual List<Segment2D> OverlappingEdges(Polygon other) {
            List<Segment2D> overlappingEdges = new List<Segment2D>();

            for (int i = 0; i < Edges.Length; i++) {
                for (int j = 0; j < other.Edges.Length; j++) {
                    if (Edges[i].Contains(other.Edges[j])) {
                        overlappingEdges.Add(Edges[i]);
                    }
                }
            }

            return overlappingEdges;
        }

        #endregion

        #region Searches

        public virtual Segment2D GetEdgeContaining(Vector2 point) {
            int i = 0;
            while (i < Edges.Length && !Edges[i].Contains(point)) {
                i++;
            }

            return i < Edges.Length ? Edges[i] : null;
        }

        public virtual Segment2D GetEdgeContaining(Vector2 point, float margin) {
            int i = 0;
            while (i < Edges.Length && !Edges[i].Contains(point, margin)) {
                i++;
            }

            return i < Edges.Length ? Edges[i] : null;
        }

        // Revisar - Closest Point debería ser el punto con menos distancia, no la intersección al centro.
        #region ClosestPointTo
        /// <summary>
        /// Returns the closest point of this polygon to a point.
        /// </summary>
        /// <param name="point"></param>
        /// <returns>The closest point of this polygon to a point.</returns>
        public virtual Vector2 ClosestPointTo(Vector2 point) {
            Segment2D cast = new Segment2D(Center, point);
            List<Vector2> intersections = IntersectionPoints(cast);
            if (intersections.Count == 0) { // Point is inside polygon
                Line2D line = new Line2D(Center, point);
                intersections = IntersectionPoints(line);
                return Hedra.ClosestPoint(point, intersections);
            } else if (intersections.Count == 1) {
                return intersections[0];
            }

            return Hedra.ClosestPoint(point, intersections);
        }

        /// <summary>
        /// Returns the closest point of this box to a line.
        /// </summary>
        /// <param name="line"></param>
        /// <returns>The closest point of this box to a line.</returns>
        public virtual Vector2 ClosestPointTo(Line2D line) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the closest point of this box to a segment.
        /// </summary>
        /// <param name="point"></param>
        /// <returns>The closest point of this box to a segment.</returns>
        public virtual Vector2 ClosestPointTo(Segment2D segment) {
            return ClosestPointTo(segment.MiddlePoint);
        }

        /// <summary>
        /// Returns the closest point of this box to another polygon.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>The closest point of this box to another polygon.</returns>
        public virtual Vector2 ClosestPointTo(Polygon other) {
            return ClosestPointTo(other.Center);
        }
        #endregion

        // Revisar
        #region FurthestPointFrom
        /// <summary>
        /// Returns the furthest point of this polygon to a point.
        /// </summary>
        /// <param name="point"></param>
        /// <returns>The furthest point of this polygon to another point.</returns>
        public virtual Vector2 FurthestPointFrom(Vector2 point) {
            Line2D line = new Line2D(Center, point);
            List<Vector2> intersections = IntersectionPoints(line);
            return Hedra.FurthestPoint(point, intersections);
        }

        /// <summary>
        /// Returns the furthest point of this polygon to a line.
        /// </summary>
        /// <param name="line"></param>
        /// <returns>The furthest point of this polygon to another line.</returns>
        public virtual Vector2 FurthestPointFrom(Line2D line) {
            float furthestDistance = int.MinValue;
            int index = 0;

            for (int i = 0; i < Vertices.Length; i++) {
                float distance = line.PerpendicularDistance(Vertices[i]);
                if (distance > furthestDistance) {
                    furthestDistance = distance;
                    index = i;
                }
            }

            return Vertices[index];
        }

        /// <summary>
        /// Returns the furthest point of this polygon to a segment.
        /// </summary>
        /// <param name="segment"></param>
        /// <returns>The furthest point of this polygon to another segment.</returns>
        public virtual Vector2 FurthestPointFrom(Segment2D segment) {
            float furthestDistance = int.MinValue;
            int index = 0;

            for (int i = 0; i < Vertices.Length; i++) {
                float distance = segment.PerpendicularDistance(Vertices[i]);
                if (distance > furthestDistance) {
                    furthestDistance = distance;
                    index = i;
                }
            }

            return Vertices[index];
        }

        /// <summary>
        /// Returns the furthest point of this box to another box.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>The furthest point of this box to another box.</returns>
        public virtual Vector2 FurthestPointFrom(Polygon other) {
            return FurthestPointFrom(other.Center);
        }
        #endregion

        #region PerpendicularPoints
        /// <summary>
        /// Returns each edge's perpendicular point to a given point.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public virtual List<Vector2> PerpendicularPointsTo(Vector2 point) {
            List<Vector2> perpendicularPoints = new List<Vector2>();

            for (int i = 0; i < Edges.Length; i++) {
                perpendicularPoints.Add(Edges[i].PerpendicularPoint(point));
            }

            return perpendicularPoints;
        }
    
        /// <summary>
        /// Returns the closest point of this box to a point.
        /// </summary>
        /// <param name="point"></param>
        /// <returns>The closest point of this box to a point.</returns>
        public virtual Vector2 ClosestPerpendicularPointTo(Vector2 point) {
            List<Vector2> perpendicularPoints = PerpendicularPointsTo(point);
            return Hedra.ClosestPoint(point, perpendicularPoints);
        }
        #endregion

        #region ClosestVertexTo
        /// <summary>
        /// Returns the closest Vertex of this box to a point.
        /// </summary>
        /// <param name="point"></param>
        /// <returns>The closest Vertex of this box to a point.</returns>
        public virtual Vector2 ClosestVertexTo(Vector2 point) {
            return Hedra.ClosestPoint(point, Vertices.ToList());
        }

        /// <summary>
        /// Returns the closest Vertex of this polygon to a line.
        /// </summary>
        /// <param name="line"></param>
        /// <returns>The closest Vertex of this polygon to a line.</returns>
        public virtual Vector2 ClosestVertexTo(Line2D line) {
            return Hedra.ClosestObject(Vertices.ToList(), (vertex => line.PerpendicularDistance(vertex)));
        }

        /// <summary>
        /// Returns the closest Vertex of this polygon to a segment.
        /// </summary>
        /// <param name="segment"></param>
        /// <returns>The closest Vertex of this polygon to a segment.</returns>
        public virtual Vector2 ClosestVertexTo(Segment2D segment) {
            return Hedra.ClosestObject(Vertices.ToList(), (vertex => segment.PerpendicularDistance(vertex)));
        }

        /// <summary>
        /// Returns the closest Vertex of this polygon to another polygon.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>The closest Vertex of this polygon to another polygon.</returns>
        public virtual Vector2 ClosestVertexTo(Polygon other) {
            throw new NotImplementedException();
        }
        #endregion

        #region FurthestVertexFrom
        /// <summary>
        /// Returns the furthest Vertex of this polygon to a point.
        /// </summary>
        /// <param name="point"></param>
        /// <returns>The furthest Vertex of this polygon to a point.</returns>
        public virtual Vector2 FurthestVertexFrom(Vector2 point) {
            return Hedra.FurthestPoint(point, Vertices.ToList());
        }

        /// <summary>
        /// Returns the furthest Vertex of this polygon to a line.
        /// </summary>
        /// <param name="line"></param>
        /// <returns>The furthest Vertex of this polygon to a line.</returns>
        public virtual Vector2 FurthestVertexFrom(Line2D line) {
            return Hedra.FurthestObject(Vertices.ToList(), (vertex => line.PerpendicularDistance(vertex)));
        }

        /// <summary>
        /// Returns the furthest Vertex of this polygon to a segment.
        /// </summary>
        /// <param name="segment"></param>
        /// <returns>The furthest Vertex of this polygon to a segment.</returns>
        public virtual Vector2 FurthestVertexFrom(Segment2D segment) {
            return Hedra.FurthestObject(Vertices.ToList(), (vertex => segment.PerpendicularDistance(vertex)));
        }

        /// <summary>
        /// Returns the furthest Vertex of this polygon to another polygon.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>The furthest Vertex of this polygon to another polygon.</returns>
        public virtual Vector2 FurthestVertexFrom(Polygon other) {
            return Hedra.FurthestPoint(other.Center, Vertices.ToList()); 
        }
        #endregion

        /// <summary>
        /// Returns the Vertices of this box inside another box.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public virtual List<Vector2> VerticesInside(Polygon other) {
            if (other == null) {
                return null;
            }

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
        public virtual Vector2 DeepestVertexIn(Polygon other) {
            Vector2 furthestVertex = FurthestVertexFrom(other);
            return Hedra.FurthestPoint(furthestVertex, Vertices.ToList());
        }

        /// <summary>
        /// Returns the Edges of this polygon inside another polygon.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public virtual List<Segment2D> EdgesInside(Polygon other, bool includeIntersections) {
            if (other == null) {
                return null;
            }

            List<Segment2D> edges = new List<Segment2D>();
            for (int i = 0; i < Edges.Length; i++) {
                Segment2D edge = Edges[i];

                bool a_contained = other.Contains(edge.PointA);
                bool b_contained = other.Contains(edge.PointB);
                bool checksOut = (a_contained && b_contained) || (includeIntersections && (a_contained || b_contained));

                if (checksOut) {
                    edges.Add(edge);
                }
            }

            return edges;
        }


        #region ClosestEdgeTo
        /// <summary>
        /// Returns the closest edge of this box to a point.
        /// </summary>
        /// <param name="point"></param>
        /// <returns>The closest edge of this box to a point.</returns>
        public virtual Segment2D ClosestEdgeTo(Vector2 point) {
            List<Vector2> perpendicularPoints = PerpendicularPointsTo(point);
            Vector2 closestPoint = Hedra.ClosestPoint(point, perpendicularPoints);
            int closestEdgeIndex = perpendicularPoints.IndexOf(closestPoint);
            return Edges[closestEdgeIndex];
        }

        public virtual Segment2D ClosestEdgeTo(Line2D line) {
            throw new NotImplementedException();
        }

        public virtual Segment2D ClosestEdgeTo(Segment2D segment) {
            Vector2 closestVertex = ClosestVertexTo(segment);
            int vertexIndex = GetVertexIndex(closestVertex);

            int previousVertex = vertexIndex > 0 ? vertexIndex - 1 : Vertices.Length - 1;
            int nextVertex = vertexIndex < Vertices.Length - 1 ? vertexIndex + 1 : 0;

            float previousDistance = segment.PerpendicularDistance(Vertices[previousVertex]);
            float nextDistance = segment.PerpendicularDistance(Vertices[nextVertex]);

            if (previousDistance >= nextDistance) {
                return new Segment2D(closestVertex, Vertices[previousVertex]);
            } else {
                return new Segment2D(closestVertex, Vertices[nextVertex]);
            }
        }

        /// <summary>
        /// Returns the closest edge of this box to another box.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>The closest edge of this box to another box.</returns>
        public virtual Segment2D ClosestEdgeTo(Polygon other) {
            return ClosestEdgeTo(other.Center);
        }
        #endregion

        #region FurthestEdgeFrom
        /// <summary>
        /// Returns the furthest edge of this box to a point.
        /// </summary>
        /// <param name="point"></param>
        /// <returns>The furthest edge of this box to a point.</returns>
        public virtual Segment2D FurthestEdgeFrom(Vector2 point) {
            List<Vector2> perpendicularPoints = PerpendicularPointsTo(point);
            Vector2 closestPoint = Hedra.FurthestPoint(point, perpendicularPoints);
            int closestEdgeIndex = perpendicularPoints.IndexOf(closestPoint);
            return Edges[closestEdgeIndex];
        }

        public virtual Segment2D FurthestEdgeFrom(Line2D line) {
            throw new NotImplementedException();
        }

        public virtual Segment2D FurthestEdgeFrom(Segment2D segment) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the furthest edge of this box to another box.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>The furthest edge of this box to another box.</returns>
        public virtual Segment2D FurthestEdgeFrom(Polygon other) {
            return FurthestEdgeFrom(other.Center);
        }
        #endregion

        #region EdgeFacingTowards        
        /// <summary>
        /// Returns the edge of this box facing a point.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public virtual Segment2D EdgeFacingTowards(Vector2 point) {
            Vector2 closestPoint = ClosestPointTo(point);

            int i = 0;
            while (i < Edges.Length && !Edges[i].Contains(closestPoint)) {
                i++;
            }

            return i < Edges.Length ? Edges[i] : null;
        }

        public virtual Segment2D EdgeFacingTowards(Line2D line) {
            throw new NotImplementedException();
        }
    
        /// <summary>
        /// Returns the edge of this box facing another edge (even if technically, it isn't the closest one).
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public virtual Segment2D EdgeFacingTowards(Segment2D edge) {
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
        public virtual Segment2D EdgeFacingTowards(Polygon box) {

            /* List<Vector2> edgeCenters = new List<Vector2> { Edges[0].MiddlePoint, Edges[1].MiddlePoint, Edges[2].MiddlePoint, Edges[3].MiddlePoint };
             Vector2 closestCenter = Hedra.GetClosestPoint(box.Center, edgeCenters);
             return Edges[edgeCenters.IndexOf(closestCenter)];
             */
            Vector2 closestPoint = ClosestPerpendicularPointTo(ClosestVertexTo(this));

            int i = 0;
            while (i < Edges.Length && !Edges[i].Contains(closestPoint)) {
                i++;
            }

            return i < Edges.Length ? Edges[i] : null;
        }
        #endregion

        #region HiddenEdgeFrom
        /// <summary>
        /// Returns the edge hidden from the view perspective of a point towards this box.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public virtual Segment2D HiddenEdgeFrom(Vector2 point) {
            Vector2 furthestPoint = FurthestPointFrom(point);

            int i = 0;
            while (i < Edges.Length && !Edges[i].Contains(furthestPoint)) {
                i++;
            }

            return i < Edges.Length ? Edges[i] : null;
        }

        public virtual Segment2D HiddenEdgeFrom(Line2D line) {
            throw new NotImplementedException();
        }

        public virtual Segment2D HiddenEdgeFrom(Segment2D edge) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the edge hidden from the view perspective of a box towards this box.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public virtual Segment2D HiddenEdgeFrom(Polygon box) {
            Vector2 furthestPoint = FurthestPointFrom(box);

            int i = 0;
            while (i < Edges.Length && !Edges[i].Contains(furthestPoint)) {
                i++;
            }

            return i < Edges.Length ? Edges[i] : null;
        }
        #endregion
        #endregion

        #region Collisions
        /// <summary>
        /// Moves this polygon to the correct position after collision with an obstacle has been detected.
        /// </summary>
        /// <param name="pastSelf"></param>
        /// <param name="obstacle"></param>
        /// <returns></returns>
        public virtual void ProcessCollision(Polygon pastSelf, Polygon obstacle) {
            Vector2 offset = this.CalculateCollisionOffset(pastSelf, obstacle);
            Translate(offset);
        }

        /// <summary>
        /// Calculates the offset that must be applied for this polygon to be positioned at the moment of collision with an obstacle.
        /// </summary>
        /// <param name="pastSelf"></param>
        /// <param name="obstacle"></param>
        /// <returns></returns>
        public abstract Vector2 CalculateCollisionOffset(Polygon pastSelf, Polygon obstacle);
        public abstract Collider2D[] CheckCollisions(LayerMask mask);
        public abstract Collider2D[] CheckCollisionsAt(Vector2 position, LayerMask mask);
        #endregion

        #region Utils

        protected List<Vector2> RemoveVertexDuplicates(List<Vector2> original, int precision = 3) {
            List<Vector2> copy = new List<Vector2>();
            for (int i = 0; i < original.Count; i++) {
                Vector2 v1 = Hedra.Round(original[i], precision);
                if (!copy.Contains(v1)) {
                    copy.Add(v1);
                }
            }
            return copy;
        }

        protected bool EqualVertices(Vector2 v1, Vector2 v2, int precision = 3) {
            Vector2 a1 = Hedra.Truncate(v1, precision);
            Vector2 a2 = Hedra.Truncate(v2, precision);
            Vector2 difference = a1 - a2;
            return difference.magnitude < EPSILON;
        }

        #endregion

        #region Debug   
        public virtual void DrawGizmos(bool drawData, float size = 0.2f) {
            DrawEdges();

            if (!drawData) {
                return;
            }

            DrawVertices(size);
            DrawNormals(size);
        }

        public virtual void DrawEdges() {
            for (int i = 1; i < Vertices.Length; i++) {
                Gizmos.DrawLine(Vertices[i - 1], Vertices[i]);
            }
            Gizmos.DrawLine(Vertices[0], Vertices[Vertices.Length - 1]);
        }

        public virtual void DrawVertices(float size = 0.2f) {
            for (int i = 0; i < Vertices.Length; i++) {
                Gizmos.DrawWireSphere(Vertices[i], size + i * size/2);
            }

            Gizmos.DrawWireSphere(Center, size);
        }

        public virtual void DrawNormals(float size = 0.2f) {
            Gizmos.color = Color.black;
            for (int i = 0; i < Normals.Length; i++) {
                Normals[i].DrawGizmos(true, size / 3);
            }
        }
 
        #endregion
    }
}