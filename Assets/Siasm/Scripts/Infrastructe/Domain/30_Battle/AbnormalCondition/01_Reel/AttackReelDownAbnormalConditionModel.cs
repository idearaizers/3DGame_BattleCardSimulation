namespace Siasm
{
    public sealed class AttackReelDownAbnormalConditionModel : BaseAbnormalConditionModel
    {
        public AttackReelDownAbnormalConditionModel(AbnormalConditionType abnormalConditionType, BattleCardAbilityModel battleCardAbilityModel) :
            base(abnormalConditionType, battleCardAbilityModel) { }

        /// <summary>
        /// マッチ時に使用
        /// リールタイプが攻撃の場合に付与する値を返す
        /// </summary>
        /// <returns></returns>
        public override int GetAddAttackReelNumber(CardReelType cardReelType)
        {
            // 指定のカードのリールタイプであれば値を返す
            switch (cardReelType)
            {
                case CardReelType.None:
                    break;
                case CardReelType.Attack:
                    return -TotalDetailNumber;
                case CardReelType.Guard:
                    break;
                default:
                    break;
            }

            return 0;
        }
    }
}
