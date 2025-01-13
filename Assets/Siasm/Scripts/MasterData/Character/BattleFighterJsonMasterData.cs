using System.Collections.Generic;

namespace Siasm
{
    [System.Serializable]
    public class AttributeResistJsonModel
    {
        public AttributeResistType NormalResist = AttributeResistType.Normal;
        public AttributeResistType JoyResist = AttributeResistType.Normal;
        public AttributeResistType TrustResist = AttributeResistType.Normal;
        public AttributeResistType FearResist = AttributeResistType.Normal;
        public AttributeResistType SurpriseResist = AttributeResistType.Normal;
        public AttributeResistType SadnessResist = AttributeResistType.Normal;
        public AttributeResistType DisgustResist = AttributeResistType.Normal;
        public AttributeResistType AngerResist = AttributeResistType.Normal;
        public AttributeResistType AnticipationResist = AttributeResistType.Normal;
    }

    [System.Serializable]
    public class PassiveAbilityJsonModel
    {
        public int ReleaseLevel;
        public string PassiveAbilityName;
        public PassiveAbilityType PassiveAbilityType;
        public int MainDetailNumber;
        public int SubDetailNumber;
        public string DevelopmentMemo;  // 開発用のメモ
    }

    [System.Serializable]
    public class DeckCardJsonModel
    {
        public int CardId;
        public int CardNumber;
    }

    [System.Serializable]
    public class DeckJsonModel
    {
        public int AchievementLevel;
        public DeckCardJsonModel[] DeckCardJsonModels;
    }

    [System.Serializable]
    public class ArchiveJsonModel
    {
        public List<string> ReportFiles;
    }

    [System.Serializable]
    public class TechnologyDeveloperJsonModel
    {
        public string DeveloperName;
        public string[] ReportFiles;
        public string DevelopmentMemo;  // 開発用のメモ
    }

    [System.Serializable]
    public class DropItemJsonModel
    {
        public int AchievementLevel;
        public DropInventoryType DropInventoryType;
        public int DetailNumber;
    }

    [System.Serializable]
    public class BattleFighterJsonModel
    {
        public string ProductName;
        public string ThemeNameMemo;    // 開発用のメモ
        public string TrueName;
        public string AdmissionName;
        public string ManagementNumber;
        public RiskLevelType RiskLevelType;
        public string DevelopmentMemo;  // 開発用のメモ
        public AttributeResistJsonModel AttributeResistJsonModel = new AttributeResistJsonModel();
        public List<PassiveAbilityJsonModel> PassiveAbilityJsonModels = new List<PassiveAbilityJsonModel>();
        public List<DeckJsonModel> DeckJsonModels = new List<DeckJsonModel>();
        public List<DropItemJsonModel> DropItemJsonModels = new List<DropItemJsonModel>();
        public ArchiveJsonModel ArchiveJsonModel = new ArchiveJsonModel();
        public TechnologyDeveloperJsonModel TechnologyDeveloperJsonModel = new TechnologyDeveloperJsonModel();
    }

    public class BattleFighterJsonMasterData
    {
        public BattleFighterJsonModel BattleFighterJsonModel;
    }
}
