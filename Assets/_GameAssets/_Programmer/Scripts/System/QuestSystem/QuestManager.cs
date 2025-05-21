//----------------------------------------------------------------------
// Author   : "Ananta Miyoru Wijaya"
//----------------------------------------------------------------------

using System;
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
        public event Action OnQuestStarted;

        [SerializeField] private List<QuestSO> _questsSO = new List<QuestSO>();
        public Dictionary<string, Quest> QuestsDictionary { get; private set; } = new Dictionary<string, Quest>();
        
        // public List<Quest> CurrentActiveQuests { get; private set; }
        public Quest CurrentActiveQuest { get; private set; }

        [SerializeField] private QuestSO _startingQuestSO;


        private void Awake()
        {
            foreach (var questSO in _questsSO)
            {
                var quest = new Quest(questSO);

                if (!QuestsDictionary.ContainsKey(questSO.QuestId))
                {
                    QuestsDictionary.Add(questSO.QuestId, quest);
                }
            }
        }

        // public void StartTheFirstQuestline()
        // {
        //     if(_startingQuestSO == null)
        //     {
        //         Debug.LogError("No starting quest found");
        //         return;
        //     }

        //     CurrentActiveQuest = new Quest(_startingQuestSO);
        //     // foreach(var questSO in _startingQuestsSO)
        //     // {
        //     //     CurrentActiveQuests.Add(new Quest(questSO));
        //     // }
        //     // CurrentActiveQuestline = QuestlineDictionary[StartingQuestlineSO.QuestlineId];
            
        //     OnQuestStarted?.Invoke();
        // }

        public void StartNewQuest(string id, bool IsStartingNewQuest = true)
        {
            CurrentActiveQuest = null;

            if(IsStartingNewQuest)
            {
                CurrentActiveQuest = new Quest(QuestsDictionary[id].QuestData);
            }
            
            OnQuestStarted?.Invoke();
        }

        public void SaveData(GameData data)
        {
            foreach (var quest in QuestsDictionary)
            {
                if(!data.PlayerQuestData.ContainsKey(quest.Key))
                {
                    data.PlayerQuestData.Add(quest.Key, quest.Value);
                }
                else
                {
                    data.PlayerQuestData[quest.Key] = quest.Value;
                }
            }
        }

        public void LoadData(GameData data)
        {
            foreach (var quest in data.PlayerQuestData)
            {
                if (!QuestsDictionary.ContainsKey(quest.Key))
                {
                    QuestsDictionary.Add(quest.Key, quest.Value);
                }
                else
                {
                    QuestsDictionary[quest.Key] = quest.Value;
                }
            }
        }

        
    }

    // /// <summary>
    // /// Class for managing quests
    // /// </summary>
    // public class QuestManager : MonoBehaviour, IDataPersistence
    // {
    //     public event Action OnQuestlineStarted;

    //     [SerializeField] private List<QuestlineSO> QuestlineList = new List<QuestlineSO>();
    //     public Dictionary<string, Questline> QuestlineDictionary { get; private set; } = new Dictionary<string, Questline>();
        
    //     [SerializeField] private QuestlineSO StartingQuestlineSO;
    //     public Questline CurrentActiveQuestline { get; private set; }

    //     [SerializeField] private List<QuestSO> _startingQuests = new List<QuestSO>();


    //     private void Awake()
    //     {
    //         foreach (var questlineSO in QuestlineList)
    //         {
    //             var questline = new Questline(questlineSO);

    //             if (!QuestlineDictionary.ContainsKey(questlineSO.QuestlineId))
    //             {
    //                 QuestlineDictionary.Add(questlineSO.QuestlineId, questline);
    //             }
    //         }
    //     }

    //     public void StartTheFirstQuestline()
    //     {
    //         CurrentActiveQuestline = QuestlineDictionary[StartingQuestlineSO.QuestlineId];
            
    //         OnQuestlineStarted?.Invoke();
    //     }

    //     public void SaveData(GameData data)
    //     {
    //         foreach (var questline in QuestlineDictionary)
    //         {
    //             if(!data.PlayerQuestData.ContainsKey(questline.Key))
    //             {
    //                 data.PlayerQuestData.Add(questline.Key, questline.Value);
    //             }
    //             else
    //             {
    //                 data.PlayerQuestData[questline.Key] = questline.Value;
    //             }
    //         }
    //     }

    //     public void LoadData(GameData data)
    //     {
    //         foreach (var questline in data.PlayerQuestData)
    //         {
    //             if (!QuestlineDictionary.ContainsKey(questline.Key))
    //             {
    //                 QuestlineDictionary.Add(questline.Key, questline.Value);
    //             }
    //             else
    //             {
    //                 QuestlineDictionary[questline.Key] = questline.Value;
    //             }
    //         }
    //     }

        
    // }
}