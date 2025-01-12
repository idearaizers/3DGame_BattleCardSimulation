using Enhanced;
using UnityEngine;

namespace Siasm
{
    public sealed class RowPassiveAbilityCellView : EnhancedScrollerCellView
    {
        [SerializeField]
        private PassiveAbilityCellView[] passiveAbilityCellViews;

        public void SetData(ref SmallList<PassiveAbilityCellView.BaseViewPrameter> data, int startingIndex)
        {
            for (int i = 0; i < passiveAbilityCellViews.Length; i++)
            {
                var modelData = startingIndex + i < data.Count
                    ? data[startingIndex + i]
                    : null;

                passiveAbilityCellViews[i].SetData(modelData);
            }
        }
    }
}
