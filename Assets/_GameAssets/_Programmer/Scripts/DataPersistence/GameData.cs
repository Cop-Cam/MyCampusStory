//----------------------------------------------------------------------
// Author   : "Ananta Miyoru Wijaya"
//----------------------------------------------------------------------

namespace MyCampusStory.DataPersistenceSystem
{
    /// <summary>
    /// Class for defining game data that will be saved and loaded
    /// Try to save only necessary information! 
    /// Ex: SAVE variable that will change class behaviour and not the behaviour itself 
    /// </summary>
    [System.Serializable]
    public class GameData
    {
        public long LastUpdated;

        public int PlayerLevel;
        /// <summary>
        /// Dictionary for storing all the player resources data. 
        /// The key is an id of the resource, while the value is the amount of the resource.
        /// </summary>
        public SerializableDictionary<string, int> PlayerResourceData;

        public SerializableDictionary<string, SerializedBuildingData> PlayerBuildingData;

        //Default Constructor
        // The values defined in this constructor will be the default values the game starts with when there's no data to load
        public GameData()
        {
            PlayerResourceData = new SerializableDictionary<string, int>();
            PlayerBuildingData = new SerializableDictionary<string, SerializedBuildingData>();
        }
    }

    [System.Serializable]
    public class SerializedBuildingData
    {
        public bool IsBuildingUnlocked;
        public int CurrentBuildingLevel;
    }
}