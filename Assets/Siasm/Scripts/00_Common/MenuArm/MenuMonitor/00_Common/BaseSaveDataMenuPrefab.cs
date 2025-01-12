using UnityEngine;

namespace Siasm
{
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

        /// <summary>
        /// ビュー表示に必要なモデルデータは各BaseMenuPrefabの継承先で取得して設定
        /// </summary>
        public override void Setup(bool isEnable)
        {
            // BaseUseCase

            base.Setup(isEnable);

            // 使用しない場合は実行しない
            if (!isEnable)
            {
                return;
            }

            // セーブデータのMax数は現状は4つ
            // これもUseCase経由で取得がいいかも
            var viewParameters = BaseUseCase.CreateSaveSlotCellViewParameters();

            // ビューを更新
            for (int i = 0; i < saveSlotCellViews.Length; i++)
            {
                saveSlotCellViews[i].Setup();
                saveSlotCellViews[i].UpdateView(viewParameters[i]);
            }
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

        protected virtual void OnClickCellView(SaveSlotCellView selectedSaveSlotCellView, bool isData)
        {
            // NOTE: 中身は継承先で設定
        }
    }
}
