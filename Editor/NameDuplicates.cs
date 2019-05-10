using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace HedraLibrary.ContextTools {
	public static partial class HedraContextTools {

		[MenuItem("Hedra/Game Objects/Name Duplicates", false)]
		[MenuItem("GameObject/Hedra/Game Objects/Name Duplicates", false)]
		static void ShowNameDuplicatesWindow(MenuCommand menuCommand) {
            NameDuplicates.Show();
		}
	}

	public class NameDuplicates : EditorWindow {

        public string nameRoot = "";
        public string separator = "_";

        private List<Transform> targets;
		
		public static void Show() {
			EditorWindow window = (EditorWindow)GetWindow(typeof(NameDuplicates));
			window.titleContent = new GUIContent("Name Duplicates");
			window.minSize = new Vector2(150f, 150f);
			window.Show();
		}

		private void OnGUI() {
            StoreSelection();
            RegisterObjects();

            EditorGUI.BeginChangeCheck();
            ShowMainGUI();
            EditorGUI.EndChangeCheck();
        }

        private void StoreSelection() {
            if (Selection.gameObjects.Length <= 0) {
                return;
            }

            targets = new List<Transform>();
            foreach(Transform child in Selection.gameObjects[0].transform) {
                targets.Add(child);
            }
        }

        private void RegisterObjects() {
            for (int i = 0; i < targets.Count; i++) {
                Undo.RecordObject(targets[i], "Name Duplicates");
            }
        }

        private void ShowMainGUI() {
            GUILayout.Space(10);
            GUILayout.Label("Root");
            nameRoot = GUILayout.TextField(nameRoot);
            GUILayout.Label("Separator");
            separator = GUILayout.TextField(separator);
            GUILayout.Space(20);
            HedraEditor.SetButton("Apply", GiveName);
        }

		private void GiveName() {
            if (targets.Count <= 0) {
                return;
            }
                        
			for (int i = 0; i < targets.Count; i++) {
                targets[i].name = nameRoot + separator + (i + 1);
			}
		}
	}
}