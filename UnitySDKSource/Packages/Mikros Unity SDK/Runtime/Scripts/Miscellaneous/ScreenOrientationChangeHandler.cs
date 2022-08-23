using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MikrosClient.UI
{
    /// <summary>
    /// UI change to handle screen orientations.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    internal sealed class ScreenOrientationChangeHandler : MonoBehaviour
    {
        /// <summary>
        /// Reference RectRransform for portrait mode.
        /// </summary>
        [SerializeField] private RectTransform portriatRectRef;

        /// <summary>
        /// Reference RectRransform for landscape mode.
        /// </summary>
        [SerializeField] private RectTransform landscapeRectRef;

        /// <summary>
        /// RectTransform of the gameObject this script is attached.
        /// </summary>
        private RectTransform thisRect;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            thisRect = GetComponent<RectTransform>();
            InitialUiChange();
        }

        /// <summary>
        /// Called everytime the attached gameobject is enabled.
        /// </summary>
        private void OnEnable()
        {
            MikrosUiCanvas.Instance.OnOrientationChange += ChangeUiWithOrientation;
        }

        /// <summary>
        /// Detect screen orientation and change UI the first time.
        /// </summary>
        private void InitialUiChange()
        {
            if (Equals(Utils.GetCurrentScreenOrientation(), ScreenOrientation.Landscape))
            {
                ChangeUiWithOrientation(ScreenOrientation.Landscape);
            }
        }

        /// <summary>
        /// UI change with respect to screen orientation.
        /// </summary>
        /// <param name="screenOrientation">Screen orientation.</param>
        private void ChangeUiWithOrientation(ScreenOrientation screenOrientation)
        {
            switch (screenOrientation)
            {
                case ScreenOrientation.Portrait:
                    ChangeRectValues(portriatRectRef);
                    break;

                case ScreenOrientation.Landscape:
                    ChangeRectValues(landscapeRectRef);
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Change the change RectTransform values according to the reference.
        /// </summary>
        /// <param name="referenceRect">Reference RectTransform.</param>
        private void ChangeRectValues(RectTransform referenceRect)
        {
            thisRect.anchorMin = referenceRect.anchorMin;
            thisRect.anchorMax = referenceRect.anchorMax;
            thisRect.pivot = referenceRect.pivot;
            thisRect.anchoredPosition = referenceRect.anchoredPosition;
            thisRect.sizeDelta = referenceRect.sizeDelta;
            thisRect.ForceUpdateRectTransforms();
        }

        /// <summary>
        /// Called everytime the attached gameobject is disabled.
        /// </summary>
        private void OnDisable()
        {
            MikrosUiCanvas.Instance.OnOrientationChange -= ChangeUiWithOrientation;
        }
    }
}
