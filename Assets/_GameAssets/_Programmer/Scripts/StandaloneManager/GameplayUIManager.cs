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
    public class GameplayUIManager : SceneSingleton<GameplayUIManager>, IObserver
    {
        private LevelManager _levelManager;

        [field:SerializeField] public BuildingUIManager BuildingUIManager { get; private set; }
        
        
        [System.Serializable]
        public struct ResourceUI
        {
            public ResourceSO ResourceSO;
            public TextMeshProUGUI ResourceText;
        }
        [SerializeField] private ResourceUI[] _resourceUIs;

        protected override void Awake()
        {
            base.Awake();

            _levelManager = LevelManager.Instance;
            // _levelManager.SetGameplayUIReference(this);
        }

        private void OnEnable()
        {
            ResourceManager.OnResourceChangedEventRegister(this);
        }

        private void OnDisable()
        {
            ResourceManager.OnResourceChangedEventUnregister(this);
        }

        public void OnNotify()
        {
            foreach (ResourceUI resourceUI in _resourceUIs)
            {
                resourceUI.ResourceText.text = _levelManager.ResourceManager.GetResourceAmount(resourceUI.ResourceSO.ResourceId).ToString();
            }
        }
        
    }
}
