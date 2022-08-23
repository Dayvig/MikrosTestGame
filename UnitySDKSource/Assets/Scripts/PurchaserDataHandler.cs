using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MikrosClient;
using UnityEngine.UI;
using MikrosClient.Analytics;
using System.Globalization;
using System;

public class PurchaserDataHandler : MonoBehaviour
{
    private Custom_PurchaseData purchaseData = new Custom_PurchaseData();
    private PurchaseDataSO ScriptableDataObj;

    [Header("UI References")]
    [Header("Price")]
    [SerializeField] private Text discountedPrice;
    [SerializeField] private Text originalPrice;
    [SerializeField] private Text discountedPercentage;

    [Header("SKU Details")]
    [SerializeField] private Text skuName;
    [SerializeField] private Text productDetails;

    /// <summary>
    /// PopulateDetails method is used for populating the  details of the Purchase Items.
    /// It is also saving thr detail in local variable
    /// 
    /// </summary>
    /// <param name="purchaseData">TgPurchaseData</param>
    public void PopulateDetails(PurchaseDataSO purchaseDataSO)
    {
        purchaseData = purchaseDataSO.purchaseData;
        ScriptableDataObj = purchaseDataSO;
        skuName.text = purchaseData.skuName;
        discountedPrice.text = "$" + purchaseData.purchasePrice.ToString();
        discountedPercentage.text = purchaseData.percentDiscount != 0 ? purchaseData.percentDiscount.ToString() + " % OFF" : "";
        if (purchaseData.purchaseDetails.Count > 0)
        {
            for (int i = 0; i < purchaseData.purchaseDetails.Count; i++)
            {
                productDetails.text += purchaseData.purchaseDetails[i].skuDescription + (i == purchaseData.purchaseDetails.Count - 1 ? "" : "| ");
            }
        }
        else
            productDetails.text = purchaseData.skuDescription;
    }

    /// <summary>
    /// SendPurchaseEvent method is used for building the TrackPurchaseRequest object and sending the event.
    /// </summary>
    public void SendPurchaseEvent()
    {
        List<TrackPurchaseRequest.PurchaseDetails> purchaseDetails = new List<TrackPurchaseRequest.PurchaseDetails>();
        foreach (Custom_PurchaseDetails purchaseDetail in purchaseData.purchaseDetails)
        {
            TrackPurchaseRequest.PurchaseDetails data = TrackPurchaseRequest.PurchaseDetails.Builder()
                .SkuName(purchaseDetail.skuName)
                .SkuDescription(purchaseDetail.skuDescription)
                .PurchaseCategory(ScriptableDataObj.GetCategory(purchaseDetail.skuType, purchaseDetail.skuSubType))
                .Create();
            purchaseDetails.Add(data);
        }
        TrackPurchaseRequest.Builder()
        .SkuName(purchaseData.skuName)
        .SkuDescription(purchaseData.skuDescription)
        .PurchaseCategory(ScriptableDataObj.GetCategory(purchaseData.skuType, purchaseData.skuSubType))
        .PurchaseType(purchaseData.purchaseType)
        .PurchaseCurrencyType(purchaseData.purchaseCurrencyType)
        .PurchasePrice(purchaseData.purchasePrice)
        .PercentDiscount(purchaseData.percentDiscount)
        .AmountRewarded(purchaseData.amountRewarded)
        .PurchaseDetail(purchaseDetails)
        .Create(
        trackPurchaseRequest => MikrosManager.Instance.AnalyticsController.LogEvent(trackPurchaseRequest),
        onFailure =>
        {
            Debug.Log("Unrecognized Error Occured");
        });
    }
}