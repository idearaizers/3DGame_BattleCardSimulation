using System;
using UnityEngine;
using TMPro;

namespace Siasm
{
    public class PlayerFieldContactActionView : BaseView
    {
        private const string talkText = "[Z] 話す";
        private const string interactText = "[Z] 調べる";

        [SerializeField]
        private TextMeshProUGUI contactText;

        public void Initialize() { }

        public void Show(FieldInteractType fieldInteractType)
        {
            this.Enable();
            switch (fieldInteractType)
            {
                case FieldInteractType.Talk:
                    contactText.text = talkText;
                    break;
                case FieldInteractType.Interact:
                    contactText.text = interactText;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(fieldInteractType));
            }
        }
    }
}
