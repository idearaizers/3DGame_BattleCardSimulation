using System;
using UnityEngine;
using Enhanced;

namespace Siasm
{
    public sealed class CreatureRecordScrollController : BaseScrollController, IEnhancedScrollerDelegate
    {
        private SmallList<CreatureRecordModel> creatureRecordModels;

        public Action<GameObject, CreatureRecordModel> OnClickAction { get; set; }

        private int currentIndex;

        public override void Initialize()
        {
            base.Initialize();

            EnhancedScroller.Delegate = this;
        }

        public void Setup(CreatureRecordModel[] creatureRecordModels, int currentIndex)
        {
            this.creatureRecordModels = new SmallList<CreatureRecordModel>();
            this.currentIndex = currentIndex;

            for (int i = 0; i < creatureRecordModels.Length; i++)
            {
                this.creatureRecordModels.Add(creatureRecordModels[i]);
            }

            base.Setup();
        }

        public int GetNumberOfCells(EnhancedScroller scroller)
        {
            return Mathf.CeilToInt((float)creatureRecordModels.Count / (float)NumberOfCellsPerRow);
        }

        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            var cellView = scroller.GetCellView(EnhancedScrollerCellViewPrefab) as RowCreatureRecordCellView;
            cellView.name = GetCellNameText(dataIndex);
            cellView.SetData(ref creatureRecordModels, dataIndex * NumberOfCellsPerRow);
            cellView.OnClickAction = OnClick;
            return cellView;
        }

        private void OnClick(GameObject selectedGameObject, CreatureRecordModel creatureRecordModel)
        {
            OnClickAction?.Invoke(selectedGameObject, creatureRecordModel);
        }
    }
}
