using System;
using UnityEngine;
using Enhanced;

namespace Siasm
{
    public sealed class MenuItemScrollController : BaseScrollController, IEnhancedScrollerDelegate
    {
        private SmallList<ItemModel> itemModels;

        public Action<MenuItemCellView> OnClickAction { get; set; }

        // EnhancedScrollerCellView Prefabが3つかな
        // EnhancedScrollerCellView Prefabが3つかな
        // EnhancedScrollerCellView Prefabが3つかな

        public override void Initialize()
        {
            base.Initialize();

            EnhancedScroller.Delegate = this;
        }

        public void Setup(ItemModel[] itemModels)
        {
            this.itemModels = new SmallList<ItemModel>();

            for (int i = 0; i < itemModels.Length; i++)
            {
                this.itemModels.Add(itemModels[i]);
            }

            // 追加してからベースを実行
            base.Setup();
        }

        public int GetNumberOfCells(EnhancedScroller scroller)
        {
            return Mathf.CeilToInt((float)itemModels.Count / (float)NumberOfCellsPerRow);
        }

        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            // 
            // if (itemModels[dataIndex] is ItemModel)
            // {
            //     // 
            //     // var rowMenuItemCellView = scroller.GetCellView(EnhancedScrollerCellViewPrefab) as RowMenuItemCellView;
            //     // rowMenuItemCellView.name = GetCellNameText(dataIndex);
            // }

            // 
            var rowMenuItemCellView = scroller.GetCellView(EnhancedScrollerCellViewPrefab) as RowMenuItemCellView;
            rowMenuItemCellView.name = GetCellNameText(dataIndex);
            rowMenuItemCellView.SetData(ref itemModels, dataIndex * NumberOfCellsPerRow);
            rowMenuItemCellView.OnClickAction = OnClick;
            return rowMenuItemCellView;
        }

        // 仮
        // public override float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
        // {
        //     if (itemModels[dataIndex] is ItemModel)
        //     {
        //         // 仮
        //         return 0.01f;
        //     }
        //     // 仮
        //     return 0.01f;
        // }

        private void OnClick(MenuItemCellView menuItemCellView)
        {
            OnClickAction?.Invoke(menuItemCellView);
        }
    }
}
