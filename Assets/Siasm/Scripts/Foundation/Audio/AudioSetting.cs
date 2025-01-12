using UnityEngine;

namespace Siasm
{
    [CreateAssetMenu(fileName = "AudioSetting", menuName = "ScriptableObjects/AudioSetting")]
    public class AudioSetting : ScriptableObject
    {
        public float DefaultBGMVolume = 1.0f;
        public float DefaultSEVolume = 1.0f;
        public float DefaultVoiceMVolume = 1.0f;
        public float AudioPoolSize = 10.0f;
    }
}
