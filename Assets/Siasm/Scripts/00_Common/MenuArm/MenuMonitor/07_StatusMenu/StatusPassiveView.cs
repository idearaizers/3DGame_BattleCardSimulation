using UnityEngine;
using TMPro;

namespace Siasm
{
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

            var customPassiveSkillModels = new MenuCustomPassiveSkillModel[]
            {
                new MenuCustomPassiveSkillModel(),
                new MenuCustomPassiveSkillModel(),
                new MenuCustomPassiveSkillModel()
            };
            customPassiveSkillScrollController.Setup(customPassiveSkillModels);

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
                currentSelectedGameObject.GetComponent<MenuCustomPassiveSkillView>()?.ChangeActiveOfSelectedImage(false);
                currentSelectedGameObject.GetComponent<MenuOwnPassiveSkillCellView>()?.ChangeActiveOfSelectedImage(false);
            }

            currentSelectedGameObject = selectedGameObject;

            // カード詳細用のサイドアームを開く
            sideArmSwitcherPrefab.PlayOpenDisplayAnimation(
                "ダミーメッセージ",
                () =>
                {
                    // TODO: 決定時の処理
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
        }

        private void OnDragOwnCard()
        {
            Debug.Log("TODO: OnDragOwnCard");
        }
    }
}
