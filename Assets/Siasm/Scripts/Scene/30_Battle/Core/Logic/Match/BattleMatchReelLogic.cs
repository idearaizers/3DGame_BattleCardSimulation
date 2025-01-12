using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Siasm
{
    /// <summary>
    /// マッチ時のリール値を管理するクラス
    /// </summary>
    public class BattleMatchReelLogic : MonoBehaviour
    {
        private BattleConfigDebug battleConfigDebug;

        public void Initialize(BattleConfigDebug battleConfigDebug)
        {
            this.battleConfigDebug = battleConfigDebug;
        }

        public void Setup() { }

        /// <summary>
        /// 思考停止の場合はリール値が最低になる
        /// バトルカードが設定されていない場合は-1を返す
        /// </summary>
        /// <param name="reelParameter"></param>
        /// <returns></returns>
        public int GetReelNumber(ReelParameter reelParameter)
        {
            // カードが設定されていなければ-1を返す
            if (reelParameter.BattleCardModel == null)
            {
                return -1;
            }

            // 思考停止の場合はリール値が最低になる
            var randomReelNumber = reelParameter.IsThinkingFreeze
                ? reelParameter.BattleCardModel.MinReelNumber
                : reelParameter.BattleCardModel.GetRandomReelNumber();

            // カードが設定されていて且つ、デバッグが有効であれば指定の値を返す
            var debugReelNumber = battleConfigDebug.GetReelNumber(reelParameter.BaseBattleFighterPrefab);
            if (debugReelNumber != -1)
            {
                // デバッグ用の値に変える
                randomReelNumber = debugReelNumber;
            }

            // 状態異常の値を追加する
            var addNumberofAbnormalCondition = GetAddNumberOfAbnormalCondition(
                reelParameter.BattleCardModel,
                reelParameter.BaseBattleFighterPrefab.CurrentBaseBattleFighterModel.BaseAbnormalConditionModels
            );

            // TODO: 必要ならパッシブのものも追加する

            return Mathf.Clamp(
                randomReelNumber + addNumberofAbnormalCondition,
                BattleConstant.LimitMinReelNumber,
                BattleConstant.LimitMaxReelNumber
            );
        }

        /// <summary>
        /// 指定したリール値に対応した補正値を取得する
        /// </summary>
        /// <param name="battleCardModel"></param>
        /// <param name="bseAbnormalConditionModel"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private int GetAddNumberOfAbnormalCondition(BattleCardModel battleCardModel, List<BaseAbnormalConditionModel> bseAbnormalConditionModel)
        {
            return bseAbnormalConditionModel
                .Select(bseAbnormalConditionMode => bseAbnormalConditionMode.GetAddAttackReelNumber(battleCardModel.CardReelType))
                .Sum();
        }
    }
}
