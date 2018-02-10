/*
 * Hedra's partial class for Unity extensions.
 * 
 * @author  Olsyx (Olatz Castaño)
 * @source  https://github.com/Darkatom/Hedra-Library
 * @since   Unity 2017.1.0p4
 * @last    Unity 2017.2.0f3
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace HedraLibrary {
    public static partial class Hedra {
        
        public static bool IsNaN (this Vector2 vector) {
            return float.IsNaN(vector.x) || float.IsNaN(vector.y);
        }

        public static bool IsNaN(this Vector3 vector) {
            return float.IsNaN(vector.x) || float.IsNaN(vector.y) || float.IsNaN(vector.z);
        }

        public static bool IsInfinity(this Vector2 vector) {
            return float.IsInfinity(vector.x) || float.IsInfinity(vector.y);
        }

        public static bool IsInfinity(this Vector3 vector) {
            return float.IsInfinity(vector.x) || float.IsInfinity(vector.y) || float.IsInfinity(vector.z);
        }

        public static string Join<T>(this T[] array, string separator) {
            string s = "";
            for (int i = 0; i < array.Length - 1; i++) {
                s += array[i].ToString() + separator;
            }
            s += array[array.Length - 1].ToString();
            return s;
        }

        public static string Join<T>(this List<T> list, string separator) {
            string s = "";
            for (int i = 0; i < list.Count - 1; i++) {
                s += list[i].ToString() + separator;
            }
            s += list[list.Count - 1].ToString();
            return s;
        }

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

        public static void DrawWireCube(Vector3 center, Vector3 size, Quaternion rotation) {
            Matrix4x4 cubeTransform = Matrix4x4.TRS(center, rotation, size);
            Matrix4x4 oldGizmosMatrix = Gizmos.matrix;
            Gizmos.matrix *= cubeTransform;
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
            Gizmos.matrix = oldGizmosMatrix;
        }

        public static void DrawUnityEventRelation(Transform self, Transform other, float size) {
            if (self == other || self == other.parent) {
                Gizmos.DrawWireSphere(self.position, size);
            } else {
                Gizmos.DrawLine(self.position, other.transform.position);
            }
        }
    }
}
