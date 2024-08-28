//----------------------------------------------------------------------
// Author   : "Ananta Miyoru Wijaya"
//----------------------------------------------------------------------

using UnityEngine;
using TMPro;

using MyCampusStory.ResourceSystem;
using MyCampusStory.BuildingSystem;

using MyCampusStory.DesignPatterns;

namespace MyCampusStory.StandaloneManager
{
    /// <summary>
    /// Class for managing UI
    /// </summary>
    [DefaultExecutionOrder(-1)]
    public class GameplayUIManager : SceneSingleton<GameplayUIManager>
    {
        [field:SerializeField] 
        public BuildingUIManager BuildingUIManager { get; private set; }

        [field:SerializeField]
        public ResourceUIManager ResourceUIManager { get; private set; }
        
        protected override void Awake()
        {
            base.Awake();
        }        
    }
}
