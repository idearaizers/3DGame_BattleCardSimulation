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

            // 仮
            this.currentIndex = currentIndex;

            for (int i = 0; i < creatureRecordModels.Length; i++)
            {
                this.creatureRecordModels.Add(creatureRecordModels[i]);
            }

            // 追加してからベースを実行
            base.Setup();
        }

        public int GetNumberOfCells(EnhancedScroller scroller)
        {
            return Mathf.CeilToInt((float)creatureRecordModels.Count / (float)NumberOfCellsPerRow);
        }

        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            var rowCreatureRecordCellView = scroller.GetCellView(EnhancedScrollerCellViewPrefab) as RowCreatureRecordCellView;
            rowCreatureRecordCellView.name = GetCellNameText(dataIndex);
            rowCreatureRecordCellView.SetData(ref creatureRecordModels, dataIndex * NumberOfCellsPerRow);
            rowCreatureRecordCellView.OnClickAction = OnClick;
            return rowCreatureRecordCellView;
        }

        private void OnClick(GameObject selectedGameObject, CreatureRecordModel creatureRecordModel)
        {
            OnClickAction?.Invoke(selectedGameObject, creatureRecordModel);
        }
    }
}
