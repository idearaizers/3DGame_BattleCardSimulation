using System;
using UnityEngine;

namespace Siasm
{
    [Serializable]
    public class BattleFighterAnimationTypeSpriteDictionary : SerializableDictionary<BattleFighterAnimationType, Sprite> { }

    [CreateAssetMenu(fileName = "BattleFighterAnimationTypeSprites_1000", menuName ="EnumAssetSetting/BattleFighterAnimationTypeSprites")]
    public class BattleFighterAnimationTypeSprites : ScriptableObject
    {
        [SerializeField]
        private BattleFighterAnimationTypeSpriteDictionary battleFighterAnimationTypeSpriteDictionary;

        public Sprite GetSprite(BattleFighterAnimationType battleFighterAnimationType)
        {
            return battleFighterAnimationTypeSpriteDictionary[battleFighterAnimationType];
        }
    }
}
