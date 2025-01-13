using System;
using UnityEngine;
using Enhanced;

namespace Siasm
{
    public sealed class BattleDeckMenuCardScrollController : BaseScrollController, IEnhancedScrollerDelegate
    {
        private SmallList<MenuDeckCardModel> deckCardModels;

        public Action<GameObject, BattleCardModel> OnClickAction { get; set; }
        public MenuCardScrollRect MenuCardScrollRect { get; private set; }

        public override void Initialize()
        {
            base.Initialize();

            MenuCardScrollRect = ScrollRect as MenuCardScrollRect;
            MenuCardScrollRect.Initialize(MenuCardScrollRect.ScrollType.None);
            EnhancedScroller.Delegate = this;
        }

        public void Setup(MenuDeckCardModel[] deckCardModels)
        {
            this.deckCardModels = new SmallList<MenuDeckCardModel>();

            for (int i = 0; i < deckCardModels.Length; i++)
            {
                this.deckCardModels.Add(deckCardModels[i]);
            }

            base.Setup();
        }

        public int GetNumberOfCells(EnhancedScroller scroller)
        {
            return Mathf.CeilToInt((float)deckCardModels.Count / (float)NumberOfCellsPerRow);
        }

        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            var cellView = scroller.GetCellView(EnhancedScrollerCellViewPrefab) as RowMenuDeckCardCellView;
            cellView.name = GetCellNameText(dataIndex);
            cellView.SetData(ref deckCardModels, dataIndex * NumberOfCellsPerRow);
            cellView.OnClickAction = OnClick;
            return cellView;
        }

        private void OnClick(GameObject selectedGameObject, BattleCardModel battleCardModel)
        {
            OnClickAction?.Invoke(selectedGameObject, battleCardModel);
        }
    }
}
