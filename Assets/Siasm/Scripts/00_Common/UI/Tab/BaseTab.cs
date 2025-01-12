using System;
using UnityEngine;
using UnityEngine.UI;

namespace Siasm
{
    public abstract class BaseTab : MonoBehaviour
    {
        [SerializeField]
        private Button button;

        public Action<BaseTab> OnClickAction { get; set; }

        private void Start()
        {
            button.onClick.AddListener(HandleClick);
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(HandleClick);
        }

        private void HandleClick()
        {
            OnClickAction?.Invoke(this);
        }

        public virtual void SetSelected() { }

        public virtual void SetDeselected() { }
    }
}
