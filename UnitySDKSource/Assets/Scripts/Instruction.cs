using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Instruction : MonoBehaviour
{
    /// <summary>
    /// All Gameobjects.
    /// Serialized Field to show up in inspector of Unity.
    /// </summary>
    [SerializeField] private GameObject loader;

    [SerializeField] private GameObject popup;

    [SerializeField] private Text popupText;

    public static Instruction Instance;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// Inactive GameObject is set to active, or after a GameObject created with Object.Instantiate is initialized. 
    /// Use Awake to initialize.
    ///
    /// Ref - https://docs.unity3d.com/ScriptReference/MonoBehaviour.Awake.html
    /// </summary>
    private void Awake()
    {
        if (Equals(Instance, null))
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Activates the loader.
    /// </summary>
    public void ShowLoader()
    {
        loader.SetActive(true);
    }

    /// <summary>
    /// Deactivates the loader.
    /// </summary>
    public void HideLoader()
    {
        loader.SetActive(false);
    }

    /// <summary>
    /// Activates the popup.
    /// </summary>
    /// <param name="text">Text to display in popup.</param>
    public void ShowPopup(string text)
    {
        popupText.text = text;
        popup.SetActive(true);
    }

    /// <summary>
    /// Deactivates the popup.
    /// </summary>
    public void HidePopup()
    {
        popup.SetActive(false);
        popupText.text = string.Empty;
    }
}
