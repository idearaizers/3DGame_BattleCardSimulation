using UnityEngine;

namespace Siasm
{
    public sealed class SEAudioPlayer : BaseAudioPlayer
    {
        [SerializeField]
        private AudioSETypeAudioClips audioSETypeAudioClips;

        protected override int AudioSourceNumber => 10;
        protected override int AudioRepetitiveNumber => 3;
        protected override bool IsLoop => false;

        public AudioSETypeAudioClips AudioSETypeAudioClips => audioSETypeAudioClips;
    }
}
