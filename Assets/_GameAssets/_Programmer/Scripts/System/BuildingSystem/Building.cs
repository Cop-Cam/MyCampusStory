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
using Unity.VisualScripting;


namespace MyCampusStory.BuildingSystem
{
    [RequireComponent(typeof(Animator))]
    public class Building : MonoBehaviour, IDataPersistence, IClickable, IInteractable
    {
        [Header("Unique Instance Fields")]
        [Tooltip("Instance Id of the building")]
        [SerializeField] private string _buildingInstanceId;

        [Tooltip("Id of the scene where the building was placed")]
        [SerializeField] private GroupData _sceneGroup;

        [Header("Building Data")]
        [SerializeField] private BuildingSO _buildingDataSO;
        [SerializeField] private bool _isBuildingUnlocked = false;

        // [Tooltip("Is the building used? Dont change it!")]
        // [SerializeField] private bool _isBuildingUsed = false;

        [Header("References")]
        [SerializeField] private Animator _buildingAnimator;
        private Coroutine GenerateResourceCoroutine;
        private Dictionary<int, BuildingStat> _buildingStatsPerLevelDictionary = new Dictionary<int, BuildingStat>();
        private BuildingStat _currentBuildingStat;
        private int _currentBuildingLevel = 0;
        private string _currentAnimState;
        private string _currentAnimStateSecondLayer;
        
        [Header("Sounds")]
        [SerializeField] private AudioClip _buildingClickSound;

        private static Dictionary<GroupData, Dictionary<string, Building>> _buildingObjectCollection
            = new Dictionary<GroupData, Dictionary<string, Building>>();

        public string Idle_Anim_StateName = "IDLE";
        public string Locked_Anim_State_Name = "LOCKED";
        public string Unlocked_Anim_State_Name = "UNLOCKED";
        public string Upgrade_Anim_State_Name = "UPGRADE";
        // public string Dummy_State_Name = "DUMMY";

        private void Awake()
        {
            //Initialize building stats dictionary
            for (int i = 0; i < _buildingDataSO.BuildingStatsPerLevel.Length; i++)
            {
                _buildingStatsPerLevelDictionary.Add(i, _buildingDataSO.BuildingStatsPerLevel[i]);
            }

            //Set current building stat
            _currentBuildingStat = _buildingStatsPerLevelDictionary[_currentBuildingLevel];

            //Add building to collection
            // Check if the group already exists in the outer dictionary
            if (!_buildingObjectCollection.ContainsKey(_sceneGroup))
            {
                // If the group doesn't exist, create a new inner dictionary for the group
                _buildingObjectCollection[_sceneGroup] = new Dictionary<string, Building>();
            }
            if (_buildingObjectCollection[_sceneGroup].ContainsKey(_buildingInstanceId))
            {
                // If the building already exists in the inner dictionary, throw an error
                throw new System.Exception("Building already exists in the collection!");
            }
            // Now add the building to the inner dictionary (under the group)
            _buildingObjectCollection[_sceneGroup][_buildingInstanceId] = this;

            // if (!_isBuildingUnlocked)
            // {
            //     SetAnimCrossFade(Locked_Anim_State_Name, 0.1f);
            // }
            // else
            // {
            //     SetAnimCrossFade(Idle_Anim_StateName, 0.1f);
            // }

            if (!_isBuildingUnlocked)
            {
                SetAnimCrossFade(Locked_Anim_State_Name, 0.1f, 0);
            }
            else
            {
                SetAnimCrossFade(Idle_Anim_StateName, 0.1f, 0);
            }
        }

        private void Update()
        {
            if(_isBuildingUnlocked && GenerateResourceCoroutine == null)
            {
                GenerateResourceCoroutine = StartCoroutine(GenerateResource());
            }
        }

