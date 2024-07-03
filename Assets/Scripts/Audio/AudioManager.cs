using System;
using UnityEngine;

namespace Audio
{
    public class AudioManager: MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }
        
        [Header("Audio Sources")]
        [SerializeField] AudioSource musicSource;
        [SerializeField] AudioSource SFxSource;
        
        [Header("Audio Clips")]
        public AudioClip defaultBackgroundMusic;
        public AudioClip mainMenuMusic;
        public AudioClip level1Music;
        public AudioClip level2Music;
        public AudioClip level3Music;
        public AudioClip buttonClickSfx;
        public AudioClip toggleSfx;
        public AudioClip tapSfx;
        public AudioClip playerMove1Sfx;
        public AudioClip playerMove2Sfx;
        public AudioClip playerMove3Sfx;
        public AudioClip playerMove4Sfx;
        public AudioClip playerMove5Sfx;
        public AudioClip crateMove1Sfx;
        public AudioClip crateMove2Sfx;
        public AudioClip crateMove3Sfx;
        public AudioClip crateMove4Sfx;
        public AudioClip crateMove5Sfx;
        public AudioClip crateTargetInSfx;
        public AudioClip crateTargetOutSfx;
        public AudioClip levelCompleteSfx;
        public AudioClip newLevelSfx;
        
        private AudioClip _forcedSfx;

        private void Awake()
        {
            EnsureSingleton();
        }

        public void PlayMusic(AudioClip clip = null)
        {
            if (clip == null) return;
            
            musicSource.clip = clip;
            musicSource.loop = true;
            musicSource.Play();
        }
        
        public void PauseMusic()
        {
            musicSource.Pause();
        }
        
        public void ResumeMusic()
        {
            musicSource.Play();
        }

        public void PlaySfx(AudioClip clip, bool force = false)
        {
            if (force)
            {
                SFxSource.clip = clip;
                SFxSource.Play();
                _forcedSfx = clip;
            }
            else if (_forcedSfx == null || !SFxSource.isPlaying)
            {
                _forcedSfx = null;
                SFxSource.clip = clip;
                SFxSource.Play();
            }
            
        }
        
        public void PlayPlayerMoveSfx()
        {
            int random = UnityEngine.Random.Range(1, 6);
            AudioClip clip = random switch
            {
                1 => playerMove1Sfx,
                2 => playerMove2Sfx,
                3 => playerMove3Sfx,
                4 => playerMove4Sfx,
                5 => playerMove5Sfx,
                _ => throw new ArgumentOutOfRangeException()
            };
            PlaySfx(clip);
        }
        
        public void PlayCrateMoveSfx()
        {
            int random = UnityEngine.Random.Range(1, 6);
            AudioClip clip = random switch
            {
                1 => crateMove1Sfx,
                2 => crateMove2Sfx,
                3 => crateMove3Sfx,
                4 => crateMove4Sfx,
                5 => crateMove5Sfx,
                _ => throw new ArgumentOutOfRangeException()
            };
            PlaySfx(clip);
        }
        
        public void PlayMainMenuMusic()
        {
            PlayMusic(mainMenuMusic);
        }
        
        public void PlayLevelMusic(int level)
        {
            AudioClip clip = level switch
            {
                1 => level1Music,
                2 => level2Music,
                3 => level3Music,
                _ => defaultBackgroundMusic,
            };
            PlayMusic(clip);
        }
        
        private void EnsureSingleton(bool destroyOnLoad = true)
        {
            if (Instance == null)
            {
                Instance = this;
                if(!destroyOnLoad) DontDestroyOnLoad(gameObject);
            } 
            else Destroy(gameObject);
        }
        
        public void OnButtonClick()
        {
            PlaySfx(buttonClickSfx);
        }
        
        public void OnButtonToggle()
        {
            PlaySfx(toggleSfx);
        }
        
        public void OnButtonTap()
        {
            PlaySfx(tapSfx);
        }
    }
}