using System;
using UnityEngine;
using UnityEngine.UI;

namespace Siasm
{
    public class LibraryDialog : BaseDialog
    {
        public static readonly string AssetAddress = "LibraryDialogPrefab";

        [SerializeField]
        private Button closeButton;

        public Action OnCloseButtonAction { get; set; }

        public override void Initialize()
        {
            closeButton.onClick.AddListener(OnCancel);
        }

        private void OnCancel()
        {
            OnCloseButtonAction?.Invoke();
        }
    }
}
