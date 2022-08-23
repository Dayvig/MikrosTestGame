namespace MikrosClient
{
    public class PurchaseCategory
    {
        public static Currency Currency => new Currency(new PurchaseCategory(0));

        public static Character Character => new Character(new PurchaseCategory(1));

        public static CharacterSkin CharacterSkin => new CharacterSkin(new PurchaseCategory(2));

        public static Cosmetic Cosmetic => new Cosmetic(new PurchaseCategory(3));

        public static Weapon Weapon => new Weapon(new PurchaseCategory(4));

        public static Armor Armor => new Armor(new PurchaseCategory(5));

        public static LevelUnlock LevelUnlock => new LevelUnlock(new PurchaseCategory(6));

        public static ContentUnlock ContentUnlock => new ContentUnlock(new PurchaseCategory(7));

        public static TimeReduction TimeReduction => new TimeReduction(new PurchaseCategory(8));

        public static Bundle Bundle => new Bundle(new PurchaseCategory(9));

        public static PurchaseCategory Other => new PurchaseCategory(10);

        private PurchaseCategory()
        { }

        private PurchaseCategory(int purchaseType)
        {
            this.purchaseType = purchaseType;
        }

        internal int purchaseType;
        internal int purchaseSubType;
    }

    public class Currency
    {
        public PurchaseCategory GOLD
        {
            get
            {
                purchaseCategory.purchaseSubType = 0;
                return purchaseCategory;
            }
        }

        public PurchaseCategory SILVER
        {
            get
            {
                purchaseCategory.purchaseSubType = 1;
                return purchaseCategory;
            }
        }

        public PurchaseCategory DIAMONDS
        {
            get
            {
                purchaseCategory.purchaseSubType = 2;
                return purchaseCategory;
            }
        }

        public PurchaseCategory EMERALDS
        {
            get
            {
                purchaseCategory.purchaseSubType = 3;
                return purchaseCategory;
            }
        }

        public PurchaseCategory GEMS
        {
            get
            {
                purchaseCategory.purchaseSubType = 4;
                return purchaseCategory;
            }
        }

        public PurchaseCategory COINS
        {
            get
            {
                purchaseCategory.purchaseSubType = 5;
                return purchaseCategory;
            }
        }

        public PurchaseCategory TOKENS
        {
            get
            {
                purchaseCategory.purchaseSubType = 6;
                return purchaseCategory;
            }
        }

        public PurchaseCategory ENERGY
        {
            get
            {
                purchaseCategory.purchaseSubType = 7;
                return purchaseCategory;
            }
        }

        public PurchaseCategory LIVES
        {
            get
            {
                purchaseCategory.purchaseSubType = 8;
                return purchaseCategory;
            }
        }

        public PurchaseCategory BADGES
        {
            get
            {
                purchaseCategory.purchaseSubType = 9;
                return purchaseCategory;
            }
        }

        public PurchaseCategory OTHER
        {
            get
            {
                purchaseCategory.purchaseSubType = 10;
                return purchaseCategory;
            }
        }

        private PurchaseCategory purchaseCategory;

        internal Currency(PurchaseCategory purchaseCategory)
        {
            this.purchaseCategory = purchaseCategory;
        }
    }

    public class Character
    {
        public PurchaseCategory OTHER
        {
            get
            {
                purchaseCategory.purchaseSubType = 0;
                return purchaseCategory;
            }
        }

        private PurchaseCategory purchaseCategory;

        internal Character(PurchaseCategory purchaseCategory)
        {
            this.purchaseCategory = purchaseCategory;
        }
    }

    public class CharacterSkin
    {
        public PurchaseCategory SUPERHERO
        {
            get
            {
                purchaseCategory.purchaseSubType = 0;
                return purchaseCategory;
            }
        }

        public PurchaseCategory FANTASY
        {
            get
            {
                purchaseCategory.purchaseSubType = 1;
                return purchaseCategory;
            }
        }

        public PurchaseCategory SCIFI
        {
            get
            {
                purchaseCategory.purchaseSubType = 2;
                return purchaseCategory;
            }
        }

        public PurchaseCategory WESTERN
        {
            get
            {
                purchaseCategory.purchaseSubType = 3;
                return purchaseCategory;
            }
        }

        public PurchaseCategory TRIBAL
        {
            get
            {
                purchaseCategory.purchaseSubType = 4;
                return purchaseCategory;
            }
        }

        public PurchaseCategory CHRISTMAS
        {
            get
            {
                purchaseCategory.purchaseSubType = 5;
                return purchaseCategory;
            }
        }

        public PurchaseCategory FESTIVAL
        {
            get
            {
                purchaseCategory.purchaseSubType = 6;
                return purchaseCategory;
            }
        }

        public PurchaseCategory HALLOWEEN
        {
            get
            {
                purchaseCategory.purchaseSubType = 7;
                return purchaseCategory;
            }
        }

        public PurchaseCategory EASTER
        {
            get
            {
                purchaseCategory.purchaseSubType = 8;
                return purchaseCategory;
            }
        }

        public PurchaseCategory CELEBRITY
        {
            get
            {
                purchaseCategory.purchaseSubType = 9;
                return purchaseCategory;
            }
        }

        public PurchaseCategory SCHOOL_UNIFORM
        {
            get
            {
                purchaseCategory.purchaseSubType = 10;
                return purchaseCategory;
            }
        }

        public PurchaseCategory WASTELAND
        {
            get
            {
                purchaseCategory.purchaseSubType = 11;
                return purchaseCategory;
            }
        }

        public PurchaseCategory NATURE
        {
            get
            {
                purchaseCategory.purchaseSubType = 12;
                return purchaseCategory;
            }
        }

        public PurchaseCategory MILITARY
        {
            get
            {
                purchaseCategory.purchaseSubType = 13;
                return purchaseCategory;
            }
        }

        public PurchaseCategory MACHINE
        {
            get
            {
                purchaseCategory.purchaseSubType = 14;
                return purchaseCategory;
            }
        }

        public PurchaseCategory GOD
        {
            get
            {
                purchaseCategory.purchaseSubType = 15;
                return purchaseCategory;
            }
        }

        public PurchaseCategory ANIMAL
        {
            get
            {
                purchaseCategory.purchaseSubType = 16;
                return purchaseCategory;
            }
        }

        public PurchaseCategory FOOD
        {
            get
            {
                purchaseCategory.purchaseSubType = 17;
                return purchaseCategory;
            }
        }

        public PurchaseCategory OTHER
        {
            get
            {
                purchaseCategory.purchaseSubType = 18;
                return purchaseCategory;
            }
        }

        private PurchaseCategory purchaseCategory;

        internal CharacterSkin(PurchaseCategory purchaseCategory)
        {
            this.purchaseCategory = purchaseCategory;
        }
    }

    public class Cosmetic
    {
        public PurchaseCategory MAKEUP
        {
            get
            {
                purchaseCategory.purchaseSubType = 0;
                return purchaseCategory;
            }
        }

        public PurchaseCategory AUDIO
        {
            get
            {
                purchaseCategory.purchaseSubType = 1;
                return purchaseCategory;
            }
        }

        public PurchaseCategory OTHER
        {
            get
            {
                purchaseCategory.purchaseSubType = 2;
                return purchaseCategory;
            }
        }

        private PurchaseCategory purchaseCategory;

        internal Cosmetic(PurchaseCategory purchaseCategory)
        {
            this.purchaseCategory = purchaseCategory;
        }
    }

    public class Weapon
    {
        public PurchaseCategory SWORD
        {
            get
            {
                purchaseCategory.purchaseSubType = 0;
                return purchaseCategory;
            }
        }

        public PurchaseCategory DAGGER
        {
            get
            {
                purchaseCategory.purchaseSubType = 1;
                return purchaseCategory;
            }
        }

        public PurchaseCategory KNIFE
        {
            get
            {
                purchaseCategory.purchaseSubType = 2;
                return purchaseCategory;
            }
        }

        public PurchaseCategory HAMMER
        {
            get
            {
                purchaseCategory.purchaseSubType = 3;
                return purchaseCategory;
            }
        }

        public PurchaseCategory AXE
        {
            get
            {
                purchaseCategory.purchaseSubType = 4;
                return purchaseCategory;
            }
        }

        public PurchaseCategory GUN
        {
            get
            {
                purchaseCategory.purchaseSubType = 5;
                return purchaseCategory;
            }
        }

        public PurchaseCategory MACE
        {
            get
            {
                purchaseCategory.purchaseSubType = 6;
                return purchaseCategory;
            }
        }

        public PurchaseCategory STAFF
        {
            get
            {
                purchaseCategory.purchaseSubType = 7;
                return purchaseCategory;
            }
        }
        public PurchaseCategory THROWABLE
        {
            get
            {
                purchaseCategory.purchaseSubType = 8;
                return purchaseCategory;
            }
        }

        public PurchaseCategory OTHER
        {
            get
            {
                purchaseCategory.purchaseSubType = 9;
                return purchaseCategory;
            }
        }

        private PurchaseCategory purchaseCategory;

        internal Weapon(PurchaseCategory purchaseCategory)
        {
            this.purchaseCategory = purchaseCategory;
        }
    }

    public class Armor
    {
        public PurchaseCategory SHIELD
        {
            get
            {
                purchaseCategory.purchaseSubType = 0;
                return purchaseCategory;
            }
        }

        public PurchaseCategory HELMET
        {
            get
            {
                purchaseCategory.purchaseSubType = 1;
                return purchaseCategory;
            }
        }

        public PurchaseCategory BODY_ARMOR
        {
            get
            {
                purchaseCategory.purchaseSubType = 2;
                return purchaseCategory;
            }
        }

        public PurchaseCategory GAUNTLET
        {
            get
            {
                purchaseCategory.purchaseSubType = 3;
                return purchaseCategory;
            }
        }

        public PurchaseCategory ACCESSORY
        {
            get
            {
                purchaseCategory.purchaseSubType = 4;
                return purchaseCategory;
            }
        }

        public PurchaseCategory OTHER
        {
            get
            {
                purchaseCategory.purchaseSubType = 5;
                return purchaseCategory;
            }
        }

        private PurchaseCategory purchaseCategory;

        internal Armor(PurchaseCategory purchaseCategory)
        {
            this.purchaseCategory = purchaseCategory;
        }
    }

    public class LevelUnlock
    {
        public PurchaseCategory OTHER
        {
            get
            {
                purchaseCategory.purchaseSubType = 0;
                return purchaseCategory;
            }
        }

        private PurchaseCategory purchaseCategory;

        internal LevelUnlock(PurchaseCategory purchaseCategory)
        {
            this.purchaseCategory = purchaseCategory;
        }
    }

    public class ContentUnlock
    {
        public PurchaseCategory OTHER
        {
            get
            {
                purchaseCategory.purchaseSubType = 0;
                return purchaseCategory;
            }
        }

        private PurchaseCategory purchaseCategory;

        internal ContentUnlock(PurchaseCategory purchaseCategory)
        {
            this.purchaseCategory = purchaseCategory;
        }
    }

    public class TimeReduction
    {
        public PurchaseCategory OTHER
        {
            get
            {
                purchaseCategory.purchaseSubType = 0;
                return purchaseCategory;
            }
        }

        private PurchaseCategory purchaseCategory;

        internal TimeReduction(PurchaseCategory purchaseCategory)
        {
            this.purchaseCategory = purchaseCategory;
        }
    }

    public class Bundle
    {
        public PurchaseCategory OTHER
        {
            get
            {
                purchaseCategory.purchaseSubType = 0;
                return purchaseCategory;
            }
        }

        private PurchaseCategory purchaseCategory;

        internal Bundle(PurchaseCategory purchaseCategory)
        {
            this.purchaseCategory = purchaseCategory;
        }
    }
}