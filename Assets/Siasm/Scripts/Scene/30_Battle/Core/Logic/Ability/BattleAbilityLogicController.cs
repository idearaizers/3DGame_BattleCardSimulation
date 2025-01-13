using System;
using UnityEngine;

namespace Siasm
{
    /// <summary>
    /// カードやパッシブのアビリティ効果を管理するクラス
    /// </summary>
    public class BattleAbilityLogicController : MonoBehaviour
    {
        [SerializeField]
        private BattlePassiveAbilityLogic battlePassiveAbilityLogic;

        [SerializeField]
        private BattleCardAbilityLogic battleCardAbilityLogic;

        public BattlePassiveAbilityLogic BattlePassiveAbilityLogic => battlePassiveAbilityLogic;

        public void Initialize(BattleFigtherAbnormalConditionController battleFigtherConditionController,
            PlayerBattleCardOperationController playerBattleCardOperationController, EnemyBattleCardOperationController enemyBattleCardOperationControllera, BattleUseCase battleUseCase)
        {
            battlePassiveAbilityLogic.Initialize(battleUseCase);
            battleCardAbilityLogic.Initialize(battleFigtherConditionController, playerBattleCardOperationController, enemyBattleCardOperationControllera);
        }

        public void Setup(PlayerBattleFighterPrefab playerBattleFighter, EnemyBattleFighterPrefab enemyBattleFighter)
        {
            battlePassiveAbilityLogic.Setup(playerBattleFighter, enemyBattleFighter);
            battleCardAbilityLogic.Setup(playerBattleFighter, enemyBattleFighter);
        }