        public void SetAnimCrossFade(string id, float time, int layer = 0)
        {
            if (_buildingAnimator == null || string.IsNullOrEmpty(id))
                return;

            // Prevent same-state crossfade per layer
            if ((layer == 0 && _currentAnimState == id) || (layer == 1 && _currentAnimStateSecondLayer == id))
                return;

            _buildingAnimator.CrossFade(id, time, layer); // normalizedTime = 0f starts from beginning

            if (layer == 1)
                _currentAnimStateSecondLayer = id;
            else if (layer == 0)
                _currentAnimState = id;
        }


        public void SaveData(GameData data)
        {
            if(data.PlayerBuildingData.ContainsKey(_buildingInstanceId))
            {
                data.PlayerBuildingData[_buildingInstanceId].IsBuildingUnlocked = _isBuildingUnlocked;
                data.PlayerBuildingData[_buildingInstanceId].CurrentBuildingLevel = _currentBuildingLevel;
            }
            else
            {
                data.PlayerBuildingData.Add(_buildingInstanceId, new SerializedBuildingData
                {
                    IsBuildingUnlocked = _isBuildingUnlocked,
                    CurrentBuildingLevel = _currentBuildingLevel
                });
            }
        }

        public void LoadData(GameData data)
        {
            if(data.PlayerBuildingData.ContainsKey(_buildingInstanceId))
            {
                _isBuildingUnlocked = data.PlayerBuildingData[_buildingInstanceId].IsBuildingUnlocked;
                _currentBuildingLevel = data.PlayerBuildingData[_buildingInstanceId].CurrentBuildingLevel;
            }

            //Reinit the building
            _currentBuildingStat = _buildingStatsPerLevelDictionary[_currentBuildingLevel];
        }

        #region IClickable
        public void OnClick()
        {
            GameManager.Instance.AudioManager.PlaySFX(_buildingClickSound);
            
            GameplayUIManager.Instance.BuildingUIManager.OpenBuildingUI(this);

            Debug.Log("Clicked at " + this.gameObject.name);
        }

        public void OnStopClick()
        {
            GameplayUIManager.Instance.BuildingUIManager.CloseBuildingUI();

            Debug.Log("Stop clicked at " + this.gameObject.name);
        }
        #endregion

        // Deprecated cause making upgrade with these is too complex with current production time
        // #region IInteractable
        // [SerializeField] private Transform _buildingEntryPoint;        
        // [SerializeField] private List<Transform> _interactPoints;
        // private List<Transform> _usedInteractPoints;
        // public Transform GetBuildingEntryPoint() => _buildingEntryPoint;
        // public List<Transform> GetInteractPoints() => _interactPoints;

        // public void ReserveInteractPoints(Transform transf)
        // {
        //     _usedInteractPoints.Add(transf);
        //     _interactPoints.Remove(transf);
        // }

        // public void UnReserveInteractPoints(Transform transf)
        // {
        //     _interactPoints.Add(transf);            
        //     _usedInteractPoints.Remove(transf);
        // }

        // public void OnInteract(GameObject interactedObject)
        // {

        // }

        // public void OnStopInteract(GameObject interactedObject)
        // {
            

        // }
        // #endregion

        #region IInteractable 
        [Header("Interaction")]
        public string Interact_Anim_StateName = "INTERACT";
        public string StopInteract_Anim_StateName = "STOPINTERACT";
        [SerializeField] private Transform _interactPoint;
        // [SerializeField] private string _interactAnimName;
        // [SerializeField] private GameObject _buildingCover;
        // private List<Transform> _usedInteractPoints;
        // private List<Furniture> _usedFurnitures;
        public List<GameObject> CharactersObjectInteracting = new List<GameObject>();  
        public Transform GetInteractPoint() => _interactPoint;
        // public string GetInteractAnimName() => _interactAnimName;

        public void OnInteract(GameObject interactedObject)
        {
            CharactersObjectInteracting.Add(interactedObject);
            
            //So that the building won't be interacted again when there is still a character inside
            if(CharactersObjectInteracting.Count > 1) 
                return;

            SetAnimCrossFade(Interact_Anim_StateName, 0.1f, 1);
        }

