using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Siasm
{
    public class BattleCardReelView : MonoBehaviour
    {
        [SerializeField]
        private CardReelTypeSpites cardReelTypeSpites;

        [SerializeField]
        private Image cardReelIconImage;

        [SerializeField]
        private TextMeshProUGUI NumberText;

        public void Initialize() { }

        public void Setup(BattleCardModel battleCardModel)
        {
            if (battleCardModel.CardReelType == CardReelType.None)
            {
                return;
            }

            cardReelIconImage.sprite = cardReelTypeSpites.GetSprite(battleCardModel.CardReelType);
        }

        public void Apply(int number)
        {
            NumberText.text = number.ToString();
        }
    }
}