        /// <summary>
        /// リール値を変更するアビリティがあるのか確認
        /// </summary>
        /// <param name="battleCardModel"></param>
        /// <param name="reelNumber"></param>
        public int GetAddReelNumber(BattleCardModel battleCardModel, int reelNumber)
        {
            // バトルカードを設定していない場合は0を返す
            if (battleCardModel == null)
            {
                return 0;
            }

            int sumNumber = 0;

            foreach (var battleCardAbilityModel in battleCardModel.BattleCardAbilityModels)
            {
                switch (battleCardAbilityModel.CardAbilityType)
                {
                    case CardAbilityType.ReelMaxNumberUp:
                        {
                            // リール値が最大か確認して値を増加させる
                            if (battleCardModel.MaxReelNumber == reelNumber)
                            {
                                sumNumber += battleCardAbilityModel.DetailNumber;
                            }
                        }
                        break;
                    case CardAbilityType.ReelMixNumberUp:
                        {
                            // リール値が最低か確認して値を増加させる
                            if (battleCardModel.MinReelNumber == reelNumber)
                            {
                                sumNumber += battleCardAbilityModel.DetailNumber;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }

            return sumNumber;
        }

        /// <summary>
        /// 成功側と失敗側のバトルカードを取得してダメージの増加を計算する
        /// </summary>
        /// <param name="damageNumber"></param>
        /// <param name="startMatchReelParameter"></param>
        /// <param name="sourceBattleFighterPrefab"></param>
        /// <param name="targetBattleFighterPrefab"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public (int, AttributeResistType) GetAddDamageNumberOfEmotionAttributeType(int damageNumber, StartMatchReelParameter startMatchReelParameter, BaseBattleFighterPrefab sourceBattleFighterPrefab, BaseBattleFighterPrefab targetBattleFighterPrefab)
        {
            var battleCardModel = sourceBattleFighterPrefab is PlayerBattleFighterPrefab
                ? startMatchReelParameter.PlayerReelParameter.BattleCardModel
                : startMatchReelParameter.EnemyReelParameter.BattleCardModel;

            // TODO: Dictionaryでの管理に変更予定
            AttributeResistType attributeResistType;
            switch (battleCardModel.EmotionAttributeType)
            {
                case EmotionAttributeType.Normal:
                    attributeResistType = targetBattleFighterPrefab.CurrentBaseBattleFighterModel.AttributeResistModel.NormalResist;
                    break;
                case EmotionAttributeType.Joy:
                    attributeResistType = targetBattleFighterPrefab.CurrentBaseBattleFighterModel.AttributeResistModel.JoyResist;
                    break;
                case EmotionAttributeType.Trust:
                    attributeResistType = targetBattleFighterPrefab.CurrentBaseBattleFighterModel.AttributeResistModel.TrustResist;
                    break;
                case EmotionAttributeType.Fear:
                    attributeResistType = targetBattleFighterPrefab.CurrentBaseBattleFighterModel.AttributeResistModel.FearResist;
                    break;
                case EmotionAttributeType.Surprise:
                    attributeResistType = targetBattleFighterPrefab.CurrentBaseBattleFighterModel.AttributeResistModel.SurpriseResist;
                    break;
                case EmotionAttributeType.Sadness:
                    attributeResistType = targetBattleFighterPrefab.CurrentBaseBattleFighterModel.AttributeResistModel.SadnessResist;
                    break;
                case EmotionAttributeType.Disgust:
                    attributeResistType = targetBattleFighterPrefab.CurrentBaseBattleFighterModel.AttributeResistModel.DisgustResist;
                    break;
                case EmotionAttributeType.Anger:
                    attributeResistType = targetBattleFighterPrefab.CurrentBaseBattleFighterModel.AttributeResistModel.AngerResist;
                    break;
                case EmotionAttributeType.Anticipation:
                    attributeResistType = targetBattleFighterPrefab.CurrentBaseBattleFighterModel.AttributeResistModel.AnticipationResist;
                    break;
                case EmotionAttributeType.None:
                default:
                    throw new ArgumentOutOfRangeException(nameof(battleCardModel.EmotionAttributeType));
            }

            // TODO: Dictionaryでの管理に変更予定
            var resultNumber = 0; 
            switch (attributeResistType)
            {
                case AttributeResistType.Immune:
                    resultNumber = damageNumber - 8;
                    break;
                case AttributeResistType.Resist:
                    resultNumber = damageNumber - 4;
                    break;
                case AttributeResistType.Endure:
                    resultNumber = damageNumber - 2;
                    break;
                case AttributeResistType.Normal:
                    resultNumber = damageNumber - 0;
                    break;
                case AttributeResistType.Weak:
                    resultNumber = damageNumber + 2;
                    break;
                case AttributeResistType.Vulnerable:
                    resultNumber = damageNumber + 4;
                    break;
                case AttributeResistType.Feeble:
                    resultNumber = damageNumber + 8;
                    break;
                case AttributeResistType.None:
                default:
                    throw new ArgumentOutOfRangeException(nameof(attributeResistType));
            }

            // 0より小さい場合は0にする
            if (resultNumber < 0)
            {
                resultNumber = 0;
            }

            return (resultNumber, attributeResistType);
        }

        /// <summary>
        /// 成功側と失敗側のバトルカードを取得してダメージの増加を計算する
        /// </summary>
        /// <param name="startMatchReelParameter"></param>
        /// <param name="sourceBattleFighterPrefab"></param>
        /// <param name="targetBattleFighterPrefab"></param>
        public void AttackAndGuard(StartMatchReelParameter startMatchReelParameter, BaseBattleFighterPrefab sourceBattleFighterPrefab, BaseBattleFighterPrefab targetBattleFighterPrefab)
        {
            // TODO: 
        }

        /// <summary>
        /// プレイヤーのバトルカードの使用時の効果を実行する
        /// </summary>
        /// <param name="cardAbilityActivateType"></param>
        /// <param name="battleCardModel"></param>
        public void ExecuteBattleCardEffectOfPlayerInvoker(CardAbilityActivateType cardAbilityActivateType, BattleCardModel battleCardModel)
        {
            battleCardAbilityLogic.ExecuteBattleCardEffect(
                isPlayerInvoker: true,
                cardAbilityActivateType,
                battleCardModel,
                isSucceededOfGuard: false
            );
        }

        /// <summary>
        /// プレイヤーのバトルカードの使用時の効果を実行する
        /// プレイヤーの攻撃でエネミーはガードを成功させたため、相手（エネミー）に状態異常は付与されない
        /// </summary>
        /// <param name="cardAbilityActivateType"></param>
        /// <param name="battleCardModel"></param>
        public void ExecuteBattleCardEffectOfPlayerInvokerToGuardSucceeded(CardAbilityActivateType cardAbilityActivateType, BattleCardModel battleCardModel)
        {
            battleCardAbilityLogic.ExecuteBattleCardEffect(
                isPlayerInvoker: true,
                cardAbilityActivateType,
                battleCardModel,
                isSucceededOfGuard: true
            );
        }

        /// <summary>
        /// エネミーのバトルカードの使用時の効果を実行する
        /// エネミーの攻撃でプレイヤーはガードを成功させた
        /// </summary>
        /// <param name="cardAbilityActivateType"></param>
        /// <param name="battleCardModel"></param>
        public void ExecuteBattleCardEffectOfEnemyInvoker(CardAbilityActivateType cardAbilityActivateType, BattleCardModel battleCardModel)
        {
            battleCardAbilityLogic.ExecuteBattleCardEffect(
                isPlayerInvoker: false,
                cardAbilityActivateType,
                battleCardModel,
                isSucceededOfGuard: false
            );
        }

        /// <summary>
        /// エネミーのバトルカードの使用時の効果を実行する
        /// エネミーの攻撃でプレイヤーはガードを成功させたため、相手（プレイヤー）に状態異常は付与されない
        /// </summary>
        /// <param name="cardAbilityActivateType"></param>
        /// <param name="battleCardModel"></param>
        public void ExecuteBattleCardEffectOfEnemyInvokerToGuardSucceeded(CardAbilityActivateType cardAbilityActivateType, BattleCardModel battleCardModel)
        {
            battleCardAbilityLogic.ExecuteBattleCardEffect(
                isPlayerInvoker: false,
                cardAbilityActivateType,
                battleCardModel,
                isSucceededOfGuard: true
            );
        }
    }
}
