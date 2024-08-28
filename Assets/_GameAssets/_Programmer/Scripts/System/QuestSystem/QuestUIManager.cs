//----------------------------------------------------------------------
// Author   : "Ananta Miyoru Wijaya"
//----------------------------------------------------------------------

using System.Collections.Generic;
using MyCampusStory.StandaloneManager;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MyCampusStory.QuestSystem
{
    /// <summary>
    /// Class for displaying quests
    /// </summary>
    public class QuestUIManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _questNameText;

        private void Start()
        {
        }

        private void Update()
        {

        }

        private void DisplayCurrentActiveQuest()
        {
            Questline currentActiveQuestline = LevelManager.Instance.QuestManager.CurrentActiveQuestline;
        }

        private void RefreshDisplay()
        {
            
        }
    }
}