//----------------------------------------------------------------------
// Author   : "Ananta Miyoru Wijaya"
//----------------------------------------------------------------------

using UnityEngine;

using MyCampusStory.DesignPatterns;
using MyCampusStory.InputSystem;


namespace MyCampusStory.StandaloneManager
{
    /// <summary>
    /// Class summary
    /// </summary>
    [DefaultExecutionOrder(-2)]
    public class GameManager : Singleton<GameManager>
    {   
        [field:SerializeField]
        public InputManager InputManager { get; private set; }

        [field:SerializeField]
        public SceneHandler SceneHandler { get; private set; }
        
        public static bool IsGamePaused { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            SetFrameRate();
        }

        private void SetFrameRate()
        {
            // Get the refresh rate of the display using refreshRateRatio
            double refreshRate = Screen.currentResolution.refreshRateRatio.value;

            // Set the target frame rate to the refresh rate of the phone's display
            Application.targetFrameRate = Mathf.RoundToInt((float)refreshRate);

            // Optionally, set the fixed delta time for physics updates
            Time.fixedDeltaTime = 1.0f / (float)refreshRate;

            Debug.Log("Setting target frame rate to: " + Mathf.RoundToInt((float)refreshRate));
        }

        public void PauseGame(bool isPausing)
        {
            IsGamePaused = isPausing;
            // AudioListener.pause = isPausing;

            if(isPausing)
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
