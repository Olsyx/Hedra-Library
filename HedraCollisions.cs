/*
 * Hedra's partial class for collision detections. 
 * 
 * @author  Olsyx (Olatz Castaño)
 * @source  https://github.com/Olsyx/Hedra-Library
 * @since   Unity 2017.1.0p4
 * @last    Unity 2018.3.0f2
 */

using System.Collections;
using System.Collections.Generic;
using HedraLibrary.Shapes;
using HedraLibrary.Shapes.Polygons;
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
            return Physics2D.OverlapBoxAll(position, boxSize, 0f, LayerMask.GetMask("Nothing"));
        }

        public static Collider2D[] CheckCollisionAt(Vector2 position, Collider2D collider, LayerMask mask) {
            if (collider == null) {
                return null;
            }

            Vector2 boxSize = new Vector2(collider.bounds.size.x, collider.bounds.size.y);
            return Physics2D.OverlapBoxAll(position, boxSize, 0f, mask);
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