using System.Collections;
using System.Collections.Generic;
using MikrosClient;
using MikrosClient.Analytics;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public GameObject cube;
    public GameObject badCube;
    // Start is called before the first frame update
    void Start()
    {
        MikrosManager.Instance.AnalyticsController.LogEvent("Test Event", (Hashtable customEventWholeData) =>
            {
                cube.SetActive(false);
            },
            onFailure =>
            {
                badCube.SetActive(true);
            });
        MikrosManager.Instance.AnalyticsController.FlushEvents();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
