/*
 * Hedra's partial class for collision detections. 
 * 
 * @author  Olsyx (Olatz Castaño)
 * @source https://github.com/Darkatom/Hedra-Library
 * @since   Unity 2017.1.0p4
 * @last    Unity 2017.2.0f3
 */

using System.Collections;
using System.Collections.Generic;
using HedraLibrary.Components;
using UnityEngine;
using System.Linq;

namespace HedraLibrary {
    public static partial class Hedra {
        public static Collider2D[] CheckCollisionAt(Vector2 position, float radius) {
            return CheckCollisionAt(position, radius, LayerMask.GetMask("Nothing"));
        }

        public static Collider2D[] CheckCollisionAt(Vector2 position, float radius, LayerMask mask) {
            return Physics2D.OverlapCircleAll(position, radius, mask);
        }

        public static Collider2D[] CheckCollisionAt(Vector2 position, Collider2D collider) {
            if (collider == null) {
                return null;
            }

            Vector2 boxSize = new Vector2(collider.bounds.size.x, collider.bounds.size.y);
            return CheckCollisionAt(position, boxSize);
        }

        public static Collider2D[] CheckCollisionAt(Vector2 position, Vector2 boxSize) {
            return CheckCollisionAt(position, boxSize, LayerMask.GetMask("Nothing"));
        }

        public static Collider2D[] CheckCollisionAt(Vector2 position, Collider2D collider, LayerMask mask) {
            if (collider == null) {
                return null;
            }

            Vector2 boxSize = new Vector2(collider.bounds.size.x, collider.bounds.size.y);
            return CheckCollisionAt(position, boxSize, mask);
        }

        public static Collider2D[] CheckCollisionAt(Vector2 position, Vector2 boxSize, LayerMask mask) {
            return Physics2D.OverlapBoxAll(position, boxSize, 0f, mask);
        }

        /// <summary>
        /// Calculates the movement of a body and corrects to a new position if a collision occurs.
        /// Only takes the first collision found into account. Doesn't need a Rigidbody.
        /// </summary>
        /// <param name="currentPosition">The current position of the object.</param>
        /// <param name="direction">The normalized direction of the movement.</param>
        /// <param name="distance">The distance to be traveled.</param>
        /// <param name="collider">The collider of the object.</param>
        /// <param name="collisionOffset">Correcting offset after collision, should there be one.</param>
        /// <param name="obstacleMask">The objects in this layer(s) will count as collisions.</param>
        /// <returns>The position of an object after executing a movement, taking into account possible 2D collisions.</returns>
        public static Vector2 MovementWithCollision(Vector2 currentPosition, Vector2 direction, float distance, Collider2D collider, float collisionOffset, LayerMask obstacleMask) {
            Vector2 newPosition = currentPosition + collider.offset + direction.normalized * distance;

            List<Collider2D> obstacles = Hedra.CheckCollisionAt(newPosition, collider, obstacleMask).ToList();
            obstacles.Remove(collider);

            if (obstacles.Count > 0) {
                newPosition = Hedra.CorrectPositionAfterCollision(newPosition, collider, obstacles[0], collisionOffset);
            }

            return newPosition - collider.offset;
        }
        
        /// <summary>
        /// Corrects the position of an object after colliding with an obstacle.
        /// </summary>
        /// <param name="position">Current position of the subject.</param>
        /// <param name="subjectCollider">Collider of the subject.</param>
        /// <param name="obstacleCollider">Collider of the obstacle.</param>
        /// <param name="collisionOffset">Correcting offset after collision.</param>
        /// <returns>The corrected position of a 2D collision.</returns>
        public static Vector2 CorrectPositionAfterCollision(Vector2 position, Collider2D subjectCollider, Collider2D obstacleCollider, float collisionOffset = 0f) {
            Box2D subject = new Box2D(subjectCollider);
            subject.Center = position;
            Box2D obstacle = new Box2D(obstacleCollider);

            Vector2 corner = subject.DeepestCornerIn(obstacle);
            Vector2 closestPoint;

            if (corner.IsNaN() || corner.IsInfinity()) {
                closestPoint = obstacle.DeepestCornerIn(subject);
                corner = subject.ClosestPointTo(closestPoint);
            } else {
                Segment2D closestEdge = obstacle.ClosestEdgeTo(subject.Center);
                closestPoint = closestEdge.PerpendicularPoint(corner);
            }

            Vector2 direction = (closestPoint - corner);
            return position + direction + direction.normalized * collisionOffset;
        }

        static void PrintColliders(Collider2D[] colliders) {
            string s = "Colliders: ";
            foreach (Collider2D target in colliders) {
                s += " " + target.name;
            }

            Debug.Log(s);
        }
    }
}