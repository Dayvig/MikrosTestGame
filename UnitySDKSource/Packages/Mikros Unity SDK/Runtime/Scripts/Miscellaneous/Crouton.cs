using MikrosClient.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MikrosClient.UI
{
    /// <summary>
    /// Crouton message display.
    /// </summary>
    internal sealed class Crouton : MonoBehaviour
    {
        [SerializeField] private Image mainImage;
        [SerializeField] private Color errorColor;
        [SerializeField] private Color successColor;
        [SerializeField] private Text messageText;

        private static Crouton instance;
        private RectTransform croutonRect;
        private float animateTime = 0.25f;
        private float viewTime = 3f;
        private Action croutonLifecycleEndCallback = null;

        /// <summary>
        /// Create an object of Crouton and set the parent under which crouton will be placed.
        /// </summary>
        /// <param name="parent">Parent gameobject transform.</param>
        /// <param name="croutonType">Status of crouton to display.</param>
        /// <returns>Crouton object.</returns>
        internal static Crouton Builder(Transform parent, STATUS_TYPE croutonType)
        {
            // if object not created then create the object first
            if (instance == null)
            {
                instance = Instantiate(Resources.Load<Crouton>(Constants.CroutonPrefabName), parent, false);
                instance.croutonRect = instance.GetComponent<RectTransform>();
            }
            else
            {
                instance.transform.SetParent(parent);
            }
            instance.SetCroutonColor(croutonType);
            instance.gameObject.SetActive(false); // Disable Crouton to enable form outside.
            return instance;
        }

        /// <summary>
        /// Set color of crouton display.
        /// </summary>
        /// <param name="croutonStatus">Status of crouton to display.</param>
        private void SetCroutonColor(STATUS_TYPE croutonStatus)
        {
            switch (croutonStatus)
            {
                case STATUS_TYPE.ERROR:
                    mainImage.color = errorColor;
                    break;

                case STATUS_TYPE.SUCCESS:
                    mainImage.color = successColor;
                    break;

                default:
                    mainImage.color = errorColor;
                    break;
            }
        }

        /// <summary>
        /// Show a message via crouton.
        /// </summary>
        /// <param name="message">Custom message.</param>
        /// <returns>Crouton object.</returns>
        internal Crouton ShowMessage(string message)
        {
            AnimateCroutonIn();
            messageText.text = message;
            return this;
        }

        /// <summary>
        /// Set callback to handle end of crouton display.
        /// </summary>
        /// <param name="callback">Callback to trigger on end of crouton display.</param>
        internal void OnCroutonEnd(Action callback)
        {
            croutonLifecycleEndCallback = callback;
        }

        /// <summary>
        /// Animate crouton into view.
        /// </summary>
        private void AnimateCroutonIn()
        {
            croutonRect.anchoredPosition = new Vector2(0, croutonRect.rect.height);
            gameObject.SetActive(true);
            Hashtable tweenArgs = new Hashtable
            {
                { "oncomplete", nameof(CountdownShowTime) }
            };
            CroutonUpDownAnimate(croutonRect.anchoredPosition.y, 0f, tweenArgs);
        }

        /// <summary>
        /// Start countdown of crouton display.
        /// </summary>
        private void CountdownShowTime()
        {
            StartCoroutine(CountdownCoroutine());
        }

        /// <summary>
        /// Coroutine to countdown showtime of crouton.
        /// </summary>
        /// <returns>Coroutine.</returns>
        private IEnumerator CountdownCoroutine()
        {
            yield return new WaitForSeconds(viewTime);
            AnimateCroutonOut();
        }

        /// <summary>
        /// Animate crouton out of view.
        /// </summary>
        private void AnimateCroutonOut()
        {
            Hashtable tweenArgs = new Hashtable
            {
                { "oncomplete", nameof(CloseCrouton) }
            };
            CroutonUpDownAnimate(0f, croutonRect.rect.height, tweenArgs);
        }

        /// <summary>
        /// Animate the crouton up or down.
        /// </summary>
        /// <param name="from">Start value.</param>
        /// <param name="to">End value.</param>
        /// <param name="additionalArgs">Additional arguments for the tween.</param>
        private void CroutonUpDownAnimate(float from, float to, Hashtable additionalArgs = null)
        {
            Hashtable tweenArgs = new Hashtable
            {
                { "from", from },
                { "to", to },
                { "time", animateTime },
                { "onupdatetarget", gameObject },
                { "onupdate", nameof(OnAnimateCrouton) },
                { "oncompletetarget", gameObject },
                { "easetype", iTween.EaseType.linear }
            };
            if (!Equals(additionalArgs, null))
            {
                foreach (DictionaryEntry item in additionalArgs)
                {
                    tweenArgs.Add(item.Key, item.Value);
                }
            }
            iTween.ValueTo(gameObject, tweenArgs);
        }

        /// <summary>
        /// Handle change of crouton position value while animating.
        /// </summary>
        /// <param name="newValue">Updated position value.</param>
        private void OnAnimateCrouton(float newValue)
        {
            croutonRect.anchoredPosition = new Vector2(0, newValue);
        }

        /// <summary>
        /// Stop showing the crouton.
        /// </summary>
        private void CloseCrouton()
        {
            messageText.text = string.Empty;
            croutonLifecycleEndCallback?.Invoke();
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Called everytime the Authentication panel is disabled.
        /// </summary>
        private void OnDisable()
        {
            croutonLifecycleEndCallback = null;
        }
    }
}
