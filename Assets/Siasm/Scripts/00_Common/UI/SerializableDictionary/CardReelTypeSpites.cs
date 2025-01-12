using System;
using UnityEngine;

namespace Siasm
{
    [Serializable]
    public class CardReelTypeDictionary : SerializableDictionary<CardReelType, Sprite> { }

    [CreateAssetMenu(fileName = "CardReelTypeSpites", menuName ="EnumAssetSetting/CardReelTypeSpites")]
    public class CardReelTypeSpites : ScriptableObject
    {
        [SerializeField]
        private CardReelTypeDictionary cardReelTypeDictionary;

        public Sprite GetSprite(CardReelType cardReelType)
        {
            return cardReelTypeDictionary[cardReelType];
        }
    }
}
