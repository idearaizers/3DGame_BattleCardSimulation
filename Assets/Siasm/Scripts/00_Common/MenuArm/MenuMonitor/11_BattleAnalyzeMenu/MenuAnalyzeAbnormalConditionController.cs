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

        /// <summary>
        /// BaseBattleFighterModelから必要な値を生成して使用する
        /// 思考停止も状態異常として扱う
        /// </summary>
        /// <param name="baseBattleFighterModel"></param>
        public void Setup(BaseBattleFighterModel baseBattleFighterModel)
        {
            this.abnormalConditionCellViewPrameters = new SmallList<AbnormalConditionCellView.BaseViewPrameter>();

            // 思考停止用のパラメータクラスで格納
            // BaseAbnormalConditionModel での管理がいいかも
            if (baseBattleFighterModel.ThinkingModel.IsThinkingFreeze)
            {
                this.abnormalConditionCellViewPrameters.Add(new AbnormalConditionCellView.ThinkingFreezeViewPrameter());
            }

            // 状態異常用のパラメータクラスで格納
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
