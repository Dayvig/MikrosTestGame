using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Assign feature info button properties.
/// </summary>
public class FeatureInfoButton : MonoBehaviour
{
    [SerializeField] private Feature_Type featureType;

    /// <summary>
    /// Type of the feature info button.
    /// </summary>
    public Feature_Type FeatureType => featureType;
}