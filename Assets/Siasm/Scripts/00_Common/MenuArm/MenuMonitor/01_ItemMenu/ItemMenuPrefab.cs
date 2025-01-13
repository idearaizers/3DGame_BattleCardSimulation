using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Siasm
{
    public sealed class ItemMenuPrefab : BaseMenuPrefab
    {
        [SerializeField]
        private MenuItemScrollController menuItemScrollController;

        [SerializeField]
        private MenuItemDetailView menuItemDetailView;

        private MenuItemCellView currentSelectedMenuItemCellView;

        public override void Initialize(SideArmSwitcherPrefab sideArmSwitcherPrefab, BaseUseCase baseUseCase, BaseCameraController baseCameraController,
            PlayerBattleFighterSpawnController playerBattleFighterSpawnController, EnemyBattleFighterSpawnController enemyBattleFighterSpawnController)
        {
            base.Initialize(sideArmSwitcherPrefab, baseUseCase, baseCameraController, playerBattleFighterSpawnController, enemyBattleFighterSpawnController);

            menuItemScrollController.Initialize();
            menuItemScrollController.OnClickAction = OnClick;
            menuItemDetailView.Initialize();
        }

        public override void Setup(bool isEnable)
        {
            base.Setup(isEnable);

            if (!isEnable)
            {
                return;
            }

            // TODO: 非アクティブの時にセットアップを行うと参照エラーが出るのでアクティブに切り替えて実行で見直し予定
            var activeSelf = gameObject.activeSelf;
            gameObject.SetActive(true);
            SetItemModel();
            gameObject.SetActive(activeSelf);
        }

        public override void UpdateContent(BaseMenuPrefabParameter baseMenuPrefabParameter)
        {
            base.UpdateContent(baseMenuPrefabParameter);

            if (!IsEnable)
            {
                return;
            }

            SetItemModel();
        }

        private void SetItemModel()
        {
            if (SaveManager.Instance.LoadedSaveDataCache == null)
            {
                // セーブデータを読み込んでいなければエラー回避用にダミーデータを入れる
                var itemModelsDammy = new ItemModel[] { };
                menuItemScrollController.Setup(itemModelsDammy);
            }
            else
            {
                var itemModels = BaseUseCase.CreateItemModelsOfAllOwn();

                // 入手した逆順で表示したいので表示を逆順に変更
                Array.Reverse(itemModels);

                menuItemScrollController.Setup(itemModels);
                menuItemDetailView.Setup();
            }
        }

        /// <summary>
        /// 選択したアイテムの詳細を表示する
        /// </summary>
        /// <param name="selectedMenuItemCellView"></param>
        private void OnClick(MenuItemCellView selectedMenuItemCellView)
        {
            // 選択状態で且つ選択したものが直前のものと違う時は、選択状態を解除する
            if (currentSelectedMenuItemCellView != null &&
                currentSelectedMenuItemCellView != selectedMenuItemCellView)
            {
                currentSelectedMenuItemCellView.ChangeActiveOfSelectedImage(false);
            }

            currentSelectedMenuItemCellView = selectedMenuItemCellView;

            menuItemDetailView.ShowDetailViewAsync(selectedMenuItemCellView.ItemModel).Forget();
        }
    }
}
