using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Siasm
{
    public sealed class ItemMenuPrefab : BaseMenuPrefab
    {
        [SerializeField]
        private TabGroup tabGroup;

        [SerializeField]
        private MenuItemScrollController menuItemScrollController;

        [SerializeField]
        private MenuItemDetailView menuItemDetailView;

        private MenuItemCellView currentSelectedMenuItemCellView;

        public override void Initialize(SideArmSwitcherPrefab sideArmSwitcherPrefab, BaseUseCase baseUseCase, BaseCameraController baseCameraController,
            PlayerBattleFighterSpawnController playerBattleFighterSpawnController, EnemyBattleFighterSpawnController enemyBattleFighterSpawnController)
        {
            base.Initialize(sideArmSwitcherPrefab, baseUseCase, baseCameraController, playerBattleFighterSpawnController, enemyBattleFighterSpawnController);

            var activeTabIndex = 0;
            tabGroup.SetActiveTab(activeTabIndex);
            tabGroup.OnChangeActiveTab = OnChangeActiveTab;

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

            // 中身が変わっていることがあるので最新に更新する
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
                // TODO: 内容に合わせてカテゴリ表示分けをしたい
                var itemModels = BaseUseCase.CreateItemModelsOfAllOwn();

                // 取得順にするため並び順を逆にする
                Array.Reverse(itemModels);

                // 反映する
                menuItemScrollController.Setup(itemModels);
                menuItemDetailView.Setup();
            }
        }

        private void OnClick(MenuItemCellView selectedMenuItemCellView)
        {
            // 選択状態で且つ選択したものが直前のものと違う時だけ選択状態を非表示にする
            if (currentSelectedMenuItemCellView != null &&
                currentSelectedMenuItemCellView != selectedMenuItemCellView)
            {
                currentSelectedMenuItemCellView.ChangeActiveOfSelectedImage(false);
            }

            // 選択中の情報を更新
            currentSelectedMenuItemCellView = selectedMenuItemCellView;

            // menuItemDetailViewに表示
            menuItemDetailView.ShowDetailViewAsync(selectedMenuItemCellView.ItemModel).Forget();
        }

        private void OnChangeActiveTab(int selectedIndex)
        {
            // TODO: 入手した時間順、またはアイテムid順に並び替える機能を実装

            Debug.Log("TODO: 並び順の切り替え実行");
        }
    }
}
