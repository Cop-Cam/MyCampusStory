//----------------------------------------------------------------------
// Author   : "Ananta Miyoru Wijaya"
//----------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

using MyCampusStory.StandaloneManager;
using MyCampusStory.DataPersistenceSystem;

namespace MyCampusStory.BuildingSystem
{
    public class Building : MonoBehaviour, IDataPersistence, IInteractable
    {
        [field:SerializeField] public BuildingSO BuildingDataSO { get; private set; }

        [Tooltip("Generate only on placed prefab in scene and not on prefab asset!")]
        [SerializeField] private string _buildingInstanceId;

        private LevelManager _levelManager;
        private bool _isBuildingUnlocked = false;
        private BuildingUpgradeManager _buildingUpgradeManager;
        private Coroutine GenerateResourceCoroutine;

        public Dictionary<int, BuildingStat> BuildingStatsPerLevelDictionary { get; private set; } = new Dictionary<int, BuildingStat>();
        public BuildingStat CurrentBuildingStat { get; private set; }
        public int CurrentBuildingLevel { get; private set; } = 1;

        private void Awake()
        {
            _levelManager = LevelManager.Instance;

            _buildingUpgradeManager = new BuildingUpgradeManager();

            //Initialize building stats dictionary
            foreach (var buildingStat in BuildingDataSO.BuildingStatsPerLevel)
            {
                BuildingStatsPerLevelDictionary.Add(buildingStat.BuildingLevelForStat, buildingStat);
            }

            CurrentBuildingStat = BuildingStatsPerLevelDictionary[CurrentBuildingLevel];
        }

        private void Start()
        {
            GenerateResourceCoroutine = StartCoroutine(GenerateResource());
        }


        public void TryUpgradeBuilding()
        {
            if(!IsBuildingUpgradeable())
            {
                return;
            }

            _buildingUpgradeManager.TryUpgradingBuilding(this, CurrentBuildingStat, _levelManager.ResourceManager);

            CurrentBuildingLevel++;
        }

        public bool IsBuildingUpgradeable()
        {
            return _buildingUpgradeManager.CheckUpgradeEligibility(this, _levelManager.ResourceManager);
        }

        public void SaveData(GameData data)
        {
            if(data.PlayerBuildingData.ContainsKey(_buildingInstanceId))
            {
                data.PlayerBuildingData[_buildingInstanceId].IsBuildingUnlocked = _isBuildingUnlocked;
                data.PlayerBuildingData[_buildingInstanceId].CurrentBuildingLevel = CurrentBuildingLevel;
            }
            else
            {
                data.PlayerBuildingData.Add(_buildingInstanceId, new SerializedBuildingData
                {
                    IsBuildingUnlocked = _isBuildingUnlocked,
                    CurrentBuildingLevel = CurrentBuildingLevel
                });
            }
        }

        public void LoadData(GameData data)
        {
            if(data.PlayerBuildingData.ContainsKey(_buildingInstanceId))
            {
                _isBuildingUnlocked = data.PlayerBuildingData[_buildingInstanceId].IsBuildingUnlocked;
                CurrentBuildingLevel = data.PlayerBuildingData[_buildingInstanceId].CurrentBuildingLevel;
            }
        }

        public void OnInteract()
        {
            _levelManager.UIGameplayManager.BuildingUIManager.OpenBuildingUI(this);
        }

        public void OnStopInteract()
        {
            _levelManager.UIGameplayManager.BuildingUIManager.CloseBuildingUI();
        }

        private IEnumerator GenerateResource()
        {
            yield return new WaitForSecondsRealtime(CurrentBuildingStat.ResourceGenerationStats[0].ResourceGenerationIntervalInSecondsRealTime);

            _levelManager.ResourceManager.ModifyResourceAmount(CurrentBuildingStat.ResourceGenerationStats[0].ResourceToGenerate.ResourceId, 
                CurrentBuildingStat.ResourceGenerationStats[0].GeneratedResourceAmount);
        }


        // Generate a unique ID
        public void GenerateUniqueID()
        {
            _buildingInstanceId = BuildingDataSO.BuildingBaseId + "_" + System.Guid.NewGuid().ToString();
        }
    }

    #if UNITY_EDITOR
    // Custom editor to ensure the ID is generated in the editor
    [CustomEditor(typeof(Building))]
    public class BuildingEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            Building buildingScript = (Building)target;
            if (GUILayout.Button("Generate Instance ID"))
            {
                buildingScript.GenerateUniqueID();
                EditorUtility.SetDirty(buildingScript);
            }
        }
    }
    #endif

}
