using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Screen Base.
/// Abstract class that is used as a base for other classes.
/// Ref- https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/abstract
/// </summary>
public abstract class ScreenBase : MonoBehaviour
{
    public float screenTime;
    public ScreenType screenType;
}