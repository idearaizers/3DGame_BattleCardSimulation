using System;
using UnityEngine;
using Enhanced;

namespace Siasm
{
    public sealed class BattleCardScrollController : BaseScrollController, IEnhancedScrollerDelegate
    {
        private SmallList<BattleCardModel> smallListOfBattleCardModels;

        public Action<BattleCardModel> OnClickAction { get; set; }

        public override void Initialize()
        {
            base.Initialize();

            EnhancedScroller.Delegate = this;
        }

        public void Setup(BattleCardModel[] deckBattleCardModels)
        {
            smallListOfBattleCardModels = new SmallList<BattleCardModel>();

            for (int i = 0; i < deckBattleCardModels.Length; i++)
            {
                smallListOfBattleCardModels.Add(deckBattleCardModels[i]);
            }

            base.Setup();
        }

        public int GetNumberOfCells(EnhancedScroller scroller)
        {
            // NOTE: 中身が空の場合は適当な値を返す
            if (smallListOfBattleCardModels == null || smallListOfBattleCardModels.Count == 0)
            {
                return 0;
            }

            return Mathf.CeilToInt((float)smallListOfBattleCardModels.Count / (float)NumberOfCellsPerRow);
        }

        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            var cellView = scroller.GetCellView(EnhancedScrollerCellViewPrefab) as RowBattleCardCellView;
            cellView.name = GetCellNameText(dataIndex);
            cellView.SetData(ref smallListOfBattleCardModels, dataIndex * NumberOfCellsPerRow);

            cellView.OnClickAction = OnClick;

            return cellView;
        }

        private void OnClick(BattleCardModel battleCardModel)
        {
            OnClickAction?.Invoke(battleCardModel);
        }
    }
}
