using HedraLibrary;
using HedraLibrary.Components;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Polygon : Hedron2D {

    public Vector2[] Vertices { get; protected set; }
    public Segment2D[] Edges { get; protected set; }
    public Line2D[] Normals { get; protected set; }
    public Triangle[] Triangles { get; protected set; }

    public virtual bool Intersects(Polygon other) {
        List<Vector2> intersectionPoints = new List<Vector2>();
        for (int i = 0; i < Edges.Length; i++) {
            intersectionPoints.AddRange(IntersectionPoints(Edges[i]));
        }
        return intersectionPoints != null && intersectionPoints.Count > 0;
    }

    public override List<Vector2> IntersectionPoints(Line2D line) {
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
    public override List<Vector2> IntersectionPoints(Segment2D segment) {
        List<Vector2> intersections = new List<Vector2>();

        Vector2 point;
        for (int i = 0; i < Edges.Length; i++) {
            point = segment.IntersectionPoint(Edges[i]);
            if (!point.IsNaN() && !point.IsInfinity() && segment.Contains(point)) {
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
    public List<Vector2> IntersectionPoints(Polygon other) {
        List<Vector2> intersections = new List<Vector2>();
        for (int i = 0; i < Edges.Length; i++) {
            intersections = intersections.Union(other.IntersectionPoints(Edges[i])).ToList();
        }
        return intersections;
    }
    
    /// <summary>
    /// Returns true if this polygon contains the given point.
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public override bool Contains(Vector2 point) {
        int i = Edges.Length - 1;   // Corners are stored in clockwise direction.

        while (i >= 0 && Edges[i].GetPointSituation(point) < 0) {
            i--;
        }

        return i < 0;
    }

    /// <summary>
    /// Returns true if this polygon contains the given segment.
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public override bool Contains(Segment2D segment) {
        return Contains(segment.PointA) && Contains(segment.PointB);
    }

    /// <summary>
    /// Returns true if this polygon contains all the given points.
    /// </summary>
    /// <param name="points"></param>
    /// <returns></returns>
    public override bool ContainsAll(Vector2[] points) {
        int i = 0;
        while (i < points.Length && Contains(points[i])) {
            i++;
        }
        return i >= points.Length;
    }

    public override bool ContainsAll(List<Vector2> points) {
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
    public virtual bool ContainsInAnyEdge(Vector2 point) {
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
    public virtual bool ContainsInAnyEdge(Vector2 point, float margin) {
        int i = 0;
        while (i < Edges.Length && !Edges[i].Contains(point, margin)) {
            i++;
        }

        return i < Edges.Length;
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
        for (int i = 0; i < other.Edges.Length; i++) {
            intersections = intersections.Union(IntersectingEdges(other.Edges[i])).ToList();
        }
        return intersections;
    }
    
    /// <summary>
    /// Returns the closest point of this polygon to a point.
    /// </summary>
    /// <param name="point"></param>
    /// <returns>The closest point of this polygon to a point.</returns>
    public override Vector2 ClosestPointTo(Vector2 point) {
        Segment2D cast = new Segment2D(Center, point);
        List<Vector2> intersections = IntersectionPoints(cast);
        if (intersections.Count == 0) { // Point is inside triangle
            Line2D line = new Line2D(Center, point);
            intersections = IntersectionPoints(line);
            return Hedra.GetClosestPoint(point, intersections);
        } else if (intersections.Count == 1) {
            return intersections[0];
        }

        return Hedra.GetClosestPoint(point, intersections);
    }

    public abstract Vector2 ClosestPointTo(Line2D line);
    public abstract Vector2 ClosestPointTo(Segment2D segment);
    public abstract Vector2 ClosestPointTo(Polygon other);

    /// <summary>
    /// Returns the furthest point of this polygon to a point.
    /// </summary>
    /// <param name="point"></param>
    /// <returns>The furthest point of this polygon to another point.</returns>
    public override Vector2 FurthestPointFrom(Vector2 point) {
        Line2D line = new Line2D(Center, point);
        List<Vector2> intersections = IntersectionPoints(line);
        return Hedra.GetFurthestPoint(point, intersections);
    }

    public abstract Vector2 FurthestPointFrom(Line2D line);
    public abstract Vector2 FurthestPointFrom(Segment2D segment);
    public abstract Vector2 FurthestPointFrom(Polygon other);

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
        return Hedra.GetClosestPoint(point, perpendicularPoints);
    }
    
    /// <summary>
    /// Returns the closest point of this box to another box.
    /// </summary>
    /// <param name="other"></param>
    /// <returns>The closest point of this box to another box.</returns>
    public virtual Vector2 ClosestPerpendicularPointTo(Polygon other) {
        return ClosestPerpendicularPointTo(other.Center);
    }
    
    public abstract Vector2 ClosestVertexTo(Vector2 point);
    public abstract Vector2 ClosestVertexTo(Line2D line);
    public abstract Vector2 ClosestVertexTo(Segment2D segment);
    public abstract Vector2 ClosestVertexTo(Polygon other);

    public abstract Vector2 FurthestVertexFrom(Vector2 point);
    public abstract Vector2 FurthestVertexFrom(Line2D line);
    public abstract Vector2 FurthestVertexFrom(Segment2D segment);
    public abstract Vector2 FurthestVertexFrom(Polygon other);

    public abstract List<Vector2> VerticesInside(Polygon other);

    public abstract Vector2 DeepestVertexIn(Polygon other);

    public abstract Segment2D ClosestEdgeTo(Vector2 point);
    public abstract Segment2D ClosestEdgeTo(Line2D line);
    public abstract Segment2D ClosestEdgeTo(Segment2D segment);
    public abstract Segment2D ClosestEdgeTo(Polygon other);
    
    public abstract Segment2D FurthestEdgeFrom(Vector2 point);
    public abstract Segment2D FurthestEdgeFrom(Line2D line);
    public abstract Segment2D FurthestEdgeFrom(Segment2D segment);
    public abstract Segment2D FurthestEdgeFrom(Polygon other);
    
    public abstract Segment2D EdgeFacingTowards(Vector2 point);
    public abstract Segment2D EdgeFacingTowards(Line2D line);
    public abstract Segment2D EdgeFacingTowards(Segment2D edge);
    public abstract Segment2D EdgeFacingTowards(Polygon other);

    public abstract Segment2D HiddenEdgeFrom(Vector2 point);
    public abstract Segment2D HiddenEdgeFrom(Line2D line);
    public abstract Segment2D HiddenEdgeFrom(Segment2D edge);
    public abstract Segment2D HiddenEdgeFrom(Polygon other);

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
            Gizmos.DrawWireSphere(Vertices[i], size);
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
