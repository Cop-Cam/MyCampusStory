//----------------------------------------------------------------------
// Author   : "Ananta Miyoru Wijaya"
//----------------------------------------------------------------------

using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

using MyCampusStory.ResourceSystem;
using MyCampusStory.BuildingSystem;

using MyCampusStory.DesignPatterns;
using System.Collections.Generic;

namespace MyCampusStory.StandaloneManager
{
    /// <summary>
    /// Class for managing UI
    /// </summary>
    [DefaultExecutionOrder(-1)]
    public class GameplayUIManager : SceneSingleton<GameplayUIManager>
    {
        // public event Action OnUIOpened;
        public event Action CloseEveryUIEvent;

        [field:SerializeField] 
        public BuildingUIManager BuildingUIManager { get; private set; }

        [field:SerializeField]
        public ResourceUIManager ResourceUIManager { get; private set; }

        [Header("Lower UI")]
        [SerializeField] private Animator _lowerUIAnimator;
        
        
        protected override void Awake()
        {
            base.Awake();
        }

        public void OpenCloseLowerUI()
        {
            _lowerUIAnimator.SetBool("isOpen", !_lowerUIAnimator.GetBool("isOpen"));
        }

        public void CloseEveryUI()
        {
            CloseEveryUIEvent?.Invoke();
        }

        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
            
        }
    }
}
