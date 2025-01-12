using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Siasm
{
    public abstract class BaseAudioPlayer : MonoBehaviour
    {
        /// <summary>
        /// NOTE: あまり活用出来ていないので変えてもいいかも
        /// </summary>
        public enum PlayType
        {
            None = 0,
            Single,     // 一つしか鳴らさない＆最初から再生
            Multiple,   // 重複。空いているオーディオリスナーを使用して再生する
            Restart     // 最初から再生し直す。あまり使用していない
        }

        [SerializeField]
        private AudioMixerGroup audioMixerGroup;

        /// <summary>
        /// SerializableDictionary関連の処理から取得したクリップデータを使用する形に変えたい
        /// </summary>
        [SerializeField]
        private AudioClip[] localAudioClips;

        protected virtual int AudioSourceNumber => 1;
        protected virtual int AudioRepetitiveNumber => 1;   // 同じ音を同時に再生できる最大数。主にSEで使用
        protected virtual bool IsLoop => false;
        protected Queue<AudioSource> unUsedAudioSourcePool = new Queue<AudioSource>();    // 未使用のものを格納
        protected Dictionary<string, List<AudioSource>> usingAudioSourceListDictionary = new Dictionary<string, List<AudioSource>>();   // 使用中のものを格納
    
        public AudioMixerGroup AudioMixerGroup => audioMixerGroup;

        public void Initialize()
        {
            for (int i = 0; i < AudioSourceNumber; i++)
            {
                var audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.playOnAwake = false;
                audioSource.loop = IsLoop;
                audioSource.outputAudioMixerGroup = audioMixerGroup;
                unUsedAudioSourcePool.Enqueue(audioSource);
            }
        }

        /// <summary>
        /// NOTE: エディター用の破棄処理
        /// </summary>
        private void OnDestroy()
        {
            unUsedAudioSourcePool = null;
            usingAudioSourceListDictionary = null;
        }

        public AudioClip GetAudioClipOfLocal(string clipName)
        {
            var audioClip = localAudioClips.FirstOrDefault(localAudioClip => localAudioClip.name == clipName);
            if (audioClip == null)
            {
                Debug.LogError($"指定したサウンドクリップはローカルに保持していないため再生できまでんした => clipName: {clipName}");
                return null;
            }

            return audioClip;
        }

        public void PlayClip(PlayType playType, AudioClip audioClip)
        {
            // 使用中かチェックして必要なら止める
            if (IsPlayingClip(audioClip.name))
            {
                switch (playType)
                {
                    case PlayType.Single:
                    case PlayType.Restart:
                        StopClip(audioClip.name);
                        break;
                    case PlayType.Multiple:
                        // 多重再生の場合は停止しない
                        break;
                    case PlayType.None:
                        break;
                }
            }

            // 停止しているクリップのリサイクル処理
            RecycleAudioSourcePool();

            if (unUsedAudioSourcePool.Count <= 0)
            {
                Debug.LogWarning("使用できるAudioSourcenの空きがないため再生できませんでした。AudioSourceの数を追加するかリスタートさせてください");
                return;
            }

            // 中身を設定
            var playAudioSource = unUsedAudioSourcePool.Dequeue();
            playAudioSource.clip = audioClip;
            playAudioSource.volume = 1;  // NOTE: 一応、ここでもボリューム初期化を行う
            playAudioSource.Play();

            SetUsingAudioDictionary(playAudioSource, audioClip.name);
        }

        /// <summary>
        /// 使用中のオーディオリストに登録
        /// </summary>
        /// <param name="audioSource"></param>
        /// <param name="clipName"></param>
        private void SetUsingAudioDictionary(AudioSource audioSource, string clipName)
        {
            if (usingAudioSourceListDictionary.ContainsKey(clipName))
            {
                usingAudioSourceListDictionary[clipName].Add(audioSource);
            }
            else
            {
                var audioSourceList = new List<AudioSource>();
                audioSourceList.Add(audioSource);
                usingAudioSourceListDictionary[clipName] = audioSourceList;
            }
        }

        /// <summary>
        /// 停止しているクリップのリサイクル処理
        /// </summary>
        private void RecycleAudioSourcePool()
        {
            var recycleTarget = new HashSet<string>();

            foreach (var (usingAudioSourceString, usingAudioSourceList) in usingAudioSourceListDictionary)
            {
                var usingAudioSourceNumber = usingAudioSourceList.Count;
                var stopAudioSourceList = new List<AudioSource>();

                // 停止済みのオーディオを破棄リストに追加
                foreach (var usingAudioSource in usingAudioSourceList)
                {
                    if (!usingAudioSource.isPlaying)
                    {
                        stopAudioSourceList.Add(usingAudioSource);
                    }
                }

                // 破棄して再度使用できるようにする
                foreach (var stopAudioSource in stopAudioSourceList)
                {
                    usingAudioSourceList.Remove(stopAudioSource);
                    unUsedAudioSourcePool.Enqueue(stopAudioSource);
                }

                // 使用している数と破棄した数が同じなら辞書から破棄を行う
                if (usingAudioSourceNumber == stopAudioSourceList.Count)
                {
                    recycleTarget.Add(usingAudioSourceString);
                }
            }

            // 辞書から破棄を実行
            foreach (var inUseSourceKey in recycleTarget)
            {
                usingAudioSourceListDictionary.Remove(inUseSourceKey);
            }
        }

        /// <summary>
        /// 使用中のオーディオか確認
        /// </summary>
        private bool IsPlayingClip(string clipName)
        {
            if (!usingAudioSourceListDictionary.ContainsKey(clipName))
            {
                return false;
            }

            // 指定したファイルが存在するかどうかを確認
            var inUseSource = usingAudioSourceListDictionary[clipName];
            if (inUseSource.Exists(sources => sources.isPlaying))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 全ての音を停止させる
        /// </summary>
        public void StopAll()
        {
            foreach (var usingAudioList in usingAudioSourceListDictionary)
            {
                StopClip(usingAudioList.Key);
            }
        }

        /// <summary>
        /// 指定したクリップ（音）の停止と破棄処理
        /// リスト化して登録しているもの（鳴らしているもの）も全て停止する
        /// リスト内の全てのオーディオを停止して辞書から削除する
        /// </summary>
        /// <param name="clipName"></param>
        /// <param name="isFadeOut"></param>
        /// <returns></returns>
        public void StopClip(string clipName)
        {
            foreach (var (usingAudioSourceString, usingAudioSourceList) in usingAudioSourceListDictionary)
            {
                // NOTE: 停止と"unUsedAudioSourcePool"への追加を行う。破棄はforeach終了後に行う
                // NOTE: 指定の数に達成していない場合は停止なし
                if (usingAudioSourceList.Count >= AudioRepetitiveNumber)
                {
                    // 指定の数に到達していた場合は最新のものを停止させる
                    var usingAudioSource = usingAudioSourceList[0];
                    StopAudioSource(usingAudioSource);
                    usingAudioSourceList.RemoveAt(0);
                }
            }
        }

        /// <summary>
        /// オーディオの停止処理
        /// </summary>
        private void StopAudioSource(AudioSource audioSource)
        {
            audioSource.Stop();
            unUsedAudioSourcePool.Enqueue(audioSource);
        }
    }
}
