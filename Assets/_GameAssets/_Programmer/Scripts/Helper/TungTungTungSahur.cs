using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MyCampusStory.StandaloneManager;

namespace MyCampusStory
{
    public class TungTungTungSahur : MonoBehaviour, IClickable
    {
        [SerializeField] private AudioClip _clip;

        public void OnClick()
        {
            GameManager.Instance.AudioManager.PlaySFX(_clip);
        }

        public void OnStopClick()
        {

        }
    }
}
