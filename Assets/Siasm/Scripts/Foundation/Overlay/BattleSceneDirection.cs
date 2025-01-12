using UnityEngine;
using TMPro;

namespace Siasm
{
    public class BattleSceneDirection : BaseView
    {
        [SerializeField]
        private TextMeshProUGUI enemyNameText;

        public void ShowEnemyName(string enemyName)
        {
            Enable();
            enemyNameText.text = enemyName;
        }
    }
}
