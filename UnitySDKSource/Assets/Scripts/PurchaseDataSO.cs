using MikrosClient;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Purchase Data", menuName = "ScriptableObjects/PurchaseDataScriptableObject", order = 1),]
public class PurchaseDataSO : ScriptableObject
{
    public Custom_PurchaseData purchaseData;

    /// <summary>
    /// GetCategory method is used for mapping the skuType and skuSubType into PurchaseCategory type.
    /// </summary>
    /// <param name="skuType"></param>
    /// <param name="skuSubType"></param>
    /// <returns>PurchaseCategory objects.</returns>
    public PurchaseCategory GetCategory(SkuType skuType, SkuSubType skuSubType)
    {
        if (skuType == SkuType.CURRENCY)
        {
            if (skuSubType == SkuSubType.COINS)
            {
                return PurchaseCategory.Currency.COINS;
            }
            else if (skuSubType == SkuSubType.DIAMONDS)
            {
                return PurchaseCategory.Currency.DIAMONDS;
            }
            else if (skuSubType == SkuSubType.EMERALDS)
            {
                return PurchaseCategory.Currency.EMERALDS;
            }
        }
        else if (skuType == SkuType.WEAPON)
        {
            if (skuSubType == SkuSubType.GUN)
            {
                return PurchaseCategory.Weapon.GUN;
            }
            else if (skuSubType == SkuSubType.THROWABLE)
            {
                return PurchaseCategory.Weapon.THROWABLE;
            }
            else if (skuSubType == SkuSubType.SWORD)
            {
                return PurchaseCategory.Weapon.SWORD;
            }
            else if (skuSubType == SkuSubType.AXE)
            {
                return PurchaseCategory.Weapon.AXE;
            }
        }
        else if (skuType == SkuType.BUNDLE)
        {
            if (skuSubType == SkuSubType.OTHER)
            {
                return PurchaseCategory.Bundle.OTHER;
            }
        }
        return PurchaseCategory.Other;
    }
}

[Serializable]
public class Custom_PurchaseData
{
    public string skuName;

    public string skuDescription;

    public SkuType skuType;

    public SkuSubType skuSubType;

    public PurchaseType purchaseType;

    public PurchaseCurrencyType purchaseCurrencyType;

    public float purchasePrice;

    public float percentDiscount;

    public float amountRewarded;

    public List<Custom_PurchaseDetails> purchaseDetails = new List<Custom_PurchaseDetails>();
}
[Serializable]
public class Custom_PurchaseDetails
{
    public string skuName;

    public string skuDescription;

    public SkuType skuType;

    public SkuSubType skuSubType;

    public string timestamp;
}