using System;
using System.Collections.Generic;
using UnityEngine;

namespace HedraLibrary.Collections {
    [Serializable]
    public struct PrefabCollectionItem {
        public string id;
        public GameObject prefab;
        public Vector3 localPosition;
        public Vector3 rotation;
    }

    [CreateAssetMenu(fileName = "PrefabCollection", menuName = "Hedra/Collections/Prefab Collection", order = 1)]
    public class PrefabCollection : ScriptableObject {
        public List<PrefabCollectionItem> items = new List<PrefabCollectionItem>();    
    }

}