﻿using UnityEngine;
using UnityEditor;

namespace CreateThis.Scaffold.VR.UI.Button {
    [CustomEditor(typeof(ScaffoldMomentaryButton))]
    [CanEditMultipleObjects]

    public class ScaffoldMomentaryButtonEditor : Editor {
        SerializedProperty thisTarget;
        SerializedProperty buttonText;
        SerializedProperty buttonBody;
        SerializedProperty material;
        SerializedProperty highlight;
        SerializedProperty outline;
        SerializedProperty buttonClickDown;
        SerializedProperty buttonClickUp;
        SerializedProperty alignment;
        SerializedProperty fontSize;
        SerializedProperty fontColor;

        void OnEnable() {
            thisTarget = serializedObject.FindProperty("target");
            buttonText = serializedObject.FindProperty("buttonText");
            buttonBody = serializedObject.FindProperty("buttonBody");
            material = serializedObject.FindProperty("material");
            highlight = serializedObject.FindProperty("highlight");
            outline = serializedObject.FindProperty("outline");
            buttonClickDown = serializedObject.FindProperty("buttonClickDown");
            buttonClickUp = serializedObject.FindProperty("buttonClickUp");
            alignment = serializedObject.FindProperty("alignment");
            fontSize = serializedObject.FindProperty("fontSize");
            fontColor = serializedObject.FindProperty("fontColor");
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();

            EditorGUILayout.PropertyField(thisTarget);
            EditorGUILayout.PropertyField(buttonText);
            EditorGUILayout.PropertyField(buttonBody);
            EditorGUILayout.PropertyField(material);
            EditorGUILayout.PropertyField(highlight);
            EditorGUILayout.PropertyField(outline);
            EditorGUILayout.PropertyField(buttonClickDown);
            EditorGUILayout.PropertyField(buttonClickUp);
            EditorGUILayout.PropertyField(alignment);
            EditorGUILayout.PropertyField(fontSize);
            EditorGUILayout.PropertyField(fontColor);

            serializedObject.ApplyModifiedProperties();

            // Take out this if statement to set the value using setter when ever you change it in the inspector.
            // But then it gets called a couple of times when ever inspector updates
            // By having a button, you can control when the value goes through the setter and getter, your self.
            if (GUILayout.Button("Generate")) {
                if (target.GetType() == typeof(ScaffoldMomentaryButton)) {
                    ScaffoldMomentaryButton scaffoldMomentaryButton = (ScaffoldMomentaryButton)target;
                    scaffoldMomentaryButton.Generate();
                }
            }
        }
    }
}