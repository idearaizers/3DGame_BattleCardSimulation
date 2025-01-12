using System;
using UnityEngine;
using UnityEngine.UI;

namespace Siasm
{
    public class TitleStartView : BaseView
    {
        [SerializeField]
        private Button touchToStartAreaButton;

        public Action OnTouchToStartAreaButtonAction { get; set; }

        private void Start()
        {
            touchToStartAreaButton.onClick.AddListener(OnSelectedTitleMenuButton);
        }

        public void Initialize() { }

        public void Setup() { }

        private void OnSelectedTitleMenuButton()
        {
            OnTouchToStartAreaButtonAction?.Invoke();
        }
    }
}
