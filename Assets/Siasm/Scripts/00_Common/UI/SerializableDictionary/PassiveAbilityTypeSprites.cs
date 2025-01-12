using System;
using UnityEngine;

namespace Siasm
{
    [Serializable]
    public class PassiveAbilityTypeSpritesDictionary : SerializableDictionary<PassiveAbilityType, Sprite> { }

    [CreateAssetMenu(fileName = "PassiveAbilityTypeSprites", menuName ="EnumAssetSetting/PassiveAbilityTypeSprites")]
    public class PassiveAbilityTypeSprites : ScriptableObject
    {
        [SerializeField]
        private PassiveAbilityTypeSpritesDictionary passiveAbilityTypeSpritesDictionary;

        public Sprite GetSprite(PassiveAbilityType passiveAbilityType)
        {
            return passiveAbilityTypeSpritesDictionary[passiveAbilityType];
        }
    }
}
