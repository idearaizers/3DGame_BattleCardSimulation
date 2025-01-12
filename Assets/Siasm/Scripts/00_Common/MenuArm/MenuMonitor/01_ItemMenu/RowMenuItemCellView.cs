using System;
using Enhanced;
using UnityEngine;

namespace Siasm
{
    public sealed class RowMenuItemCellView : EnhancedScrollerCellView
    {
        [SerializeField]
        private MenuItemCellView[] menuItemCellViews;

        public Action<MenuItemCellView> OnClickAction { get; set; }

        public void SetData(ref SmallList<ItemModel> data, int startingIndex)
        {
            for (int i = 0; i < menuItemCellViews.Length; i++)
            {
                var itemModel = startingIndex + i < data.Count
                    ? data[startingIndex + i]
                    : null;

                menuItemCellViews[i].SetData(itemModel);
                menuItemCellViews[i].OnClickAction = OnClick;
            }
        }

        private void OnClick(MenuItemCellView menuItemCellView)
        {
            OnClickAction?.Invoke(menuItemCellView);
        }
    }
}
