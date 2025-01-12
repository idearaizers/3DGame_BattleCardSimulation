using UnityEngine;

namespace Siasm
{
    public sealed class StatusMenuPrefab : BaseMenuPrefab
    {
        [SerializeField]
        private TabContentSwitcher tabContentSwitcher;

        [SerializeField]
        private StatusParameterView statusParameterView;

        [SerializeField]
        private StatusPassiveView statusPassiveView;

        public override void Initialize(SideArmSwitcherPrefab sideArmSwitcherPrefab, BaseUseCase baseUseCase, BaseCameraController baseCameraController,
            PlayerBattleFighterSpawnController playerBattleFighterSpawnController, EnemyBattleFighterSpawnController enemyBattleFighterSpawnController)
        {
            base.Initialize(sideArmSwitcherPrefab, baseUseCase, baseCameraController, playerBattleFighterSpawnController, enemyBattleFighterSpawnController);

            var activeTabIndex = 0;
            tabContentSwitcher.Initialize(activeTabIndex);

            statusParameterView.Initialize();
            statusPassiveView.Initialize(baseCameraController, SideArmSwitcherPrefab);
        }

        public override void Setup(bool isActive)
        {
            base.Setup(isActive);

            // 使用しない場合は実行しない
            if (!isActive)
            {
                return;
            }

            // 非アクティブの時にセットアップを行うと参照エラーが出るのでアクティブに切り替えて実行
            var activeSelf = gameObject.activeSelf;
            gameObject.SetActive(true);

            // 更新する
            SetItemModel();

            // 設定が完了したら変更前の状態に戻す
            gameObject.SetActive(activeSelf);

            // 
            tabContentSwitcher.Setup();
        }

        public override void UpdateContent(BaseMenuPrefabParameter baseMenuPrefabParameter)
        {
            base.UpdateContent(baseMenuPrefabParameter);

            // // 使用しない場合は実行しない
            // if (!IsActive)
            // {
            //     return;
            // }

            // // 中身が変わっていることがあるので最新に更新する
            // SetItemModel();
        }

        private void SetItemModel()
        {
            if (SaveManager.Instance.LoadedSaveDataCache == null)
            {
                // 非表示になるので単に処理させないだけでもいいかも
                statusParameterView.Setup(null);
                statusPassiveView.Setup(null);
            }
            else
            {
                var battleFighterStatusModel = BaseUseCase.CreateBattleFighterStatusModelOfPlayer();
                statusParameterView.Setup(battleFighterStatusModel);

                // 仮
                var activeSelf = statusPassiveView.gameObject.activeSelf;
                statusPassiveView.gameObject.SetActive(true);

                var battleFighterPassiveModel = BaseUseCase.CreateBattleFighterPassiveModel();
                statusPassiveView.Setup(battleFighterPassiveModel);

                statusPassiveView.gameObject.SetActive(activeSelf);
            }
        }
    }
}
