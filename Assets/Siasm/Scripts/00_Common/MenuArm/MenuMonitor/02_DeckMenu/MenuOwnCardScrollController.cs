using System;
using UnityEngine;
using Enhanced;

namespace Siasm
{
    public sealed class MenuOwnCardScrollController : BaseScrollController, IEnhancedScrollerDelegate
    {
        private SmallList<MenuOwnCardModel> ownCardModels;

        public Action<GameObject, BattleCardModel> OnClickAction { get; set; }
        public MenuCardScrollRect MenuCardScrollRect { get; private set; }

        public override void Initialize()
        {
            base.Initialize();

            MenuCardScrollRect = ScrollRect as MenuCardScrollRect;
            MenuCardScrollRect.Initialize(MenuCardScrollRect.ScrollType.OwnCard);

            EnhancedScroller.Delegate = this;
        }

        public void Setup(MenuOwnCardModel[] ownCardModels)
        {
            this.ownCardModels = new SmallList<MenuOwnCardModel>();

            for (int i = 0; i < ownCardModels.Length; i++)
            {
                this.ownCardModels.Add(ownCardModels[i]);
            }

            // 追加してからベースを実行
            base.Setup();
        }

        public int GetNumberOfCells(EnhancedScroller scroller)
        {
            return Mathf.CeilToInt((float)ownCardModels.Count / (float)NumberOfCellsPerRow);
        }

        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            var cellView = scroller.GetCellView(EnhancedScrollerCellViewPrefab) as RowMenuOwnCardCellView;
            cellView.name = GetCellNameText(dataIndex);
            cellView.SetData(ref ownCardModels, dataIndex * NumberOfCellsPerRow);
            cellView.OnClickAction = OnClick;
            return cellView;
        }

        private void OnClick(GameObject selectedGameObject, BattleCardModel battleCardModel)
        {
            OnClickAction?.Invoke(selectedGameObject, battleCardModel);
        }
    }
}
