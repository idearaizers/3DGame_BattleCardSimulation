using System;
using System.Linq;
using UnityEngine;

namespace Siasm
{
    /// <summary>
    /// 思考停止状態と状態異常（バフ/デバフ）を管理するクラス
    /// </summary>
    public class BattleFigtherAbnormalConditionController : MonoBehaviour
    {
        private PlayerBattleFighterPrefab playerBattleFighterPrefab;
        private EnemyBattleFighterPrefab enemyBattleFighterPrefab;

        public void Initialize() { }
 
        public void Setup(PlayerBattleFighterPrefab playerBattleFighterPrefab, EnemyBattleFighterPrefab enemyBattleFighterPrefab)
        {
            this.playerBattleFighterPrefab = playerBattleFighterPrefab;
            this.enemyBattleFighterPrefab = enemyBattleFighterPrefab;
        }

        /// <summary>
        /// 戦闘終了後に実行する処理
        /// 主にターン終了時にダメージを受けるなどで使用
        /// </summary>
        public void ExecuteCombatEnd()
        {
            // TODO: 死んでいた場合はスキップさせた方がいいかも

            foreach (var baseStatusConditionModel in playerBattleFighterPrefab.CurrentBaseBattleFighterModel.BaseAbnormalConditionModels)
            {
                baseStatusConditionModel.ExecuteCombatEnd();
            }

            foreach (var baseStatusConditionModel in enemyBattleFighterPrefab.CurrentBaseBattleFighterModel.BaseAbnormalConditionModels)
            {
                baseStatusConditionModel.ExecuteCombatEnd();
            }
        }

        /// <summary>
        /// 思考停止状態のターンを経過させ必要なら自然回復する
        /// </summary>
        public void ElapsedThinkingFreezeOfAllFighter()
        {
            ElapsedThinkingFreeze(playerBattleFighterPrefab);
            ElapsedThinkingFreeze(enemyBattleFighterPrefab);
        }

        /// <summary>
        /// 思考停止状態のターンを経過させ必要なら回復する
        /// </summary>
        public void ElapsedThinkingFreeze(BaseBattleFighterPrefab baseBattleFighterPrefab)
        {
            // 指定の状態異常でなければ処理を終了
            if (baseBattleFighterPrefab.CurrentBaseBattleFighterModel.ThinkingModel.IsThinkingFreeze == false)
            {
                return;
            }

            // 0ターン目であればターンを経過させる
            if (baseBattleFighterPrefab.CurrentBaseBattleFighterModel.ThinkingModel.ElapsedTurn == 0)
            {
                baseBattleFighterPrefab.CurrentBaseBattleFighterModel.ThinkingModel.ElapsedTurn++;
            }
            // 1ターン経過したので回復を実行
            else
            {
                // モデルクラスを更新
                baseBattleFighterPrefab.CurrentBaseBattleFighterModel.ThinkingModel.MaxRecovery();

                // 見た目を更新
                baseBattleFighterPrefab.BattleFighterResidentEffectView.HideShowThinkingFreezeEffect();
            }
        }

        /// <summary>
        /// 状態異常のターンを経過させ必要なら自然回復する
        /// </summary>
        public void ElapsedAbnormalConditionOfAllFighter()
        {
            // モデルクラスを更新
            ElapsedAbnormalCondition(playerBattleFighterPrefab);
            ElapsedAbnormalCondition(enemyBattleFighterPrefab);

            // 見た目を更新
            // NOTE: 別の場所で実行しているみたい
        }

        /// <summary>
        /// 思考停止状態のターンを経過させ必要なら回復する
        /// 効果を重ね掛けした際に経過ターンを初期化しているのでここでは経過しているかかどうかで実行
        /// </summary>
        public void ElapsedAbnormalCondition(BaseBattleFighterPrefab baseBattleFighterPrefab)
        {
            // 先にターン経過しているものを取り除く
            baseBattleFighterPrefab.CurrentBaseBattleFighterModel.BaseAbnormalConditionModels
                .RemoveAll(baseAbnormalConditionModel => baseAbnormalConditionModel.ElapsedTurn != 0);

            // ターンを経過させる
            foreach (var baseAbnormalConditionModel in baseBattleFighterPrefab.CurrentBaseBattleFighterModel.BaseAbnormalConditionModels)
            {
                baseAbnormalConditionModel.IncreaseElapsedTurn();
            }
        }

