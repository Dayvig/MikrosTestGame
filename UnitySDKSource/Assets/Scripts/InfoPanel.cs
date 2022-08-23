using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Information panel for features of Mikros.
/// </summary>
public class InfoPanel : MonoBehaviour
{
    public Text headingTxt;
    public Text descriptionTxt;
    public RectTransform rectTransform;
    private float offset= 300;

    /// <summary>
    /// Sets the heading, description
    /// And position of the info panel.
    /// </summary>
    /// <param name="featureButton">Reference of the feature's info button.</param>
    public void SetPanelPosition(FeatureInfoButton featureButton)
    {
        switch (featureButton.FeatureType)
        {
            case Feature_Type.MIKROS_SSO:
                headingTxt.text = Constants.mikrosHeading;
                descriptionTxt.text = Constants.mikrosDescription;
                rectTransform.anchoredPosition =new Vector2(rectTransform.anchoredPosition.x ,featureButton.GetComponent<RectTransform>().anchoredPosition.y);
                break;

            case Feature_Type.AUTH:
                headingTxt.text = Constants.authHeading;
                descriptionTxt.text = Constants.authDescription;
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, featureButton.GetComponent<RectTransform>().anchoredPosition.y);
                break;

            case Feature_Type.ANALYTICS:
                headingTxt.text = Constants.analyticsHeading;
                descriptionTxt.text = Constants.analyticsDescription;
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, featureButton.GetComponent<RectTransform>().anchoredPosition.y);
                break;

            case Feature_Type.REPUTATION:
                headingTxt.text = Constants.reputationHeading;
                descriptionTxt.text = Constants.reputationDescription;
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, featureButton.GetComponent<RectTransform>().anchoredPosition.y+offset);
                break;

            case Feature_Type.CRASH:
                headingTxt.text = Constants.crashHeading;
                descriptionTxt.text = Constants.crashDescription;
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, featureButton.GetComponent<RectTransform>().anchoredPosition.y+offset);
                break;

            case Feature_Type.IN_APP_PURCHASE:
                headingTxt.text = Constants.purchaseHeading;
                descriptionTxt.text = Constants.purchaseDescription;
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, featureButton.GetComponent<RectTransform>().anchoredPosition.y + offset);
                break;
        }
    }
}
