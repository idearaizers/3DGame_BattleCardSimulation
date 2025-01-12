using System;
using UnityEngine;

namespace Siasm
{
    public class MainStateMachineController : MonoBehaviour
    {
        public enum MainState
        {
            None = 0,
            FreeExploration,    // 自由探索
            InteractAction,     // 接触する
            PauseAndMenu,       // メニューを開いてポーズする
            PlayingDirection    // 演出中。別の日に進む。バトルに遷移
        }

        private MainState mainState = MainState.None;

        public Action<MainState> OnExitMainStateAction { get; set; }
        public Action<MainState> OnEnterMainStateAction { get; set; }

        public void Initialize() { }

        public void Setup() { }

        public bool IsCurrentMainState(MainState mainState)
        {
            if (this.mainState == mainState)
            {
                return true;
            }

            return false;
        }

        public void ChangeMainState(MainState newState)
        {
            if (mainState == newState)
            {
                Debug.LogWarning($"同じstateに変更しようとしています => newState: {newState}");
                return;
            }

            OnExitState();
            mainState = newState;
            OnEnterState();
        }

        private void OnExitState()
        {
            OnExitMainStateAction?.Invoke(mainState);
        }

        private void OnEnterState()
        {
            Debug.Log($"<color=cyan>◆MainState => {mainState}◆</color>");
            OnEnterMainStateAction?.Invoke(mainState);
        }
    }
}
