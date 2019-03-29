using HedraLibrary.Shapes.Polygons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HedraLibrary.Shapes.Tests {

    [RequireComponent(typeof(BoxCollider2D))]
    public class HyperColliderMachine : MonoBehaviour {

        [SerializeField] protected int spawn;
        [SerializeField] protected float speed;
        [SerializeField] protected float granularity = 1;

        List<Polygon> polygons;
        List<Vector2> directions;
        float size;

        BoxCollider2D space;

        void Awake() {
            Init();
            Spawn();
        }

        void Init() {
            space = GetComponent<BoxCollider2D>();
            polygons = new List<Polygon>();
            directions = new List<Vector2>();
            size = (Mathf.Min(space.bounds.size.x, space.bounds.size.y) / spawn) / granularity;
        }

        void Spawn() {
            Vector2 center = (Vector2) transform.position + space.offset;
            for (int i = 0; i < spawn; i++) {
                float x = Random.Range(center.x - (space.bounds.size.x / 2) + size / 2, center.x + (space.bounds.size.x / 2) - size / 2);
                float y = Random.Range(center.y - (space.bounds.size.y / 2) + size / 2, center.y + (space.bounds.size.y / 2) - size / 2);
                Triangle polygon = new Triangle(new Vector2(x, y), size/2);
                polygons.Add(polygon);
                directions.Add(new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)));
            }
        }
        
        void Start() {

        }
        
        void Update() {
            for (int i = 0; i < polygons.Count; i++) {
                UpdatePolygon(i);
            }
        }

        void UpdatePolygon(int index) {
            Polygon polygon = polygons[index];
            
            Vector2 newDirection = HandleCollisions(polygon, directions[index]);
            if (directions[index] != newDirection) {
                AddVertex(index);
            }

            directions[index] = newDirection;

            if (!IsWithinSpace(polygon.Center)) {
                directions[index] = ((Vector2) transform.position - polygon.Center).normalized;
            } 
        }

        Vector2 HandleCollisions(Polygon polygon, Vector2 direction) {
            Vector2 newPosition = direction * speed * Time.deltaTime;
            Polygon fake = PolygonManager.Create2D(polygon);
            fake.Center = newPosition;

            List<Polygon> collisions = CollidingPolygons(fake);
            if (collisions == null || collisions.Count <= 0) {
                return direction;
            }

            for (int i = 0; i < collisions.Count; i++) {
                if (fake.Intersects(collisions[i])) { // It may have been corrected
                    fake.ProcessCollision(polygon, collisions[i]);
                }
            }
            
            Vector2 newDirection = -direction + new Vector2(Random.Range(-1, 1), Random.Range(-1, 1));
            return newDirection.normalized;
        }

        void AddVertex(int index) {
            Polygon polygon = polygons[index];
            if (polygon is Triangle) {
                polygon = new Rectangle(polygon.Center, new Vector2(size, size), 0f);
            } else if (polygon is Rectangle) {
                polygon = new RegularPolygon(polygon.Center, 5, size/2, 0f);
            } else if (polygon.Vertices.Length < 12) {
                polygon = new RegularPolygon(polygon.Center, polygon.Vertices.Length + 1, size/2, 0f);
            }
            polygons[index] = polygon;
        }

        List<Polygon> CollidingPolygons(Polygon agent) {
            List<Polygon> collisions = new List<Polygon>();
            for (int i = 0; i < polygons.Count; i++) {
                if (polygons[i] != agent && polygons[i].Intersects(agent)) {
                    collisions.Add(polygons[i]);
                }
            }
            return collisions;
        }

        bool IsWithinSpace(Vector2 position) {
            Vector2 center = (Vector2)transform.position + space.offset;
            if (position.x < center.x - (space.bounds.size.x / 2) || position.x > center.x + (space.bounds.size.x / 2)) {
                return false;
            }

            if (position.y < center.y - (space.bounds.size.y / 2) || position.y > center.y + (space.bounds.size.y / 2)) {
                return false;
            }

            return true;
        }

        void OnDrawGizmos() {
            if (!Application.isPlaying || polygons.Count <= 0) {
                return;
            }

            for (int i = 0; i < polygons.Count; i++) {
                polygons[i].DrawEdges();
                Gizmos.DrawWireSphere(polygons[i].Center, 0.02f);
            }
        }
    }
}
