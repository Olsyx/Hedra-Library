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
    }
}
