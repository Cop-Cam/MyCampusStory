//----------------------------------------------------------------------
// Author   : "Ananta Miyoru Wijaya"
//----------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

using MyCampusStory.DesignPatterns;
using System;
using MyCampusStory.StandaloneManager;

namespace MyCampusStory.QuestSystem
{
    /// <summary>
    /// Class for quest
    /// </summary>
    public class Quest
    {
        // public delegate void QuestIsCompletedDelegate();
        public event Action OnQuestIsCompleted;

        public QuestSO QuestData { get; private set; }
        public bool IsQuestCompleted { get; private set; }
        public List<Objective> QuestObjectives { get; private set; }
        // public List<Quest> NextQuests { get; private set; }
        
        public Quest(QuestSO questData)
        {
            QuestData = questData;
            IsQuestCompleted = false;

            // foreach (var objective in QuestData.QuestObjectives)
            // {
            //     objective.OnObjectiveIsCompleted += EvaluateAllQuestObjectives;
            // }

            foreach (var objectiveSO in QuestData.QuestObjectivesSO)
            {
                Objective objective = new Objective(objectiveSO);
                QuestObjectives.Add(objective);
                objective.OnObjectiveIsCompleted += EvaluateAllQuestObjectives;
            }
        }

        private void EvaluateAllQuestObjectives()
        {
            foreach (var objective in QuestObjectives)
            {
                if(!objective.IsObjectiveCompleted)
                {
                    return;
                }
            }

            // Complete quest
            IsQuestCompleted = true;
            OnQuestIsCompleted?.Invoke();

            foreach (var objective in QuestObjectives)
            {
                objective.OnObjectiveIsCompleted -= EvaluateAllQuestObjectives;
            }

            // if(NextQuests.Count <= 0 || NextQuests == null) return;
            if(QuestData.NextQuest == null)
            {
                LevelManager.Instance.QuestManager.StartNewQuest(null, false);
            }
            else
            {
                LevelManager.Instance.QuestManager.StartNewQuest(QuestData.NextQuest.QuestId, true);
            }
        }
    }
    
    /// <summary>
    /// Class for quest
    /// </summary>
    [CreateAssetMenu(fileName = "Name_QuestSO", menuName = "ScriptableObjects/QuestSO")]
    public class QuestSO : ScriptableObject
    {
        public string QuestId;
        public string QuestName;

        [TextArea]
        public string QuestDescription;
        public ObjectiveSO[] QuestObjectivesSO;
        public QuestSO NextQuest;
    }

    // /// <summary>
    // /// Class for quest
    // /// </summary>
    // public class Quest
    // {
    //     // public delegate void QuestIsCompletedDelegate();
    //     public event Action OnQuestIsCompleted;

    //     public QuestSO QuestData { get; private set; }
    //     public bool IsQuestCompleted { get; private set; } = false;
        
    //     public Quest(QuestSO questData)
    //     {
    //         QuestData = questData;

    //         foreach (var objective in QuestData.QuestObjectives)
    //         {
    //             objective.OnObjectiveIsCompleted += EvaluateAllQuestObjectives;
    //         }
    //     }

    //     private void EvaluateAllQuestObjectives()
    //     {
    //         foreach (var objective in QuestData.QuestObjectives)
    //         {
    //             if(!objective.IsObjectiveCompleted)
    //             {
    //                 return;
    //             }
    //         }

    //         // Complete quest
    //         IsQuestCompleted = true;
    //         OnQuestIsCompleted?.Invoke();

    //         foreach (var objective in QuestData.QuestObjectives)
    //         {
    //             objective.OnObjectiveIsCompleted -= EvaluateAllQuestObjectives;
    //         }
    //     }
    // }
    
    // /// <summary>
    // /// Class for quest
    // /// </summary>
    // [CreateAssetMenu(fileName = "Name_QuestSO", menuName = "ScriptableObjects/QuestSO")]
    // public class QuestSO : ScriptableObject
    // {
    //     public string QuestId;
    //     public string QuestName;
    //     public Objective[] QuestObjectives;
    // }        
}