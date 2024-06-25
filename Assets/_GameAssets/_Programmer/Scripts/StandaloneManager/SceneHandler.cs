//----------------------------------------------------------------------
// Author   : "Ananta Miyoru Wijaya"
//----------------------------------------------------------------------

using UnityEngine;
using UnityEngine.SceneManagement;

namespace MyCampusStory.StandaloneManager
{
    /// <summary>
    /// Class for managing scene
    /// </summary>
    public class SceneHandler : MonoBehaviour
    {
        //Can be used as transition animator
        [SerializeField] private Animator _sceneTransitionAnimator;

        
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public void LoadScene(int sceneIndex)
        {
            SceneManager.LoadScene(sceneIndex);
        }

        public void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        #region Additive
        // Load a scene additively
        public void LoadSceneAdditive(string sceneName)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }

        // Unload a scene
        public void UnloadScene(string sceneName)
        {
            SceneManager.UnloadSceneAsync(sceneName);
        }
        #endregion
    }
}
