namespace Siasm
{
    public class BattleCardAbilityModel
    {
        public CardAbilityActivateType CardAbilityActivateType { get; set; }    // 効果が発動するタイミング
        public CardAbilityTargetType CardAbilityTargetType { get; set; }        // 効果の対象先
        public CardAbilityType CardAbilityType { get; set; }                    // 付与するエフェクトの種類
        public int DetailNumber { get; set; }                                   // 設定値
    }
}
