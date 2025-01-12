using UnityEngine;

namespace Siasm
{
    /// <summary>
    /// セーブとロードで共通する処理を記載
    /// </summary>
    public abstract class BaseSaveDataMenuPrefab : BaseMenuPrefab
    {
        [SerializeField]
        private SaveSlotCellView[] saveSlotCellViews;

        protected SaveSlotCellView[] SaveSlotCellViews => saveSlotCellViews;

        public override void Initialize(SideArmSwitcherPrefab sideArmSwitcherPrefab, BaseUseCase baseUseCase, BaseCameraController baseCameraController,
            PlayerBattleFighterSpawnController playerBattleFighterSpawnController, EnemyBattleFighterSpawnController enemyBattleFighterSpawnController)
        {
            base.Initialize(sideArmSwitcherPrefab, baseUseCase, baseCameraController, playerBattleFighterSpawnController, enemyBattleFighterSpawnController);

            foreach (var saveSlotDetailCellView in saveSlotCellViews)
            {
                saveSlotDetailCellView.Initialize();
                saveSlotDetailCellView.OnClickCellViewAction = OnClickCellView;
            }
        }

        public override void Setup(bool isEnable)
        {
            base.Setup(isEnable);

            if (!isEnable)
            {
                return;
            }

            var viewParameters = BaseUseCase.CreateSaveSlotCellViewParameters();

            for (int i = 0; i < saveSlotCellViews.Length; i++)
            {
                saveSlotCellViews[i].Setup();
                saveSlotCellViews[i].UpdateView(viewParameters[i]);
            }
        }

        protected virtual void OnClickCellView(SaveSlotCellView selectedSaveSlotCellView, bool isData)
        {
            // NOTE: 中身は継承先で設定
        }
    }
}
