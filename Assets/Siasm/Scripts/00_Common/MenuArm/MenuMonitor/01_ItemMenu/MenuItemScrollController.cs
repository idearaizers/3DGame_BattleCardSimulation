using System;
using UnityEngine;
using Enhanced;

namespace Siasm
{
    public sealed class MenuItemScrollController : BaseScrollController, IEnhancedScrollerDelegate
    {
        private SmallList<ItemModel> itemModels;

        public Action<MenuItemCellView> OnClickAction { get; set; }

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

            base.Setup();
        }

        public int GetNumberOfCells(EnhancedScroller scroller)
        {
            return Mathf.CeilToInt((float)itemModels.Count / (float)NumberOfCellsPerRow);
        }

        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            var cellView = scroller.GetCellView(EnhancedScrollerCellViewPrefab) as RowMenuItemCellView;
            cellView.name = GetCellNameText(dataIndex);
            cellView.SetData(ref itemModels, dataIndex * NumberOfCellsPerRow);
            cellView.OnClickAction = OnClick;
            return cellView;
        }

        private void OnClick(MenuItemCellView menuItemCellView)
        {
            OnClickAction?.Invoke(menuItemCellView);
        }
    }
}
