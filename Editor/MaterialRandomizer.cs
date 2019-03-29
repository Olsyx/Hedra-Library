using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace HedraLibrary.ContextTools {
	public static partial class HedraContextTools {

		[MenuItem("Hedra/Random/Material", false)]
		[MenuItem("GameObject/Hedra/Random/Material", false)]
		static void ShowMaterialRandomizerWindow(MenuCommand menuCommand) {
			MaterialRandomizer.Show();
		}
	}

	public class MaterialRandomizer : EditorWindow {

        private List<MeshRenderer> targets;
        private TargetOptions targetOption;
		public List<Material> materials = new List<Material>();
		
		public static void Show() {
			EditorWindow window = (EditorWindow)GetWindow(typeof(MaterialRandomizer));
			window.titleContent = new GUIContent("Material Randomizer");
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
            List<GameObject> selection = HedraContextTools.GetSelectionTargets(targetOption);
            targets = new List<MeshRenderer>();
            for (int i = 0; i < selection.Count; i++) {
                MeshRenderer renderer = selection[i].GetComponent<MeshRenderer>();
                if (renderer != null) {
                    targets.Add(renderer);
                }
            }
        }

        private void RegisterObjects() {
            for (int i = 0; i < targets.Count; i++) {
                Undo.RecordObject(targets[i], "Material Randomizer");
            }
        }

        private void ShowMainGUI() {
            GUILayout.Space(10);
            targetOption = (TargetOptions)EditorGUILayout.EnumPopup("Affect: ", targetOption);
            GUILayout.Space(20);
            HedraEditor.InspectorView(this, "materials");
            GUILayout.Space(20);
            HedraEditor.SetButton("Randomize!", RandomizeMaterial);
        }

		private void RandomizeMaterial() {
            if (materials.Count <= 0) {
                return;
            }

            float percentage = 1 / (float)materials.Count * 100f;
            
			for (int i = 0; i < targets.Count; i++) {
				RandomMaterialChange(targets[i], percentage);
			}
		}

		private void RandomMaterialChange(MeshRenderer target, float percentage) {
			if (target == null) {
				return;
			}

			bool change = Random.Range(0, 100f) >= percentage;
			if (!change) {
				return;
			}

			Material selectedMaterial = materials[Random.Range(0, materials.Count)];
			target.sharedMaterial = selectedMaterial;
		}
	}
}