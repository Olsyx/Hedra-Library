using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace HedraLibrary.ContextTools {

    public enum TargetOptions {
        Selected,
        Children,
        SelectedAndChildren
    }

    public static partial class HedraContextTools {

        public static List<GameObject> GetSelectionTargets(TargetOptions targetOption) {
            List<GameObject> targets = new List<GameObject>();

            if (targetOption != TargetOptions.Children) {
                targets.AddRange(Selection.gameObjects);
            }

            if (targetOption == TargetOptions.Selected) {
                return targets;
            }

            for (int i = 0; i < Selection.gameObjects.Length; i++) {
                targets.AddRange(GetChildrenOf(Selection.gameObjects[i]));
            }

            return targets;
        }

        public static List<GameObject> GetChildrenOf(GameObject parent) {
            List<GameObject> children = new List<GameObject>();
            foreach (Transform child in parent.transform) {
                children.Add(child.gameObject);
            }
            return children;
        }
    }
}