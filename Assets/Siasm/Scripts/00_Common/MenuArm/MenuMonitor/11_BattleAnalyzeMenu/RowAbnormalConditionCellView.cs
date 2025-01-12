using Enhanced;
using UnityEngine;

namespace Siasm
{
    public sealed class RowAbnormalConditionCellView : EnhancedScrollerCellView
    {
        [SerializeField]
        private AbnormalConditionCellView[] abnormalConditionCellViews;

        public void SetData(ref SmallList<AbnormalConditionCellView.BaseViewPrameter> data, int startingIndex)
        {
            for (int i = 0; i < abnormalConditionCellViews.Length; i++)
            {
                var modelData = startingIndex + i < data.Count
                    ? data[startingIndex + i]
                    : null;

                abnormalConditionCellViews[i].SetData(modelData);
            }
        }
    }
}
