using System;
using UnityEngine;

namespace Siasm
{
    [Serializable]
    public class AudioBGMTypeAudioClipDictionary : SerializableDictionary<AudioBGMType, AudioClip> { }

    [CreateAssetMenu(fileName = "AudioBGMTypeAudioClips", menuName ="EnumAssetSetting/AudioBGMTypeAudioClips")]
    public class AudioBGMTypeAudioClips : ScriptableObject
    {
        [SerializeField]
        private AudioBGMTypeAudioClipDictionary audioBGMTypeAudioClipDictionary;

        public AudioClip GetAudioClip(AudioBGMType audioBGMType)
        {
            return audioBGMTypeAudioClipDictionary[audioBGMType];
        }
    }
}
