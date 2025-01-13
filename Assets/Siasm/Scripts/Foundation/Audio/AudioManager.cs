using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Siasm
{
    public class AudioManager : SingletonMonoBehaviour<AudioManager>
    {
        private const string ExposedMasterVolume = "MasterVolume";
        private const string ExposedBGMVolume = "BGMVolume";
        private const string ExposedSEVolume = "SEVolume";
        private const string ExposedVoiceVolume = "VoiceVolume";

        public enum ExposedType
        {
            None = 0,
            Master,
            BGM,
            SE,
            Voice
        }

        [SerializeField]
        private AudioMixerGroup audioMixerGroupOfMaster;

        [SerializeField]
        private BGMAudioPlayer bgmAudioPlayer;

        [SerializeField]
        private SEAudioPlayer seAudioPlayer;

        [SerializeField]
        private VoiceAudioPlayer voiceAudioPlayer;

        public void Initialize()
        {
            bgmAudioPlayer.Initialize();
            seAudioPlayer.Initialize();
            voiceAudioPlayer.Initialize();
        }

        /// <summary>
        /// NOTE: Initializeのタイミングで使用すると反映されないため注意
        /// NOTE: StartAsyncのタイミングで適用させている
        /// </summary>
        public void SetCurrentSoundSetting()
        {
            var soundSetting = PlayerPrefsStorage.Get<SoundSetting>();
            SetAudioMixerVolume(ExposedType.Master, soundSetting.MasterVolumeNumber);
            SetAudioMixerVolume(ExposedType.BGM, soundSetting.BGMVolumeNumber);
            SetAudioMixerVolume(ExposedType.SE, soundSetting.SEVolumeNumber);
            SetAudioMixerVolume(ExposedType.Voice, soundSetting.VoiceVolumeNumber);
        }

        public void SetAudioMixerVolume(ExposedType exposedType, int volumeNumber)
        {
            const float minDecibe = -80.0f;
            const float decibeRate = -2.0f;

            // NOTE: 値に係数をかけただけだとふり幅が大きすぎるので幅を調整しています
            var decibelNumber = minDecibe;
            if (volumeNumber != 0)
            {
                decibelNumber = (10 - volumeNumber) * decibeRate;
            }

            switch (exposedType)
            {
                case ExposedType.Master:
                    audioMixerGroupOfMaster.audioMixer.SetFloat(ExposedMasterVolume, decibelNumber);
                    break;
                case ExposedType.BGM:
                    bgmAudioPlayer.AudioMixerGroup.audioMixer.SetFloat(ExposedBGMVolume, decibelNumber);
                    break;
                case ExposedType.SE:
                    seAudioPlayer.AudioMixerGroup.audioMixer.SetFloat(ExposedSEVolume, decibelNumber);
                    break;
                case ExposedType.Voice:
                    voiceAudioPlayer.AudioMixerGroup.audioMixer.SetFloat(ExposedVoiceVolume, decibelNumber);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(exposedType));
            }
        }

        public void PlayBGMOfLocal(BaseAudioPlayer.PlayType playType, AudioBGMType audioBGMType)
        {
            var audioClip = bgmAudioPlayer.AudioBGMTypeAudioClips.GetAudioClip(audioBGMType);
            bgmAudioPlayer.PlayClip(playType, audioClip);
        }

        public void StopAllBGM()
        {
            bgmAudioPlayer.StopAll();
        }

        public void PlaySEOfLocal(BaseAudioPlayer.PlayType playType, AudioSEType audioSEType)
        {
            var audioClip = seAudioPlayer.AudioSETypeAudioClips.GetAudioClip(audioSEType);
            seAudioPlayer.PlayClip(playType, audioClip);
        }

        public void PlaySEOfAudioClip(BaseAudioPlayer.PlayType playType, AudioClip audioClip)
        {
            seAudioPlayer.PlayClip(playType, audioClip);
        }

        public void PlayVoiceOfLocal(BaseAudioPlayer.PlayType playType, string clipName)
        {
            var audioClip = bgmAudioPlayer.GetAudioClipOfLocal(clipName);
            voiceAudioPlayer.PlayClip(playType, audioClip);
        }

        public void FadeOutBGM()
        {
            bgmAudioPlayer.FadeOutBGM();
        }
    }
}
