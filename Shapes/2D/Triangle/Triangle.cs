using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HedraLibrary.Components {
    public partial class Triangle : Polygon {

        // 0: A | 1: B | 2: C

        public float[] Angles { get; protected set; }
        public Segment2D[] Heights { get; protected set; }

        public Triangle(Vector2 a, float ab, float ac, float alpha) {
            // https://www.mathsisfun.com/algebra/trig-solving-sas-triangles.html
            Vertices = new Vector2[3];
            Angles = new float[3];

            Vertices[0] = a;
            Angles[0] = alpha;

            float radAlpha = alpha * Mathf.Rad2Deg;
            float bc = Mathf.Sqrt(Mathf.Pow(ac, 2) + Mathf.Pow(ab, 2) - 2 * ab * ac * Mathf.Cos(radAlpha));

            Angles[1] = Mathf.Asin((Mathf.Sin(radAlpha) * ac) / bc) * Mathf.Deg2Rad;
            Angles[2] = 180 - Angles[1] - Angles[0];

            Vertices[2] = Vertices[0] + Vector2.right * ac;
            Vertices[1] = Hedra.Rotate(Vertices[0], Vertices[2], Angles[0]).normalized * ab;

            CalculateCenter();
            StoreEdges();
            StoreNormals();
            StoreHeights();
            CalculateArea();
            rotation = 0;
        }

        public Triangle(Vector2 a, Vector2 b, Vector2 c) {
            Vertices = new Vector2[] { a, b, c };
            CalculateCenter();
            StoreEdges();
            CalculateAngles();
            StoreNormals();
            StoreHeights();
            CalculateArea();
            rotation = 0;
        }

        public Triangle(Segment2D a, Segment2D b, Segment2D c) {

        }

        public Triangle(Vector2 a, Vector2 b, Vector2 c, float alpha, float beta, float gamma) {
            Vertices = new Vector2[] { a, b, c };
            Angles = new float[] { alpha, beta, gamma };

            CalculateCenter();
            StoreEdges();
            StoreNormals();
            StoreHeights();
            CalculateArea();
            rotation = 0;
        }

        protected virtual void CalculateCenter() {
            Vector2 center = Vector2.zero;
            center.x = (Vertices[0].x + Vertices[1].x + Vertices[2].x) / 3f;
            center.y = (Vertices[0].y + Vertices[1].y + Vertices[2].y) / 3f;
            Center = center;
        }

        protected virtual void StoreEdges() {
            Edges = new Segment2D[3];
            Edges[0] = new Segment2D(Vertices[0], Vertices[1]); // AB
            Edges[1] = new Segment2D(Vertices[1], Vertices[2]); // BC
            Edges[2] = new Segment2D(Vertices[0], Vertices[2]); // AC
        }

        protected virtual void CalculateAngles() {
            Angles = new float[3];
            Angles[0] = Hedra.Angle(Edges[0].Vector, Edges[2].Vector); // Alpha
            Angles[1] = Hedra.Angle(Edges[0].Vector, Edges[1].Vector); // Beta
            Angles[2] = 180 - Angles[0] - Angles[1]; // Gamma
        }

        protected virtual void StoreHeights() {
            Heights = new Segment2D[3];
            Heights[0] = new Segment2D(Vertices[0], Edges[1].PerpendicularPoint(Vertices[0]));    // Height A
            Heights[1] = new Segment2D(Vertices[1], Edges[2].PerpendicularPoint(Vertices[1]));    // Height B
            Heights[2] = new Segment2D(Vertices[2], Edges[0].PerpendicularPoint(Vertices[2]));    // Height C
        }

        protected virtual void StoreNormals() {
            Normals = new Line2D[3];
            Normals[0] = new Line2D(Center, Edges[1].MiddlePoint);
            Normals[1] = new Line2D(Center, Edges[2].MiddlePoint);
            Normals[2] = new Line2D(Center, Edges[0].MiddlePoint);
        }

        protected virtual void CalculateArea() {
            // Heron's formula            
            float a = Edges[1].Vector.magnitude;
            float b = Edges[2].Vector.magnitude;

            Area = 0.5f * a * b * Mathf.Sin(Angles[2] * Mathf.Rad2Deg);
        }

        protected virtual void StoreTriangles() {
            Triangles = new Triangle[3];
            Triangles[0] = new Triangle(center, Vertices[0], Vertices[1]); // C A B
            Triangles[1] = new Triangle(center, Vertices[1], Vertices[2]); // C B C
            Triangles[2] = new Triangle(center, Vertices[0], Vertices[2]); // C A C
        }

        public override string ToString() {
            string s = "";
            s += "Vertices: " + Vertices.Join(", ") + "\n";
            s += "Edges: " + Edges.Join(", ") + "\n";
            s += "Normals: " + Normals.Join(", ") + "\n";
            s += "Angles: " + Angles.Join(", ") + "\n";
            s += "Heights: " + Heights.Join(", ") + "\n";
            return s;
        }

        #region Debug   
        public virtual void DrawGizmos(bool drawData, float size = 0.2f) {
            base.DrawGizmos(drawData, size);

            if (!drawData) {
                return;
            }

            DrawHeights(size / 3);
            DrawInnerTriangles();
        }

        public virtual void DrawHeights(float size = 0.2f) {
            Gizmos.color = Color.yellow;
            for (int i = 0; i < Heights.Length; i++) {
                Heights[i].DrawGizmos(true, size);
            }
        }

        public virtual void DrawInnerTriangles() {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(Center, Vertices[0]);
            Gizmos.DrawLine(Center, Vertices[1]);
            Gizmos.DrawLine(Center, Vertices[2]);
        }
        #endregion
    }
}