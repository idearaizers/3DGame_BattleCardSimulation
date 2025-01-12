using System;
using UnityEngine;

namespace Siasm
{
    [Serializable]
    public class AttributeTypeColorsDictionary : SerializableDictionary<EmotionAttributeType, Color> { }

    [CreateAssetMenu(fileName = "AttributeTypeColors", menuName ="EnumAssetSetting/AttributeTypeColors")]
    public class AttributeTypeColors : ScriptableObject
    {
        [SerializeField]
        private AttributeTypeColorsDictionary attributeTypeColorsDictionary;

        public Color GetColor(EmotionAttributeType attributeType)
        {
            return attributeTypeColorsDictionary[attributeType];
        }
    }
}
