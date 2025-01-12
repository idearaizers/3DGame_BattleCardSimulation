#if UNITY_EDITOR

using UnityEditor;

namespace Siasm
{
    [CustomPropertyDrawer(typeof(AttributeTypeColorsDictionary))]
    [CustomPropertyDrawer(typeof(AudioBGMTypeAudioClipDictionary))]
    [CustomPropertyDrawer(typeof(AudioSETypeAudioClipDictionary))]
    [CustomPropertyDrawer(typeof(AbnormalConditionSpritesDictionary))]
    [CustomPropertyDrawer(typeof(BattleCommonSETypeAudioClipDictionary))]
    [CustomPropertyDrawer(typeof(BattleFighterAnimationTypeSpriteDictionary))]
    [CustomPropertyDrawer(typeof(CardSpecTypeColorsDictionary))]
    [CustomPropertyDrawer(typeof(CardRarityTypeColorsDictionary))]
    [CustomPropertyDrawer(typeof(CardReelTypeDictionary))]
    [CustomPropertyDrawer(typeof(DialogMenuPrefabDictionary))]
    [CustomPropertyDrawer(typeof(MenuPrefabTypePrefabDictionary))]
    [CustomPropertyDrawer(typeof(PassiveAbilityTypeSpritesDictionary))]
    public class AnySerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer { }
}

#endif
