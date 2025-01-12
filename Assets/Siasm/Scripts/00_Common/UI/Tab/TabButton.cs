using System;
using UnityEngine;
using UnityEngine.UI;

namespace Siasm
{
    public class TabButton : MonoBehaviour
    {
        [SerializeField]
        private Button button;

        [SerializeField]
        private GameObject onObject;

        [SerializeField]
        private GameObject offObject;

        public Action<bool> OnClickAction { get; set; }

        private bool isActive;

        private void Start()
        {
            button.onClick.AddListener(OnClick);
        }

        public void Initialize()
        {
            // 最初はoffの状態にする
            isActive = false;
            SetActive(isActive);
        }

        public void Setup() { }

        private void SetActive(bool isActive)
        {
            if (onObject != null) onObject.SetActive(isActive);
            if (offObject != null) offObject.SetActive(!isActive);
        }

        private void OnClick()
        {
            isActive = !isActive;
            SetActive(isActive);

            OnClickAction?.Invoke(isActive);
        }
    }
}
