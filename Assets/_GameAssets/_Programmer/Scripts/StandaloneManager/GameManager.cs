//----------------------------------------------------------------------
// Author   : "Ananta Miyoru Wijaya"
//----------------------------------------------------------------------

using UnityEngine;

using SingletonsCollection;
using MyCampusStory.InputSystem;

namespace MyCampusStory.StandaloneManager
{
    /// <summary>
    /// Class summary
    /// </summary>
    [DefaultExecutionOrder(-2)]
    public class GameManager : DontDestroyOnLoadSingletonMonoBehaviour<GameManager>
    {   
        [field:SerializeField]
        public InputManager InputManager { get; private set; }

        [field:SerializeField]
        public AudioManager AudioManager { get; private set; }

        [field:SerializeField]
        public SceneHandler SceneHandler { get; private set; }
    }
}
