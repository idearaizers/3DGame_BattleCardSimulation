#if UNITY_EDITOR

using System.Collections.Generic;

namespace Siasm
{
    public class AttributeResistEditModel
    {
        public AttributeResistType NormalResist { get; set; } = AttributeResistType.Normal;
        public AttributeResistType JoyResist { get; set; } = AttributeResistType.Normal;
        public AttributeResistType TrustResist { get; set; } = AttributeResistType.Normal;
        public AttributeResistType FearResist { get; set; } = AttributeResistType.Normal;
        public AttributeResistType SurpriseResist { get; set; } = AttributeResistType.Normal;
        public AttributeResistType SadnessResist { get; set; } = AttributeResistType.Normal;
        public AttributeResistType DisgustResist { get; set; } = AttributeResistType.Normal;
        public AttributeResistType AngerResist { get; set; } = AttributeResistType.Normal;
        public AttributeResistType AnticipationResist { get; set; } = AttributeResistType.Normal;
    }

    public class PassiveAbilityEditModel
    {
        public int ReleaseLevel { get; set; }
        public string PassiveAbilityName { get; set; }
        public PassiveAbilityType PassiveAbilityType { get; set; }
        public int MainDetailNumber { get; set; }
        public int SubDetailNumber { get; set; }
        public string DevelopmentMemo { get; set; }  // 開発用のメモ
    }

    public class DeckCardEditModel
    {
        public int CardId { get; set; }
        public int CardNumber { get; set; }
    }

    public class DeckEditModel
    {
        public int AchievementLevel { get; set; }
        public List<DeckCardEditModel> DeckCardEditModels { get; set; } = new List<DeckCardEditModel>();
    }

    public class ArchiveEditModel
    {
        public List<string> ReportFiles { get; set; } = new List<string>();
    }

    public class TechnologyDeveloperEditModel
    {
        public string DeveloperName { get; set; }
        public List<string> ReportFiles { get; set; } = new List<string>();
        public string DevelopmentMemo { get; set; }  // 開発用のメモ
    }

    public class DropItemEditModel
    {
        public int AchievementLevel { get; set; }
        public DropInventoryType DropInventoryType { get; set; }
        public int DetailNumber { get; set; }
    }

    public class BattleFighterEditModel
    {
        public string ProductName { get; set; }
        public string ThemeNameMemo { get; set; }   // 開発用のメモ
        public string TrueName { get; set; }
        public string AdmissionName { get; set; }
        public string ManagementNumber { get; set; } = "1-12-123-1234-1";
        public RiskLevelType RiskLevelType { get; set; } = RiskLevelType.Dubhena;
        public string DevelopmentMemo { get; set; }  // 開発用のメモ
        public AttributeResistEditModel AttributeResistEditModel { get; set; } = new AttributeResistEditModel();
        public List<PassiveAbilityEditModel> PassiveAbilityEditModels { get; set; } = new List<PassiveAbilityEditModel>();
        public List<DeckEditModel> DeckEditModels { get; set; } = new List<DeckEditModel>();
        public List<DropItemEditModel> DropItemEditModels { get; set; } = new List<DropItemEditModel>();
        public ArchiveEditModel ArchiveEditModel { get; set; } = new ArchiveEditModel();
        public TechnologyDeveloperEditModel TechnologyDeveloperEditModel { get; set; } = new TechnologyDeveloperEditModel();
    }
}

#endif
