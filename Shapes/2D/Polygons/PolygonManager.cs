using HedraLibrary.Components;
using HedraLibrary.Shapes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HedraLibrary.Shapes.Polygons {

    public static class PolygonManager {        
        public static List<PolygonCollider> Colliders { get; private set; }

        public static void Register(PolygonCollider polygon) {
            if (Colliders == null) {
                Colliders = new List<PolygonCollider>();
            }

            if (Colliders.Contains(polygon)) {
                return;
            }

            Colliders.Add(polygon);
        }
        
        public static void Remove(PolygonCollider polygon) {
            if (Colliders == null) {
                Colliders = new List<PolygonCollider>();
                return;
            }

            Colliders.Remove(polygon);
        }

        public static List<PolygonCollider> CheckCollisions(Polygon subject) {
            List<PolygonCollider> collisions = new List<PolygonCollider>();

            for (int i = 0; i < Colliders.Count; i++) {
                if (Colliders[i].Polygon.Intersects(subject)) {
                    collisions.Add(Colliders[i]);
                }
            }

            return collisions;
        }

        public static List<PolygonCollider> CheckCollisionsAt(Vector2 position, Polygon subject) {
            Polygon fake = PolygonManager.Create2D(subject);
            fake.Center = position;
            return CheckCollisions(fake);
        }

        public static Polygon Create2D(Collider2D collider) {
            if (collider is BoxCollider2D) {
                return new Rectangle((BoxCollider2D)collider);
            } else if (collider is CircleCollider2D) {
                return new RegularPolygon((CircleCollider2D)collider, 32);
            }

            return null;
        }

        public static Polygon Create2D(Polygon polygon) {
            if (polygon is Triangle) {
                return new Triangle((Triangle)polygon);
            } else if (polygon is Rectangle) {
                return new Rectangle((Rectangle)polygon);
            } else if (polygon is RegularPolygon) {
                return new RegularPolygon((RegularPolygon)polygon);
            }

            return null;
        }
    }
}