using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace AdventureKit.Utils
{
    public class SoundManager : SingletonMonoBehaviour<SoundManager>
    {
        [Header("Themes")]
        [SerializeField]
        private List<AudioClip> m_GameSounds;

        [Header("SFX")]
        [SerializeField]
        private List<AudioClip> m_SFX;

        [Header("Volume")]
        [SerializeField]
        private float m_MusicVolume = 0.1f;
        [SerializeField]
        private float m_SFXVolume = 0.3f;
        [SerializeField]
        private float m_FadeDuration = 1f;

        private bool mSoundEnable;
        private bool mSFXEnable;

        protected AudioSource music_chanel_0;
        protected AudioSource music_chanel_1;
        private AudioSource current_music_chanel;

        protected AudioSource fx_chanel;

        protected override void Initialize()
        {
            base.Initialize();

            music_chanel_0 = (AudioSource)gameObject.AddComponent<AudioSource>();
            music_chanel_0.rolloffMode = AudioRolloffMode.Linear;
            music_chanel_0.minDistance = 500f;
            music_chanel_0.volume = 0;

            music_chanel_1 = (AudioSource)gameObject.AddComponent<AudioSource>();
            music_chanel_1.rolloffMode = AudioRolloffMode.Linear;
            music_chanel_1.minDistance = 500f;
            music_chanel_1.volume = 0;

            fx_chanel = (AudioSource)gameObject.AddComponent<AudioSource>();
            fx_chanel.rolloffMode = AudioRolloffMode.Linear;
            fx_chanel.minDistance = 500f;
            fx_chanel.volume = m_SFXVolume;

            current_music_chanel = music_chanel_0;

            mSoundEnable = true;
            mSFXEnable = true;
        }

        #region GET SOUNDS
        //Return an specific theme
        public AudioClip GetSoundByName(string theme_name)
        {
            for (int i = 0; i < m_GameSounds.Count; i++)
            {
                if (m_GameSounds[i].name.Equals(theme_name))
                    return m_GameSounds[i];
            }

            return null;
        }

        //Return an specific sfx
        public AudioClip GetSFXByName(string sfx_name)
        {
            for (int i = 0; i < m_SFX.Count; i++)
            {
                if (m_SFX[i].name.Equals(sfx_name))
                    return m_SFX[i];
            }

            return null;
        }
        #endregion

        #region MUSIC FUNCTIONS
        //Play a music theme
        public void PlayMusic(AudioClip sound, bool loop)
        {
            //Set as default music chanel the chanel zero
            if (current_music_chanel == null)
                current_music_chanel = music_chanel_0;

            //If the current channel is playing stop it
            if (current_music_chanel.isPlaying)
                StopMusic();

            current_music_chanel.clip = sound;
            current_music_chanel.loop = loop;
            current_music_chanel.volume = m_MusicVolume;

                current_music_chanel.Play();
        }

        //Checl if the current music channel is playing
        public bool IsCurrentMusicChanelPlaying()
        {
            return current_music_chanel.isPlaying;
        }

        public bool SoundEnabled
        {
            get
            {
                return mSoundEnable;
            }
        }

        //Make a fade out/in effect
        public void PlayMusicWithFadeEffect(AudioClip sound, bool loop)
        {
            if (mSoundEnable)
                StartCoroutine(_PlayMusicWithFadeEffect(sound, loop));
        }

        IEnumerator _PlayMusicWithFadeEffect(AudioClip sound, bool loop)
        {
            //fade out
            while (current_music_chanel.volume >= 0.01f)
            {
                current_music_chanel.volume -= Time.deltaTime / 1f;
                yield return null;
            }


            //Make sure that the volume is zero
            current_music_chanel.volume = 0;

            //Wait before start fade in
            yield return new WaitForSeconds(m_FadeDuration);

            //Change the current music chanel
            if (current_music_chanel == music_chanel_0)
                current_music_chanel = music_chanel_1;
            else
                current_music_chanel = music_chanel_0;

            //Configure the current music chanel to reproduce the new sound
            current_music_chanel.clip = sound;
            current_music_chanel.loop = loop;

            if (mSoundEnable)
                current_music_chanel.Play();

            //fade in
            while (current_music_chanel.volume <= m_MusicVolume)
            {
                current_music_chanel.volume += Time.deltaTime / 1f;
                yield return null;
            }

            //Make sure that the volume is the desired volume
            current_music_chanel.volume = m_MusicVolume;

            yield return null;

        }

        public void FadeOutMusicChanel()
        {
            StartCoroutine("_FadeOutMusicChanel");
        }

        private IEnumerator _FadeOutMusicChanel()
        {
            //fade out
            while (current_music_chanel.volume >= 0.01f)
            {
                current_music_chanel.volume -= Time.deltaTime / 1f;
                yield return null;
            }

            //Make sure that the volume is zero
            current_music_chanel.volume = 0;
        }

        public void FadeInMusicChanel()
        {
            StartCoroutine("_FadeInMusicChanel");
        }

        private IEnumerator _FadeInMusicChanel()
        {
            //fade in
            while (current_music_chanel.volume <= m_MusicVolume)
            {
                current_music_chanel.volume += Time.deltaTime / 1f;
                yield return null;
            }

            current_music_chanel.volume = m_MusicVolume;
        }


        //Turn to stop the music chanel
        public void StopMusic()
        {
            current_music_chanel.Stop();
            mSoundEnable = false;
        }

        //Turn to play the music chanel
        public void ResumeMusic()
        {
            current_music_chanel.Play();
            mSoundEnable = true;
        }

        //Pause the music chanel
        public void PauseMusic()
        {
            current_music_chanel.Pause();
            mSoundEnable = false;
        }

        //Set the volume of the music chanel
        public void SetMusicVolume(float vol)
        {
            current_music_chanel.volume = vol;
        }
        #endregion

        #region FX FUNCTIONS
        public void PlayFXSound(AudioClip sound)
        {
            fx_chanel.loop = false;
            fx_chanel.volume = m_SFXVolume;

            if (mSFXEnable)
                fx_chanel.PlayOneShot(sound);
        }

        public bool SFXEnabled
        {
            get
            {
                return mSFXEnable;
            }
        }

        //Turn to stop the music chanel
        public void StopFx()
        {
            fx_chanel.Stop();
            mSFXEnable = false;
        }

        //Turn to play the music chanel
        public void ResumeFx()
        {
            fx_chanel.Play();
            mSFXEnable = true;
        }

        //Pause the music chanel
        public void PauseFx()
        {
            fx_chanel.Pause();
            mSFXEnable = false;
        }

        //Set the volume of the music chanel
        public void SetFxVolume(float vol)
        {
            fx_chanel.volume = vol;
        }
        #endregion

        #region AUDIOLISTENER
        //Increment the audio listener volume progresively
        public void FadeIn()
        {
            StartCoroutine("fadeIn");
        }
        private IEnumerator fadeIn()
        {
            while (AudioListener.volume < 1)
            {
                AudioListener.volume += 1 * Time.deltaTime * 2;
                yield return null;
            }

            AudioListener.volume = 1;
        }

        //Decrement the audio listener volume progresively
        public void FadeOut()
        {
            StartCoroutine("fadeOut");
        }

        private IEnumerator fadeOut()
        {
            while (AudioListener.volume > 0.1f)
            {
                AudioListener.volume -= 1 * Time.deltaTime * 2;
                yield return null;
            }

            AudioListener.volume = 0;
        }

        //Set audio listener volume
        public void SetVolume(float vol)
        {
            AudioListener.volume = vol;
        }

        //Mute audio listener
        public void Mute()
        {
            if (AudioListener.volume > 0)
            {
                AudioListener.volume = 0f;
                mSoundEnable = false;
            }
            else
            {
                AudioListener.volume = 1f;
                mSoundEnable = true;
            }
        }

        //Set the audio listener using fade efect
        public void FadeAudioListenerVolume()
        {
            if (mSoundEnable)
            {
                FadeOut();
            }
            else
            {
                FadeIn();
            }
        }

        public void StopAllSounds()
        {
            music_chanel_0.Stop();
            music_chanel_1.Stop();
            current_music_chanel.Stop();

            fx_chanel.Stop();

        }
        #endregion

        public bool AudioSourceMuted { get { return current_music_chanel.mute;  } }

        public void ResumeAudioSource()
        {
            mSoundEnable = true;
            current_music_chanel.mute = false;
        }

        public void MuteAudioSource()
        {
            mSoundEnable = false;
            current_music_chanel.mute = true;
        }
    }

}
