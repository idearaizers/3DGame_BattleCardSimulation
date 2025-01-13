using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Siasm
{
    public class BattleCardListView : BaseView
    {
        public enum ShowTargetType
        {
            None = 0,
            PlayerDeck,
            PlayerCemetery,
            EnemyCemetery
        }

        [SerializeField]
        private TextMeshProUGUI labelText;

        [SerializeField]
        private BattleCardScrollController battleCardScrollController;

        [SerializeField]
        private Button backButton;

        public Action OnBackButtonAction;

        private PlayerBattleCardOperationController playerBattleCardController;
        private EnemyBattleCardOperationController enemyBattleCardController;

        public void Initialize(PlayerBattleCardOperationController playerBattleCardController, EnemyBattleCardOperationController enemyBattleCardController)
        {
            this.playerBattleCardController = playerBattleCardController;
            this.enemyBattleCardController = enemyBattleCardController;

            battleCardScrollController.Initialize();
            backButton.onClick.AddListener(OnBackButton);
        }

        public void Show(ShowTargetType showTargetType)
        {
            Enable();

            switch (showTargetType)
            {
                case ShowTargetType.PlayerDeck:
                    // TODO: 
                    break;

                case ShowTargetType.PlayerCemetery:
                    // TODO: 
                    break;

                case ShowTargetType.EnemyCemetery:
                    // TODO: 
                    break;

                case ShowTargetType.None:
                default:
                    throw new ArgumentOutOfRangeException(nameof(showTargetType));
            }
        }

        private void OnBackButton()
        {
            OnBackButtonAction?.Invoke();
        }
    }
}
