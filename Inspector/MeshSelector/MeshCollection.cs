using System;
using System.Collections.Generic;
using UnityEngine;

namespace HedraLibrary.Inspector {
    [Serializable]
    public struct MeshCollectionItem {
        public string id;
        public Mesh mesh;
        public Material[] materials;
        public Vector3 localPosition;
        public Vector3 rotation;
    }

    [CreateAssetMenu(fileName = "MeshCollection", menuName = "Hedra/Inspector/MeshCollection", order = 1)]
    public class MeshCollection : ScriptableObject {
        public List<MeshCollectionItem> items = new List<MeshCollectionItem>();    
    }

}