namespace Siasm
{
    public class ReelParameter
    {
        public BaseBattleFighterPrefab BaseBattleFighterPrefab { get; set; }
        public bool IsThinkingFreeze { get; set; }
        public BattleCardModel BattleCardModel { get; set; }
        public int RemainingBattleCardNumber { get; set; }  // 残りの攻撃バトルカードで格納時に-1している

        /// <summary>
        /// バトルカードモデルがない場合はCardReelType.Noneを返す
        /// </summary>
        /// <returns></returns>
        public CardReelType GetCardReelType()
        {
            if (BattleCardModel == null)
            {
                return CardReelType.None;
            }

            return BattleCardModel.CardReelType;
        }
    }

    /// <summary>
    /// マッチ演出で使用するパラメータをまとめたクラス
    /// </summary>
    public class StartMatchReelParameter
    {
        public ReelParameter PlayerReelParameter { get; set; }
        public ReelParameter EnemyReelParameter { get; set; }
    }
}
