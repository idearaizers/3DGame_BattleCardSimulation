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

        public override void Initialize(SideArmSwitcherPrefab sideArmSwitcherPrefab, BaseUseCase baseUseCase, BaseCameraController baseCameraController, BattleSpaceManager battleSpaceManager)
        {
            base.Initialize(sideArmSwitcherPrefab, baseUseCase, baseCameraController, battleSpaceManager);

            var activeTabIndex = 0;
            tabGroup.SetActiveTab(activeTabIndex);
            tabGroup.OnChangeActiveTab = OnChangeActiveTab;

            menuItemScrollController.Initialize();
            menuItemScrollController.OnClickAction = OnClick;
            menuItemDetailView.Initialize();
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
        }

        public override void UpdateContent(BaseMenuPrefabParameter baseMenuPrefabParameter)
        {
            base.UpdateContent(baseMenuPrefabParameter);

            // 使用しない場合は実行しない
            if (!IsActive)
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
            // NOTE: itemModels を保持して、一旦、仮で逆順の並び替えができるようにしてもいいかも
            // Debug.Log("TODO: 並び順の切り替えを実行");
        }
    }
}
