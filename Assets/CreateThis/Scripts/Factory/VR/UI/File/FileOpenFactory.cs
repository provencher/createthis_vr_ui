﻿using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using CreateThis.Factory.VR.UI.Button;
using CreateThis.Factory.VR.UI.Scroller;
using CreateThis.Factory.VR.UI.Container;
using CreateThis.System;
using CreateThis.VRTK;
using CreateThis.VR.UI;
using CreateThis.VR.UI.Panel;
using CreateThis.VR.UI.File;
using CreateThis.VR.UI.Interact;
using CreateThis.VR.UI.Scroller;
using CreateThis.VR.UI.Container;

namespace CreateThis.Factory.VR.UI.File {
    public class FileOpenFactory : BaseFactory {
        public GameObject parent;
        public GameObject buttonBody;
        public GameObject folderPrefab;
        public Material buttonMaterial;
        public Material panelMaterial;
        public Material highlight;
        public Material outline;
        public AudioClip buttonClickDown;
        public AudioClip buttonClickUp;
        public int fontSize;
        public Color fontColor;
        public float labelZ;
        public float buttonZ;
        public Vector3 bodyScale;
        public Vector3 labelScale;
        public float padding;
        public float spacing;
        public float buttonPadding;
        public float buttonMinWidth;
        public float buttonCharacterSize;
        public float labelCharacterSize;
        public float kineticScrollerSpacing;
        public float scrollerHeight;
        public string searchPattern;
        public Camera sceneCamera;
        public Vector3 offset;
        public float minDistance;
        public bool hideOnAwake;

        protected GameObject fileOpenContainerInstance;
        protected Rigidbody fileOpenContainerRigidbody;
        protected GameObject fileOpenInstance;
        protected FileOpen fileOpenPanel;
        private Drives drives;
        private GameObject disposable;
        private GameObject currentPathLabel;
        private KineticScroller kineticScroller;
        private GameObject kineticScrollerItem;
        private GameObject kineticScrollerInstance;

        protected void SetButtonValues(MomentaryButtonFactory factory, StandardPanel panel, GameObject parent) {
            factory.useVRTK = useVRTK;
            factory.parent = parent;
            factory.buttonBody = buttonBody;
            factory.material = buttonMaterial;
            factory.highlight = highlight;
            factory.outline = outline;
            factory.buttonClickDown = buttonClickDown;
            factory.buttonClickUp = buttonClickUp;
            factory.alignment = TextAlignment.Center;
            factory.fontSize = fontSize;
            factory.fontColor = fontColor;
            factory.labelZ = labelZ;
            factory.bodyScale = bodyScale;
            factory.labelScale = labelScale;
            factory.minWidth = buttonMinWidth;
            factory.padding = buttonPadding;
            factory.characterSize = buttonCharacterSize;
            factory.panel = panel;
        }

        protected void SetKeyboardButtonPosition(GameObject button) {
            Vector3 localPosition = button.transform.localPosition;
            localPosition.z = buttonZ;
            button.transform.localPosition = localPosition;
        }

        protected GameObject GenerateKeyboardButtonAndSetPosition(MomentaryButtonFactory factory) {
            GameObject button = factory.Generate();
            SetKeyboardButtonPosition(button);
            return button;
        }

        protected GameObject DriveButton(FileOpen panel, GameObject parent, string buttonText) {
            DriveButtonFactory factory = SafeAddComponent<DriveButtonFactory>(disposable);
            SetButtonValues(factory, panel, parent);
            factory.buttonText = buttonText;
            factory.filePanel = panel;
            factory.minWidth = buttonMinWidth;
            return GenerateKeyboardButtonAndSetPosition(factory);
        }

        protected GameObject KnownFolderButton(FileOpen panel, GameObject parent, string buttonText, KnownFolder knownFolder) {
            KnownFolderButtonFactory factory = SafeAddComponent<KnownFolderButtonFactory>(disposable);
            SetButtonValues(factory, panel, parent);
            factory.buttonText = buttonText;
            factory.filePanel = panel;
            factory.minWidth = buttonMinWidth;
            factory.knownFolder = knownFolder;
            return GenerateKeyboardButtonAndSetPosition(factory);
        }

        protected GameObject Row(GameObject parent, string name = null, TextAlignment alignment = TextAlignment.Center) {
            RowContainerFactory factory = SafeAddComponent<RowContainerFactory>(disposable);
            factory.containerName = name;
            factory.parent = parent;
            factory.padding = padding;
            factory.spacing = spacing;
            factory.alignment = alignment;
            return factory.Generate();
        }

