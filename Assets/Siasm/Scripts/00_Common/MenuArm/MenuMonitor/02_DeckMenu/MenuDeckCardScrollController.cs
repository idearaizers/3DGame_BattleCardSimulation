using System;
using UnityEngine;
using Enhanced;

namespace Siasm
{
    public sealed class MenuDeckCardScrollController : BaseScrollController, IEnhancedScrollerDelegate
    {
        private SmallList<MenuDeckCardModel> menuDeckCardModels;

        public Action<GameObject, BattleCardModel> OnClickAction { get; set; }
        public MenuCardScrollRect MenuCardScrollRect { get; private set; }

        public override void Initialize()
        {
            base.Initialize();

            MenuCardScrollRect = ScrollRect as MenuCardScrollRect;
            MenuCardScrollRect.Initialize(MenuCardScrollRect.ScrollType.DeckCard);

            EnhancedScroller.Delegate = this;
        }

        public void Setup(MenuDeckCardModel[] deckCardModels)
        {
            this.menuDeckCardModels = new SmallList<MenuDeckCardModel>();

            for (int i = 0; i < deckCardModels.Length; i++)
            {
                this.menuDeckCardModels.Add(deckCardModels[i]);
            }

            base.Setup();
        }

        public int GetNumberOfCells(EnhancedScroller scroller)
        {
            return Mathf.CeilToInt((float)menuDeckCardModels.Count / (float)NumberOfCellsPerRow);
        }

        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            var cellView = scroller.GetCellView(EnhancedScrollerCellViewPrefab) as RowMenuDeckCardCellView;
            cellView.name = GetCellNameText(dataIndex);
            cellView.SetData(ref menuDeckCardModels, dataIndex * NumberOfCellsPerRow);
            cellView.OnClickAction = OnClick;
            return cellView;
        }

        private void OnClick(GameObject selectedGameObject, BattleCardModel battleCardModel)
        {
            OnClickAction?.Invoke(selectedGameObject, battleCardModel);
        }
    }
}
