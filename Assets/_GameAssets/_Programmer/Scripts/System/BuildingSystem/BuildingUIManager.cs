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
        private GameplayUIManager _gameplayUIManager;

        [Header("ObjectPool")]
        private ObjectPool _buildingUIObjectPool;
        [SerializeField] private BuildingRequirementUI _buildingRequirementUIPrefab;
        [SerializeField] private Transform _poolHolder;

        [Header("Canvas")]
        [SerializeField] private Canvas _cnvBuildingUI;
        [SerializeField] private Canvas _cnvDescriptionUI;
        [SerializeField] private Canvas _cnvRequirementUI;

        [Header("Dynamics")]
        [SerializeField] private Transform _activeRequirementUIHolder;

        
        [Header("Buttons")]
        [SerializeField] private Button _btnUpgradeButton;
        
        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI _buildingName;
        [SerializeField] private TextMeshProUGUI _buildingDescription;
        [SerializeField] private TextMeshProUGUI _buildingLevel;
        
        private Building _currentOpenedBuilding;
        private Coroutine _uiUpdateCoroutine;

        
        private void Awake()
        {
            _gameplayUIManager = GameplayUIManager.Instance;

            _buildingUIObjectPool = GetComponent<ObjectPool>();
            _buildingUIObjectPool.Init(_poolHolder, _buildingRequirementUIPrefab.gameObject, 5);

            _cnvBuildingUI.enabled = false;
            _cnvDescriptionUI.enabled = false;
            _cnvRequirementUI.enabled = false;
        }

        public void OpenBuildingUI(Building currentOpenedBuilding)
        {
            _currentOpenedBuilding = currentOpenedBuilding;
            
            _uiUpdateCoroutine = StartCoroutine(UpdateUI());

            _cnvBuildingUI.enabled = true;
        }

        public void CloseBuildingUI()
        {
            if(_cnvRequirementUI.enabled)
                OpenCloseRequirementUI();
            
            if(_cnvDescriptionUI.enabled)
                OpenCloseDescriptionUI();

            _cnvBuildingUI.enabled = false;

            StopCoroutine(_uiUpdateCoroutine);

            _currentOpenedBuilding = null;
        }

        public void OpenCloseDescriptionUI()
        {
            _cnvDescriptionUI.enabled = !_cnvDescriptionUI.enabled;

            if(_cnvDescriptionUI.enabled)
            {
                _buildingDescription.text = _currentOpenedBuilding.BuildingDataSO.BuildingDescription;
            }
            else
            {
                _buildingDescription.text = string.Empty;
            }
        }

        public void OpenCloseRequirementUI()
        {
            _cnvRequirementUI.enabled = !_cnvRequirementUI.enabled;

            if(_cnvRequirementUI.enabled)
            {
                foreach (var buildingRequirement in _currentOpenedBuilding.CurrentBuildingStat.BuildingUpgradeRequirements)
                {
                    var buildingRequirementUI = _buildingUIObjectPool.GetObject();

                    var buildingRequirementUIScript = buildingRequirementUI.GetComponent<BuildingRequirementUI>();
                    buildingRequirementUIScript.Init(buildingRequirement.ResourceRequired, buildingRequirement.AmountRequired);
                    buildingRequirementUI.transform.SetParent(_activeRequirementUIHolder.transform);
                    buildingRequirementUI.gameObject.SetActive(true);
                }
            }
            else
            {
                foreach (var buildingRequirementUI in _cnvRequirementUI.GetComponentsInChildren<BuildingRequirementUI>())
                {
                    var buildingRequirementUIScript = buildingRequirementUI.GetComponent<BuildingRequirementUI>();
                    buildingRequirementUIScript.DeInit();

                    _buildingUIObjectPool.ReturnObject(buildingRequirementUI.gameObject);
                }
            }
        }

        public void UpgradeBuilding()
        {
            if(_currentOpenedBuilding.IsBuildingUpgradeable())
            {
                _currentOpenedBuilding.TryUpgradeBuilding();
                
                //refresh requirement ui
                if(_cnvRequirementUI.enabled)
                {
                    OpenCloseRequirementUI();
                    OpenCloseRequirementUI();
                }
            }
            else //kinda show the upgrade requirement
            {
                if(!_cnvRequirementUI.enabled)
                    OpenCloseRequirementUI();
            }

        }

        private IEnumerator UpdateUI()
        {
            while (true)
            {
                _buildingName.text = _currentOpenedBuilding.BuildingDataSO.BuildingName;
                _buildingLevel.text = _currentOpenedBuilding.CurrentBuildingLevel.ToString();
                
                yield return new WaitForSecondsRealtime(0.1f);
            }
        }
    }
}
