using System;
using Enhanced;
using UnityEngine;

namespace Siasm
{
    public sealed class RowMenuDeckCardCellView : EnhancedScrollerCellView
    {
        [SerializeField]
        private MenuDeckCardCellView[] menuDeckCardCellViews;

        public Action<GameObject, BattleCardModel> OnClickAction { get; set; }

        public void SetData(ref SmallList<MenuDeckCardModel> data, int startingIndex)
        {
            for (int i = 0; i < menuDeckCardCellViews.Length; i++)
            {
                var ownCardModel = startingIndex + i < data.Count
                    ? data[startingIndex + i]
                    : null;

                menuDeckCardCellViews[i].SetData(ownCardModel);
                menuDeckCardCellViews[i].OnClickAction = OnClick;
            }
        }

        private void OnClick(GameObject selectedGameObject, BattleCardModel battleCardModel)
        {
            OnClickAction?.Invoke(selectedGameObject, battleCardModel);
        }
    }
}
