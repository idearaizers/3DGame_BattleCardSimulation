using System;
using System.Linq;
using UnityEngine;

namespace Siasm
{
    /// <summary>
    /// バトルカードのエフェクト効果を処理するクラス
    /// </summary>
    public class BattleCardAbilityLogic : MonoBehaviour
    {
        private BattleFigtherAbnormalConditionController battleFigtherAbnormalConditionController;
        private PlayerBattleFighterPrefab playerBattleFighter;
        private EnemyBattleFighterPrefab enemyBattleFighter;
        private PlayerBattleCardOperationController playerBattleCardOperationController;
        private EnemyBattleCardOperationController enemyBattleCardOperationController;

        public void Initialize(BattleFigtherAbnormalConditionController battleFigtherAbnormalConditionController,
            PlayerBattleCardOperationController playerBattleCardOperationController, EnemyBattleCardOperationController enemyBattleCardOperationController)
        {
            this.battleFigtherAbnormalConditionController = battleFigtherAbnormalConditionController;
            this.playerBattleCardOperationController = playerBattleCardOperationController;
            this.enemyBattleCardOperationController = enemyBattleCardOperationController;
        }

        public void Setup(PlayerBattleFighterPrefab playerBattleFighter, EnemyBattleFighterPrefab enemyBattleFighter)
        {
            this.playerBattleFighter = playerBattleFighter;
            this.enemyBattleFighter = enemyBattleFighter;
        }

        /// <summary>
        /// バトルカードの使用時の効果を実行する
        /// </summary>
        /// <param name="isPlayerInvoker"></param>
        /// <param name="cardEffectActivateType"></param>
        /// <param name="battleCardModel"></param>
        /// <param name="isSucceededOfGuard">CardEffectActivateType.Succeededの時で相手に付与する状態異常があれば付与しない</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void ExecuteBattleCardEffect(bool isPlayerInvoker, CardAbilityActivateType cardEffectActivateType, BattleCardModel battleCardModel, bool isSucceededOfGuard)
        {
            // バトルカードがなければ処理は実行しない
            if (battleCardModel == null)
            {
                return;
            }

            // バトルカードの効果を全て確認して、指定したCardAbilityActivateTypeのものを全て取得する
            var battleCardAbilityModels = battleCardModel.BattleCardAbilityModels.Where(battleCardEffectModel => battleCardEffectModel.CardAbilityActivateType == cardEffectActivateType).ToArray();
            if (battleCardAbilityModels.Length == 0)
            {
                // 指定した条件に合う効果がなければ処理は不要なので終了する
                return;
            }

            // 取得したエフェクトを全て実行する
            foreach (var battleCardAbilityModel in battleCardAbilityModels)
            {
                // ターゲット先が相手で且つ、ガード成功時は付与しないため次の処理を実行する
                if (battleCardAbilityModel.CardAbilityTargetType == CardAbilityTargetType.Your &&
                    isSucceededOfGuard)
                {
                    continue;
                }

                // カードエフェクトを実行するターゲット先を取得
                var targetBaseBattleFighterPrefab = GetBaseBattleFighterPrefabOfTarget(isPlayerInvoker, battleCardAbilityModel.CardAbilityTargetType);

                // 付与するものと即時発動するもので処理を分ける
                // 一旦、文言で出し分けにする
                // 将来的にはenumで出し分けがいいかも
                if (battleCardAbilityModel.CardAbilityType.ToString().Contains("Add"))
                {
                    // ターゲット先に状態異常を付与する
                    battleFigtherAbnormalConditionController.AddAbnormalCondition(targetBaseBattleFighterPrefab, battleCardAbilityModel);
                }
                else
                {
                    ExecuteImmediate(targetBaseBattleFighterPrefab, battleCardAbilityModel);
                }
            }
        }

        /// <summary>
        /// カードエフェクトを実行するターゲット先を取得
        /// </summary>
        /// <param name="isPlayerInvoker"></param>
        /// <param name="cardEffectTargetType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private BaseBattleFighterPrefab GetBaseBattleFighterPrefabOfTarget(bool isPlayerInvoker, CardAbilityTargetType cardEffectTargetType)
        {
            BaseBattleFighterPrefab baseBattleFighter;
            switch (cardEffectTargetType)
            {
                case CardAbilityTargetType.Self:
                    baseBattleFighter = isPlayerInvoker
                        ? playerBattleFighter
                        : enemyBattleFighter;
                    break;

                case CardAbilityTargetType.Your:
                    baseBattleFighter = isPlayerInvoker
                        ? enemyBattleFighter
                        : playerBattleFighter;
                    break;

                case CardAbilityTargetType.None:
                default:
                    throw new ArgumentOutOfRangeException(nameof(cardEffectTargetType));
            }

            return baseBattleFighter;
        }

        /// <summary>
        /// NOTE: 将来的にクラスに処理を分離した方が拡張がしやすいかも
        /// </summary>
        /// <param name="targetBaseBattleFighterPrefab"></param>
        /// <param name="battleCardAbilityModel"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void ExecuteImmediate(BaseBattleFighterPrefab targetBaseBattleFighterPrefab, BattleCardAbilityModel battleCardAbilityModel)
        {
            switch (battleCardAbilityModel.CardAbilityType)
            {
                // 即時発動系（3001）関連
                case CardAbilityType.ImmediateDeckReload:
                    {
                        BaseBattleCardOperationController baseBattleCardOperationController = targetBaseBattleFighterPrefab is PlayerBattleFighterPrefab
                            ? playerBattleCardOperationController
                            : enemyBattleCardOperationController;

                        // NOTE: 墓地のカードをどこかで見れる機能があると便利かも
                        baseBattleCardOperationController.DeckReload();
                    }
                    break;

                case CardAbilityType.ImmediateHandDraw:
                    {
                        BaseBattleCardOperationController baseBattleCardOperationController = targetBaseBattleFighterPrefab is PlayerBattleFighterPrefab
                            ? playerBattleCardOperationController
                            : enemyBattleCardOperationController;

                        baseBattleCardOperationController.DrawHandCard();
                    }
                    break;

                case CardAbilityType.ImmediateRecoveryHealthPoint:
                    {
                        targetBaseBattleFighterPrefab.ApplyHealthRecovery(battleCardAbilityModel.DetailNumber);
                    }
                    break;

                case CardAbilityType.ImmediateRecoveryThinkingPoint:
                    {
                        targetBaseBattleFighterPrefab.ApplyThinkingRecovery(battleCardAbilityModel.DetailNumber);
                    }
                    break;

                // リール（1001）関連
                // 状態異常（2001）関連
                // はここでは管理しないのでエラー処理を追加
                default:
                    throw new ArgumentOutOfRangeException(nameof(battleCardAbilityModel.CardAbilityType));
            }
        }
    }
}
