using System.Collections;
using System.Collections.Generic;
using HedraLibrary.Shapes.Polygons;
using UnityEngine;
using System.Linq;
using HedraLibrary.Shapes;

namespace HedraLibrary.Components {
    public interface IPolygonColliderManager {
        void SetId(string newId);
    }

    [RequireComponent(typeof(CircleCollider2D))]
    public class RegularPolygonCollider : PolygonCollider {
        private readonly int MIN_VERTICES = 3;
        private readonly int MAX_VERTICES = 60;
        private readonly float MIN_RADIUS = 0.001f;

        [SerializeField] protected int vertices = 3;
        [SerializeField] protected LayerMask polygonMask;

        [Header("Debug")]
        [SerializeField] protected float gizmosSize = 0.02f;
        public Vector2 debugMovement = new Vector2(1,1);

        void Awake() {
            Init();
        }

        protected override void Init() {
            vertices = Hedra.Clamp(vertices, MIN_VERTICES, MAX_VERTICES);
            polygon = new RegularPolygon(GetComponent<CircleCollider2D>(), vertices);
            base.Init();
        }

        void OnDrawGizmos() {
            if (!Application.isPlaying) {
                Init();
            }

            Gizmos.DrawWireSphere(polygon.Center, gizmosSize);
            polygon.DrawEdges();

            Polygon fake = PolygonManager.Create2D(polygon);
            fake.Translate(debugMovement);
            Gizmos.color = Color.gray;
           // fake.DrawEdges();

            List<Collider2D> collisions = fake.CheckCollisionsAt(fake.Center, polygonMask).ToList();// PolygonManager.CheckCollisions(fake);
            collisions.Remove(GetComponent<CircleCollider2D>());
            for (int i = 0; i < collisions.Count; i++) {
                Polygon obstacle = new RegularPolygon((CircleCollider2D)collisions[i], vertices);
                List<Vector2> o_vertices = obstacle.VerticesInside(fake);

                Gizmos.color = Color.white;
                obstacle.DrawEdges();

                DrawDebugCollision(fake, obstacle);
                fake.ProcessCollision(polygon, obstacle);
                Gizmos.color = Color.yellow;
                fake.DrawEdges();
            }
        }

        void DrawDebugCollision(Polygon fake, Polygon obstacle) {
            Gizmos.color = Color.gray;
            ((RegularPolygon)fake).ReduceCollision(obstacle, debugMovement);
            fake.DrawEdges();

            List<Vector2> p_vertices = fake.VerticesInside(obstacle);
            List<Vector2> o_vertices = obstacle.VerticesInside(fake);

            Vector2 p_vertex = Vector2.zero;
            if (p_vertices.Count > 0) {
                p_vertex = p_vertices[0];
                Gizmos.DrawWireSphere(p_vertex, gizmosSize);
            }

            Vector2 o_vertex = Vector2.zero;
            if (o_vertices.Count > 0) {
                o_vertex = o_vertices[0];
                Gizmos.DrawWireSphere(o_vertex, gizmosSize);
            }

            //----------------------------------------------------------------

            if (p_vertex == Vector2.zero || o_vertex == Vector2.zero) {
                return;
            }

            Segment2D p_vertexPath = new Segment2D(polygon.Vertices[fake.GetVertexIndex(p_vertex)], p_vertex);
            p_vertexPath.DrawGizmos(false);

            List<Segment2D> o_edges = obstacle.IntersectingEdges(p_vertexPath);
            if (o_edges == null || o_edges.Count <= 0) {
                return;
            }

            if (fake.ContainsInEdge(o_vertex)) {
                Gizmos.color = Color.magenta;
                List<Vector2> intPoints = obstacle.IntersectionPoints(fake);
                Vector2 pPoint = new Segment2D(intPoints[0], intPoints[1]).PerpendicularPoint(p_vertex);
                Gizmos.DrawLine(pPoint, p_vertex);
                Gizmos.DrawWireSphere(p_vertex, gizmosSize);
                Gizmos.DrawWireSphere(pPoint, gizmosSize);
                return;
            }



            Gizmos.color = Color.cyan;

            Segment2D o_edge = o_edges[0];
            o_edge.DrawGizmos(true, gizmosSize);
            Vector2 p_PPoint = o_edge.ToLine().PerpendicularPoint(p_vertex);
            Gizmos.DrawWireSphere(p_PPoint, gizmosSize/2);

            if (o_edge.Contains(p_PPoint)) {
                Vector2 offset = p_PPoint - p_vertex;
                Gizmos.DrawLine(p_PPoint, p_vertex);
            }

            Gizmos.color = Color.yellow;
            foreach (Vector2 p in fake.IntersectionPoints(obstacle)) {
                Gizmos.DrawWireSphere(p, gizmosSize);
            }

            Gizmos.color = Color.blue;
            List<Segment2D> p_edges = fake.EdgesInside(obstacle, true);
            foreach(Segment2D s in p_edges) {
                s.DrawGizmos(true, gizmosSize);
            }
            
            List<Vector2> o_PPoints = new List<Vector2>();
            
            List<Segment2D> intersectingEdges = fake.EdgesInside(obstacle, true);
            for (int i = 0; i < intersectingEdges.Count; i++) {
                Line2D line = new Line2D(intersectingEdges[i]);
                Vector2 pPoint = line.PerpendicularPoint(o_vertex);
                if (intersectingEdges[i].Contains(pPoint)) {
                    o_PPoints.Add(pPoint);
                }
            }
            Gizmos.color = Color.magenta;
            for (int i = 0; i < o_PPoints.Count; i++) {
                Gizmos.DrawWireSphere(o_PPoints[i], gizmosSize + (i * gizmosSize / 2));
                Gizmos.DrawLine(o_vertex, o_PPoints[i]);
            }

            Gizmos.color = Color.white;
            Gizmos.DrawLine(p_vertex, fake.Center);
            Gizmos.DrawWireSphere(fake.Center, gizmosSize);

        }

        protected void DrawFakeSolution(Polygon original, Polygon obstacle, Vector2 offset) {
            Polygon fakeSolution = PolygonManager.Create2D(original);
            fakeSolution.Translate(offset);
            fakeSolution.DrawEdges();

            List<Vector2> remaining = fakeSolution.VerticesInside(obstacle);

            Gizmos.color = Color.black;
            foreach (Vector2 v in obstacle.IntersectionPoints(fakeSolution)) {
                Gizmos.DrawWireSphere(v, gizmosSize / 2);
            }
            foreach (Vector2 v in remaining) {
                Gizmos.DrawWireSphere(v, gizmosSize / 2);
            }
        }

    }
}