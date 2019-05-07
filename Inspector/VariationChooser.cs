using System.Collections;
using HedraLibrary;
using UnityEngine;

namespace HedraLibrary.Inspector {
    public class VariationChooser : MonoBehaviour {

        [SerializeField] protected int variation;
    
	    void Start () {
            ChooseVariation();
        }
	
        void ChooseVariation() {
            if (variation < 0) {
                variation = 0;
            }

            int childCount = this.transform.childCount;
            if (variation >= childCount) {
                variation = variation % childCount;
            }
                 
            foreach (Transform child in this.transform) {
                child.gameObject.SetActive(false);
            }
            this.transform.GetChild(variation).gameObject.SetActive(true);
        }

	    void OnDrawGizmos () {
            if (Application.isPlaying) {
                return;
            }
            ChooseVariation();
        }
    }

}