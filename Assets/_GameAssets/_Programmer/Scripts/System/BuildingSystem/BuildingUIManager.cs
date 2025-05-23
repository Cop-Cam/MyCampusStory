//----------------------------------------------------------------------
// Author   : "Ananta Miyoru Wijaya"
//----------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using MyCampusStory.DesignPatterns;
using MyCampusStory.StandaloneManager;
namespace MyCampusStory.BuildingSystem
{
    public class BuildingUIManager : MonoBehaviour
    {
        // private GameplayUIManager _gameplayUIManager;

        [Header("ObjectPool")]
        private ObjectPool _buildingUIObjectPool;
        [SerializeField] private BuildingRequirementUI _buildingRequirementUIPrefab;
        [SerializeField] private Transform _poolHolder;

        [Header("Canvas")]
        // [SerializeField] private Canvas _cnvBuildingUI;
        // [SerializeField] private Canvas _cnvDescriptionUI;
        // [SerializeField] private Canvas _cnvRequirementUI;

        [Header("Dynamics")]
        [SerializeField] private Transform _activeRequirementUIHolder;
        [SerializeField] private Transform _activeGenerationUIHolder;
        [SerializeField] private Transform _activeGenerationOnUpgradeUIHolder;

        
        [Header("Buttons")]
        [SerializeField] private Button _btnUpgradeButton;
        
        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI _buildingName;
        [SerializeField] private TextMeshProUGUI _buildingDescription;
        [SerializeField] private TextMeshProUGUI _buildingLevel;
        
        private Building _currentOpenedBuilding;
        // private Coroutine _uiUpdateCoroutine;

        [SerializeField] private Animator _animator;

        public string _currentAnimState;

        public string OpenUI_Anim_State = "OPEN";
        public string CloseUI_Anim_State = "CLOSE";

        public void SetAnimCrossFade(string id, float time)
        {
            if (!string.IsNullOrEmpty(id) && _currentAnimState != id)
            {
                _animator?.CrossFade(id, time);
                _currentAnimState = id;
            }
        }

        private void OnEnable()
        {
            GameplayUIManager.Instance.CloseEveryUIEvent += CloseBuildingUI;
        }

        private void OnDisable()
        {
            GameplayUIManager.Instance.CloseEveryUIEvent -= CloseBuildingUI;
        }

        private void Awake()
        {
            _buildingUIObjectPool = GetComponent<ObjectPool>();
            _buildingUIObjectPool.Init(_poolHolder, _buildingRequirementUIPrefab.gameObject, 5);

            // _cnvBuildingUI.enabled = false;
            // _cnvDescriptionUI.enabled = false;
            // _cnvRequirementUI.enabled = false;
        }

        private void Update()
        {
            if (_currentOpenedBuilding == null) return;

            if (_currentOpenedBuilding.IsBuildingMaxLevel())
            {
                _btnUpgradeButton.gameObject.SetActive(false);
            }

            // if(_currentOpenedBuilding.IsBuildingUpgradeable())
            // {
            //     SetAnimCrossFade(AbleToUpgrade_Anim_State, 0.1f);
            // }
        }

        public void OpenBuildingUI(Building currentOpenedBuilding)
        {
            GameplayUIManager.Instance.CloseEveryUI();
            SetAnimCrossFade(OpenUI_Anim_State, 0.1f);

            _currentOpenedBuilding = currentOpenedBuilding;

            // Fill UI Fields
            _buildingName.text = _currentOpenedBuilding.GetBuildingSO().BuildingName;
            _buildingLevel.text = _currentOpenedBuilding.GetCurrentBuildingLevel().ToString();
            _buildingDescription.text = _currentOpenedBuilding.GetBuildingSO().BuildingDescription;

            // Populate requirement UI
            PopulateRequirementsUI();
            PopulateGenerationUI();
            PopulateGenerationOnUpgradeUI();

            SetAnimCrossFade(OpenUI_Anim_State, 0.1f);
        }

