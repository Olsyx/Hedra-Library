/*
* Hedra's partial class responsible for all other functions
* that do not fit in other categories.
*
* @author  Olsyx (Olatz Castaño)
* @source https://github.com/Darkatom/Hedra-Library
* @since   Unity 2017.1.0p4
* @last    Unity 2017.2.0f3
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace HedraLibrary {
    #region Useful Structs
    [Serializable]
    public struct Range {
        public float min;
        public float max;
        
        public Range(float min, float max) {
            this.min = min;
            this.max = max;
        }

        public float Resolve(float value) {
            return min + (value * (max - min));
        }

        public float Resolve(float value, float otherMax, float otherMin) {
            return min + ((value * (max - min)) / (otherMax - min));
        }

        public bool Contains(float value) {
            return min <= value && value >= max;
        }
    }

    [Serializable]
    public struct AnimationCurve3 {
        public AnimationCurve x;
        public AnimationCurve y;
        public AnimationCurve z;

        public Vector3 Evaluate(float step) {
            return new Vector3(x.Evaluate(step), y.Evaluate(step), z.Evaluate(step));
        }
    }
    #endregion

    public static partial class Hedra {
        
        public static void AssignEvent(GameObject target, EventTriggerType eventType, UnityAction<BaseEventData> handler) {
            EventTrigger eventTrigger = target.AddComponent<EventTrigger>();
            EventTrigger.Entry eventEntry = new EventTrigger.Entry();
            eventEntry.eventID = eventType;
            eventEntry.callback.AddListener(handler);
            eventTrigger.triggers.Add(eventEntry);
        }


        #region Finding points and objects
        /// <summary>
        /// Returns the closest point to the origin point.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Vector2 ClosestPoint(Vector2 origin, List<Vector2> points) {
            Vector2 closestPoint = new Vector2(float.MaxValue, float.MaxValue);
            float closestDistance = float.MaxValue;
            for (int i = 0; i < points.Count; i++) {
                float distance = Vector2.Distance(origin, points[i]);
                if (distance < closestDistance) {
                    closestDistance = distance;
                    closestPoint = points[i];
                }
            }

            return closestPoint;
        }

        /// <summary>
        /// Returns the closest point to the origin point.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Vector3 ClosestPoint(Vector3 origin, List<Vector3> points) {
            Vector3 closestPoint = new Vector3(float.MaxValue, float.MaxValue);
            float closestDistance = float.MaxValue;
            for (int i = 0; i < points.Count; i++) {
                float distance = Vector3.Distance(origin, points[i]);
                if (distance < closestDistance) {
                    closestDistance = distance;
                    closestPoint = points[i];
                }
            }

            return closestPoint;
        }
        
        /// <summary>
        /// Returns the furthest point to the origin point.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Vector2 FurthestPoint(Vector2 origin, List<Vector2> points) {
            Vector2 furthestPoint = new Vector2(float.MinValue, float.MinValue);
            float furthestDistance = float.MinValue;
            for (int i = 0; i < points.Count; i++) {
                float distance = Vector2.Distance(origin, points[i]);
                if (distance > furthestDistance) {
                    furthestDistance = distance;
                    furthestPoint = points[i];
                }
            }

            return furthestPoint;
        }

        /// <summary>
        /// Returns the furthest point to the origin point.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Vector3 FurthestPoint(Vector3 origin, List<Vector3> points) {
            Vector3 furthestPoint = new Vector3(float.MaxValue, float.MaxValue);
            float furthestDistance = float.MaxValue;
            for (int i = 0; i < points.Count; i++) {
                float distance = Vector3.Distance(origin, points[i]);
                if (distance > furthestDistance) {
                    furthestDistance = distance;
                    furthestPoint = points[i];
                }
            }

            return furthestPoint;
        }

        /// <summary>
        /// Returns the closest object to the origin object.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static T ClosestObject<T>(List<T> objects, Func<T, float> checkFunction) {
            T target = default(T);
            float closestDistance = float.MaxValue;
            for (int i = 0; i < objects.Count; i++) {
                float distance = checkFunction(objects[i]);
                if (distance < closestDistance) {
                    closestDistance = distance;
                    target = objects[i];
                }
            }

            return target;
        }

        /// <summary>
        /// Returns the furthest object to the origin object.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static T FurthestObject<T>(List<T> objects, Func<T, float> checkFunction) {
            T target = default(T);
            float furthestDistance = float.MinValue;
            for (int i = 0; i < objects.Count; i++) {
                float distance = checkFunction(objects[i]);
                if (distance > furthestDistance) {
                    furthestDistance = distance;
                    target = objects[i];
                }
            }

            return target;
        }
        #endregion
    }
}