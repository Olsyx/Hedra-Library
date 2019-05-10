using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HedraLibrary.Utils.Selectors {

    [CanEditMultipleObjects]
    [CustomEditor(typeof(GraphicsSelector))]
    public class GraphicsSelectorInspector : Editor {

        GraphicsSelector control;
        public override void OnInspectorGUI() {
            base.DrawDefaultInspector();
            control = (GraphicsSelector)target;

            if (Application.isPlaying) {
                return;
            }

            if (Selection.activeGameObject != control.gameObject) {
                return;
            }

            GUILayout.Label($"Current: {control.GetCurrentVariationId()}");

            Input();
            control.EmptyChildren();
            control.SetGraphics();
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