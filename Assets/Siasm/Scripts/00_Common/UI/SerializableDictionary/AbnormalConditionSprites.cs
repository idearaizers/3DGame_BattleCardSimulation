using System;
using UnityEngine;

namespace Siasm
{
    [Serializable]
    public class AbnormalConditionSpritesDictionary : SerializableDictionary<AbnormalConditionType, Sprite> { }

    [CreateAssetMenu(fileName = "AbnormalConditionSprites", menuName ="EnumAssetSetting/AbnormalConditionSprites")]
    public class AbnormalConditionSprites : ScriptableObject
    {
        [SerializeField]
        private AbnormalConditionSpritesDictionary abnormalConditionSpritesDictionary;

        public Sprite GetSprite(AbnormalConditionType abnormalConditionType)
        {
            return abnormalConditionSpritesDictionary[abnormalConditionType];
        }
    }
}
