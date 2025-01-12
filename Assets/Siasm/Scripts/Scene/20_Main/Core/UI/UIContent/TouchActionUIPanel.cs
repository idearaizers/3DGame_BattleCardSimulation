using System;
using UnityEngine;
using UnityEngine.UI;

namespace Siasm
{
    public class TouchActionUIPanel : MonoBehaviour
    {
        [SerializeField]
        private Button touchButton;

        public Action OnTouchAction { get; set; }

        public void Initialize()
        {
            touchButton.onClick.AddListener(OnTouch);
        }

        private void OnTouch()
        {
            OnTouchAction?.Invoke();
        }
    }
}
