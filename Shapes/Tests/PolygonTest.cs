using HedraLibrary.Shapes.Polygons;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HedraLibrary.Shapes.Tests {
    public class PolygonTest : MonoBehaviour {
        public enum Shape {
            Triangle,
            Rectangle,
            RegularPolygon
        }

        [Serializable]
        public struct Segment2DTest {
            public Transform A;
            public Transform B;
        }

        [Serializable]
        public struct Shape2DTest {
            public Shape shape;
            public int vertices;
            public float size;
            public Vector2 center;

            public Polygon GetPolygon(Transform transform) {
                switch (shape) {
                    case Shape.Triangle:
                        return GetTriangle(transform);
                    case Shape.Rectangle:
                        return GetRectangle(transform);
                    case Shape.RegularPolygon:
                        return GetCircle(transform);
                }

                return null;
            }

            Polygon GetTriangle(Transform transform) {
                vertices = 3;
                List<Vector2> circle = Hedra.Circle(transform.position, size, vertices);
                Polygon polygon = new Triangle(circle[0], circle[1], circle[2]);
                polygon.Rotation = transform.rotation.eulerAngles.z;
                polygon.Center = (Vector2)transform.position + center;
                return polygon;
            }

            Polygon GetRectangle(Transform transform) {
                vertices = 4;
                BoxCollider2D collider = transform.GetComponent<BoxCollider2D>();
                if (collider == null) {
                    return new Rectangle((Vector2)transform.position + center, new Vector2(size, size), transform.rotation.eulerAngles.z);
                } else {
                    Polygon polygon = new Rectangle(collider);
                    return polygon;
                }
            }

            Polygon GetCircle(Transform transform) {
                CircleCollider2D collider = transform.GetComponent<CircleCollider2D>();
                if (collider == null) {
                    return new RegularPolygon((Vector2)transform.position + center, vertices, size, transform.rotation.eulerAngles.z);
                } else {
                    Polygon polygon = new RegularPolygon(collider, vertices);
                    return polygon;
                }
            }
        }

        public List<Shape2DTest> shapes;
        public Transform point;
        public float gizmosSize = 0.5f;

        List<Polygon> polygons;

        void OnDrawGizmos() {
            polygons = new List<Polygon>();
            for (int i = 0; i < shapes.Count; i++) {
                polygons.Add(shapes[i].GetPolygon(this.transform));
            }

            Gizmos.color = Color.white;
            DrawPolygons();

            for (int i = 0; i < polygons.Count; i++) {
                Segment2D segment = new Segment2D(polygons[i].Center, point.position);
                DrawSegment(segment);
                ProcessPolygon(polygons[i], segment);
            }

            Gizmos.color = Color.white;
        }

        void DrawPolygons() {
            for (int i = 0; i < polygons.Count; i++) {
                polygons[i].DrawEdges();
                polygons[i].DrawNormals();
                Gizmos.color = Color.white;
            }
        }

        void DrawSegment(Segment2D segment) {
            if (AllContainSegment(segment)) {
                Gizmos.color = Color.magenta;
            } else if (AnyContainsSegment(segment)) {
                Gizmos.color = Color.green;
            }

            segment.DrawGizmos(false, gizmosSize);
        }

        bool AllContainSegment(Segment2D segment) {
            int i = 0;
            while (i < polygons.Count && polygons[i].Contains(segment)) {
                i++;
            }
            return i >= polygons.Count;
        }

        bool AnyContainsSegment(Segment2D segment) {
            int i = 0;
            while (i < polygons.Count && !polygons[i].Contains(segment)) {
                i++;
            }
            return i <= polygons.Count;
        }

        void ProcessPolygon(Polygon P, Segment2D S) {
            if (P.ContainsInEdge(point.position)) {
                Gizmos.color = Color.red;
            } else if (P.Contains(point.position)) {
                Gizmos.color = Color.green;
            } else {
                Gizmos.color = Color.white;
            }

            Gizmos.DrawWireSphere(point.position, gizmosSize);

            
            DrawPointIntersections(P, S);
            DrawSegmentIntersections(P, S);
        }

        void DrawPointIntersections(Polygon P, Segment2D S) {
            if (!P.Intersects(S)) {
                return;
            }

            List<Vector2> points = P.IntersectionPoints(S);
            for (int i = 0; i < points.Count; i++) {
                Gizmos.DrawWireSphere(points[i], gizmosSize / 3f);
            }

            List<Segment2D> edges = P.IntersectingEdges(S);
            foreach (Segment2D edge in edges) {
                Gizmos.color = Color.red;
                edge.DrawGizmos(false);

                Vector2 edgePPoint = edge.PerpendicularPoint(point.position);
                Gizmos.color = Color.black;
                Gizmos.DrawWireSphere(edgePPoint, gizmosSize / 3f);
                Gizmos.DrawLine(edgePPoint, point.position);

                Vector2 linePPoint = new Line2D(edge.PointA, edge.PointB).PerpendicularPoint(point.position);
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(linePPoint, gizmosSize / 3f);
                Gizmos.DrawLine(linePPoint, point.position);
            }
            
        }

        void DrawSegmentIntersections(Polygon P, Segment2D S) {
            if (!P.Intersects(S)) {
                return;
            }

            Gizmos.color = Color.magenta;
            List<Vector2> points = P.IntersectionPoints(S);
            for (int i = 0; i < points.Count; i++) {
                Gizmos.DrawWireSphere(points[i], gizmosSize / 3f);
            }
        }
    }
}