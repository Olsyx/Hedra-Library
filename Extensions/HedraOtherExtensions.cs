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
        public static bool Contains(this LayerMask mask, int layer) {
            return mask == (mask | (1 << layer));
        }
    }
}
