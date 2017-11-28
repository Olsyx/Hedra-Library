/*
 * Hedra's partial class for collision detections. 
 * 
 * @author  Olsyx (Olatz Castaño)
 * @source https://github.com/Darkatom/Hedra-Library
 * @since   Unity 2017.1.0p4
 */

using System.Collections;
using System.Collections.Generic;
using HedraLibrary.Components;
using UnityEngine;

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

        public static Vector2 CorrectPositionAfterCollision(Vector2 position, Collider2D subjectCollider, Collider2D obstacleCollider, float offset = 0f) {
            Box2D subject = new Box2D(subjectCollider);
            subject.Center = position;
            Box2D obstacle = new Box2D((Collider2D)obstacleCollider);            
            Vector2 closestPoint = obstacle.ClosestPointTo(subject);

            Vector2 subjectPoint, distance;
            if (obstacle.Contains(subject.Center)) {
                subjectPoint = subject.FurthestPointFrom(closestPoint);
                distance = (closestPoint - subjectPoint);
            } else {
                subjectPoint = subject.ClosestPointTo(closestPoint);
                distance = -((closestPoint - subjectPoint).magnitude * (closestPoint - subject.Center).normalized);
            }
            
            return position + distance + distance.normalized * offset;
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