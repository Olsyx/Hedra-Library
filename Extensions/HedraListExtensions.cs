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
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace HedraLibrary {
    public static partial class Hedra {

        /// <summary>
        /// Joins a list of items in a single string, separated by a token.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string Join<T>(this List<T> list, string separator) {
            string s = "";
            for (int i = 0; i < list.Count - 1; i++) {
                s += list[i].ToString() + separator;
            }
            s += list[list.Count - 1].ToString();
            return s;
        }

        public static List<T> Copy<T>(List<T> original) {
            List<T> copy = new List<T>();
            for (int i = 0; i < original.Count; i++) {
                copy.Add(original[i]);
            }
            return copy;
        }

        /// <summary>
        /// Gets the minimum of a list of objects. The minimum function is given by the user. Returns true if A < B.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="original"></param>
        /// <param name="checkFunction"></param>
        /// <returns></returns>
        public static T GetMinimum<T>(this List<T> original, Func<T, T, bool> checkFunction) {
            if (original == null || original.Count == 0) {
                return default(T);
            }

            T min = original[0];

            for (int i = 1; i < original.Count; i++) {
                bool comparison = checkFunction(original[i], min);
                if (comparison) { 
                    min = original[i];
                }
            }

            return min;
        }
            
        public static T Last<T>(this List<T> objects) {
            return objects.Count > 0 ? objects[objects.Count - 1] : default(T);
        }

        public static T Random<T>(this List<T> objects) {
            return objects[UnityEngine.Random.Range(0, objects.Count)];
        }

    }
}
