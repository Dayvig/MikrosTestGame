using System.Collections;
using System.Collections.Generic;
using UnityEngine;
     
public class AspectRatioChangerScript : MonoBehaviour
{
    private GameObject camera;
     
    void Start()
    {
        camera = GameObject.Find("Main Camera");
    }
    void OnRectTransformDimensionsChange()
    {
     
        StartCoroutine(WaitOneFrame());
    }
     
    IEnumerator WaitOneFrame()
    {
        //returning 0 will make it wait 1 frame
        yield return 0;
     
        //Change the aspect ratio
        Vector2 aspectRatio = GetAspectRatio(Screen.width, Screen.height);
        camera.transform.GetComponent<Camera>().aspect = aspectRatio.x / aspectRatio.y;
    }
     
    public static Vector2 GetAspectRatio(int x, int y)
    {
        float f = (float)x / (float)y;
        int i = 0;
        while (true)
        {
            i++;
            if (System.Math.Round(f * i, 2) == Mathf.RoundToInt(f * i))
                break;
        }
        return new Vector2((float)System.Math.Round(f * i, 2), i);
    }
     
    public static Vector2 GetAspectRatio(int x, int y, bool debug)
    {
        float f = (float)x / (float)y;
        int i = 0;
        while (true)
        {
            i++;
            if (System.Math.Round(f * i, 2) == Mathf.RoundToInt(f * i))
                break;
        }
        if (debug)
            Debug.Log("Aspect ratio is " + f * i + ":" + i + " (Resolution: " + x + "x" + y + ")");
        return new Vector2((float)System.Math.Round(f * i, 2), i);
    }
}