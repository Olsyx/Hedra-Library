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
        
        /// <summary>
        /// Joins an array of items in a single string, separated by a token.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string Join<T>(this T[] array, string separator) {
            string s = "";
            for (int i = 0; i < array.Length - 1; i++) {
                s += array[i].ToString() + separator;
            }
            s += array[array.Length - 1].ToString();
            return s;
        }
        
        public static T[] Copy<T>(T[] original) {
            T[] copy = new T[original.Length];
            for (int i = 0; i < original.Length; i++) {
                copy[i] = original[i];
            }
            return copy;
        }
        
        public static T Last<T>(this T[] objects) {
            return objects.Length > 0 ? objects[objects.Length - 1] : default(T);
        }

        public static T Random<T>(this T[] objects) {
            return objects[UnityEngine.Random.Range(0, objects.Length)];
        }

    }
}
