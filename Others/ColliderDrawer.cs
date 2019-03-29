using System.Collections;
using HedraLibrary;
using UnityEngine;

public class ColliderDrawer : MonoBehaviour {

    public enum DrawOptions {
        Own,
        Children,
        All
    }

    [SerializeField] DrawOptions draw;
    [SerializeField] Color color = Color.blue;
    private Collider2D[] colliders;
    
	void Start () {
        StoreColliders();
    }
	
    void StoreColliders() {
        switch (draw) {
            case DrawOptions.Own:
                colliders = GetComponents<Collider2D>();
                break;
            case DrawOptions.Children:
                colliders = GetComponentsInChildren<Collider2D>();
                break;
            case DrawOptions.All:
                colliders = FindObjectsOfType<Collider2D>();
                break;
        }
    }

	void OnDrawGizmos () {
        if (!Application.isPlaying) {
            StoreColliders();
        }
         
        Gizmos.color = color;
        for (int i = 0; i < colliders.Length; i++) {
            Collider2D collider = colliders[i];
            if (collider != null) {
                if (collider is BoxCollider2D) {
                    Hedra.DrawWireCube(collider.bounds.center, ((BoxCollider2D)collider).size, collider.transform.rotation);
                } else {
                    Hedra.DrawWireCube(collider.bounds.center, collider.bounds.size, collider.transform.rotation);
                }
            }
        }	
	}
}
