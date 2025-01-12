namespace Siasm
{
    public sealed class EnemyBattleFighterModel : BaseBattleFighterModel
    {
        public int FighterLevel { get; set; }
        public BattleDeckModel BattleDeckModel { get; set; }
        public BattleFighterMessageModel[] BattleFighterMessageModels { get; set; }
    }
}
