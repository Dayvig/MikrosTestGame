using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MikrosClient
{
    public enum SkuType
    {
        NONE,
        CURRENCY,
        CHARACTER,
        CHARACTER_SKIN,
        COSMETIC,
        WEAPON,
        ARMOR,
        LEVEL_UNLOCK,
        CONTENT_UNLOCK,
        TIME_REDUCTION,
        BUNDLE,
        OTHER
    }

    public enum SkuSubType
    {
        NONE,
        //Currency
        GOLD,
        SILVER,
        DIAMONDS,
        EMERALDS,
        GEMS,
        COINS,
        TOKENS,
        ENERGY,
        LIVES,
        BADGES,
        //Character skin
        SUPERHERO,
        FANTASY,
        SCI_FI,
        WESTERN,
        TRIBAL,
        CHRISTMAS,
        FESTIVAL,
        HALLOWEEN,
        EASTER,
        CELEBRITY,
        SCHOOL_UNIFORM,
        WASTELAND,
        NATURE,
        MILITARY,
        MACHINE,
        GOD,
        ANIMAL,
        FOOD,
        //Cosmetic
        MAKEUP,
        AUDIO,
        //Weapon
        SWORD,
        DAGGER,
        KNIFE,
        HAMMER,
        AXE,
        GUN,
        THROWABLE,
        MACE,
        STAFF,
        //Armor
        SHIELD,
        HELMET,
        BODY_ARMOR,
        GAUNTLER,
        ACCESSORY,
        //Commom
        OTHER
    }

    public enum PurchaseType
    {
        NONE,
        IN_APP,
        IN_GAME
    }

    public enum PurchaseCurrencyType
    {
        NONE,
        USD,
        INR
    }
}
