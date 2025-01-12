using UnityEngine;
using TMPro;

namespace Siasm
{
    public class DateDirection : MonoBehaviour
    {
        private string dateStringFormatText = "{0}日目";

        [SerializeField]
        private TextMeshProUGUI dateText;

        public void Initialize(int dateNumber)
        {
            if (dateNumber == 1)
            {
                // 最初の日だけ文言を変える
                this.dateText.text = "初日";
            }
            else
            {
                this.dateText.text = string.Format(dateStringFormatText, dateNumber);
            }
        }
    }
}
