using System;
using UnityEngine;

namespace Siasm
{
    public class CardTypeView : MonoBehaviour
    {
        [SerializeField]
        private GameObject attackThumbnail;

        [SerializeField]
        private GameObject guardThumbnail;

        public void Apply(CardReelType cardReelType)
        {
            switch (cardReelType)
            {
                case CardReelType.Attack:
                    attackThumbnail.gameObject.SetActive(true);
                    guardThumbnail.gameObject.SetActive(false);
                    break;
                case CardReelType.Guard:
                    attackThumbnail.gameObject.SetActive(false);
                    guardThumbnail.gameObject.SetActive(true);
                    break;
                case CardReelType.None:
                default:
                    throw new ArgumentOutOfRangeException(nameof(cardReelType));
            }
        }
    }
}