        private void PopulateRequirementsUI()
        {
            var BuildingUpgradeRequirements = _currentOpenedBuilding.GetCurrentBuildingStat().BuildingUpgradeRequirements;
            if(BuildingUpgradeRequirements == null || BuildingUpgradeRequirements.Length == 0) return;

            // Populate new requirement UI objects
            foreach (var buildingRequirement in _currentOpenedBuilding.GetCurrentBuildingStat().BuildingUpgradeRequirements)
            {
                var buildingRequirementUI = _buildingUIObjectPool.GetObject();
                var buildingRequirementUIScript = buildingRequirementUI.GetComponent<BuildingRequirementUI>();

                buildingRequirementUI.transform.SetParent(_activeRequirementUIHolder.transform, false);
                buildingRequirementUI.SetActive(true);
                buildingRequirementUIScript.Init(buildingRequirement.ResourceRequired, buildingRequirement.AmountRequired);
            }
        }

        private void PopulateGenerationUI()
        {
            var ResourceGenerationStats = _currentOpenedBuilding.GetCurrentBuildingStat().ResourceGenerationStats;
            if(ResourceGenerationStats == null || ResourceGenerationStats.Length == 0) return;

            foreach (var buildingGeneration in _currentOpenedBuilding.GetCurrentBuildingStat().ResourceGenerationStats)
            {
                var buildingGenerationUI = _buildingUIObjectPool.GetObject();
                var buildingGenerationUIScript = buildingGenerationUI.GetComponent<BuildingRequirementUI>(); //temp use the same script for now

                buildingGenerationUI.transform.SetParent(_activeGenerationUIHolder.transform, false);
                buildingGenerationUI.SetActive(true);
                buildingGenerationUIScript.Init(buildingGeneration.ResourceGenerated, buildingGeneration.AmountGenerated);
            }
        }

        private void PopulateGenerationOnUpgradeUI()
        {
            // if(!_currentOpenedBuilding.IsBuildingUpgradeable()) return;

            var ResourceGenerationStats = _currentOpenedBuilding.GetBuildingSO().BuildingStatsPerLevel[_currentOpenedBuilding.GetCurrentBuildingLevel()+1].ResourceGenerationStats;
            if(ResourceGenerationStats == null || ResourceGenerationStats.Length == 0) return;

            foreach (var buildingGenerationOnUpgrade in _currentOpenedBuilding.GetBuildingSO().BuildingStatsPerLevel[_currentOpenedBuilding.GetCurrentBuildingLevel()+1].ResourceGenerationStats)
            {
                var buildingGenerationUI = _buildingUIObjectPool.GetObject();
                var buildingGenerationUIScript = buildingGenerationUI.GetComponent<BuildingRequirementUI>(); //temp use the same script for now

                buildingGenerationUI.transform.SetParent(_activeGenerationOnUpgradeUIHolder.transform, false);
                buildingGenerationUI.SetActive(true);
                buildingGenerationUIScript.Init(buildingGenerationOnUpgrade.ResourceGenerated, buildingGenerationOnUpgrade.AmountGenerated);
            }
        }

        public void CloseBuildingUI()
        {
            SetAnimCrossFade(CloseUI_Anim_State, 0.1f);

            // Clear existing UI objects
            var activeRequirementUIs = _activeRequirementUIHolder.GetComponentsInChildren<BuildingRequirementUI>();
            if (activeRequirementUIs != null && activeRequirementUIs.Length > 0)
            {
                foreach (var buildingRequirementUI in _activeRequirementUIHolder.GetComponentsInChildren<BuildingRequirementUI>())
                {
                    buildingRequirementUI.DeInit();
                    _buildingUIObjectPool.ReturnObject(buildingRequirementUI.gameObject);
                }
            }

            var activeGenerationUIs = _activeGenerationUIHolder.GetComponentsInChildren<BuildingRequirementUI>();
            if (activeGenerationUIs != null && activeGenerationUIs.Length > 0)
            {
                foreach (var buildingGenerationUI in _activeGenerationUIHolder.GetComponentsInChildren<BuildingRequirementUI>())
                {
                    buildingGenerationUI.DeInit();
                    _buildingUIObjectPool.ReturnObject(buildingGenerationUI.gameObject);
                }
            }

            var activeGenerationOnUpgradeUIs = _activeGenerationOnUpgradeUIHolder.GetComponentsInChildren<BuildingRequirementUI>();
            if (activeGenerationOnUpgradeUIs != null && activeGenerationOnUpgradeUIs.Length > 0)
            {
                foreach (var buildingGenerationOnUpgradeUI in _activeGenerationOnUpgradeUIHolder.GetComponentsInChildren<BuildingRequirementUI>())
                {
                    buildingGenerationOnUpgradeUI.DeInit();
                    _buildingUIObjectPool.ReturnObject(buildingGenerationOnUpgradeUI.gameObject);
                }
            }

            // if(_cnvRequirementUI.enabled)
            //     OpenCloseRequirementUI();
            
            // if(_cnvDescriptionUI.enabled)
            //     OpenCloseDescriptionUI();

            // _cnvBuildingUI.enabled = false;

            // StopCoroutine(_uiUpdateCoroutine);

            _currentOpenedBuilding = null;
        }


