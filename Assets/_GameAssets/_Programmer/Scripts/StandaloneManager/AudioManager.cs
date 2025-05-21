//----------------------------------------------------------------------
// Author   : "Ananta Miyoru Wijaya"
//----------------------------------------------------------------------

using UnityEngine;

namespace MyCampusStory.StandaloneManager
{
    /// <summary>
    /// Class for managing audio in game
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        [Header("AUDIO SOURCES")]
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _sfxSource;
        
        private bool _isMusicMuted;
        private bool _isSFXMuted;
        
        public void PlaySFX(AudioClip clip)
        {
            if(clip == null) return;
            
            _sfxSource.PlayOneShot(clip);
        }

        public void PlayMusic(AudioClip clip)
        {
            if(clip == null) return;

            _musicSource.clip = clip;
            _musicSource.Play();
        }

        public void SetMusicMuted(bool isMuted)
        {
            _isMusicMuted = isMuted;
            _musicSource.mute = isMuted;
        }

        public void SetSFXMuted(bool isMuted)
        {
            _isSFXMuted = isMuted;
            _sfxSource.mute = isMuted;
        }

        public void SetMusicVolume(float volume)
        {
            _musicSource.volume = volume;
        }

        public void SetSoundVolume(float volume)
        {
            _musicSource.volume = volume;
        }
    }
}