        protected GameObject Column(GameObject parent) {
            ColumnContainerFactory factory = SafeAddComponent<ColumnContainerFactory>(disposable);
            factory.parent = parent;
            factory.padding = padding;
            factory.spacing = spacing;
            return factory.Generate();
        }

        protected GameObject Panel(GameObject parent, string name) {
            PanelContainerFactory factory = SafeAddComponent<PanelContainerFactory>(disposable);
            factory.parent = parent;
            factory.containerName = name;
            factory.panelBody = buttonBody;
            factory.material = panelMaterial;
            factory.highlight = highlight;
            factory.outline = outline;
            factory.fontColor = fontColor;
            factory.bodyScale = bodyScale;
            GameObject panel = factory.Generate();

            fileOpenPanel = SafeAddComponent<FileOpen>(panel);
            fileOpenPanel.grabTarget = fileOpenContainerInstance.transform;
            fileOpenPanel.folderPrefab = folderPrefab;
            fileOpenPanel.kineticScrollItemPrefab = kineticScrollerItem;
            fileOpenPanel.height = scrollerHeight;
            fileOpenPanel.searchPattern = searchPattern;
            fileOpenPanel.sceneCamera = sceneCamera;
            fileOpenPanel.offset = offset;
            fileOpenPanel.minDistance = minDistance;
            fileOpenPanel.hideOnAwake = hideOnAwake;

            if (useVRTK) {
                CreateThis_VRTK_Interactable interactable = SafeAddComponent<CreateThis_VRTK_Interactable>(panel);
                CreateThis_VRTK_GrabAttach grabAttach = SafeAddComponent<CreateThis_VRTK_GrabAttach>(panel);
                interactable.isGrabbable = true;
                interactable.grabAttachMechanicScript = grabAttach;
            }

            drives = SafeAddComponent<Drives>(panel);

            Rigidbody rigidbody = SafeAddComponent<Rigidbody>(panel);
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;

            return panel;
        }

        protected GameObject Label(GameObject parent, string name, string text) {
            GameObject label = EmptyChild(parent, name);
            label.transform.localScale = labelScale;
            Vector3 localPosition = new Vector3(0, 0, labelZ);
            label.transform.localPosition = localPosition;
            TextMesh textMesh = SafeAddComponent<TextMesh>(label);
            textMesh.text = text;
            textMesh.fontSize = fontSize;
            textMesh.color = fontColor;
            textMesh.characterSize = labelCharacterSize;
            textMesh.anchor = TextAnchor.MiddleCenter;

            BoxCollider boxCollider = SafeAddComponent<BoxCollider>(label);

            UpdateBoxColliderFromTextMesh updateBoxCollider = SafeAddComponent<UpdateBoxColliderFromTextMesh>(label);
            updateBoxCollider.textMesh = textMesh;
            updateBoxCollider.boxCollider = boxCollider;
            return label;
        }

        protected GameObject CurrentPathRow(GameObject parent) {
            GameObject row = Row(parent, "CurrentPathRow", TextAlignment.Left);
            Label(row, "PathLabel", "                 Path");
            currentPathLabel = Label(row, "CurrentPathLabel", "C:/Foo/Blah/Stuff");
            fileOpenPanel.currentPathLabel = currentPathLabel;
            return row;
        }

        protected GameObject DriveButtonRow(GameObject parent) {
            GameObject row = Row(parent, "DriveButtonRow", TextAlignment.Left);
            Label(row, "DrivesLabel", "              Drives");
            GameObject driveButtonPrefab = DriveButton(fileOpenPanel, row, "C");
            drives.driveButtonPrefab = driveButtonPrefab;
            return row;
        }

        protected GameObject SpecialFoldersRow(GameObject parent) {
            GameObject row = Row(parent, "SpecialFoldersRow", TextAlignment.Left);
            Label(row, "SpecialFoldersLabel", "Special Folders");

            KnownFolderButton(fileOpenPanel, row, "Documents", KnownFolder.Documents);
            KnownFolderButton(fileOpenPanel, row, "Downloads", KnownFolder.Downloads);
            KnownFolderButton(fileOpenPanel, row, "Desktop", KnownFolder.Desktop);

            return row;
        }

        private void CreateDisposable(GameObject parent) {
            if (disposable) return;
            disposable = EmptyChild(parent, "disposable");
        }

