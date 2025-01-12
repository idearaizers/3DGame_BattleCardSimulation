using UnityEngine;
using TMPro;

namespace Siasm
{
    /// <summary>
    /// 仮
    /// </summary>
    public class BattleFighterPassiveModel
    {
        public int CurrentCostNumber { get; set; }
        public int MaxCostNumber { get; set; }
    }

    public class StatusPassiveView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI totalPassiveCostText;

        [SerializeField]
        private CustomPassiveSkillScrollController customPassiveSkillScrollController;

        [SerializeField]
        private OwnPassiveSkillScrollController ownPassiveSkillScrollController;

        [SerializeField]
        private PassiveSkillDragController passiveSkillDragController;

        private GameObject currentSelectedGameObject;
        private SideArmSwitcherPrefab sideArmSwitcherPrefab;

        public void Initialize(BaseCameraController baseCameraController, SideArmSwitcherPrefab sideArmSwitcherPrefab)
        {
            this.sideArmSwitcherPrefab = sideArmSwitcherPrefab;

            customPassiveSkillScrollController.Initialize();
            customPassiveSkillScrollController.OnClickAction = OnSelectedBattleCard;
            customPassiveSkillScrollController.MenuCardScrollRect.OnDraggingAction = OnDragging;
            customPassiveSkillScrollController.MenuCardScrollRect.OnEndDragAction = OnEndDrag;

            ownPassiveSkillScrollController.Initialize();
            ownPassiveSkillScrollController.OnClickAction = OnSelectedBattleCard;
            ownPassiveSkillScrollController.MenuCardScrollRect.OnDraggingAction = OnDragging;
            ownPassiveSkillScrollController.MenuCardScrollRect.OnEndDragAction = OnEndDrag;

            passiveSkillDragController.Initialize(baseCameraController);
            passiveSkillDragController.OnDragDeckCard = OnDragDeckCard;
            passiveSkillDragController.OnDragOwnCard = OnDragOwnCard;
        }

        public void Setup(BattleFighterPassiveModel battleFighterPassiveModel)
        {
            if (battleFighterPassiveModel == null)
            {
                return;
            }

            totalPassiveCostText.text = $"コスト: {battleFighterPassiveModel.CurrentCostNumber} / {battleFighterPassiveModel.MaxCostNumber}";

            // 仮
            var customPassiveSkillModels = new MenuCustomPassiveSkillModel[]
            {
                new MenuCustomPassiveSkillModel(),
                new MenuCustomPassiveSkillModel(),
                new MenuCustomPassiveSkillModel()
            };
            customPassiveSkillScrollController.Setup(customPassiveSkillModels);

            // 仮
            var ownPassiveModels = new MenuOwnPassiveModel[]
            {
                new MenuOwnPassiveModel(),
                new MenuOwnPassiveModel(),
                new MenuOwnPassiveModel()
            };
            ownPassiveSkillScrollController.Setup(ownPassiveModels);

            passiveSkillDragController.Setup();
        }

        private void OnSelectedBattleCard(GameObject selectedGameObject, BattleCardModel battleCardModel)
        {
            // 選択済みのものがまたは同じものでなければ非選択状態にする
            if (currentSelectedGameObject != null &&
                currentSelectedGameObject != selectedGameObject)
            {
                // 仮
                currentSelectedGameObject.GetComponent<MenuCustomPassiveSkilllView>()?.ChangeActiveOfSelectedImage(false);
                currentSelectedGameObject.GetComponent<MenuOwnPassiveSkillCellView>()?.ChangeActiveOfSelectedImage(false);
            }

            currentSelectedGameObject = selectedGameObject;

            // カード詳細用のサイドアームを開く
            sideArmSwitcherPrefab.PlayOpenDisplayAnimation(
                "ダミーメッセージ",
                () =>
                {
                    // OnYesButton(selectedIndex).Forget();
                },
                () =>
                {
                    // NOTE: キャンセルの際は何もしない
                }
            );
        }

        private void OnDragging(MenuCardScrollRect.ScrollType scrollType)
        {
            // カードの表示
            if (!passiveSkillDragController.CurrentDraggingCardPrefab)
            {
                passiveSkillDragController.ShowDraggingCard(scrollType);
            }

            // 移動処理
            passiveSkillDragController.MovingDragCard();
        }

        private void OnEndDrag()
        {
            // 移動終了処理
            passiveSkillDragController.MovedDragCard();
        }

        private void OnDragDeckCard()
        {
            Debug.Log("TODO: OnDragDeckCard");

            // var selectedIndex = holdBattleCardModelList.IndexOf(selectedHoldBattleCardModel);

            // // 所持側のカードを整理
            // var targetHoldBattleCardModel = holdBattleCardModelList.FirstOrDefault(holdBattleCardModel => holdBattleCardModel.BattleCardModel.CardId == selectedHoldBattleCardModel.BattleCardModel.CardId);
            // if (targetHoldBattleCardModel != null)
            // {
            //     // TODO: カウントがゼロになった時の処理を決めておく
            //     targetHoldBattleCardModel.HoldNumber--;
            // }

            // holdCardScrollController.Setup(holdBattleCardModelList.ToArray());

            // // デッキ側のカード整理
            // deckBattleCardModelList.Add(selectedHoldBattleCardModel.BattleCardModel);

            // // カードの並び順を整える
            // deckBattleCardModelList.Sort((a, b) => a.CardId - b.CardId);

            // deckCardScrollController.Setup(deckBattleCardModelList.ToArray());
        }

        private void OnDragOwnCard()
        {
            Debug.Log("TODO: OnDragOwnCard");

            // var selectedIndex = deckBattleCardModelList.IndexOf(selectedBattleCardModel);

            // // デッキ側のカード整理
            // deckBattleCardModelList.RemoveAt(selectedIndex);
            // deckCardScrollController.Setup(deckBattleCardModelList.ToArray());

            // // 所持側のカードを整理
            // var holdBattleCardModel = holdBattleCardModelList.FirstOrDefault(holdBattleCardModel => holdBattleCardModel.BattleCardModel.CardId == selectedBattleCardModel.CardId);
            // if (holdBattleCardModel != null)
            // {
            //     holdBattleCardModel.HoldNumber++;
            // }
            // else
            // {
            //     holdBattleCardModelList.Add(new HoldBattleCardModel
            //     {
            //         BattleCardModel = selectedBattleCardModel,
            //         HoldNumber = 1
            //     });
            // }

            // holdCardScrollController.Setup(holdBattleCardModelList.ToArray());
        }
    }
}
