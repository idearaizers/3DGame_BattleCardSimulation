using System;
using UnityEngine;
using TMPro;

namespace Siasm
{
    public class TitleUIContent : MonoBehaviour
    {
        private const string versionTextFormat = "ver.{0}";

        [SerializeField]
        private TextMeshProUGUI versionText;

        [SerializeField]
        private TitleStartView titleStartView;

        [SerializeField]
        private TitleMenuView titleMenuView;

        public Action<int> OnTitleMenuButtonAction { get; set; }

        public void Initialize()
        {
            versionText.text = string.Format(versionTextFormat, Application.version);

            titleStartView.Initialize();
            titleStartView.OnTouchToStartAreaButtonAction = OnTouchToStartAreaButton;

            titleMenuView.Initialize();
            titleMenuView.OnTitleMenuButtonAction = OnTitleMenuButton;

            // 表示状態を設定
            versionText.gameObject.SetActive(true);
            titleMenuView.Disable();
            titleMenuView.Disable();
        }

        public void Setup()
        {
            titleStartView.Setup();
            titleMenuView.Setup();
        }

        public void ShowTitleStart()
        {
            titleStartView.Enable();
        }

        public void ShowTitleMenu()
        {
            titleMenuView.Enable();
        }

        private void OnTouchToStartAreaButton()
        {
            titleStartView.Disable();
            titleMenuView.Enable();
        }

        private void OnTitleMenuButton(int selectedIndex)
        {
            titleMenuView.Disable();
            OnTitleMenuButtonAction?.Invoke(selectedIndex);
        }
    }
}
