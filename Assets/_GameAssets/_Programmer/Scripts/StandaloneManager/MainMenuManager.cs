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
        [SerializeField] AudioClip _mainMenuMusic;
        [SerializeField] private Canvas _cnvQuitGameMenu;

        private void Awake()
        {

        }

        private void Start()
        {
            GameManager.Instance.AudioManager.PlayMusic(_mainMenuMusic);
        }

        public void NewGame()
        {
            Debug.Log("New Game");
        }

        public void LoadGame()
        {
            Debug.Log("Load Game");
        }

        public void OpenQuitGameMenu(bool isOpened)
        {
            _cnvQuitGameMenu.enabled = isOpened;
        }
        
        public void QuitGame()
        {
            GameManager.Instance.QuitGame();
        }
    }
}
