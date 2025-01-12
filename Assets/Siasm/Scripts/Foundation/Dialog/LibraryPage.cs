using System;
using UnityEngine;
using UnityEngine.UI;

namespace Siasm
{
    public class LibraryPage : BasePage
    {
        public static readonly string AssetAddress = "LibraryPagePrefab";

        [SerializeField]
        private Button closeButton;

        public Action OnCloseButton { get; set; }

        private void Start()
        {
            closeButton.onClick.AddListener(() => OnCloseButton?.Invoke());
        }

        public void Initialize()
        {
            // NOTE: 図鑑の表示に使うモデルデータを渡す
        }
    }
}
