using System;
using UnityEngine;
using Enhanced;

namespace Siasm
{
    public sealed class MenuDeckCardScrollController : BaseScrollController, IEnhancedScrollerDelegate
    {
        private SmallList<MenuDeckCardModel> deckCardModels;

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
            this.deckCardModels = new SmallList<MenuDeckCardModel>();

            for (int i = 0; i < deckCardModels.Length; i++)
            {
                this.deckCardModels.Add(deckCardModels[i]);
            }

            // 追加してからベースを実行
            base.Setup();
        }

        public int GetNumberOfCells(EnhancedScroller scroller)
        {
            return Mathf.CeilToInt((float)deckCardModels.Count / (float)NumberOfCellsPerRow);
        }

        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            var rowMenuDeckCardCellView = scroller.GetCellView(EnhancedScrollerCellViewPrefab) as RowMenuDeckCardCellView;
            rowMenuDeckCardCellView.name = GetCellNameText(dataIndex);
            rowMenuDeckCardCellView.SetData(ref deckCardModels, dataIndex * NumberOfCellsPerRow);
            rowMenuDeckCardCellView.OnClickAction = OnClick;
            return rowMenuDeckCardCellView;
        }

        private void OnClick(GameObject selectedGameObject, BattleCardModel battleCardModel)
        {
            OnClickAction?.Invoke(selectedGameObject, battleCardModel);
        }
    }
}