        /// <summary>
        /// 状態異常を付与する
        /// モデルクラスを更新する
        /// </summary>
        /// <param name="battleCardAbilityModel"></param>
        /// <param name="baseBattleFighterPrefab"></param>
        public void AddAbnormalCondition(BaseBattleFighterPrefab baseBattleFighterPrefab, BattleCardAbilityModel battleCardAbilityModel)
        {
            // 状態異常のタイプを取得
            var abnormalConditionType = BattleConstant.AbnormalConditionTypeDictionary[battleCardAbilityModel.CardAbilityType];

            // 取得した状態異常のタイプが既に付与されていた場合は値を更新する
            var hasBattleFighterConditionModel = baseBattleFighterPrefab.CurrentBaseBattleFighterModel.BaseAbnormalConditionModels
                .FirstOrDefault(baseAbnormalConditionModel => baseAbnormalConditionModel.AbnormalConditionType == abnormalConditionType);

            if (hasBattleFighterConditionModel == null)
            {
                // 新しく付与する
                var battleFighterConditionModel = GetBaseAbnormalConditionModel(battleCardAbilityModel);
                baseBattleFighterPrefab.CurrentBaseBattleFighterModel.BaseAbnormalConditionModels.Add(battleFighterConditionModel);
            }
            else
            {
                // 付与されているので値を更新する
                hasBattleFighterConditionModel.IncreaseTotalDetailNumber(battleCardAbilityModel.DetailNumber);
                hasBattleFighterConditionModel.ResetElapsedTurn();
            }
        }

        private BaseAbnormalConditionModel GetBaseAbnormalConditionModel(BattleCardAbilityModel battleCardAbilityModel)
        {
            BaseAbnormalConditionModel baseAbnormalConditionModel = null;

            // 生成したクラス側に即時か付与かの値を設定した方がよさそうやね
            switch (battleCardAbilityModel.CardAbilityType)
            {
                // リール（1001）関連
                case CardAbilityType.AddAttackReelUp:
                    baseAbnormalConditionModel = new AttackReelUpAbnormalConditionModel(
                        BattleConstant.AbnormalConditionTypeDictionary[CardAbilityType.AddAttackReelUp],
                        battleCardAbilityModel
                    );
                    break;
                case CardAbilityType.AddAttackReelDown:
                    baseAbnormalConditionModel = new AttackReelDownAbnormalConditionModel(
                        BattleConstant.AbnormalConditionTypeDictionary[CardAbilityType.AddAttackReelDown],
                        battleCardAbilityModel
                    );
                    break;
                case CardAbilityType.AddGuardReelUp:
                    baseAbnormalConditionModel = new GuardReelUpAbnormalConditionModel(
                        BattleConstant.AbnormalConditionTypeDictionary[CardAbilityType.AddGuardReelUp],
                        battleCardAbilityModel
                    );
                    break;
                case CardAbilityType.AddGuardReelDown:
                    baseAbnormalConditionModel = new GuardReelDownAbnormalConditionModel(
                        BattleConstant.AbnormalConditionTypeDictionary[CardAbilityType.AddGuardReelDown],
                        battleCardAbilityModel
                    );
                    break;

                // 状態異常（2001）関連

                // この処理では状態異常の付与になるので下記の即時関連は別で処理にする
                // 即時発動系（3001）関連
                default:
                case CardAbilityType.None:
                case CardAbilityType.ImmediateDeckReload:
                case CardAbilityType.ImmediateHandDraw:
                case CardAbilityType.ImmediateRecoveryHealthPoint:
                case CardAbilityType.ImmediateRecoveryThinkingPoint:
                // case CardAbilityType.ImmediateTakeDamageUp:
                // case CardAbilityType.ImmediateTakeDamageDwon:
                    throw new ArgumentOutOfRangeException(nameof(battleCardAbilityModel.CardAbilityType));
            }


            return baseAbnormalConditionModel;
        }
    }
}
