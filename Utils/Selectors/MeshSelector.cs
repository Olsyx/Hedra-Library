using HedraLibrary.Collections;
using UnityEngine;

namespace HedraLibrary.Utils.Selectors {

    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class MeshSelector : MonoBehaviour {

        [SerializeField] protected MeshCollection collection;
        [SerializeField] protected int variation;

        protected MeshFilter filter;
        protected MeshRenderer renderer;
        protected BoxCollider collider;
        protected int selected;

        #region Init
        void Awake() {
            StoreComponents();
        }

        void Start() {
            SetMesh();
            Destroy(this);
        }

        public void StoreComponents() {
            filter = filter ?? GetComponent<MeshFilter>();
            renderer = renderer ?? GetComponent<MeshRenderer>();
        }
        #endregion

        #region Control
        public void ScrollVariation(int increment) {
            variation += increment;
        }

        void ParseVariation() {
            if (variation < 0) {
                variation = collection.items.Count - 1;
            }

            int childCount = collection.items.Count;
            if (variation >= childCount) {
                variation = variation % collection.items.Count;
            }
        }
        #endregion

        #region Actions
        public void SetMesh() {
            if (collection == null || collection.items.Count <= 0) {
                return;
            }

            ParseVariation();
            SelectMesh();
        }

        void SelectMesh() {

            MeshCollectionItem item = collection.items[variation];

            filter.mesh = item.mesh;
            if (item.materials.Length > 0) {
                renderer.materials = item.materials;
            }

            transform.localPosition = item.localPosition;
            transform.localRotation = Quaternion.Euler(item.rotation);
            AdaptCollider();
        }

        void AdaptCollider() {
            collider = collider ?? GetComponent<BoxCollider>();
            if (!collider) {
                return;
            }

            collider.center = filter.sharedMesh.bounds.center;
            collider.size = filter.sharedMesh.bounds.size;
        }
        #endregion

        #region Queries
        public string GetCurrentVariationId() {
            if (collection.items.Count <= 0) {
                return "";
            }
            return collection.items[variation].id;
        }
        #endregion
    }
}