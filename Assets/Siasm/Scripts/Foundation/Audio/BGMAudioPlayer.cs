using System.Collections;
using UnityEngine;

namespace Siasm
{
    public sealed class BGMAudioPlayer : BaseAudioPlayer
    {
        [SerializeField]
        private AudioBGMTypeAudioClips audioBGMTypeAudioClips;

        protected override int AudioSourceNumber => 1;
        protected override bool IsLoop => true;

        public AudioBGMTypeAudioClips AudioBGMTypeAudioClips => audioBGMTypeAudioClips;

        public void FadeOutBGM()
        {
            foreach (var (usingAudioSourceString, usingAudioSourceList) in usingAudioSourceListDictionary)
            {
                foreach (var usingAudioSource in usingAudioSourceList)
                {
                    StartCoroutine(FadeOutBGMCoroutine(usingAudioSource));
                }
            }
        }

        private IEnumerator FadeOutBGMCoroutine(AudioSource usingAudioSource)
        {
            while (usingAudioSource.volume > 0)
            {
                usingAudioSource.volume -= Time.deltaTime * 1.0f;
                yield return null;
            }

            usingAudioSource.Stop();
            unUsedAudioSourcePool.Enqueue(usingAudioSource);
        }
    }
}
