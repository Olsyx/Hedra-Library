using System.Collections;
using HedraLibrary;
using UnityEditor;
using UnityEngine;

namespace HedraLibrary.Inspector {

    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class MeshSelector : MonoBehaviour {

        [SerializeField] protected MeshCollection collection;
        [SerializeField] protected int variation;

        protected MeshFilter filter;
        protected MeshRenderer renderer;
        protected int selected;

        void Awake() {
            StoreComponents();
        }

        void Start() {
            SetMesh();
        }

        public void StoreComponents() {
            filter = filter ?? GetComponent<MeshFilter>();
            renderer = renderer ?? GetComponent<MeshRenderer>();
        }
        
        public void ScrollVariation(int increment) {
            variation += increment;
        }

        public void SetMesh() {
            if (collection == null || collection.items.Count <= 0) {
                return;
            }

            ParseVariation();
            SelectMesh();
        }

        void ParseVariation() {
            if (variation < 0) {
                variation = collection.items.Count - 1;
                Debug.Log(variation);
            }

            int childCount = collection.items.Count;
            if (variation >= childCount) {
                variation = variation % collection.items.Count;
                Debug.Log(variation);
            }
        }

        void SelectMesh() {
            MeshCollectionItem item = collection.items[variation];

            filter.mesh = item.mesh;
            if (item.materials.Length > 0) {
                renderer.materials = item.materials;
            }
            transform.localPosition = item.localPosition;
            transform.localRotation = Quaternion.Euler(item.rotation);
        }
    }
}