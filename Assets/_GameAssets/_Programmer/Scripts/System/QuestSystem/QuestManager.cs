//----------------------------------------------------------------------
// Author   : "Ananta Miyoru Wijaya"
//----------------------------------------------------------------------

using System.Collections.Generic;
using MyCampusStory.DataPersistenceSystem;
using UnityEngine;

namespace MyCampusStory.QuestSystem
{
    /// <summary>
    /// Class for managing quests
    /// </summary>
    public class QuestManager : MonoBehaviour, IDataPersistence
    {
        [SerializeField] private List<QuestlineSO> QuestlineList = new List<QuestlineSO>();
        public Dictionary<string, Questline> QuestlineDictionary { get; private set; } = new Dictionary<string, Questline>();
        
        [SerializeField] private QuestlineSO StartingQuestlineSO;
        public Questline CurrentActiveQuestline { get; private set; }


        private void Awake()
        {
            foreach (var questlineSO in QuestlineList)
            {
                var questline = new Questline(questlineSO);

                if (!QuestlineDictionary.ContainsKey(questlineSO.QuestlineId))
                {
                    QuestlineDictionary.Add(questlineSO.QuestlineId, questline);
                }
            }
        }

        public void StartTheFirstQuestline()
        {
            CurrentActiveQuestline = QuestlineDictionary[StartingQuestlineSO.QuestlineId];
        }

        public void SaveData(GameData data)
        {
            foreach (var questline in QuestlineDictionary)
            {
                if(!data.PlayerQuestData.ContainsKey(questline.Key))
                {
                    data.PlayerQuestData.Add(questline.Key, questline.Value);
                }
                else
                {
                    data.PlayerQuestData[questline.Key] = questline.Value;
                }
            }
        }

        public void LoadData(GameData data)
        {
            foreach (var questline in data.PlayerQuestData)
            {
                if (!QuestlineDictionary.ContainsKey(questline.Key))
                {
                    QuestlineDictionary.Add(questline.Key, questline.Value);
                }
                else
                {
                    QuestlineDictionary[questline.Key] = questline.Value;
                }
            }
        }

        
    }
}