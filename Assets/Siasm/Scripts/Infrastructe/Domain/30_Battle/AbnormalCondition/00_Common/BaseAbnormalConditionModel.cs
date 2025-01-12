namespace Siasm
{
    /// <summary>
    /// 状態異常
    /// </summary>
    public abstract class BaseAbnormalConditionModel
    {
        /// <summary>
        /// 主にCardAbilityTypeで付与する際に中身を指定
        /// </summary>
        public AbnormalConditionType AbnormalConditionType { get; private set; }

        /// <summary>
        /// 現在の累積している効果の値
        /// </summary>
        public int TotalDetailNumber { get; private set; }

        public int ElapsedTurn { get; private set; }

        public BaseAbnormalConditionModel(AbnormalConditionType abnormalConditionType, BattleCardAbilityModel battleCardAbilityModel)
        {
            AbnormalConditionType = abnormalConditionType;
            TotalDetailNumber = battleCardAbilityModel.DetailNumber;
            ElapsedTurn = 0;
        }

        public void IncreaseTotalDetailNumber(int addNumber)
        {
            TotalDetailNumber += addNumber;
        }

        public void IncreaseElapsedTurn()
        {
            ElapsedTurn++;
        }

        public void ResetElapsedTurn()
        {
            ElapsedTurn = 0;
        }

        /// <summary>
        /// 戦闘終了後に実行する処理
        /// 主にターン終了時にダメージを受けるなどで使用
        /// </summary>
        public virtual void ExecuteCombatEnd() { }

        /// <summary>
        /// マッチ時に使用
        /// リールタイプが攻撃の場合に付与する値を返す
        /// </summary>
        /// <returns></returns>
        public virtual int GetAddAttackReelNumber(CardReelType cardReelType)
        {
            return 0;
        }

        /// <summary>
        /// マッチ時に使用
        /// リールタイプがガードの場合に付与する値を返す
        /// </summary>
        /// <returns></returns>
        public virtual int GetAddGuardReelNumber(CardReelType cardReelType)
        {
            return 0;
        }
    }
}
