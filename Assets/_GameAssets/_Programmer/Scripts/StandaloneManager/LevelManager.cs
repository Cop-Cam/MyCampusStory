//----------------------------------------------------------------------
// Author   : "Ananta Miyoru Wijaya"
//----------------------------------------------------------------------

using UnityEngine;

using MyCampusStory.DesignPatterns;
using MyCampusStory.ResourceSystem;
using MyCampusStory.CameraSystem;

namespace MyCampusStory.StandaloneManager
{
    /// <summary>
    /// Class for managing gameplay scene or level in game
    /// </summary>
    [DefaultExecutionOrder(-1)]
    public class LevelManager : SceneSingleton<LevelManager>
    {
        public GameManager GameManager { get; private set; }

        [field:Header("DEPENDENCIES")]
        [field:SerializeField]
        public ResourceManager ResourceManager { get; private set; }
        
        [field:SerializeField]
        public CameraManager CameraManager { get; private set; }

        [field:SerializeField]
        public AudioManager AudioManager { get; private set; }


        [Header("LEVEL PROPERTIES")]
        [SerializeField] AudioClip _levelMusic;

        protected override void Awake()
        {
            base.Awake();

            GameManager = GameManager.Instance;
            // GameManager.SceneHandler.LoadSceneAdditive("GameplayUI");
        }

        private void Start()
        {
            AudioManager.PlayMusic(_levelMusic);
        }


        // [field:SerializeField]
        // public GameplayUIManager GameplayUIManager { get; private set; }
        // public void SetGameplayUIReference(GameplayUIManager gameplayUIManager)
        // {
        //     if(gameplayUIManager != null) 
        //         return;

        //     GameplayUIManager = gameplayUIManager;
        // }

        
    }
}
