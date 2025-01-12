using UnityEngine;

namespace Siasm
{
    public sealed class BattleHelpMenuPrefab : BaseMenuPrefab
    {
        [SerializeField]
        private TabContentSwitcher tabContentSwitcher;

        [SerializeField]
        private HelpContentsView[] helpContentsViews;

        public override void Initialize(SideArmSwitcherPrefab sideArmSwitcherPrefab, BaseUseCase baseUseCase, BaseCameraController baseCameraController,
            PlayerBattleFighterSpawnController playerBattleFighterSpawnController, EnemyBattleFighterSpawnController enemyBattleFighterSpawnController)
        {
            base.Initialize(sideArmSwitcherPrefab, baseUseCase, baseCameraController, playerBattleFighterSpawnController, enemyBattleFighterSpawnController);

            var activeTabIndex = 0;
            tabContentSwitcher.Initialize(activeTabIndex);

            foreach (var helpContentsView in helpContentsViews)
            {
                helpContentsView.Initialize();
            }
        }

        public override void Setup(bool isEnable)
        {
            base.Setup(isEnable);

            // 使用しない場合は実行しない
            if (!isEnable)
            {
                return;
            }

            foreach (var helpContentsView in helpContentsViews)
            {
                helpContentsView.Setup();
            }
        }

        public override void UpdateContent(BaseMenuPrefabParameter baseMenuPrefabParameter)
        {
            base.UpdateContent(baseMenuPrefabParameter);

            // 使用しない場合は実行しない
            if (!IsEnable)
            {
                return;
            }
        }
    }
}
