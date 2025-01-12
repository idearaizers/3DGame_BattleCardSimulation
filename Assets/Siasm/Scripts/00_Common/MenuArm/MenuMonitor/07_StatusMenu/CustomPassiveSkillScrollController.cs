using System;
using UnityEngine;
using Enhanced;

namespace Siasm
{
    public sealed class CustomPassiveSkillScrollController : BaseScrollController, IEnhancedScrollerDelegate
    {
        private SmallList<MenuCustomPassiveSkillModel> customPassiveSkillModels;

        public Action<GameObject, BattleCardModel> OnClickAction { get; set; }
        public MenuCardScrollRect MenuCardScrollRect { get; private set; }

        public override void Initialize()
        {
            base.Initialize();

            MenuCardScrollRect = ScrollRect as MenuCardScrollRect;
            MenuCardScrollRect.Initialize(MenuCardScrollRect.ScrollType.DeckCard);

            EnhancedScroller.Delegate = this;
        }

        public void Setup(MenuCustomPassiveSkillModel[] customPassiveSkillModels)
        {
            this.customPassiveSkillModels = new SmallList<MenuCustomPassiveSkillModel>();

            for (int i = 0; i < customPassiveSkillModels.Length; i++)
            {
                this.customPassiveSkillModels.Add(customPassiveSkillModels[i]);
            }

            // 追加してからベースを実行
            base.Setup();
        }

        public int GetNumberOfCells(EnhancedScroller scroller)
        {
            return Mathf.CeilToInt((float)customPassiveSkillModels.Count / (float)NumberOfCellsPerRow);
        }

        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            var rowMenuCustomPassiveSkilllView = scroller.GetCellView(EnhancedScrollerCellViewPrefab) as RowMenuCustomPassiveSkilllView;
            rowMenuCustomPassiveSkilllView.name = GetCellNameText(dataIndex);
            rowMenuCustomPassiveSkilllView.SetData(ref customPassiveSkillModels, dataIndex * NumberOfCellsPerRow);
            rowMenuCustomPassiveSkilllView.OnClickAction = OnClick;
            return rowMenuCustomPassiveSkilllView;
        }

        private void OnClick(GameObject selectedGameObject, MenuCustomPassiveSkillModel customPassiveSkillModel)
        {
            OnClickAction?.Invoke(selectedGameObject, customPassiveSkillModel);
        }
    }
}
