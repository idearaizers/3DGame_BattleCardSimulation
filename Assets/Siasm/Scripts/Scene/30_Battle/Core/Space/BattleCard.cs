using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Siasm
{
    public class BattleCard : MonoBehaviour
    {
        /// <summary>
        /// ドラッグ中にカードを半透明にする際に使用
        /// </summary>
        [SerializeField]
        private CanvasGroup canvasGroup;

        [Space]
        [SerializeField]
        private CardFrameView cardFrameView;

        [SerializeField]
        private TextMeshProUGUI cardNameText;

        [SerializeField]
        private Image cardImage;

        [SerializeField]
        private TextMeshProUGUI cardCostText;

        [SerializeField]
        private TextMeshProUGUI descriptionText;

        [SerializeField]
        private CardTypeView cardTypeView;

        public CanvasGroup CanvasGroup => canvasGroup;

        public void Initialize()
        {
            // NOTE: 特になし
        }

        public void Apply(BattleCardModel battleCardModel)
        {
            cardFrameView.Apply(battleCardModel.CardSpecType);
            cardNameText.text = battleCardModel.CardName;
            // cardImage = battleCardModel.CardImage;
            cardCostText.text = battleCardModel.CostNumber.ToString();
            cardTypeView.Apply(battleCardModel.CardReelType);
            descriptionText.text = battleCardModel.DescriptionText;
        }

        public void SetCanvasGroupAlpha(float number)
        {
            canvasGroup.alpha = number;
        }
    }
}
