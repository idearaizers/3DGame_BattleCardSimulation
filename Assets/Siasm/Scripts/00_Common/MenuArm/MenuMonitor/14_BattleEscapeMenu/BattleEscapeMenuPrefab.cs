using System;
using UnityEngine;
using UnityEngine.UI;

namespace Siasm
{
    public sealed class BattleEscapeMenuPrefab : BaseMenuPrefab
    {
        private const string notChangeMessageText = "本当に撤退しますか？";

        [SerializeField]
        private Button retryButton;

        [SerializeField]
        private Button withdrawalButton;

        public Action OnEscapeAction { get; set; }

        public override void Initialize(SideArmSwitcherPrefab sideArmSwitcherPrefab, BaseUseCase baseUseCase, BaseCameraController baseCameraController,
            PlayerBattleFighterSpawnController playerBattleFighterSpawnController, EnemyBattleFighterSpawnController enemyBattleFighterSpawnController)
        {
            base.Initialize(sideArmSwitcherPrefab, baseUseCase, baseCameraController, playerBattleFighterSpawnController, enemyBattleFighterSpawnController);

            retryButton.onClick.AddListener(() =>
            {
                Debug.Log("TODO: リトライ実行");
            });

            withdrawalButton.onClick.AddListener(OnWithdrawalButton);
        }

        public override void Setup(bool isEnable)
        {
            base.Setup(isEnable);

            if (!isEnable)
            {
                return;
            }
        }

        public override void UpdateContent(BaseMenuPrefabParameter baseMenuPrefabParameter)
        {
            base.UpdateContent(baseMenuPrefabParameter);

            if (!IsEnable)
            {
                return;
            }
        }

        private void OnWithdrawalButton()
        {
            SideArmSwitcherPrefab.PlayOpenDisplayAnimation(
                notChangeMessageText,
                () =>
                {
                    OnEscapeAction?.Invoke();
                },
                () =>
                {
                    // NOTE: キャンセルの際は何もしない
                }
            );
        }
    }
}
