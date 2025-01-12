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
                    // labelText.text = "プレイヤーのデッキに残っているカード一覧";
                    // var battleCardModelOfDeckCards = playerBattleCardController.PlayerDeckBattleCardModel.BattleCardModelOfDeckCards;
                    // if (battleCardModelOfDeckCards != null && battleCardModelOfDeckCards.Count > 0)
                    // {
                    //     battleCardScrollController.Setup(battleCardModelOfDeckCards.ToArray());
                    // }
                    // else
                    // {
                    //     var battleCardModels = new BattleCardModel[]{};
                    //     battleCardScrollController.Setup(battleCardModels);
                    // }
                    break;

                case ShowTargetType.PlayerCemetery:
                    {
                        // labelText.text = "プレイヤーの墓地にあるカード一覧";
                        // var battleCardModelOfCemeteryCards = playerBattleCardController.PlayerDeckBattleCardModel.BattleCardModelOfCemeteryCards;
                        // if (battleCardModelOfCemeteryCards != null && battleCardModelOfCemeteryCards.Count > 0)
                        // {
                        //     battleCardScrollController.Setup(battleCardModelOfCemeteryCards.ToArray());
                        // }
                        // else
                        // {
                        //     var battleCardModels = new BattleCardModel[]{};
                        //     battleCardScrollController.Setup(battleCardModels);
                        // }
                    }
                    break;

                case ShowTargetType.EnemyCemetery:
                    {
                        // labelText.text = "エネミーの墓地にあるカード一覧";
                        // var battleCardModelOfCemeteryCards = enemyBattleCardController.BaseFighterBattleCardModel.BattleCardModelOfCemeteryCards;
                        // if (battleCardModelOfCemeteryCards != null && battleCardModelOfCemeteryCards.Count > 0)
                        // {
                        //     battleCardScrollController.Setup(battleCardModelOfCemeteryCards.ToArray());
                        // }
                        // else
                        // {
                        //     var battleCardModels = new BattleCardModel[]{};
                        //     battleCardScrollController.Setup(battleCardModels);
                        // }
                    }
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
