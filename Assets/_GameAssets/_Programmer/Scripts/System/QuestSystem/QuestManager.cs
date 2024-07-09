//----------------------------------------------------------------------
// Author   : "Ananta Miyoru Wijaya"
//----------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace MyCampusStory.QuestSystem
{
    /// <summary>
    /// Class for managing quests
    /// </summary>
    public class QuestManager : MonoBehaviour
    {
        [SerializeField] private List<QuestlineSO> QuestlinesList = new List<QuestlineSO>();
    
        public Dictionary<string, Questline> QuestlineDictionary { get; private set; } = new Dictionary<string, Questline>();

        public Questline CurrentActiveQuestline { get; private set; }

        private void Awake()
        {
            foreach (var questlineSO in QuestlinesList)
            {
                var questline = new Questline(questlineSO);

                if (!QuestlineDictionary.ContainsKey(questlineSO.QuestlineId))
                {
                    QuestlineDictionary.Add(questlineSO.QuestlineId, questline);
                }
            }
        }

        
    }
}