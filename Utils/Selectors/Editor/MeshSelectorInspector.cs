using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HedraLibrary.Utils.Selectors {

    [CanEditMultipleObjects]
    [CustomEditor(typeof(MeshSelector))]
    public class MeshSelectorInspector : Editor {
        
        MeshSelector control;
        public override void OnInspectorGUI() {
            base.DrawDefaultInspector();
            control = (MeshSelector)target;

            if (Application.isPlaying) {
                return;
            }

            if (Selection.activeGameObject != control.gameObject) {
                return;
            }

            GUILayout.Label($"Current: {control.GetCurrentVariationId()}");

            Input();
            control.StoreComponents();
            control.SetMesh();
        }

        bool oncePerFrame;
        void Input() {
            if (oncePerFrame) {
                oncePerFrame = false;
                return;
            }
            oncePerFrame = true;

            Event e = Event.current;

            if (!e.control || e.delta.y == 0) {
                return;
            }
            int increment = e.delta.y > 0 ? 1 : -1;
            control.ScrollVariation(increment);
        }
    }

}