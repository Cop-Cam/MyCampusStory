//----------------------------------------------------------------------
// Author   : "Ananta Miyoru Wijaya"
//----------------------------------------------------------------------

using System.Collections;
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
            SceneManager.LoadSceneAsync(sceneIndex);
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

        public void LoadSceneUsingAnimator(string sceneName)
        {
            // SceneManager.LoadSceneAsync(sceneName);

            // _sceneTransitionAnimator.CrossFade("LOADSCENE", 0.1f);
            StartCoroutine(LoadLevelWithTransition(sceneName));
        }

        IEnumerator LoadLevelWithTransition(string sceneName)
        {
            _sceneTransitionAnimator.CrossFade("FADEIN", 0.1f);
            yield return new WaitForSecondsRealtime(1f);
            SceneManager.LoadSceneAsync(sceneName);
            _sceneTransitionAnimator.CrossFade("FADEOUT", 0.1f);
        }
    }
}
