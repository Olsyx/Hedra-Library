using HedraLibrary.Collections;
using UnityEngine;

namespace HedraLibrary.Utils.Selectors {

    public class GraphicsSelector : MonoBehaviour {

        [SerializeField] protected PrefabCollection collection;
        [SerializeField] protected int variation;

        protected Transform graphics;
        protected int selected;

        #region Init
        void Awake() {
            SetGraphics();
            graphics.parent = this.transform.parent;
            Destroy(this.gameObject);
        }
        #endregion

        #region Control
        public void EmptyChildren() {
            foreach (Transform child in this.transform) {
                GameObject.DestroyImmediate(child.gameObject);
            }
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
        public void ScrollVariation(int increment) {
            variation += increment;
        }

        public void SetGraphics() {
            if (collection == null || collection.items.Count <= 0) {
                return;
            }

            ParseVariation();
            SelectGraphics();
        }

        void SelectGraphics() {
            PrefabCollectionItem item = collection.items[variation];
            if (item.prefab == null) {
                return;
            }

            EmptyChildren();

            GameObject target = Instantiate(item.prefab, this.transform);
            target.name = item.prefab.name;
            graphics = target.transform;
            graphics.localPosition = item.localPosition;
            graphics.localRotation = Quaternion.Euler(item.rotation);
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