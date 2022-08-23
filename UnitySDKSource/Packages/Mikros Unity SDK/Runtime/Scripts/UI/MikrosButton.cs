using MikrosClient.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MikrosClient.UI
{
    /// <summary>
    /// Dedicated Mikros UI Button to open the Mikros Custom UI.
    /// </summary>
    [AddComponentMenu("UI/Button - Mikros", 31)]
    [RequireComponent(typeof(MikrosImage))]
    [DisallowMultipleComponent]
    public sealed class MikrosButton : Selectable, IPointerClickHandler, ISubmitHandler, IDragHandler
    {
        /// <summary>
        /// buttonStyle is used for determining the placement style of Mikros button.
        /// </summary>
        [Tooltip("Determine the placement style of Mikros button")]
        [SerializeField] private MIKROS_BUTTON_STYLE buttonStyle;

        /// <summary>
        /// followDelayFactor is used for follow speed of button while dragging it.
        /// </summary>
        [Tooltip("Used for follow speed of button while dragging it")]
        [SerializeField] private float followDelayFactor = 10;

        /// <summary>
        /// floatingMoveSpeed is used for button moving spped when user cancel/complete dragging of the icon.
        /// </summary>
        [Tooltip("Used for button moving spped when user cancel/complete dragging of the icon")]
        [SerializeField] private float floatingMoveSpeed = 0.2f;

        /// <summary>
        /// Function definition for a button click event.
        /// </summary>
        [Serializable] public class ButtonClickedEvent : UnityEvent { }

        /// <summary>
        /// Event delegates triggered on click.
        /// </summary>
        [SerializeField] private ButtonClickedEvent m_OnClick = new ButtonClickedEvent();

        /// <summary>
        /// Screen Width of the device.
        /// </summary>
        private int screenWidth;

        /// <summary>
        /// Screen Height of the device.
        /// </summary>
        private int screenHeight;

        /// <summary>
        /// Screen half width of the device.
        /// </summary>
        private int screenHalfWidth;

        /// <summary>
        /// _sizeXOffest is used for determining button X Position offset.
        /// </summary>
        private float sizeXOffest;

        /// <summary>
        /// _sizeYOffset is used for determining button Y Position offset.
        /// </summary>
        private float sizeYOffset;

        /// <summary>
        /// Rect transform of Mikros Button.
        /// </summary>
        private RectTransform rectTransform;

        /// <summary>
        /// isDragging is a boolean value to used as a flag variable that specify dragged event is fired or not.
        /// </summary>
        private bool isDragging = false;

        /// <summary>
        /// Holds drag position.
        /// </summary>
        private Vector2 tempDragPos;

        /// <summary>
        /// Scale factor used for calculation of position of floating Mikros button.
        /// </summary>
        private float mainCanvasScaleFactor => GetComponentInParent<Canvas>() == null ? 0.35f : GetComponentInParent<Canvas>().scaleFactor;

        /// <summary>
        /// RectTransform of the Mikros Button.
        /// </summary>
        private RectTransform buttonRect
        {
            get
            {
                if (rectTransform == null)
                {
                    rectTransform = GetComponent<RectTransform>();
                }
                return rectTransform;
            }
        }

        /// <summary>
        /// Returns the number of persistent event listeners.
        /// </summary>
        public int PersistentEventCount => m_OnClick.GetPersistentEventCount();

        /// <summary>
        /// Temporarily holds button reposition completion callback.
        /// </summary>
        private Action moveEndCallback = null;

        /// <summary>
        /// Awake function is called whenever the class is initialized first time.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            rectTransform = GetComponent<RectTransform>();
            ForcePersistentInteractable();
        }

        /// <summary>
        /// Start is called before the first frame update.
        /// </summary>
        protected override void Start()
        {
            base.Start();
            RectTransformDimensionsChange();
            Utils.SetActivityIndicatorStyle();
        }

        /// <summary>
        /// Called everytime attached gameobject is disabled.
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();
            if (Application.isPlaying)
            {
                MikrosUiCanvas.Instance.OnOrientationChange += OnScreenOrientationChange;
            }
        }

        /// <summary>
        /// Handle screen orientation change events.
        /// </summary>
        /// <param name="screenOrientation">Screen orientation.</param>
        private void OnScreenOrientationChange(ScreenOrientation screenOrientation)
        {
            RectTransformDimensionsChange();
        }

        /// <summary>
        /// OnRectTransformDimensionsChange will called whenever device rotate.
        /// </summary>
        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();
        }

        /// <summary>
        /// Initializing dimensions.
        /// </summary>
        private void RectTransformDimensionsChange()
        {
            if (!Application.isPlaying)
            {
                return;
            }
            screenWidth = Screen.width;
            screenHeight = Screen.height;
            screenHalfWidth = screenWidth / 2;
            sizeXOffest = (buttonRect.sizeDelta.x * buttonRect.localScale.x * mainCanvasScaleFactor) / 4;
            sizeYOffset = (buttonRect.sizeDelta.y * buttonRect.localScale.y * mainCanvasScaleFactor) / 2;
            RePositionButton();
        }

        /// <summary>
        /// Resetting the Mikros button position to screen's left or right.
        /// </summary>
        /// <param name="onEndReposition">Callback for reposition end.</param>
        private void RePositionButton(Action onEndReposition = null)
        {
            if (!Equals(buttonStyle, MIKROS_BUTTON_STYLE.FLOATING))
            {
                return;
            }
            Vector3 pos = transform.position;

            // If pos.x less than device's screen half width then set the position X to left.
            if (pos.x < screenHalfWidth)
            {
                pos.x = sizeXOffest;
            }
            // If pos.x greater than device's screen half width then set the position X to right.
            else
            {
                pos.x = screenWidth - sizeXOffest;
            }
            pos.y = Mathf.Clamp(pos.y, sizeYOffset, screenHeight - sizeYOffset); // claming the position Y based on device's screen Height bound.
            moveEndCallback = onEndReposition;
            // Moving the Widget small icon to it's target position.
            iTween.MoveTo(gameObject, iTween.Hash("position", pos, "time", floatingMoveSpeed, "oncomplete", nameof(MoveComplete), "oncompletetarget", gameObject));
        }

        /// <summary>
        /// Task after button reposition completed.
        /// </summary>
        private void MoveComplete()
        {
            moveEndCallback?.Invoke();
            moveEndCallback = null;
        }

        /// <summary>
        /// Force button to remain interactable.
        /// </summary>
        private void ForcePersistentInteractable()
        {
            if (!interactable)
            {
                interactable = true;
            }
        }

        /// <summary>
        /// Button press operations.
        /// </summary>
        private void Press()
        {
            if (!IsActive() || !IsInteractable())
            {
                return;
            }

            UISystemProfilerApi.AddMarker("Button.onClick", this);
            m_OnClick.Invoke();
        }

        /// <summary>
        /// Call all registered IPointerClickHandlers.
        /// </summary>
        /// <param name="eventData">Pointer click event data.</param>
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

            Press();
        }

        /// <summary>
        /// Call all registered ISubmitHandlers.
        /// </summary>
        /// <param name="eventData">Submit event data.</param>
        public void OnSubmit(BaseEventData eventData)
        {
            Press();

            // if we get set disabled during the press
            // don't run the coroutine.
            if (!IsActive() || !IsInteractable())
            {
                return;
            }

            DoStateTransition(SelectionState.Pressed, false);
            StartCoroutine(OnFinishSubmit());
        }

        /// <summary>
        /// Coroutine for graphical representation of submit finished event.
        /// </summary>
        /// <returns>Coroutine.</returns>
        private IEnumerator OnFinishSubmit()
        {
            var fadeTime = colors.fadeDuration;
            var elapsedTime = 0f;

            while (elapsedTime < fadeTime)
            {
                elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }

            DoStateTransition(currentSelectionState, false);
        }

        /// <summary>
        /// Mikros button onClick event.
        /// </summary>
        /// <param name="sender">Gameobject from where function is invoked.</param>
        public void OnMikrosButtonClick(GameObject sender)
        {
            if (!isDragging)
            {
                MikrosUiCanvas.Instance.OpenMikrosPanel(rectTransform);
            }
        }

        /// <summary>
        /// OnPointerUp function is used to track mouse up event.
        /// Used here to end the dragging of the Mikros button.
        /// </summary>
        public override void OnPointerUp(PointerEventData eventData)
        {
            if (isDragging)
            {
                RePositionButton(() => isDragging = false);
            }
            else
            {
                isDragging = false;
            }
            base.OnPointerUp(eventData);
        }

        /// <summary>
        /// OnDrag function is used to track mouse drag event.
        /// Used here to drag the Mikros button on the screen.
        /// </summary>
        public void OnDrag(PointerEventData eventData)
        {
            if (Equals(buttonStyle, MIKROS_BUTTON_STYLE.STATIC))
            {
                return;
            }
            isDragging = true;
            tempDragPos = eventData.position;
        }

        /// <summary>
        /// Update is called once per frame.
        /// </summary>
        private void Update()
        {
            ForcePersistentInteractable();
            if (isDragging)
            {
                transform.position = Vector2.Lerp(transform.position, tempDragPos, Time.deltaTime * followDelayFactor);
            }
        }

        /// <summary>
        /// Called everytime attached gameobject is disabled.
        /// </summary>
        protected override void OnDisable()
        {
            base.OnDisable();
            if (Application.isPlaying)
            {
                MikrosUiCanvas.Instance.OnOrientationChange -= OnScreenOrientationChange;
            }
            enabled = true;
        }
    }
}
