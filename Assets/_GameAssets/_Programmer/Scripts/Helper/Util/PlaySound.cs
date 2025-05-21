using System.Collections;
using System.Collections.Generic;
using MyCampusStory.StandaloneManager;
using UnityEngine;

namespace MyCampusStory
{
    public class PlaySound : MonoBehaviour
    {
        [SerializeField] private AudioClip _clip;

        public void PlayAsSFX()
        {
            GameManager.Instance.AudioManager.PlaySFX(_clip);
        }

        public void PlayAsMusic()
        {
            GameManager.Instance.AudioManager.PlayMusic(_clip);
        }

    }
}
