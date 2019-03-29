using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderPacker : MonoBehaviour {
    
    void Awake() {
        PackChildrenColliders();
    }

    void PackChildrenColliders() {
        BoxCollider2D[] childrenColliders = GetComponentsInChildren<BoxCollider2D>();
        for (int i = 0; i < childrenColliders.Length; i++) {
            BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
            BoxCollider2D model = childrenColliders[i];
            collider.size = model.size;
            collider.offset = (Vector2)model.transform.localPosition + model.offset;

            Destroy(childrenColliders[i].gameObject);
        }
    }
}
