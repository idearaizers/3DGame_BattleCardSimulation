using System;
using UnityEngine;
using UnityEngine.UI;

namespace Siasm
{
    public sealed class ConfirmDialog : BaseDialog
    {
        [SerializeField]
        private Button cancelButton;

        [SerializeField]
        private Button decideButton;

        public Action OnCancelAction { get; set; }
        public Action OnDecideAction { get; set; }

        public override void Initialize()
        {
            cancelButton.onClick.AddListener(OnCancel);
            decideButton.onClick.AddListener(OnDecide);
        }

        private void OnCancel()
        {
            OnCancelAction?.Invoke();
        }

        private void OnDecide()
        {
            OnDecideAction?.Invoke();
        }
    }
}
