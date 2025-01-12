using MessagePack;
using MasterMemory;

namespace Siasm
{
    [MessagePackObject(true)]
    public class AttributeResistMasterData
    {
        public AttributeResistType NormalResist { get; set; }
        public AttributeResistType JoyResist { get; set; }
        public AttributeResistType TrustResist { get; set; }
        public AttributeResistType FearResist { get; set; }
        public AttributeResistType SurpriseResist { get; set; }
        public AttributeResistType SadnessResist { get; set; }
        public AttributeResistType DisgustResist { get; set; }
        public AttributeResistType AngerResist { get; set; }
        public AttributeResistType AnticipationResist { get; set; }
    }

    [MessagePackObject(true)]
    public class PassiveAbilityMasterData
    {
        public int ReleaseLevel { get; set; }
        public string PassiveAbilityName { get; set; }
        public PassiveAbilityType PassiveAbilityType { get; set; }
        public int MainDetailNumber { get; set; }
        public int SubDetailNumber { get; set; }
        public string DevelopmentMemo { get; set; }  // 開発用のメモ
    }

    [MessagePackObject(true)]
    public class DeckCardMasterData
    {
        public int CardId { get; set; }
        public int CardNumber { get; set; }
    }

    [MessagePackObject(true)]
    public class DeckMasterData
    {
        public int AchievementLevel { get; set; }
        public DeckCardMasterData[] DeckCardMasterDataArray { get; set; }
    }

    [MessagePackObject(true)]
    public class ArchiveMasterData
    {
        public string[] ReportFiles;
    }

    [MessagePackObject(true)]
    public class TechnologyDeveloperMasterData
    {
        public string DeveloperName { get; set; }
        public string[] ReportFiles { get; set; }
        public string DevelopmentMemo { get; set; }     // 開発用のメモ
    }

    [MessagePackObject(true)]
    public class DropItemMasterData
    {
        public int AchievementLevel { get; set; }
        public DropInventoryType DropInventoryType { get; set; }
        public int DetailNumber { get; set; }
    }

    [MemoryTable("BattleFighterMasterData"), MessagePackObject(true)]
    public sealed class BattleFighterMasterData
    {
        [PrimaryKey]
        public int Id { get; private set; }

        public string ProductName { get; private set; }
        public string ThemeNameMemo { get; private set; }   // 開発用のメモ
        public string TrueName { get; private set; }
        public string AdmissionName { get; private set; }
        public string ManagementNumber { get; private set; }
        public RiskLevelType RiskLevelType { get; private set; }
        public string DevelopmentMemo { get; private set; } // 開発用のメモ
        public AttributeResistMasterData AttributeResistMasterData { get; private set; }
        public PassiveAbilityMasterData[] PassiveAbilityMasterDataArray { get; private set; }
        public DeckMasterData[] DeckMasterDataArray { get; private set; }
        public DropItemMasterData[] DropItemMasterDataArray { get; private set; }
        public ArchiveMasterData ArchiveMasterData { get; private set; }
        public TechnologyDeveloperMasterData TechnologyDeveloperMasterData { get; private set; }

        public BattleFighterMasterData(int Id, string ProductName, string ThemeNameMemo, string TrueName, string AdmissionName, string ManagementNumber, RiskLevelType RiskLevelType, string DevelopmentMemo, AttributeResistMasterData AttributeResistMasterData, PassiveAbilityMasterData[] PassiveAbilityMasterDataArray, DeckMasterData[] DeckMasterDataArray, DropItemMasterData[] DropItemMasterDataArray, ArchiveMasterData ArchiveMasterData, TechnologyDeveloperMasterData TechnologyDeveloperMasterData)
        {
            this.Id = Id;
            this.ProductName = ProductName;
            this.ThemeNameMemo = ThemeNameMemo;
            this.TrueName = TrueName;
            this.AdmissionName = AdmissionName;
            this.ManagementNumber = ManagementNumber;
            this.RiskLevelType = RiskLevelType;
            this.DevelopmentMemo = DevelopmentMemo;
            this.AttributeResistMasterData = AttributeResistMasterData;
            this.PassiveAbilityMasterDataArray = PassiveAbilityMasterDataArray;
            this.DeckMasterDataArray = DeckMasterDataArray;
            this.DropItemMasterDataArray = DropItemMasterDataArray;
            this.ArchiveMasterData = ArchiveMasterData;
            this.TechnologyDeveloperMasterData = TechnologyDeveloperMasterData;
        }
    }
}