        private GameObject CreateKineticScrollerItem(GameObject parent) {
            KineticScrollerItemFactory kineticScrollerItemFactory = SafeAddComponent<KineticScrollerItemFactory>(disposable);
            kineticScrollerItemFactory.useVRTK = useVRTK;
            kineticScrollerItemFactory.parent = parent;
            kineticScrollerItemFactory.material = buttonMaterial;
            kineticScrollerItemFactory.highlight = highlight;
            kineticScrollerItemFactory.outline = outline;
            kineticScrollerItemFactory.fontColor = fontColor;
            return kineticScrollerItemFactory.Generate();
        }

        private GameObject CreateKineticScroller(GameObject parent) {
            kineticScrollerInstance = EmptyChild(parent, "KineticScroller");
            kineticScroller = SafeAddComponent<KineticScroller>(kineticScrollerInstance);
            kineticScroller.space = kineticScrollerSpacing;
            fileOpenPanel.kineticScroller = kineticScroller;

            Rigidbody rigidbody = SafeAddComponent<Rigidbody>(kineticScrollerInstance);
            rigidbody.useGravity = false;

            Selectable selectable = SafeAddComponent<Selectable>(kineticScrollerInstance);
            selectable.highlightMaterial = highlight;
            selectable.outlineMaterial = outline;
            selectable.textColor = fontColor;
            selectable.unselectedMaterials = new Material[] { buttonMaterial };
            selectable.recursive = true;

            ConfigurableJoint configurableJoint = SafeAddComponent<ConfigurableJoint>(kineticScrollerInstance);
            configurableJoint.connectedBody = fileOpenContainerRigidbody;
            configurableJoint.anchor = Vector3.zero;
            configurableJoint.xMotion = ConfigurableJointMotion.Limited;
            configurableJoint.yMotion = ConfigurableJointMotion.Locked;
            configurableJoint.zMotion = ConfigurableJointMotion.Locked;
            configurableJoint.angularXMotion = ConfigurableJointMotion.Locked;
            configurableJoint.angularYMotion = ConfigurableJointMotion.Locked;
            configurableJoint.angularZMotion = ConfigurableJointMotion.Locked;
            configurableJoint.breakForce = float.PositiveInfinity;
            configurableJoint.breakTorque = float.PositiveInfinity;

            return kineticScrollerInstance;
        }

        protected void PanelHeader(StandardPanel panel, GameObject parent) {
            CurrentPathRow(parent);
        }

        protected void FileOpenPanel(GameObject parent) {
            if (fileOpenInstance) return;

            fileOpenInstance = EmptyChild(parent, "FileOpenPanel");
            Vector3 localPosition = fileOpenInstance.transform.localPosition;
            localPosition.y = -scrollerHeight * 1.50f;
            fileOpenInstance.transform.localPosition = localPosition;

            Rigidbody rigidbody = SafeAddComponent<Rigidbody>(fileOpenInstance);
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;

            GameObject panel = Panel(fileOpenInstance, "DrivesPanel");
            GameObject column = Column(panel);

            PanelHeader(fileOpenPanel, column);
            DriveButtonRow(column);
            SpecialFoldersRow(column);

            panel.transform.localPosition = Vector3.zero;
        }

        public override GameObject Generate() {
            base.Generate();

#if UNITY_EDITOR
            Undo.SetCurrentGroupName("FileOpenFactory Generate");
            int group = Undo.GetCurrentGroup();

            Undo.RegisterCompleteObjectUndo(this, "FileOpenFactory state");
#endif
            CreateDisposable(parent);

            fileOpenContainerInstance = EmptyChild(parent, "FileOpenContainer");
            fileOpenContainerRigidbody = SafeAddComponent<Rigidbody>(fileOpenContainerInstance);
            fileOpenContainerRigidbody.useGravity = false;
            fileOpenContainerRigidbody.isKinematic = true;

            kineticScrollerItem = CreateKineticScrollerItem(fileOpenContainerInstance);
            kineticScrollerItem.SetActive(false);

            FileOpenPanel(fileOpenContainerInstance);

            CreateKineticScroller(fileOpenContainerInstance);

#if UNITY_EDITOR
            Undo.DestroyObjectImmediate(disposable);
            Undo.CollapseUndoOperations(group);
#else
            Destroy(disposable);
#endif
            return fileOpenInstance;
        }
    }
}