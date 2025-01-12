using System;
using UnityEngine;

namespace Siasm
{
    public abstract class BaseBattleFighterPrefab : MonoBehaviour
    {
        [SerializeField]
        private BattleFighterMovement battleFighterMovement;

        [SerializeField]
        private CommonBattleFighterContent commonBattleFighterContent;

        public BaseBattleFighterModel CurrentBaseBattleFighterModel { get; private set; }
        public bool IsPlayer { get; private set;}

        public Action<int, bool> OnShowMatchBattleBoxAction { get; set; }
        public Action<BattleCardModel> OnCancelBattleCardAction { get; set; }
        public Action<BaseBattleFighterModel> OnShowFighterStatusViewAction { get; set; }

        public BattleFighterMovement BattleFighterMovement => battleFighterMovement;
        public BattleFighterBoxView BattleFighterBoxView => commonBattleFighterContent.BattleFighterBoxView;
        public BattleFighterAnimation BattleFighterAnimation => commonBattleFighterContent.BattleFighterAnimation;
        public BattleFighterResidentEffectView BattleFighterResidentEffectView => commonBattleFighterContent.BattleFighterResidentEffectView;
        public BattleMatchReelView BattleMatchReelView => commonBattleFighterContent.BattleMatchReelView;

        public bool IsDead => CurrentBaseBattleFighterModel.HealthModel.CurrentPoint == 0;
        public bool IsThinkingFreeze => CurrentBaseBattleFighterModel.ThinkingModel.IsThinkingFreeze;

        public void Initialize(BaseBattleFighterModel baseBattleFighterModel, bool isPlayer, Camera mainCamera,
            BattleObjectPoolContainer battleObjectPoolContainer, BattleFighterAnimationTypeSprites battleFighterAnimationTypeSprites)
        {
            CurrentBaseBattleFighterModel = baseBattleFighterModel;
            IsPlayer = isPlayer;

            BattleFighterMovement.Initialize(isPlayer);
            commonBattleFighterContent.Initialize(isPlayer, mainCamera, battleObjectPoolContainer, battleFighterAnimationTypeSprites);
            commonBattleFighterContent.OnShowMatchBattleBoxAction = (boxIndex, isUpdate) => OnShowMatchBattleBoxAction?.Invoke(boxIndex, isUpdate);
            commonBattleFighterContent.OnCancelBattleCardAction = (battleCardModel) => OnCancelBattleCardAction?.Invoke(battleCardModel);
            commonBattleFighterContent.OnShowFighterStatusViewAction = () => OnShowFighterStatusViewAction?.Invoke(CurrentBaseBattleFighterModel);
        }

        public void Setup()
        {
            BattleFighterMovement.Setup();
            commonBattleFighterContent.Setup();
        }

        /// <summary>
        /// バトルボックスの数を変更して中身を空にリセットする
        /// </summary>
        public void ShowAndResetBattleBoxView()
        {
            BattleFighterBoxView.Enable();
            BattleFighterBoxView.ChangeBattleBoxNumber(CurrentBaseBattleFighterModel.BattleBoxModel.CurrentNumber);
            BattleFighterBoxView.ResetContentOfAllBattleBox();
        }

        public void HideBattleBoxView()
        {
            BattleFighterBoxView.Disable();
        }

        public void ShowThinkingFreezeEffect()
        {
            commonBattleFighterContent.BattleFighterResidentEffectView.ShowThinkingFreezeEffect();
        }

        /// <summary>
        /// HPとTPの両方にダメージ
        /// </summary>
        /// <param name="damageNumber"></param>
        public void ApplyDamage(int damageNumber)
        {
            CurrentBaseBattleFighterModel.ApplyHealthDamage(damageNumber);
            CurrentBaseBattleFighterModel.ApplyThinkingDamage(damageNumber);
        }

        public void ApplyHealthRecovery(int damageNumber)
        {
            CurrentBaseBattleFighterModel.ApplyHealthRecovery(damageNumber);
        }

        public void ApplyThinkingRecovery(int damageNumber)
        {
            CurrentBaseBattleFighterModel.ApplyThinkingRecovery(damageNumber);
        }
    }
}
