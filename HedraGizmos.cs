/*
 * Hedra's partial class for Unity extensions.
 * 
 * @author  Olsyx (Olatz Castaño)
 * @source  https://github.com/Olsyx/Hedra-Library
 * @since   Unity 2017.1.0p4
 * @last    Unity 2018.3.0f2
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace HedraLibrary {
    public static partial class Hedra {
        
        public static void DrawGizmos(this UnityEvent unityEvent, Transform self, Color color, float size) {
            ((UnityEventBase)unityEvent).DrawGizmos(self, color, size);
        }

        public static void DrawGizmos(this UnityEvent<object> unityEvent, Transform self, Color color, float size) {
            ((UnityEventBase)unityEvent).DrawGizmos(self, color, size);
        }

        public static void DrawGizmos(this UnityEventBase unityEvent, Transform self, Color color, float size) {
            List<GameObject> seenListeners = new List<GameObject>();

            int count = unityEvent.GetPersistentEventCount();
            Gizmos.color = color;
            for (int i = 0; i < count; i++) {
                UnityEngine.Object target = unityEvent.GetPersistentTarget(i);
                GameObject raw = target as GameObject;
                MonoBehaviour mono = target as MonoBehaviour;
                NetworkBehaviour net = target as NetworkBehaviour;

                GameObject listener = raw ?? (mono != null ? mono.gameObject : null) ?? (net != null ? net.gameObject : null);
                if (listener != null && !seenListeners.Contains(listener)) {
                    DrawUnityEventRelation(self, listener.transform, size);
                    seenListeners.Add(listener);
                }
            }
        }

        public static void DrawUnityEventRelation(Transform self, Transform other, float size) {
            if (self == other || self == other.parent) {
                Gizmos.DrawWireSphere(self.position, size);
            } else {
                Gizmos.DrawLine(self.position, other.transform.position);
            }
        }

        public static void DrawWireCube(Vector3 center, Vector3 size, Quaternion rotation) {
            Matrix4x4 cubeTransform = Matrix4x4.TRS(center, rotation, size);
            Matrix4x4 oldGizmosMatrix = Gizmos.matrix;
            Gizmos.matrix *= cubeTransform;
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
            Gizmos.matrix = oldGizmosMatrix;
        }


        public static void DrawWireCube(Collider collider, Quaternion rotation) {
            if (collider == null) {
                return;
            }

            if (collider is BoxCollider) {
                BoxCollider box = (BoxCollider)collider;
                Hedra.DrawWireCube(box.bounds.center, box.size, rotation);
            } else {
                Hedra.DrawWireCube(collider.bounds.center, collider.bounds.size, rotation);
            }
        }

#if UNITY_EDITOR
        public static void DrawCircle(Color color, Vector3 center, Vector3 up, float radius) {
            Handles.color = color;
            Handles.DrawWireDisc(center, up, radius);
        }
#endif
    }
}
