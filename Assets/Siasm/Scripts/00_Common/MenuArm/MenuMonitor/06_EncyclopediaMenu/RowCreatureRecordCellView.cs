using System;
using Enhanced;
using UnityEngine;

namespace Siasm
{
    public sealed class RowCreatureRecordCellView : EnhancedScrollerCellView
    {
        [SerializeField]
        private CreatureRecordCellView[] creatureRecordCellViews;

        public Action<GameObject, CreatureRecordModel> OnClickAction { get; set; }

        public void SetData(ref SmallList<CreatureRecordModel> data, int startingIndex)
        {
            for (int i = 0; i < creatureRecordCellViews.Length; i++)
            {
                var creatureRecordModel = startingIndex + i < data.Count
                    ? data[startingIndex + i]
                    : null;

                creatureRecordCellViews[i].SetData(creatureRecordModel);
                creatureRecordCellViews[i].OnClickAction = OnClick;
            }
        }

        private void OnClick(GameObject selectedGameObject, CreatureRecordModel creatureRecordModel)
        {
            OnClickAction?.Invoke(selectedGameObject, creatureRecordModel);
        }
    }
}
