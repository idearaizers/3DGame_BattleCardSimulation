using System;
using UnityEngine;
using UnityEngine.UI;

namespace Siasm
{
    public sealed class ContinuePlayDialog : BaseDialog
    {
        public static readonly string AssetAddress = "ContinuePlayDialogPrefab";

        [SerializeField]
        private Button cancelButton;

        [SerializeField]
        private Button deleteButton;

        [SerializeField]
        private Button decideButton;

        private int saveSlotIndex;

        public Action OnCancelAction { get; set; }
        public Action<int> OnDeleteAction { get; set; }
        public Action<int> OnDecideAction { get; set; }

        public override void Initialize()
        {
            cancelButton.onClick.AddListener(OnCancel);
            deleteButton.onClick.AddListener(OnDelete);
            decideButton.onClick.AddListener(OnDecide);
        }

        public void Apply(int saveSlotIndex)
        {
            this.saveSlotIndex = saveSlotIndex;
        }

        private void OnCancel()
        {
            OnCancelAction?.Invoke();
        }

        private void OnDelete()
        {
            OnDeleteAction?.Invoke(saveSlotIndex);
        }

        private void OnDecide()
        {
            OnDecideAction?.Invoke(saveSlotIndex);
        }
    }
}
