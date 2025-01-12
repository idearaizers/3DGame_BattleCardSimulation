using System;
using UnityEngine;

namespace Siasm
{
    [Serializable]
    public class BattleCommonSETypeAudioClipDictionary : SerializableDictionary<AudioSEType, AudioClip> { }

    [CreateAssetMenu(fileName = "BattleCommonSETypeAudioClips", menuName ="EnumAssetSetting/BattleCommonSETypeAudioClips")]
    public class BattleCommonSETypeAudioClips : ScriptableObject
    {
        public static readonly string AssetName = "BattleCommonSETypeAudioClips";

        [SerializeField]
        private BattleCommonSETypeAudioClipDictionary battleCommonSETypeAudioClipDictionary;

        public AudioClip GetAudioClip(AudioSEType audioSEType)
        {
            return battleCommonSETypeAudioClipDictionary[audioSEType];
        }
    }
}
