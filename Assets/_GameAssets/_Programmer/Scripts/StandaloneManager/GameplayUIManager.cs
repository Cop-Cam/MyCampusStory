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

        
        protected override void Awake()
        {
            base.Awake();
        }

        #region Lower UI
        [Header("Lower UI")]
        [SerializeField] private string _currentLowerUIAnimState;
        [SerializeField] private Animator _lowerUIAnimator;
        [SerializeField] private Canvas _cnvLowerUI;
        public void OpenLowerUI()
        {
            if (_cnvLowerUI.enabled) return;

            _cnvLowerUI.enabled = true;
            SetAnimCrossFade(_lowerUIAnimator, ref _currentLowerUIAnimState, "OPEN", 0.1f);
        }

        public void CloseLowerUI()
        {
            if (!_cnvLowerUI.enabled) return;

            SetAnimCrossFade(_lowerUIAnimator, ref _currentLowerUIAnimState, "CLOSE", 0.1f);
            _cnvLowerUI.enabled = false;
        }

        public void CloseEveryUI()
        {
            CloseEveryUIEvent?.Invoke();
        }
        #endregion

        #region Setting Menu
        [Header("Setting Menu")]
        [SerializeField] private string _currentSettingMenuAnimState;
        [SerializeField] private Animator _settingMenuAnimator;
        [SerializeField] private Canvas _cnvSettingMenu;
        public void OpenSettingMenu()
        {
            if (_cnvSettingMenu.enabled) return;

            _cnvSettingMenu.enabled = true;
            SetAnimCrossFade(_settingMenuAnimator, ref _currentSettingMenuAnimState, "OPEN", 0.1f);
        }

        public void CloseSettingMenu()
        {
            if (!_cnvSettingMenu.enabled) return;

            SetAnimCrossFade(_lowerUIAnimator, ref _currentSettingMenuAnimState, "CLOSE", 0.1f);
            _cnvSettingMenu.enabled = false;
        }
        #endregion
        
        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
            
        }

        public void SetAnimCrossFade(Animator animator, ref string state, string id, float time)
        {
            if (!string.IsNullOrEmpty(id) && state != id)
            {
                animator?.CrossFade(id, time);
                state = id;
            }
        }
    }
}
