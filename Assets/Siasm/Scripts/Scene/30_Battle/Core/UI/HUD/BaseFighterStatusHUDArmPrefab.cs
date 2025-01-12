using System;
using UnityEngine;

namespace Siasm
{
    public abstract class BaseFighterStatusHUDArmPrefab : BaseHUDArmPrefab
    {
        protected override string ShowStateName => "CommonBattleStatusHUDArmPrefab_Show";
        protected override string OnCursorStateName => "CommonBattleStatusHUDArmPrefab_OnCursor";

        [Header("Status関連")]
        [SerializeField]
        private CommonFighterStatusHUDView commonFighterStatusHUDView;

        public Action<bool> OnFighterStatusAction { get; set; }

        public void Initialize(bool isPlayer, Camera uiCamera, ButtonOrientationType buttonOrientationType, BattleObjectPoolContainer battleObjectPoolContainer)
        {
            base.Initialize(uiCamera, buttonOrientationType);

            OnClickAction = () => OnFighterStatusAction?.Invoke(isPlayer);
            commonFighterStatusHUDView.Initialize(uiCamera, battleObjectPoolContainer);
        }

        public void Setup(BaseBattleFighterModel baseBattleFighterModel)
        {
            commonFighterStatusHUDView.Setup(baseBattleFighterModel);
        }

        /// <summary>
        /// アニメーションして表示する
        /// </summary>
        /// <param name="baseBattleFighterModel">nullの場合は表示内容を更新しない</param>
        public void PlayShowAnimation(BaseBattleFighterModel baseBattleFighterModel)
        {
            if (baseBattleFighterModel != null)
            {
                commonFighterStatusHUDView.Apply(baseBattleFighterModel);
            }

            base.PlayShowAnimation();
        }
    }
}
