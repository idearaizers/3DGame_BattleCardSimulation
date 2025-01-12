using System;
using UnityEngine;

namespace Siasm
{
    public class BattleStateMachineController : MonoBehaviour
    {
        public enum BattleState
        {
            None = 0,
            AppearanceDirection,    // 登場演出
            CardSelection,          // カード操作
            TurnStart,              // ターン開始
            CombatStart,            // 戦闘開始
            TurnEnd,                // ターン終了
            ResultScreen            // リザルト画面
        }

        private BattleState battleState = BattleState.None;

        public Action<BattleState> OnExitBattleStateAction { get; set; }
        public Action<BattleState> OnEnterBattleStateAction { get; set; }

        public void Initialize() { }

        public void Setup() { }

        public bool IsCurrentBattleState(BattleState battleState)
        {
            if (this.battleState == battleState)
            {
                return true;
            }

            return false;
        }

        public void ChangeMainState(BattleState newState)
        {
            if (battleState == newState)
            {
                Debug.LogWarning($"同じstateに変更しようとしています => newState: {newState}");
                return;
            }

            OnExitState();
            battleState = newState;
            OnEnterState();
        }

        private void OnExitState()
        {
            OnExitBattleStateAction?.Invoke(battleState);
        }

        private void OnEnterState()
        {
            Debug.Log($"<color=cyan>◆BattleState => {battleState}◆</color>");
            OnEnterBattleStateAction?.Invoke(battleState);
        }
    }
}
