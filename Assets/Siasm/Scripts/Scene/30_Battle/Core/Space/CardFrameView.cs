using UnityEngine;
using UnityEngine.UI;

namespace Siasm
{
    public class CardFrameView : MonoBehaviour
    {
        [SerializeField]
        private CardSpecTypeColors cardSpecTypeColors;

        [Space]
        [SerializeField]
        private Image frameImage;

        public void Apply(CardSpecType cardSpecType)
        {
            frameImage.color = cardSpecTypeColors.GetColor(cardSpecType);
        }
    }
}
