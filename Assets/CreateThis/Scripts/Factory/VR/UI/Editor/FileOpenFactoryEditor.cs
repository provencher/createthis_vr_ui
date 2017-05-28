﻿using UnityEngine;
using UnityEditor;

namespace CreateThis.Factory.VR.UI {
    [CustomEditor(typeof(FileOpenFactory))]
    [CanEditMultipleObjects]

    public class FileOpenFactoryEditor : Editor {
        SerializedProperty parent;
        SerializedProperty buttonBody;
        SerializedProperty buttonMaterial;
        SerializedProperty panelMaterial;
        SerializedProperty highlight;
        SerializedProperty outline;
        SerializedProperty buttonClickDown;
        SerializedProperty buttonClickUp;
        SerializedProperty fontSize;
        SerializedProperty fontColor;
        SerializedProperty labelZ;
        SerializedProperty buttonZ;
        SerializedProperty bodyScale;
        SerializedProperty labelScale;
        SerializedProperty padding;
        SerializedProperty spacing;
        SerializedProperty buttonPadding;
        SerializedProperty buttonMinWidth;
        SerializedProperty buttonCharacterSize;
        SerializedProperty labelCharacterSize;

        protected void OnEnable() {
            parent = serializedObject.FindProperty("parent");
            buttonBody = serializedObject.FindProperty("buttonBody");
            buttonMaterial = serializedObject.FindProperty("buttonMaterial");
            panelMaterial = serializedObject.FindProperty("panelMaterial");
            highlight = serializedObject.FindProperty("highlight");
            outline = serializedObject.FindProperty("outline");
            buttonClickDown = serializedObject.FindProperty("buttonClickDown");
            buttonClickUp = serializedObject.FindProperty("buttonClickUp");
            fontSize = serializedObject.FindProperty("fontSize");
            fontColor = serializedObject.FindProperty("fontColor");
            labelZ = serializedObject.FindProperty("labelZ");
            buttonZ = serializedObject.FindProperty("buttonZ");
            bodyScale = serializedObject.FindProperty("bodyScale");
            labelScale = serializedObject.FindProperty("labelScale");
            padding = serializedObject.FindProperty("padding");
            spacing = serializedObject.FindProperty("spacing");
            buttonPadding = serializedObject.FindProperty("buttonPadding");
            buttonMinWidth = serializedObject.FindProperty("buttonMinWidth");
            buttonCharacterSize = serializedObject.FindProperty("buttonCharacterSize");
            labelCharacterSize = serializedObject.FindProperty("labelCharacterSize");
        }

        protected void BuildGenerateButton() {
            // Take out this if statement to set the value using setter when ever you change it in the inspector.
            // But then it gets called a couple of times when ever inspector updates
            // By having a button, you can control when the value goes through the setter and getter, your self.
            if (GUILayout.Button("Generate")) {
                if (target.GetType() == typeof(FileOpenFactory)) {
                    FileOpenFactory factory = (FileOpenFactory)target;
                    factory.Generate();
                }
            }
        }

        protected void AdditionalProperties() {
            // put your properties here in the override
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();

            EditorGUILayout.PropertyField(parent);
            EditorGUILayout.PropertyField(buttonBody);
            EditorGUILayout.PropertyField(buttonMaterial);
            EditorGUILayout.PropertyField(panelMaterial);
            EditorGUILayout.PropertyField(highlight);
            EditorGUILayout.PropertyField(outline);
            EditorGUILayout.PropertyField(buttonClickDown);
            EditorGUILayout.PropertyField(buttonClickUp);
            EditorGUILayout.PropertyField(fontSize);
            EditorGUILayout.PropertyField(fontColor);
            EditorGUILayout.PropertyField(labelZ);
            EditorGUILayout.PropertyField(buttonZ);
            EditorGUILayout.PropertyField(bodyScale);
            EditorGUILayout.PropertyField(labelScale);
            EditorGUILayout.PropertyField(padding);
            EditorGUILayout.PropertyField(spacing);
            EditorGUILayout.PropertyField(buttonPadding);
            EditorGUILayout.PropertyField(buttonMinWidth);
            EditorGUILayout.PropertyField(buttonCharacterSize);
            EditorGUILayout.PropertyField(labelCharacterSize);

            AdditionalProperties();

            serializedObject.ApplyModifiedProperties();

            BuildGenerateButton();
        }
    }
}