        // public void OpenBuildingUI(Building currentOpenedBuilding)
        // {
        //     GameplayUIManager.Instance.CloseEveryUI();

        //     _currentOpenedBuilding = currentOpenedBuilding;
            
        //     _animator.SetBool("ISOPEN", true);

        //     // _cnvDescriptionUI.enabled = !_cnvDescriptionUI.enabled;

        //     if(_cnvDescriptionUI.enabled)
        //     {
        //         _buildingDescription.text = _currentOpenedBuilding.GetBuildingSO().BuildingDescription;
        //     }
        //     else
        //     {
        //         _buildingDescription.text = string.Empty;
        //     }
        // }

        

        // public void OpenCloseDescriptionUI()
        // {
        //     _cnvDescriptionUI.enabled = !_cnvDescriptionUI.enabled;

        //     if(_cnvDescriptionUI.enabled)
        //     {
        //         _buildingDescription.text = _currentOpenedBuilding.GetBuildingSO().BuildingDescription;
        //     }
        //     else
        //     {
        //         _buildingDescription.text = string.Empty;
        //     }
        // }

        // public void OpenCloseRequirementUI()
        // {
        //     // _cnvRequirementUI.enabled = !_cnvRequirementUI.enabled;

        //     if(_cnvRequirementUI.enabled)
        //     {
        //         foreach (var buildingRequirement in _currentOpenedBuilding.GetCurrentBuildingStat().BuildingUpgradeRequirements)
        //         {
        //             var buildingRequirementUI = _buildingUIObjectPool.GetObject();

        //             var buildingRequirementUIScript = buildingRequirementUI.GetComponent<BuildingRequirementUI>();
        //             buildingRequirementUIScript.Init(buildingRequirement.ResourceRequired, buildingRequirement.AmountRequired);
        //             buildingRequirementUI.transform.SetParent(_activeRequirementUIHolder.transform);
        //             buildingRequirementUI.gameObject.SetActive(true);
        //         }
        //     }
        //     else
        //     {
        //         foreach (var buildingRequirementUI in _cnvRequirementUI.GetComponentsInChildren<BuildingRequirementUI>())
        //         {
        //             var buildingRequirementUIScript = buildingRequirementUI.GetComponent<BuildingRequirementUI>();
        //             buildingRequirementUIScript.DeInit();

        //             _buildingUIObjectPool.ReturnObject(buildingRequirementUI.gameObject);
        //         }
        //     }
        // }

        public void UpgradeBuilding()
        {
            if(_currentOpenedBuilding.IsBuildingUpgradeable())
            {
                _currentOpenedBuilding.TryUpgradeBuilding();
                
                CloseBuildingUI();
                // //refresh requirement ui
                // if(_cnvRequirementUI.enabled)
                // {
                //     OpenCloseRequirementUI();
                //     OpenCloseRequirementUI();
                // }
            }
            // else //kinda show the upgrade requirement
            // {
            //     // if(!_cnvRequirementUI.enabled)
            //     //     OpenCloseRequirementUI();

            //     SetAnimCrossFade(UnableToUpgrade_Anim_State, 0.1f);
            // }

        }

        // private IEnumerator UpdateUI()
        // {
        //     while (true)
        //     {
        //         _buildingName.text = _currentOpenedBuilding.GetBuildingSO().BuildingName;
        //         _buildingLevel.text = _currentOpenedBuilding.GetCurrentBuildingLevel().ToString();
                
        //         yield return new WaitForSecondsRealtime(0.1f);
        //     }
        // }
    }
}
