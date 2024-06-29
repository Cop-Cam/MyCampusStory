//----------------------------------------------------------------------
// Author   : "Ananta Miyoru Wijaya"
//----------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
using TMPro;

using MyCampusStory.ResourceSystem;
using SingletonsCollection;
using MyCampusStory.BuildingSystem;

namespace MyCampusStory.StandaloneManager
{
    /// <summary>
    /// Class for managing UI
    /// </summary>
    public class UIGameplayManager : DestroyOnLoadSingletonMonoBehaviour<UIGameplayManager>
    {
        LevelManager _levelManager;

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
        }

        private void Update()
        {
            UpdateResourceUI();
        }

        private void UpdateResourceUI()
        {
            foreach (ResourceUI resourceUI in _resourceUIs)
            {
                resourceUI.ResourceText.text = _levelManager.ResourceManager.GetResourceAmount(resourceUI.ResourceSO.ResourceId).ToString();
            }
        }
    }
}
