using System;
using UnityEngine;
using UnityEngine.UI;

namespace Siasm
{
    public sealed class MessageDialog : BaseDialog
    {
        [SerializeField]
        private Button closeButton;

        public Action OnCloseAction { get; set; }

        public override void Initialize()
        {
            closeButton.onClick.AddListener(OnClose);
        }

        private void OnClose()
        {
            OnCloseAction?.Invoke();
        }
    }
}
