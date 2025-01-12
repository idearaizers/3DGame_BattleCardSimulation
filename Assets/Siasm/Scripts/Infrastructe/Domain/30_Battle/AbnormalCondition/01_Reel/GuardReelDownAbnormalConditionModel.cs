namespace Siasm
{
    public sealed class GuardReelDownAbnormalConditionModel : BaseAbnormalConditionModel
    {
        public GuardReelDownAbnormalConditionModel(AbnormalConditionType abnormalConditionType, BattleCardAbilityModel battleCardAbilityModel) :
            base(abnormalConditionType, battleCardAbilityModel) { }

        /// <summary>
        /// マッチ時に使用
        /// リールタイプがガードの場合に付与する値を返す
        /// </summary>
        /// <returns></returns>
        public override int GetAddGuardReelNumber(CardReelType cardReelType)
        {
            // 指定のカードのリールタイプであれば値を返す
            switch (cardReelType)
            {
                case CardReelType.None:
                    break;
                case CardReelType.Attack:
                    break;
                case CardReelType.Guard:
                    return -TotalDetailNumber;
                default:
                    break;
            }

            return 0;
        }
    }
}
