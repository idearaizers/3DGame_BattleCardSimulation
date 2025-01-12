using System;
using UnityEngine;
using Enhanced;

namespace Siasm
{
    public sealed class OwnPassiveSkillScrollController : BaseScrollController, IEnhancedScrollerDelegate
    {
        private SmallList<MenuOwnPassiveModel> ownPassiveModels;

        public Action<GameObject, BattleCardModel> OnClickAction { get; set; }
        public MenuCardScrollRect MenuCardScrollRect { get; private set; }

        public override void Initialize()
        {
            base.Initialize();

            MenuCardScrollRect = ScrollRect as MenuCardScrollRect;
            MenuCardScrollRect.Initialize(MenuCardScrollRect.ScrollType.OwnCard);

            EnhancedScroller.Delegate = this;
        }

        public void Setup(MenuOwnPassiveModel[] ownPassiveModels)
        {
            this.ownPassiveModels = new SmallList<MenuOwnPassiveModel>();

            for (int i = 0; i < ownPassiveModels.Length; i++)
            {
                this.ownPassiveModels.Add(ownPassiveModels[i]);
            }

            // 追加してからベースを実行
            base.Setup();
        }

        public int GetNumberOfCells(EnhancedScroller scroller)
        {
            return Mathf.CeilToInt((float)ownPassiveModels.Count / (float)NumberOfCellsPerRow);
        }

        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            var cellView = scroller.GetCellView(EnhancedScrollerCellViewPrefab) as RowMenuOwnPassiveSkillCellView;
            cellView.name = GetCellNameText(dataIndex);
            cellView.SetData(ref ownPassiveModels, dataIndex * NumberOfCellsPerRow);
            cellView.OnClickAction = OnClick;
            return cellView;
        }

        private void OnClick(GameObject selectedGameObject, MenuOwnPassiveModel ownPassiveModel)
        {
            OnClickAction?.Invoke(selectedGameObject, ownPassiveModel);
        }
    }
}
