using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HedraLibrary {
    public static class HedraEditor {
        public static void SetButton(string buttonName, Action handler) {
            if (GUILayout.Button(buttonName)) {
                handler();
            }
        }

        public static void SetToggle(string toggleName, ref bool variable, Action handler) {
            variable = EditorGUILayout.Toggle(toggleName, variable);
            if (variable) {
                handler();
            }
        }

		public static void InspectorView(ScriptableObject target, string listProperty) {
			SerializedObject so = new SerializedObject(target);
			SerializedProperty stringsProperty = so.FindProperty(listProperty);

			EditorGUILayout.PropertyField(stringsProperty, true); // True means show children
			so.ApplyModifiedProperties(); // Remember to apply modified properties
		} 
    }
}