//----------------------------------------------------------------------
// Author   : "Ananta Miyoru Wijaya"
//----------------------------------------------------------------------

using UnityEngine;

using MyCampusStory.InputSystem;
using MyCampusStory.ResourceSystem;

namespace MyCampusStory
{
    /// <summary>
    /// Class summary
    /// </summary>
    public class GameManager : SingletonMonoBehaviour<GameManager>
    {
        [field:SerializeField]
        public ResourceManager ResourceManager { get; private set; }
        
        [field:SerializeField]
        public InputManager InputManager { get; private set; }

        [field:SerializeField]
        public AudioManager AudioManager { get; private set; }
    }
}
