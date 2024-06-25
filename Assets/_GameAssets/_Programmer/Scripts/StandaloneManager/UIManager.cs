//----------------------------------------------------------------------
// Author   : "Ananta Miyoru Wijaya"
//----------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
using TMPro;

using MyCampusStory.ResourceSystem;

namespace MyCampusStory.StandaloneManager
{
    /// <summary>
    /// Class for managing UI
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        LevelManager _levelManager;

        [SerializeField] ResourceUI[] _resourceUIs;
        
        [System.Serializable]
        public struct ResourceUI
        {
            public ResourceSO ResourceSO;
            public TextMeshProUGUI ResourceText;
        }

        private void Awake()
        {
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