        public void OnStopInteract(GameObject interactedObject)
        {
            CharactersObjectInteracting.Remove(interactedObject);

            //So that the building won't be interacted again when there is still a character inside
            if(CharactersObjectInteracting.Count > 0) 
                return;

            SetAnimCrossFade(StopInteract_Anim_StateName, 0.1f, 1);

            // _buildingAnimator.CrossFade(Idle_Anim_StateName, 0.1f);
        }
        #endregion
        
        private IEnumerator GenerateResource()
        {
            while(true)
            {
                foreach (var resourceGenerationStat in _currentBuildingStat.ResourceGenerationStats)
                {
                    yield return new WaitForSeconds(resourceGenerationStat.GenerationTimeInterval);
                    LevelManager.Instance.ResourceManager.ModifyResourceAmount(resourceGenerationStat.ResourceGenerated.ResourceId, 
                        resourceGenerationStat.AmountGenerated);
                }
            }
        }

        #region UpgradeMechanics
        public void TryUpgradeBuilding()
        {
            if (!IsBuildingUpgradeable())
            {
                return;
            }
            if (!_buildingStatsPerLevelDictionary.ContainsKey(_currentBuildingLevel + 1))
            {
                return;
            }
            if (!_isBuildingUnlocked)
            {
                _isBuildingUnlocked = true;
                SetAnimCrossFade(Unlocked_Anim_State_Name, 0.1f, 1);
                SetAnimCrossFade(Idle_Anim_StateName, 0.1f, 0);
                // return;
            }
            else
            {
                SetAnimCrossFade(Upgrade_Anim_State_Name, 0.1f, 1);
            }

            Debug.Log("Upgrade building");

            _currentBuildingStat = _buildingStatsPerLevelDictionary[_currentBuildingLevel + 1];
            _currentBuildingLevel++;
            
        }

        public bool IsBuildingUpgradeable()
        {
            // Check if the building is at max level
            if(_currentBuildingLevel >= _buildingDataSO.MaxBuildingLevel)
            {
                return false;
            }
            
            var buildingUpgradeRequirements = _currentBuildingStat.BuildingUpgradeRequirements;
            if(buildingUpgradeRequirements.Length == 0 || buildingUpgradeRequirements == null)
            {
                return false;
            }

            //Check if the resource is enough to upgrade the building
            foreach (var buildingUpgradeRequirement in _currentBuildingStat.BuildingUpgradeRequirements)
            {
                if (LevelManager.Instance.ResourceManager.GetResourceAmount(buildingUpgradeRequirement.ResourceRequired.ResourceId) < buildingUpgradeRequirement.AmountRequired)
                {
                    return false;
                }
            }

            return true;
        }
        #endregion


        #region Getter
        public BuildingSO GetBuildingSO() => _buildingDataSO;
        public bool IsBuildingUnlocked() => _isBuildingUnlocked;
        // public bool IsBuildingUsed() => _isBuildingUsed;
        public Dictionary<int, BuildingStat> GetBuildingStatsPerLevelDictionary() => _buildingStatsPerLevelDictionary;
        public BuildingStat GetCurrentBuildingStat() => _currentBuildingStat;
        public int GetCurrentBuildingLevel() => _currentBuildingLevel;
        public bool IsBuildingMaxLevel() => _currentBuildingLevel >= _buildingDataSO.MaxBuildingLevel;

        public static List<Building> GetAllBuildings(GroupData groupData)
        {
            var buildingList = new List<Building>();

            if (_buildingObjectCollection.ContainsKey(groupData))
            {
                foreach (var building in _buildingObjectCollection[groupData])
                {
                    buildingList.Add(building.Value);
                }
            }

            return buildingList;
        }
        #endregion

        // Generate a unique ID
        public void GenerateInstanceID()
        {
            _buildingInstanceId = _buildingDataSO.BuildingBaseId + "_" + System.Guid.NewGuid().ToString();
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

            Building script = (Building)target;
            if (GUILayout.Button("Generate Instance ID"))
            {
                script.GenerateInstanceID();
                EditorUtility.SetDirty(script);
            }
        }
    }
#endif

}
