//----------------------------------------------------------------------
// Author   : "Ananta Miyoru Wijaya"
//----------------------------------------------------------------------

namespace MyCampusStory.DataPersistenceSystem
{
    /// <summary>
    /// Interface for defining contract to implement load and save data behaviour in class
    /// </summary>
    public interface IDataPersistence
    {
        void SaveData(GameData data);
        void LoadData(GameData data);
    }
}