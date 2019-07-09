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

        public static bool Any<T>(this T[] objects, Func<T, bool> checkFunction) {
            int i = 0;
            while (i < objects.Length && !checkFunction(objects[i])) {
                i++;
            }
            return i < objects.Length;
        }

        public static bool All<T>(this T[] objects, Func<T, bool> checkFunction) {
            int i = 0;
            while (i < objects.Length && checkFunction(objects[i])) {
                i++;
            }
            return i >= objects.Length;
        }

        /// <summary>
        /// Returns de index of the item that complies with the check function.
        /// Returns -1 if none complies.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objects"></param>
        /// <param name="checkFunction"></param>
        /// <returns></returns>
        public static int Which<T>(this T[] objects, Func<T, bool> checkFunction) {
            int i = 0;
            while (i < objects.Length && !checkFunction(objects[i])) {
                i++;
            }
            return i < objects.Length ? i : -1;
        }

        public static bool Any<T>(this List<T> objects, Func<T, bool> checkFunction) {
            int i = 0;
            while (i < objects.Count && !checkFunction(objects[i])) {
                i++;
            }
            return i < objects.Count;
        }

        public static bool All<T>(this List<T> objects, Func<T, bool> checkFunction) {
            int i = 0;
            while (i < objects.Count && checkFunction(objects[i])) {
                i++;
            }
            return i >= objects.Count;
        }

        /// <summary>
        /// Returns de index of the item that complies with the check function.
        /// Returns -1 if none complies.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objects"></param>
        /// <param name="checkFunction"></param>
        /// <returns></returns>
        public static int Which<T>(this List<T> objects, Func<T, bool> checkFunction) {
            int i = 0;
            while (i < objects.Count && !checkFunction(objects[i])) {
                i++;
            }
            return i < objects.Count ? i : -1;
        }
    }
}
