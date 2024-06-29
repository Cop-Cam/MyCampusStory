//----------------------------------------------------------------------
// Author   : "Ananta Miyoru Wijaya"
//----------------------------------------------------------------------

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MyCampusStory.BuildingSystem
{
    public class BuildingUIManager : MonoBehaviour
    {
        [Header("Canvas")]
        [SerializeField] private Canvas _cnvBuildingUI;
        [SerializeField] private Canvas _cnvDescriptionUI;
        
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
            _cnvBuildingUI.enabled = false;
            _cnvDescriptionUI.enabled = false;
        }

        public void OpenBuildingUI(Building currentOpenedBuilding)
        {
            _currentOpenedBuilding = currentOpenedBuilding;
            
            _uiUpdateCoroutine = StartCoroutine(UpdateUI());

            _cnvBuildingUI.enabled = true;
        }

        public void CloseBuildingUI()
        {
            _cnvDescriptionUI.enabled = false;
            _cnvBuildingUI.enabled = false;

            StopCoroutine(_uiUpdateCoroutine);

            _currentOpenedBuilding = null;
        }

        public void OpenDescriptionUI()
        {
            _cnvDescriptionUI.enabled = true;
            _buildingDescription.text = _currentOpenedBuilding.BuildingDataSO.BuildingDescription;
        }

        public void UpgradeBuilding()
        {
            _currentOpenedBuilding.TryUpgradeBuilding();
        }

        private void UpdateUpgradeUI()
        {
            if(_currentOpenedBuilding.IsBuildingUpgradeable()) 
                _btnUpgradeButton.interactable = true;
            else 
                _btnUpgradeButton.interactable = false;
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
