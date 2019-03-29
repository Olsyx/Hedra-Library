using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HedraLibrary.Shapes.Polygons {
    public class RegularPolygon : Polygon {

        private const float EPSILON = 0.005f;

        public float Angle { get; set; }
        public float Radius { get; set; }
        private int vertexCount;

        public RegularPolygon(RegularPolygon other) {
            this.Radius = other.Radius;
            this.center = other.Center;
            this.rotation = other.Rotation;
            this.Collider = other.Collider;
            vertexCount = other.Vertices.Length;

            Init();

            Angle = Hedra.Angle(Edges[0], Edges[1]);
        }

        public RegularPolygon(Vector2 position, int vertices, float radius, float rotation) {
            this.Radius = radius;
            center = position;
            this.rotation = rotation;
            vertexCount = vertices;

            Init();

            Angle = Hedra.Angle(Edges[0], Edges[1]);
        }

        public RegularPolygon(CircleCollider2D collider, int vertices) {
            Transform owner = collider.gameObject.transform;

            Collider = collider;
            Radius = collider.radius;
            rotation = owner.rotation.eulerAngles.z;
            center = collider.bounds.center;
            vertexCount = vertices;

            Init();

            Angle = Hedra.Angle(Edges[0], Edges[1]);
        }

        protected override void CreateVertices() {
            Vertices = Hedra.Circle(Center, Radius, vertexCount).ToArray();
            for (int i = 0; i < Vertices.Length; i++) {
                Vertices[i] = Hedra.Rotate(Center, Vertices[i], rotation);
            }
            SortVertices();
        }

        protected override void CalculateArea() {
            Area = -1;// TO-DO
        }

        #region Collisions
        protected const float EdgeMargin = 0.005f;

        public override Vector2 CalculateCollisionOffset(Polygon pastSelf, Polygon obstacle) {
            if (!this.Intersects(obstacle)) {
                return Vector2.zero;
            }

            List<Vector2> p_vertices = this.VerticesInside(obstacle);
            List<Vector2> o_vertices = obstacle.VerticesInside(this);

            Vector2 movement = (this.Center - pastSelf.Center).normalized;

            while (p_vertices.Count > 1 || o_vertices.Count > 1) {
                this.MoveTo(this.Center - movement * 0.01f);
                p_vertices = this.VerticesInside(obstacle);
                o_vertices = obstacle.VerticesInside(this);
            }

            if (p_vertices.Count == 1 && o_vertices.Count <= 0) {
                Debug.Log("A");
                return SimpleCollisionOffset(this, p_vertices[0], obstacle);

            } else if (p_vertices.Count <= 0 && o_vertices.Count == 1) {
                Debug.Log("B");
                return -SimpleCollisionOffset(obstacle, o_vertices[0], this);

            } else if (p_vertices.Count == o_vertices.Count) {
                Debug.Log("Z");
                return ComplexCollisionOffset(pastSelf, obstacle, p_vertices[0], o_vertices[0]);
            }


            return Vector2.zero;
        }

        public virtual void ReduceCollision(Polygon obstacle, Vector2 movement) {
            List<Vector2> p_vertices = this.VerticesInside(obstacle);
            List<Vector2> o_vertices = obstacle.VerticesInside(this);

            while (p_vertices.Count > 1 || o_vertices.Count > 1) {
                this.MoveTo(this.Center - movement * 0.01f);
                p_vertices = this.VerticesInside(obstacle);
                o_vertices = obstacle.VerticesInside(this);
            }
        }

        protected virtual Vector2 SimpleCollisionOffset(Polygon agent, Vector2 agentVertex, Polygon pasive) {
            Segment2D edge = pasive.IntersectingEdges(new Segment2D(agentVertex, agent.Center))[0];
            Vector2 PPoint = edge.PerpendicularPoint(agentVertex);
            return PPoint - agentVertex;
        }

        protected virtual Vector2 ComplexCollisionOffset(Polygon pastSelf, Polygon obstacle, Vector2 p_vertex, Vector2 o_vertex) {
            if (this.ContainsInEdge(o_vertex)) {
                return obstacle.ClosestPerpendicularPointTo(p_vertex) - p_vertex;
            }

            if (obstacle.ContainsInEdge(p_vertex)) {
                return o_vertex - this.ClosestPerpendicularPointTo(o_vertex);
            }

            Vector2 vertexPathOffset = PathCollisionOffset(pastSelf, obstacle, p_vertex);
            if (vertexPathOffset.magnitude > 0 && CheckSolution(obstacle, vertexPathOffset)) {
                Debug.Log("A");
                return vertexPathOffset;
            }

            Vector2 obstaclePathOffset = ObstacleCollisionOffset(pastSelf, p_vertex, o_vertex);
            if (obstaclePathOffset.magnitude > 0 && CheckSolution(obstacle, obstaclePathOffset)) {
                Debug.Log("B");
                return obstaclePathOffset;
            }

            Vector2 obstacleSolution = ObstaclePerpendicularPointsOffsets(obstacle, p_vertex, o_vertex);
            if (obstacleSolution.magnitude > 0) {
                Debug.Log("C");
                return obstacleSolution;
            }

            Debug.Log("D");
            Segment2D lastResort = new Segment2D(p_vertex, Center);
            Segment2D intersectingEdge = obstacle.IntersectingEdges(lastResort)[0];
            return intersectingEdge.PerpendicularPoint(p_vertex) - p_vertex;
        }

        protected virtual Vector2 PathCollisionOffset(Polygon pastSelf, Polygon obstacle, Vector2 p_vertex) {
            Segment2D p_vertexPath = new Segment2D(pastSelf.Vertices[GetVertexIndex(p_vertex)], p_vertex);
            List<Segment2D> o_edges = obstacle.IntersectingEdges(p_vertexPath);

            if (o_edges == null || o_edges.Count <= 0) {
                throw new ImpossibleCollisionException();
            }

            Segment2D o_edge = o_edges[0];
            Vector2 p_PPoint = o_edge.ToLine().PerpendicularPoint(p_vertex);

            if (o_edge.Contains(p_PPoint)) {
                Vector2 offset = p_PPoint - p_vertex;
                return offset;
            }

            return Vector2.zero;
        }

        protected virtual Vector2 ObstacleCollisionOffset(Polygon pastSelf, Vector2 p_vertex, Vector2 o_vertex) {
            Segment2D p_vertexPath = new Segment2D(pastSelf.Vertices[GetVertexIndex(p_vertex)], p_vertex);
            Line2D o_vertexPath = new Line2D(o_vertex, o_vertex + p_vertexPath.Vector);
            if (o_vertexPath.Contains(o_vertex) && o_vertexPath.Contains(p_vertex)) {
                return o_vertex - p_vertex;
            }
            return Vector2.zero;
        }

        protected virtual Vector2 ObstaclePerpendicularPointsOffsets(Polygon obstacle, Vector2 p_vertex, Vector2 o_vertex) {
            List<Vector2> solutions = new List<Vector2>();

            List<Segment2D> p_edges = this.EdgesInside(obstacle, true);
            for (int i = 0; i < p_edges.Count; i++) {
                Vector2 pPoint = p_edges[i].ToLine().PerpendicularPoint(o_vertex);
                if (p_edges[i].Contains(pPoint)) {
                    solutions.Add(o_vertex - pPoint);
                }
            }

            solutions = RemoveVertexDuplicates(solutions);
            solutions = solutions.Where(p => CheckSolution(obstacle, p)).ToList();
            return solutions.GetMinimum(delegate (Vector2 a, Vector2 b) { return a.magnitude < b.magnitude; });
        }

        protected bool CheckSolution(Polygon obstacle, Vector2 offset) {
            Polygon fakeSolution = PolygonManager.Create2D(this);
            fakeSolution.Translate(offset);

            List<Vector2> p_remaining = fakeSolution.VerticesInside(obstacle);
            List<Vector2> o_remaining = obstacle.VerticesInside(fakeSolution);

            if (p_remaining.Count > 1 || o_remaining.Count > 1) {
                return false;
            }

            if (p_remaining.Count > 0 && !obstacle.ContainsInEdge(p_remaining[0]) || (o_remaining.Count > 0 && !fakeSolution.ContainsInEdge(o_remaining[0]))) {
                return false;
            }

            List<Vector2> intersections = obstacle.IntersectionPoints(fakeSolution, 3);
            Debug.Log("Remaining.Count = " + p_remaining.Count + " | Obstacle.IntersectionPoints " + intersections.Count);

            if (intersections.Count > 2) {
                return false;
            }

            if (p_remaining.Count == 0) {
                return intersections.Count < 2 || EqualVertices(intersections[0], intersections[1], 2) || Vector2.Distance(intersections[0], intersections[1]) <= EPSILON;
            }

            if (intersections.Count == 2) {
                if (EqualVertices(intersections[0], intersections[1], 3)) {
                    return true;
                }

                Vector2 vertex = p_remaining[0];
                return Vector2.Distance(vertex, intersections[0]) <= EPSILON && Vector2.Distance(vertex, intersections[1]) <= EPSILON;
            }

            if (p_remaining.Count == 1) {
                float remainingDistance = Vector2.Distance(p_remaining[0], obstacle.ClosestPerpendicularPointTo(p_remaining[0]));
                return remainingDistance <= EPSILON || obstacle.ContainsInEdge(p_remaining[0]);
            }

            return true;
        }

        public override Collider2D[] CheckCollisions(LayerMask mask) {
            return Physics2D.OverlapCircleAll(Center, Radius, mask);
        }

        public override Collider2D[] CheckCollisionsAt(Vector2 position, LayerMask mask) {
            return Physics2D.OverlapCircleAll(position, Radius, mask);
        }
        #endregion
    }
}