using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace HedraLibrary.ContextTools {
	public static partial class HedraContextTools {

		[MenuItem("Hedra/Random/Rotation", false)]
		[MenuItem("GameObject/Hedra/Random/Rotation", false)]
		static void ShowRotationRandomizerWindow(MenuCommand menuCommand) {
			RotationRandomizer.Show();
		}
	}

	public class RotationRandomizer : EditorWindow {
        
        private enum RotationOptions {
            Local,
            Global
        }

        private class Range {
            public float min;
            public float max;

            public void Show() {
                EditorGUILayout.BeginHorizontal();
                min = EditorGUILayout.FloatField("", min);
                max = EditorGUILayout.FloatField("", max);
                EditorGUILayout.EndHorizontal();

                min = Mathf.Min(min, max);
                max = Mathf.Max(min, max);
            }

            public float GetRotation() {
                return Random.Range(min, max);
            }
        }
        
		private List<GameObject> targets;

        public List<Vector3> rotations = new List<Vector3>();

        private bool rangedOption;
        private TargetOptions targetOption;
        private RotationOptions rotationOption;

        private bool X;
        private bool Y;
        private bool Z;

        private Range xRange = new Range();
        private Range yRange = new Range();
        private Range zRange = new Range();
        
        private float percentage;
        		
		public static void Show() {
			EditorWindow window = (EditorWindow)GetWindow(typeof(RotationRandomizer));
			window.titleContent = new GUIContent("Rotation Randomizer");
			window.minSize = new Vector2(150f, 150f);
			window.Show();
		}

		private void OnGUI() {
            targets = HedraContextTools.GetSelectionTargets(targetOption);
            RegisterObjects();

            EditorGUI.BeginChangeCheck();
            ShowMainGUI();
            EditorGUI.EndChangeCheck();
        }
        
        private void ShowMainGUI() {
            GUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            HedraEditor.SetButton("Specific", delegate { rangedOption = false; });
            HedraEditor.SetButton("Ranged", delegate { rangedOption = true; });
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(20);

            targetOption = (TargetOptions)EditorGUILayout.EnumPopup("Affect: ", targetOption);
            rotationOption = (RotationOptions)EditorGUILayout.EnumPopup("Rotate: ", rotationOption);

            GUILayout.Space(10);

            if (rangedOption) {
                ShowRangedGUI();
            } else {
                ShowSpecificGUI();
            }
        }
        
        private void RegisterObjects() {
            for (int i = 0; i < targets.Count; i++) {
                Undo.RecordObject(targets[i].transform, "Rotation Randomizer");
            }
        }

        #region Specific Rotations
        private void ShowSpecificGUI() {
            HedraEditor.InspectorView(this, "rotations");
            GUILayout.Space(20);

            HedraEditor.SetButton("Randomize!", SpecificRandomization);
        }

        private void SpecificRandomization() {
            if (targets == null || targets.Count <= 0) {
                return;
            }

            if (rotations.Count <= 0) {
                return;
            }

            percentage = 1f / (float)rotations.Count * 100f;

            for (int i = 0; i < targets.Count; i++) {
                SpecificRandomization(targets[i].transform);
            }
        }

        private void SpecificRandomization(Transform target) {
            bool change = Random.Range(0, 100f) <= percentage;
            if (!change) {
                return;
            }

            Vector3 rotation = rotations[Random.Range(0, rotations.Count)];
            if (rotationOption == RotationOptions.Global) {
                target.rotation = Quaternion.Euler(rotation);
            } else {
                target.localRotation = Quaternion.Euler(rotation);
            }
        }
        #endregion

        #region Ranged Rotations
        private void ShowRangedGUI() {
            HedraEditor.SetToggle("X", ref X, xRange.Show);
            GUILayout.Space(10);
            HedraEditor.SetToggle("Y", ref Y, yRange.Show);
            GUILayout.Space(10);
            HedraEditor.SetToggle("Z", ref Z, zRange.Show);

            GUILayout.Space(20);
            percentage = EditorGUILayout.FloatField("Percentage", percentage);
            GUILayout.Space(20);


            HedraEditor.SetButton("Randomize!", RangedRandomization);
        }

        private void RangedRandomization() {
            if (targets == null || targets.Count <= 0) {
                return;
            }

            if (!X && !Y && !Z) {
                return;
            }

            for (int i = 0; i < targets.Count; i++) {
                RangedRanomization(targets[i].transform);
            }
        }

		private void RangedRanomization(Transform target) {
            bool change = Random.Range(0, 100f) <= percentage;
            if (!change) {
                return;
            }

            Vector3 rotation = rotationOption == RotationOptions.Global ? target.rotation.eulerAngles
                                                                        : target.localRotation.eulerAngles;

            rotation.x = X ? xRange.GetRotation() : rotation.x;
            rotation.y = Y ? yRange.GetRotation() : rotation.y;
            rotation.z = Z ? zRange.GetRotation() : rotation.z;

            if (rotationOption == RotationOptions.Global) {
                target.rotation = Quaternion.Euler(rotation);
            } else {
                target.localRotation = Quaternion.Euler(rotation);
            }
        }
        #endregion
    }
}