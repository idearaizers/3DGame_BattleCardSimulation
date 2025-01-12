using UnityEngine;
using TMPro;

namespace Siasm
{
    public class EnemyIntroductionDirection : BaseView
    {
        private const string levelStringFormat = "Lv.{0}";

        [SerializeField]
        private TextMeshProUGUI enemyNameText;

        [SerializeField]
        private TextMeshProUGUI enemyLevelText;

        public void Initialize() { }

        public void Setup() { }

        public void Show(string enemyName, int enemyLevel)
        {
            enemyNameText.text = enemyName;
            enemyLevelText.text = string.Format(levelStringFormat, enemyLevel);
            Enable();
        }

        public void Hide()
        {
            Disable();
        }
    }
}
