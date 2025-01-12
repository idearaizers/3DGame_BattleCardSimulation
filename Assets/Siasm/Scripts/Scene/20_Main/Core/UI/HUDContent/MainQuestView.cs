using UnityEngine;
using TMPro;

namespace Siasm
{
    public class MainQuestView : BaseView
    {
        public class Parameter
        {
            public string TitleText { get; set; }
            public string DetialText { get; set; }
        }

        [SerializeField]
        private TextMeshProUGUI titleText;

        [SerializeField]
        private TextMeshProUGUI detialText;

        public void Initialize() { }

        public void Apply(Parameter parameter)
        {
            this.titleText.text = parameter.TitleText;
            this.detialText.text = parameter.DetialText;
        }
    }
}
