/*
* Hedra's partial class responsible for all other functions
* that do not fit in other categories.
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

namespace HedraLibrary {
    public static partial class Hedra {
        /// <summary>
        /// Returns the closest point to the origin point.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Vector2 GetClosestPoint(Vector2 origin, List<Vector2> points) {
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
        public static Vector3 GetClosestPoint(Vector3 origin, List<Vector3> points) {
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
        public static Vector2 GetFurthestPoint(Vector2 origin, List<Vector2> points) {
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
        public static Vector3 GetFurthestPoint(Vector3 origin, List<Vector3> points) {
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
    }
}