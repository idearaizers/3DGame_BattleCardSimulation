using System;
using Enhanced;
using UnityEngine;

namespace Siasm
{
    public sealed class RowMenuCustomPassiveSkillView : EnhancedScrollerCellView
    {
        [SerializeField]
        private MenuCustomPassiveSkillView[] menuCustomPassiveSkilllViews;

        public Action<GameObject, MenuCustomPassiveSkillModel> OnClickAction { get; set; }

        public void SetData(ref SmallList<MenuCustomPassiveSkillModel> data, int startingIndex)
        {
            for (int i = 0; i < menuCustomPassiveSkilllViews.Length; i++)
            {
                var ownCardModel = startingIndex + i < data.Count
                    ? data[startingIndex + i]
                    : null;

                menuCustomPassiveSkilllViews[i].SetData(ownCardModel);
                menuCustomPassiveSkilllViews[i].OnClickAction = OnClick;
            }
        }

        private void OnClick(GameObject selectedGameObject, MenuCustomPassiveSkillModel customPassiveSkillModel)
        {
            OnClickAction?.Invoke(selectedGameObject, customPassiveSkillModel);
        }
    }
}
