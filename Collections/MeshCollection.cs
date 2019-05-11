using System;
using System.Collections.Generic;
using UnityEngine;

namespace HedraLibrary.Collections {
    [Serializable]
    public struct MeshCollectionItem {
        public string id;
        public Mesh mesh;
        public Material[] materials;
        public Vector3 localPosition;
        public Vector3 rotation;
        public Vector3 scale;
    }

    [CreateAssetMenu(fileName = "MeshCollection", menuName = "Hedra/Collections/Mesh Collection", order = 1)]
    public class MeshCollection : ScriptableObject {
        public List<MeshCollectionItem> items = new List<MeshCollectionItem>();    
    }

}