using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MikrosClient.UI
{
    /// <summary>
    /// Dedicated Mikros UI Image.
    /// </summary>
    [RequireComponent(typeof(MikrosButton))]
    [DisallowMultipleComponent]
    public class MikrosImage : Image
    {
        private Sprite mikrosLogo;

        /// <summary>
        /// Awake function is called whenever the class is initialized first time.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            SetupImageComponent();
        }

        /// <summary>
        /// Initializing the image component.
        /// </summary>
        private void SetupImageComponent()
        {
            mikrosLogo = Resources.Load<Sprite>(Constants.MikrosLogoAssetName);
            ForcePersistentImage();
        }

        /// <summary>
        /// Prohibit change of sprite.
        /// </summary>
        private void ForcePersistentImage()
        {
            if (!preserveAspect)
                preserveAspect = true;
            if (sprite == null || !Equals(sprite, mikrosLogo))
                sprite = mikrosLogo;
            if (!Equals(color, Color.white))
                color = Color.white;
        }

        /// <summary>
        /// Called everytime attached gameobject is enabled.
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();
            preserveAspect = true;
        }

        /// <summary>
        /// Called everytime attached gameobject is disabled.
        /// </summary>
        protected override void OnDisable()
        {
            base.OnDisable();
            enabled = true;
        }

        /// <summary>
        /// Update is called once per frame.
        /// </summary>
        private void Update()
        {
            ForcePersistentImage();
        }
    }
}
