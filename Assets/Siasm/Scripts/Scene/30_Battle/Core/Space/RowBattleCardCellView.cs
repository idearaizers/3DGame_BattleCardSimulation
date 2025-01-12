using System;
using Enhanced;
using UnityEngine;

namespace Siasm
{
    public sealed class RowBattleCardCellView : EnhancedScrollerCellView
    {
        [SerializeField]
        private BattleCardCellView[] battleCardCellView;

        public Action<BattleCardModel> OnClickAction { get; set; }

        public void SetData(ref SmallList<BattleCardModel> data, int startingIndex)
        {
            for (int i = 0; i < battleCardCellView.Length; i++)
            {
                var battleCardModel = startingIndex + i < data.Count
                    ? data[startingIndex + i]
                    : null;

                battleCardCellView[i].SetData(battleCardModel);
                battleCardCellView[i].OnClickAction = OnClick;
            }
        }

        private void OnClick(BattleCardModel battleCardModel)
        {
            OnClickAction?.Invoke(battleCardModel);
        }
    }
}
