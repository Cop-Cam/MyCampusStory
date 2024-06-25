//----------------------------------------------------------------------
// Author   : "Ananta Miyoru Wijaya"
//----------------------------------------------------------------------

using UnityEngine;

using SingletonsCollection;
using MyCampusStory.InputSystem;
using MyCampusStory.ResourceSystem;

namespace MyCampusStory.StandaloneManager
{
    /// <summary>
    /// Class for managing gameplay scene or level in game
    /// </summary>
    [DefaultExecutionOrder(-1)]
    public class LevelManager : DestroyOnLoadSingletonMonoBehaviour<LevelManager>
    {
        public GameManager GameManager { get; private set; }

        [field:Header("DEPENDENCIES")]
        [field:SerializeField]
        public ResourceManager ResourceManager { get; private set; }
        
        [Header("LEVEL PROPERTIES")]
        [SerializeField] AudioClip _levelMusic;

        public bool IsGamePaused { get; private set; }

        protected override void Awake()
        {
            GameManager = GameManager.Instance;
        }

        private void Start()
        {
            GameManager.AudioManager.PlayMusic(_levelMusic);
        }

        public void PauseGame(bool isPaused)
        {
            IsGamePaused = isPaused;
            AudioListener.pause = isPaused;

            if(isPaused)
            {
                Time.timeScale = 0.0f;
            }
            else
            {
                Time.timeScale = 1.0f;
            }
        }
    }
}
