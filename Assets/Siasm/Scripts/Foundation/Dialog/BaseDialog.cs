using UnityEngine;
using TMPro;

namespace Siasm
{
    /// <summary>
    /// Acitonが必要な場合は継承先で定義する
    /// </summary>
    public abstract class BaseDialog : MonoBehaviour
    {
        public class Parameter
        {
            public string Message { get; set; }
        }

        [SerializeField]
        private TextMeshProUGUI messageText;

        public virtual void Initialize() { }

        public void ApplyParameter(Parameter parameter)
        {
            if (messageText == null)
            {
                return;
            }

            messageText.text = parameter.Message;
        }
    }
}
