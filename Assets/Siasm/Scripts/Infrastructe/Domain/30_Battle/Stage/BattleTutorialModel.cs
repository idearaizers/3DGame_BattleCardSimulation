namespace Siasm
{
    public class BattleTutorialModel
    {
        public int BattleId { get; set; }   // 検索に使用するid
        public int PlayerBattleFighterId { get; set; }
        public BattleTutorialHoldCardModel[] PlayerBattleTutorialHoldCardModels { get; set; }
        public int EnemyBattleFighterId { get; set; }
    }
}
