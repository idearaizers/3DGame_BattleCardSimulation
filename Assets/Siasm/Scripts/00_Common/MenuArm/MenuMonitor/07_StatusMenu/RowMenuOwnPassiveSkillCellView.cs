using System;
using Enhanced;
using UnityEngine;

namespace Siasm
{
    public sealed class RowMenuOwnPassiveSkillCellView : EnhancedScrollerCellView
    {
        [SerializeField]
        private MenuOwnPassiveSkillCellView[] menuOwnPassiveSkillCellViews;

        public Action<GameObject, MenuOwnPassiveModel> OnClickAction { get; set; }

        public void SetData(ref SmallList<MenuOwnPassiveModel> data, int startingIndex)
        {
            for (int i = 0; i < menuOwnPassiveSkillCellViews.Length; i++)
            {
                var ownCardModel = startingIndex + i < data.Count
                    ? data[startingIndex + i]
                    : null;

                menuOwnPassiveSkillCellViews[i].SetData(ownCardModel);
                menuOwnPassiveSkillCellViews[i].OnClickAction = OnClick;
            }
        }

        private void OnClick(GameObject selectedGameObject, MenuOwnPassiveModel ownPassiveModel)
        {
            OnClickAction?.Invoke(selectedGameObject, ownPassiveModel);
        }
    }
}
