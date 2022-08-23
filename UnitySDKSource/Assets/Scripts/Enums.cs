using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Type of Mikros feature.
/// </summary>
[Serializable]
public enum Feature_Type
{
    MIKROS_SSO,
    AUTH,
    ANALYTICS,
    REPUTATION,
    CRASH,
    IN_APP_PURCHASE
}

[Serializable]
public enum ScreenType
{
    MAIN,
    ANALYTICS,
    REPUTATION_SCORE,
    AUTHENTICATION,
    CRASH,
    IN_APP_PURCHASE
}