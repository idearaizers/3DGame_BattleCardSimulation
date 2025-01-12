using MessagePack;
using MasterMemory;

namespace Siasm
{
    [MessagePackObject(true)]
    public class BattleCardAbilityMasterData
    {
        public CardAbilityActivateType CardAbilityActivateType { get; set; }
        public CardAbilityTargetType CardAbilityTargetType { get; set; }
        public CardAbilityType CardAbilityType { get; set; }
        public int DetailNumber { get; set; }
    }

    [MemoryTable("BattleCardMasterData"), MessagePackObject(true)]
    public class BattleCardMasterData
    {
        [PrimaryKey]
        public int Id { get; private set; }

        public string CardName { get; private set; }
        public CardReelType CardReelType { get; private set; }
        public int MinReelNumber { get; private set; }
        public int MaxReelNumber { get; private set; }
        public string FlavorText { get; private set; }
        public EmotionAttributeType EmotionAttributeType { get; private set; }
        public BattleCardAbilityMasterData[] BattleCardAbilityMasterDataArray { get; private set; }
        public string DevelopmentMemo { get; private set; }  // 開発用のメモ

        public BattleCardMasterData(int Id, string CardName, CardReelType CardReelType, int MinReelNumber, int MaxReelNumber, string FlavorText, EmotionAttributeType EmotionAttributeType, BattleCardAbilityMasterData[] BattleCardAbilityMasterDataArray, string DevelopmentMemo)
        {
            this.Id = Id;
            this.CardName = CardName;
            this.CardReelType = CardReelType;
            this.MinReelNumber = MinReelNumber;
            this.MaxReelNumber = MaxReelNumber;
            this.FlavorText = FlavorText;
            this.EmotionAttributeType = EmotionAttributeType;
            this.BattleCardAbilityMasterDataArray = BattleCardAbilityMasterDataArray;
            this.DevelopmentMemo = DevelopmentMemo;
        }
    }
}
