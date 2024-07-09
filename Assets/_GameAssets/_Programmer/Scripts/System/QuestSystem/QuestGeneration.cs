//----------------------------------------------------------------------
// Author   : "Ananta Miyoru Wijaya"
//----------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

using MyCampusStory.StandaloneManager;

namespace MyCampusStory.QuestSystem
{
    /// <summary>
    /// Class for managing quests
    /// </summary>
    public class QuestGeneration : MonoBehaviour
    {
        private LevelManager _levelManager;
        

        private void Awake()
        {
            _levelManager = LevelManager.Instance;
        }

        public Quest GetNewGeneratedQuest()
        {
            return null;
        }


        
    }
}