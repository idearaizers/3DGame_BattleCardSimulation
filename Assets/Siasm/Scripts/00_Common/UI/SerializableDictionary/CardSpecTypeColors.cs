using System;
using UnityEngine;

namespace Siasm
{
    [Serializable]
    public class CardSpecTypeColorsDictionary : SerializableDictionary<CardSpecType, Color> { }

    [CreateAssetMenu(fileName = "CardSpecTypeColors", menuName ="EnumAssetSetting/CardSpecTypeColors")]
    public class CardSpecTypeColors : ScriptableObject
    {
        [SerializeField]
        private CardSpecTypeColorsDictionary cardSpecTypeColorsDictionary;

        public Color GetColor(CardSpecType cardSpecType)
        {
            return cardSpecTypeColorsDictionary[cardSpecType];
        }
    }
}
