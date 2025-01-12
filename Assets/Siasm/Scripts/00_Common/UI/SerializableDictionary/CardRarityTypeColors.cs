using System;
using UnityEngine;

namespace Siasm
{
    [Serializable]
    public class CardRarityTypeColorsDictionary : SerializableDictionary<CardRarityType, Color> { }

    [CreateAssetMenu(fileName = "CardRarityTypeColors", menuName ="EnumAssetSetting/CardRarityTypeColors")]
    public class CardRarityTypeColors : ScriptableObject
    {
        [SerializeField]
        private CardRarityTypeColorsDictionary cardRarityTypeColorsDictionary;

        public Color GetColor(CardRarityType cardRarityType)
        {
            return cardRarityTypeColorsDictionary[cardRarityType];
        }
    }
}
