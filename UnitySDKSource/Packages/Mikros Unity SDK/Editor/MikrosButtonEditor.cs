using MikrosClient.UI;
using System.Reflection;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MikrosEditor
{
    /// <summary>
    /// Editor class for Mikros Button display in Inspector.
    /// </summary>
    [CustomEditor(typeof(MikrosButton)), CanEditMultipleObjects]
    internal sealed class MikrosButtonEditor : SelectableEditor
    {
		#region MikrosButton
		private SerializedProperty buttonStyleField, followDelayField, floatingMoveSpeedField; // Mikros Button field(s).
		#endregion MikrosButton

		#region UnityButton
		SerializedProperty m_OnClickProperty; // Unity UI Button field(s). Don't change the variable naming.
        #endregion UnityButton

        #region RectTransform
        RectTransform rectTransform; // Rect transform of the button in editor.
        DrivenRectTransformTracker drivenRectTransformTracker; // Rec Transform tracker to disable its specific fields.
        #endregion RectTransform

        /// <summary>
        /// Scale factor used for calculation of position of floating Mikros button in Editor.
        /// </summary>
        private float mainCanvasScaleFactor => rectTransform?.GetComponentInParent<Canvas>() == null ? 0.35f : rectTransform.GetComponentInParent<Canvas>().scaleFactor;

        /// <summary>
        /// Called everytime attached gameobject is enabled.
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();
            SetButtonFields();
            SetRectTransform();
            SetPersistentOnClickListener();
        }

        /// <summary>
        /// Initialize the serialized fields of button.
        /// </summary>
        private void SetButtonFields()
		{
            m_OnClickProperty = serializedObject.FindProperty("m_OnClick");
            buttonStyleField = serializedObject.FindProperty("buttonStyle");
            followDelayField = serializedObject.FindProperty("followDelayFactor");
            floatingMoveSpeedField = serializedObject.FindProperty("floatingMoveSpeed");
        }

        /// <summary>
        /// Initialize the rect transform.
        /// </summary>
        private void SetRectTransform()
		{
            rectTransform = (serializedObject.targetObject as MonoBehaviour).GetComponent<RectTransform>();
            drivenRectTransformTracker = new DrivenRectTransformTracker();
        }

        /// <summary>
        /// Setup the Driven Rect Transform Tracker which is used to block relevant fields of Rect transform.
        /// </summary>
        private void PersistentRectTrackers()
		{
            drivenRectTransformTracker.Clear();
            drivenRectTransformTracker.Add(this, rectTransform, DrivenTransformProperties.Rotation);
        }

        /// <summary>
        /// Provide onClick listener to Mikros button.
        /// </summary>
        private void SetPersistentOnClickListener()
		{
            MikrosButton mikrosButton = (serializedObject.targetObject as MonoBehaviour).GetComponent<MikrosButton>();
            if(mikrosButton.PersistentEventCount >= 1)
                return;
            UnityAction<GameObject> action = new UnityAction<GameObject>(mikrosButton.OnMikrosButtonClick);
            var field = typeof(MikrosButton).GetField(m_OnClickProperty.name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
            var invokeCallList = field.GetValue(mikrosButton);
            UnityEventTools.AddObjectPersistentListener<GameObject>(invokeCallList as UnityEventBase, action, mikrosButton.gameObject);
        }

        /// <summary>
        /// Called every frame in Unity editor.
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();
            PersistentRectTrackers();
            EditorGUILayout.PropertyField(buttonStyleField);
            if(Equals(buttonStyleField.enumValueIndex, 1))
            {
                EditorGUILayout.PropertyField(followDelayField);
                EditorGUILayout.PropertyField(floatingMoveSpeedField);
                RePositionButton();
                drivenRectTransformTracker.Add(this, rectTransform, DrivenTransformProperties.AnchoredPosition3D);
            }
            EditorGUILayout.Space();
            using(new EditorGUI.DisabledScope(true))
            {
                EditorGUILayout.PropertyField(m_OnClickProperty);
            }
            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// Re-position the Mikros button when Floating.
        /// </summary>
		private void RePositionButton()
        {
            if(EditorApplication.isPlaying)
                return;
            Vector3 pos = rectTransform.transform.position;
            pos.x = Screen.width - (rectTransform.sizeDelta.x * rectTransform.localScale.x * mainCanvasScaleFactor / 4);
            rectTransform.position = pos;
		}

        /// <summary>
        /// Add new gameobject with MikrosButton component in active scene.
        /// </summary>
		[MenuItem("GameObject/UI/Button - Mikros", false, 2031)]
        private static void AddMikrosButtonToScene()
		{
            GameObject selection = Selection.activeGameObject;
            if(selection == null)
            {
                selection = FindObjectOfType<Canvas>()?.gameObject;
                if(selection == null)
                    selection = new GameObject("GameObject");
            }
            Transform parentForButton = DetermineParentForButton(selection);

            GameObject buttonGameObject = new GameObject("Button (Mikros)", typeof(MikrosButton));
            buttonGameObject.transform.parent = parentForButton;
            buttonGameObject.transform.localPosition = Vector3.zero;
            buttonGameObject.transform.localRotation = Quaternion.identity;
            buttonGameObject.transform.localScale = Vector3.one;
            buttonGameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(35, 35);

            Selection.activeObject = buttonGameObject;

            EditorUtility.SetDirty(buttonGameObject);
		}

        /// <summary>
        /// Determine the parent of the would-be created Mikros button.
        /// </summary>
        /// <param name="gameObject">GameObject to determine the parent of.</param>
        /// <returns>Parent transform.</returns>
        private static Transform DetermineParentForButton(GameObject gameObject)
		{
            if(!IsCorrectCanvasSetup(gameObject))
                return CreateCanvas(gameObject.transform).transform;
            else
                return gameObject.transform;
        }

        /// <summary>
        /// Create a new Canvas UI in scene.
        /// </summary>
        /// <param name="parent">Parent for the canvas gameObject</param>
        /// <returns></returns>
        private static Canvas CreateCanvas(Transform parent = null)
		{
            GameObject canvasGO = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasGO.transform.parent = parent;
            Canvas canvas = canvasGO.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            return canvas;
        }

        /// <summary>
        /// Check for correct canvas setup.
        /// </summary>
        /// <param name="canvasInParentCheck">GameObject to check canvas in parent</param>
        /// <returns></returns>
        private static bool IsCorrectCanvasSetup(GameObject canvasInParentCheck)
		{
            Canvas canvas = canvasInParentCheck.GetComponentInParent<Canvas>();
            if(canvas == null)
                return false;
            else if(canvas.GetComponent<CanvasScaler>() == null || canvas.GetComponent<GraphicRaycaster>() == null)
                return false;
            else
                return true;
		}
    }
}
