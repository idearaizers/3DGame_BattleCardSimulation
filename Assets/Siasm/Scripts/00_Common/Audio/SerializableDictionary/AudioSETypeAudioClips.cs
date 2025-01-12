using System;
using UnityEngine;

namespace Siasm
{
    [Serializable]
    public class AudioSETypeAudioClipDictionary : SerializableDictionary<AudioSEType, AudioClip> { }

    [CreateAssetMenu(fileName = "AudioSETypeAudioClips", menuName ="EnumAssetSetting/AudioSETypeAudioClips")]
    public class AudioSETypeAudioClips : ScriptableObject
    {
        [SerializeField]
        private AudioSETypeAudioClipDictionary audioSETypeAudioClipDictionary;

        public AudioClip GetAudioClip(AudioSEType audioSEType)
        {
            return audioSETypeAudioClipDictionary[audioSEType];
        }
    }
}
