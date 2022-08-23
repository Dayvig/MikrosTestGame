using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MikrosClient.Analytics;

public class InAppPurchaseScreen : ScreenBase
{
    
    [SerializeField] private List<PurchaseDataSO> purchaseSoData;
    [SerializeField] private PurchaserDataHandler buttonPrefab;
    [SerializeField] private Transform contentParent;
    // Start is called before the first frame update
    void Start()
    {
        CreateDemoInAppButtonObjects();
    }
    private void OnEnable()
    {
        screenTime = Time.time;
    }

    private void CreateDemoInAppButtonObjects()
    {
        foreach (PurchaseDataSO data in purchaseSoData)
        {
            PurchaserDataHandler obj = Instantiate<PurchaserDataHandler>(buttonPrefab,contentParent);
            obj.PopulateDetails(data);
        }
    }
}