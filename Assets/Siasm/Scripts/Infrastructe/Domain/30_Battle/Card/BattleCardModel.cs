using UnityEngine;
using UnityEngine.UI;

namespace Siasm
{
    /// <summary>
    /// デバッグモードの際に値が見れるようにSerializableを設定
    /// </summary>
    [System.Serializable]
    public class BattleCardModel
    {
        public int CardId { get; set; }
        public string CardName { get; set; }
        public CardReelType CardReelType { get; set; }
        public int MinReelNumber { get; set; }
        public int MaxReelNumber { get; set; }
        public EmotionAttributeType EmotionAttributeType { get; set; }
        public BattleCardAbilityModel[] BattleCardAbilityModels { get; set; }

        // 頻繁に変更するのでコンストラクタを見直した方がいいかも
        public CardPlaceType CardPlaceType { get; set; }

        // 整理してよさそうなもの
        public Image CardImage { get; set; }
        public CardSpecType CardSpecType { get; set; }
        public int CostNumber { get; set; }
        public string DescriptionText { get; set; }

        public BattleCardModel Clone()
        {
            return (BattleCardModel)MemberwiseClone();
        }

        public int GetRandomReelNumber()
        {
            return Random.Range(MinReelNumber, MaxReelNumber);
        }

        public void SetCardPlaceType(CardPlaceType cardPlaceType)
        {
            CardPlaceType = cardPlaceType;
        }
    }
}
