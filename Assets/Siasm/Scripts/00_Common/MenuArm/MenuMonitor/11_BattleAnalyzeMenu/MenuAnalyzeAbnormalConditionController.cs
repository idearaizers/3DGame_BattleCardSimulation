using UnityEngine;
using Enhanced;

namespace Siasm
{
    public sealed class MenuAnalyzeAbnormalConditionController : BaseScrollController, IEnhancedScrollerDelegate
    {
        private SmallList<AbnormalConditionCellView.BaseViewPrameter> abnormalConditionCellViewPrameters;

        public override void Initialize()
        {
            base.Initialize();

            EnhancedScroller.Delegate = this;
        }

        public void Setup(BaseBattleFighterModel baseBattleFighterModel)
        {
            this.abnormalConditionCellViewPrameters = new SmallList<AbnormalConditionCellView.BaseViewPrameter>();

            if (baseBattleFighterModel.ThinkingModel.IsThinkingFreeze)
            {
                this.abnormalConditionCellViewPrameters.Add(new AbnormalConditionCellView.ThinkingFreezeViewPrameter());
            }

            for (int i = 0; i < baseBattleFighterModel.BaseAbnormalConditionModels.Count; i++)
            {
                this.abnormalConditionCellViewPrameters.Add(new AbnormalConditionCellView.AbnormalConditionViewPrameter
                {
                    BaseAbnormalConditionModel = baseBattleFighterModel.BaseAbnormalConditionModels[i]
                });
            }

            base.Setup();
        }

        public int GetNumberOfCells(EnhancedScroller scroller)
        {
            return Mathf.CeilToInt((float)abnormalConditionCellViewPrameters.Count / (float)NumberOfCellsPerRow);
        }

        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            var cellView = scroller.GetCellView(EnhancedScrollerCellViewPrefab) as RowAbnormalConditionCellView;
            cellView.name = GetCellNameText(dataIndex);
            cellView.SetData(ref abnormalConditionCellViewPrameters, dataIndex * NumberOfCellsPerRow);
            return cellView;
        }
    }
}
