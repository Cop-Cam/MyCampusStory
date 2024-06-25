//----------------------------------------------------------------------
// Author   : "Ananta Miyoru Wijaya"
//----------------------------------------------------------------------

using UnityEngine;


namespace MyCampusStory.StandaloneManager
{
    /// <summary>
    /// Class for managing main menu
    /// </summary>
    public class MainMenuManager : MonoBehaviour
    {
        private GameManager _gameManager;

        [SerializeField] private Canvas _cnvQuitGameMenu;

        [SerializeField] AudioClip _mainMenuMusic;

        private void Awake()
        {
            _gameManager = GameManager.Instance;
        }

        private void Start()
        {
            _gameManager.AudioManager.PlayMusic(_mainMenuMusic);
        }

        public void NewGame()
        {
            
        }

        public void LoadGame()
        {
               
        }

        public void QuitGameMenu(bool isOpened)
        {
            _cnvQuitGameMenu.enabled = isOpened;
        }
        
        public void QuitGame()
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
    }
}
