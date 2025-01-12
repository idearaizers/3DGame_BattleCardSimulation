using System;
using UnityEngine;

namespace Siasm
{
    public class CommonBattleFighterContent : MonoBehaviour
    {
        [SerializeField]
        private Canvas canvas;

        [SerializeField]
        private BattleFighterFallingShadow battleFighterFallingShadow;

        [SerializeField]
        private BattleFighterAnimation battleFighterAnimation;

        [SerializeField]
        private BattleFighterBoxView battleFighterBoxView;

        [SerializeField]
        private BattleFighterClickCollider battleFighterClickCollider;

        [SerializeField]
        private BattleFighterResidentEffectView battleFighterResidentEffectView;

        [SerializeField]
        private BattleMatchReelView battleMatchReelView;

        public Action<int, bool> OnShowMatchBattleBoxAction { get; set; }
        public Action<BattleCardModel> OnCancelBattleCardAction { get; set; }
        public Action OnShowFighterStatusViewAction { get; set; }

        public BattleFighterBoxView BattleFighterBoxView => battleFighterBoxView;
        public BattleFighterAnimation BattleFighterAnimation => battleFighterAnimation;
        public BattleFighterResidentEffectView BattleFighterResidentEffectView => battleFighterResidentEffectView;
        public BattleMatchReelView BattleMatchReelView => battleMatchReelView;

        public void Initialize(bool isPlayer, Camera mainCamera, BattleObjectPoolContainer battleObjectPoolContainer,
            BattleFighterAnimationTypeSprites battleFighterAnimationTypeSprites)
        {
            canvas.worldCamera = mainCamera;

            battleFighterFallingShadow.Initialize();
            battleFighterAnimation.Initialize(isPlayer, battleFighterAnimationTypeSprites);

            battleFighterBoxView.Initialize(isPlayer, mainCamera);
            battleFighterBoxView.OnShowMatchBattleBoxAction = (boxIndex, isUpdate) => OnShowMatchBattleBoxAction?.Invoke(boxIndex, isUpdate);
            battleFighterBoxView.OnCancelBattleCardAction = (battleCardModel) => OnCancelBattleCardAction?.Invoke(battleCardModel);

            battleFighterClickCollider.Initialize();
            battleFighterClickCollider.OnMouseLeftClickAction = () => OnShowFighterStatusViewAction?.Invoke();

            battleFighterResidentEffectView.Initialize(battleObjectPoolContainer);
            battleMatchReelView.Initialize(mainCamera);
        }

        public void Setup()
        {
            battleFighterFallingShadow.Setup();
            battleFighterAnimation.Setup();

            // TODO: この値はファイター情報を見て設定する
            var beginBoxNumber = 1;
            battleFighterBoxView.Setup(beginBoxNumber);

            battleFighterClickCollider.Setup();
            battleFighterResidentEffectView.Setup();
            battleMatchReelView.Setup();
        }
    }
}
