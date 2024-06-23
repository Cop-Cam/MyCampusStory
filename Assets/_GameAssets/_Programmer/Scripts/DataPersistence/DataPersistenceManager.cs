//----------------------------------------------------------------------
// Author   : "Ananta Miyoru Wijaya"
//----------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

namespace MyCampusStory.DataPersistenceSystem
{
    public class DataPersistenceManager : MonoBehaviour
    {
        [Header("Debugging")]
        [SerializeField] private bool _disableDataPersistence = false;
        [SerializeField] private bool _initializeDataIfNull = false;
        [SerializeField] private bool _overrideSelectedProfileId = false;
        [SerializeField] private string _testSelectedProfileId = "test";

        [Header("File Storage Config")]
        [SerializeField] private string _fileName;
        [SerializeField] private bool _useEncryption;

        [Header("Auto Saving Configuration")]
        [SerializeField] private bool _enableAutoSave = false;
        [SerializeField] private float _autoSaveTimeSeconds = 60f;

        private GameData _gameData;
        private List<IDataPersistence> _dataPersistenceObjects;
        private FileDataHandler _dataHandler;

        private string _selectedProfileId = "";

        private Coroutine _autoSaveCoroutine;

        private void Awake() 
        {
            if (_disableDataPersistence) 
            {
                Debug.LogWarning("Data Persistence is currently disabled!");
            }

            _dataHandler = new FileDataHandler(Application.persistentDataPath, _fileName, _useEncryption);

            InitializeSelectedProfileId();
        }

        private void OnEnable() 
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void OnDisable() 
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode mode) 
        {
            _dataPersistenceObjects = FindAllDataPersistenceObjects();
            LoadState();

            // start up the auto saving coroutine
            if (_autoSaveCoroutine != null) 
            {
                StopCoroutine(_autoSaveCoroutine);
            }

            if(_enableAutoSave)
            {
                _autoSaveCoroutine = StartCoroutine(AutoSave());
            }
        }

        private void OnSceneUnloaded(Scene scene)
        {
            SaveState();
        }

        public void ChangeSelectedProfileId(string newProfileId) 
        {
            // update the profile to use for saving and loading
            _selectedProfileId = newProfileId;
            // load the game, which will use that profile, updating our game data accordingly
            LoadGame();
            LoadState();
        }

        public void DeleteProfileData(string profileId) 
        {
            // delete the data for this profile id
            _dataHandler.Delete(profileId);
            // initialize the selected profile id
            InitializeSelectedProfileId();
            // reload the game so that our data matches the newly selected profile id
            LoadGame();
        }

        private void InitializeSelectedProfileId() 
        {
            _selectedProfileId = _dataHandler.GetMostRecentlyUpdatedProfileId();
            if (_overrideSelectedProfileId) 
            {
                _selectedProfileId = _testSelectedProfileId;
                Debug.LogWarning("Overrode selected profile id with test id: " + _testSelectedProfileId);
            }
        }

        public void NewGame() 
        {
            _gameData = new GameData();
        }

        public void SaveGame()
        {
            // return right away if data persistence is disabled
            if (_disableDataPersistence) 
            {
                return;
            }

            // if we don't have any data to save, log a warning here
            if (_gameData == null) 
            {
                Debug.LogWarning("No data was found. A New Game needs to be started before data can be saved.");
                return;
            }

            // pass the data to other scripts so they can update it
            foreach (IDataPersistence dataPersistenceObj in _dataPersistenceObjects) 
            {
                dataPersistenceObj.SaveData(_gameData);
            }

            // timestamp the data so we know when it was last saved
            _gameData.LastUpdated = System.DateTime.Now.ToBinary();

            // save that data to a file using the data handler
            _dataHandler.SaveToFile(_gameData, _selectedProfileId);
        }

        public void LoadGame()
        {
            // return right away if data persistence is disabled
            if (_disableDataPersistence) 
            {
                return;
            }

            // load any saved data from a file using the data handler
            _gameData = _dataHandler.LoadFromFile(_selectedProfileId);

            // start a new game if the data is null and we're configured to initialize data for debugging purposes
            if (_gameData == null && _initializeDataIfNull) 
            {
                NewGame();
            }

            // if no data can be loaded, don't continue
            if (_gameData == null) 
            {
                Debug.Log("No data was found. A New Game needs to be started before data can be loaded.");
                return;
            }

            // push the loaded data to all other scripts that need it
            // foreach (IDataPersistence dataPersistenceObj in _dataPersistenceObjects) 
            // {
            //     dataPersistenceObj.LoadData(_gameData);
            // }
        }

        private void SaveState()
        {
            // return right away if data persistence is disabled
            if (_disableDataPersistence) 
            {
                return;
            }

            // if we don't have any data to save, log a warning here
            if (_gameData == null) 
            {
                Debug.LogWarning("No data was found. A New Game needs to be started before data can be saved.");
                return;
            }

            // pass the data to other scripts so they can update it
            foreach (IDataPersistence dataPersistenceObj in _dataPersistenceObjects) 
            {
                dataPersistenceObj.SaveData(_gameData);
            }
        }

        private void LoadState()
        {
            // push the loaded data to all other scripts that need it
            foreach (IDataPersistence dataPersistenceObj in _dataPersistenceObjects) 
            {
                dataPersistenceObj.LoadData(_gameData);
            }
        }

        // private void OnApplicationQuit() 
        // {
        //     SaveGame();
        // }

        private List<IDataPersistence> FindAllDataPersistenceObjects() 
        {
            // FindObjectsofType takes in an optional boolean to include inactive gameobjects
            IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>(true)
                .OfType<IDataPersistence>();

            foreach(IDataPersistence dataPersistenceObj in dataPersistenceObjects)
            {
                Debug.Log("Found and initialized: "+ dataPersistenceObj.GetType().Name);
            }

            return new List<IDataPersistence>(dataPersistenceObjects);
        }

        public bool HasGameData() 
        {
            return _gameData != null;
        }

        public Dictionary<string, GameData> GetAllProfilesGameData() 
        {
            return _dataHandler.LoadAllProfiles();
        }

        private IEnumerator AutoSave() 
        {
            while (true) 
            {
                yield return new WaitForSeconds(_autoSaveTimeSeconds);
                SaveGame();
                Debug.Log("Auto Saved Game");
            }
        }
    }

}
