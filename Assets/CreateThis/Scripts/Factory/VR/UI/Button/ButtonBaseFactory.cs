﻿using UnityEngine;
using CreateThis.VR.UI.Button;
using CreateThis.VR.UI.Interact;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CreateThis.Factory.VR.UI.Button {
    public abstract class ButtonBaseFactory : BaseFactory {
        public GameObject parent;
        public string buttonText;
        public GameObject buttonBody;
        public Material material;
        public Material highlight;
        public Material outline;
        public AudioClip buttonClickDown;
        public AudioClip buttonClickUp;
        public TextAlignment alignment;
        public int fontSize;
        public Color fontColor;
        public float labelZ;
        public Vector3 bodyScale;
        public Vector3 labelScale;

        protected GameObject buttonInstance;
        protected GameObject buttonBodyInstance;
        protected GameObject buttonTextLabelInstance;

        private AudioSource AddAudioSource(GameObject target, AudioClip audioClip) {
            if (!audioClip) return null;
            AudioSource audioSource = SafeAddComponent<AudioSource>(target);
            audioSource.clip = buttonClickDown;
            audioSource.spatialBlend = 1f;
            return audioSource;
        }

        protected virtual void AddButton(GameObject target, AudioSource audioSourceDown, AudioSource audioSourceUp) {
            MomentaryButton button = SafeAddComponent<MomentaryButton>(target);
            if (audioSourceDown) button.buttonClickDown = audioSourceDown;
            if (audioSourceUp) button.buttonClickUp = audioSourceUp;
            button.buttonBody = buttonBodyInstance;
            button.buttonText = buttonTextLabelInstance;
        }

        private void PopulateButton() {
            if (!buttonBodyInstance) return;

#if UNITY_EDITOR
            Undo.RegisterCompleteObjectUndo(buttonInstance, "Change name before");
#endif

            buttonInstance.name = "Button_" + buttonText;

            AudioSource audioSourceDown = AddAudioSource(buttonInstance, buttonClickDown);
            AudioSource audioSourceUp = AddAudioSource(buttonInstance, buttonClickUp);

            BoxCollider boxCollider = SafeAddComponent<BoxCollider>(buttonInstance);
            boxCollider.size = bodyScale;

            AddButton(buttonInstance, audioSourceDown, audioSourceUp);

            GrowButtonByTextMesh growButton = SafeAddComponent<GrowButtonByTextMesh>(buttonInstance);
            growButton.buttonBody = buttonBodyInstance;
            growButton.textMesh = buttonTextLabelInstance.GetComponent<TextMesh>();
            growButton.alignment = alignment;

            Rigidbody rigidBody = SafeAddComponent<Rigidbody>(buttonInstance);
            rigidBody.isKinematic = true;

            Selectable selectable = SafeAddComponent<Selectable>(buttonInstance);
            selectable.highlightMaterial = highlight;
            selectable.outlineMaterial = outline;
            selectable.textColor = fontColor;
            selectable.unselectedMaterials = new Material[] { material };
            selectable.recursive = true;

            growButton.Resize();
        }

        private void CreateButton() {
            if (buttonInstance) return;
            buttonInstance = new GameObject();
#if UNITY_EDITOR
            Undo.RegisterCreatedObjectUndo(buttonInstance, "Created Button");
#endif
            buttonInstance.transform.parent = parent.transform;
            buttonInstance.transform.localPosition = Vector3.zero;
            buttonInstance.transform.localRotation = Quaternion.identity;
        }

        private void CreateButtonBody() {
            if (buttonBodyInstance) return;
            buttonBodyInstance = Instantiate(buttonBody);

#if UNITY_EDITOR
            Undo.RegisterCreatedObjectUndo(buttonBodyInstance, "Created ButtonBody");
#endif
            buttonBodyInstance.SetActive(true);
            buttonBodyInstance.transform.localScale = bodyScale;
            buttonBodyInstance.transform.parent = buttonInstance.transform;
            buttonBodyInstance.transform.localPosition = Vector3.zero;
            buttonBodyInstance.transform.localRotation = Quaternion.identity;
            buttonBodyInstance.name = "ButtonBody";

            MeshRenderer meshRenderer = buttonBodyInstance.GetComponent<MeshRenderer>();
            meshRenderer.materials = new Material[1] { material };

            Selectable selectable = buttonBodyInstance.AddComponent<Selectable>();
            selectable.highlightMaterial = highlight;
            selectable.outlineMaterial = outline;
            selectable.textColor = fontColor;
            selectable.unselectedMaterials = new Material[] { material };

            if (!buttonBodyInstance.GetComponent<BoxCollider>()) {
                buttonBodyInstance.AddComponent<BoxCollider>();
            }
        }

        private void CreateTextLabel() {
            if (buttonTextLabelInstance) return;
            buttonTextLabelInstance = new GameObject();

#if UNITY_EDITOR
            Undo.RegisterCreatedObjectUndo(buttonTextLabelInstance, "Created ButtonTextLabel");
#endif

            buttonTextLabelInstance.transform.localScale = labelScale;
            buttonTextLabelInstance.transform.parent = buttonInstance.transform;
            buttonTextLabelInstance.transform.localPosition = new Vector3(0, 0, labelZ);
            buttonTextLabelInstance.transform.localRotation = Quaternion.identity;
            buttonTextLabelInstance.name = "ButtonTextLabel";

            TextMesh textMesh = buttonTextLabelInstance.AddComponent<TextMesh>();
            textMesh.text = buttonText;
            textMesh.fontSize = fontSize;
            textMesh.color = fontColor;
            textMesh.anchor = TextAnchor.MiddleCenter;
            textMesh.alignment = TextAlignment.Left;
        }

        public override GameObject Generate() {
            base.Generate();
            
#if UNITY_EDITOR
            Undo.SetCurrentGroupName("ButtonFactory Generate");
            int group = Undo.GetCurrentGroup();

            Undo.RegisterCompleteObjectUndo(this, "ButtonFactory state");
#endif
            CreateButton();
            CreateButtonBody();
            CreateTextLabel();
            PopulateButton();
#if UNITY_EDITOR
            Undo.CollapseUndoOperations(group);
#endif
            return buttonInstance;
        }
    }
}