using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace HedraLibrary {
    public static partial class Hedra  {

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

                GameObject listener = raw ?? mono.gameObject ?? net.gameObject;                
                if (target != null && !seenListeners.Contains(listener)) {
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
    }
}
