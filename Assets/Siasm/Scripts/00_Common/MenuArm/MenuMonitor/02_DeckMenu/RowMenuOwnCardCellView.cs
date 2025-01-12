using System;
using Enhanced;
using UnityEngine;

namespace Siasm
{
    public sealed class RowMenuOwnCardCellView : EnhancedScrollerCellView
    {
        [SerializeField]
        private MenuOwnCardCellView[] menuOwnCardCellViews;

        public Action<GameObject, BattleCardModel> OnClickAction { get; set; }

        public void SetData(ref SmallList<MenuOwnCardModel> data, int startingIndex)
        {
            for (int i = 0; i < menuOwnCardCellViews.Length; i++)
            {
                var ownCardModel = startingIndex + i < data.Count
                    ? data[startingIndex + i]
                    : null;

                menuOwnCardCellViews[i].SetData(ownCardModel);
                menuOwnCardCellViews[i].OnClickAction = OnClick;
            }
        }

        private void OnClick(GameObject selectedGameObject, BattleCardModel battleCardModel)
        {
            OnClickAction?.Invoke(selectedGameObject, battleCardModel);
        }
    }
}
