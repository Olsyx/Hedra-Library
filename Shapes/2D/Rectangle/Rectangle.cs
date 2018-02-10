using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HedraLibrary.Components {
    public partial class Rectangle : Polygon {

        public Collider2D Collider { get; protected set; }

        public new Vector2 Center {
            get { return center; }
            set {
                center = value;
                StoreInformation();
            }
        }

        public new float Rotation {
            get { return rotation; }
            set {
                rotation = value;
                StoreInformation();
            }
        }

        private Vector2 size;
        public Vector2 Size {
            get { return size; }
            set {
                size = value;
                StoreInformation();
            }
        }

        public Triangle[] Triangles { get; protected set; }
        
        public Rectangle(Vector2 position, Vector2 size, float rotation) {
            this.rotation = rotation;
            this.size = size;
            Center = position;
            StoreInformation();
        }

        public Rectangle(Collider2D collider) {
            Collider = collider;
            Transform owner = collider.gameObject.transform;

            if (collider.GetType() == typeof(BoxCollider2D)) {
                size = ((BoxCollider2D)collider).size;
            } else {
                size = collider.bounds.size;
            }

            rotation = owner.rotation.eulerAngles.z;
            Center = collider.bounds.center;
            StoreInformation();
        }

        public Rectangle(BoxCollider2D collider) {
            Transform owner = collider.gameObject.transform;

            Collider = collider;
            Size = collider.size;
            rotation = owner.rotation.eulerAngles.z;
            Center = collider.bounds.center;
            StoreInformation();
        }

        protected virtual void StoreInformation() {
            StoreVertices();
            StoreEdges();
            StoreNormals();
            StoreArea();
        }

        protected virtual void StoreVertices() {
            Vector2 halfSize = size / 2f;
            Vertices = new Vector2[4];
            Vertices[0] = Hedra.Rotate(Center, new Vector2(Center.x - halfSize.x, Center.y + halfSize.y), Rotation);
            Vertices[1] = Hedra.Rotate(Center, new Vector2(Center.x + halfSize.x, Center.y + halfSize.y), Rotation);
            Vertices[2] = Hedra.Rotate(Center, new Vector2(Center.x + halfSize.x, Center.y - halfSize.y), Rotation);
            Vertices[3] = Hedra.Rotate(Center, new Vector2(Center.x - halfSize.x, Center.y - halfSize.y), Rotation);
        }

        protected virtual void StoreEdges() {
            Edges = new Segment2D[4];
            Edges[0] = new Segment2D(Vertices[0], Vertices[1]);
            Edges[1] = new Segment2D(Vertices[1], Vertices[2]);
            Edges[2] = new Segment2D(Vertices[2], Vertices[3]);
            Edges[3] = new Segment2D(Vertices[3], Vertices[0]);
        }

        protected virtual void StoreNormals() {
            Normals = new Segment2D[Edges.Length];

            for (int i = 0; i < Edges.Length; i++) {
                Normals[i] = new Segment2D(Center, Edges[i].MiddlePoint);
            }
        }
        
        protected virtual void StoreArea() {
            Area = Size.x * Size.y;
        }

        protected virtual void GenerateTriangles() {
            Triangles = new Triangle[4];
            Triangles[0] = new Triangle(Center, Vertices[0], Vertices[1]);
            Triangles[1] = new Triangle(Center, Vertices[1], Vertices[2]);
            Triangles[2] = new Triangle(Center, Vertices[2], Vertices[3]);
            Triangles[3] = new Triangle(Center, Vertices[0], Vertices[3]);
        }

    }
}