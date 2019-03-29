using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HedraLibrary.Shapes.Polygons {
    public partial class Rectangle : Polygon {

        private Vector2 size;
        public Vector2 Size {
            get { return size; }
            set {
                size = value;
                Init();
            }
        }

        #region Init
        public Rectangle() {

        }

        public Rectangle(Rectangle other) {
            this.Collider = other.Collider;
            this.rotation = other.rotation;
            this.size = other.size;
            this.center = other.Center;
            this.Vertices = other.Vertices;
            this.Edges = other.Edges;
            this.Area = other.Area;
            this.Normals = other.Normals;
        }

        public Rectangle(Vector2 position, Vector2 size, float rotation) {
            this.rotation = rotation;
            this.size = size;
            center = position;
            Init();
        }
        
        public Rectangle(BoxCollider2D collider) {
            Transform owner = collider.gameObject.transform;

            Collider = collider;
            Size = collider.size;
            rotation = owner.rotation.eulerAngles.z;
            center = collider.bounds.center;
            Init();
        }

        protected override void CreateVertices() {
            Vector2 halfSize = size / 2f;
            Vertices = new Vector2[4];
            Vertices[0] = Hedra.Rotate(Center, new Vector2(Center.x - halfSize.x, Center.y + halfSize.y), Rotation);
            Vertices[1] = Hedra.Rotate(Center, new Vector2(Center.x + halfSize.x, Center.y + halfSize.y), Rotation);
            Vertices[2] = Hedra.Rotate(Center, new Vector2(Center.x + halfSize.x, Center.y - halfSize.y), Rotation);
            Vertices[3] = Hedra.Rotate(Center, new Vector2(Center.x - halfSize.x, Center.y - halfSize.y), Rotation);
            SortVertices();
        }
        
        protected override void CalculateArea() {
            Area = Size.x * Size.y;
        }
        #endregion
        
        /// <summary>
        /// Returns the deepest Vertex of this box in another box.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>The closest Vertex of this box to another box.</returns>
        public virtual Vector2 DeepestVertexIn(Rectangle other) {
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

        #region Collisions
        public override Vector2 CalculateCollisionOffset(Polygon pastSelf, Polygon obstacle) {
            return Vector2.zero;
        }

        public override Collider2D[] CheckCollisions(LayerMask mask) {
            return Physics2D.OverlapBoxAll(Center, Size, Rotation, mask);
        }

        public override Collider2D[] CheckCollisionsAt(Vector2 position, LayerMask mask) {
            return Physics2D.OverlapBoxAll(position, Size, Rotation, mask);
        }
        #endregion

    }
}