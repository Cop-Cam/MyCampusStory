//----------------------------------------------------------------------
// Author   : "Ananta Miyoru Wijaya"
//----------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

using MyCampusStory.DesignPatterns;

namespace MyCampusStory.QuestSystem
{
    /// <summary>
    /// Class for quest
    /// </summary>
    public class Quest
    {
        public delegate void QuestIsCompletedDelegate();
        public event QuestIsCompletedDelegate OnQuestIsCompleted;

        public QuestSO QuestData { get; private set; }
        public bool IsQuestCompleted { get; private set; } = false;
        

        public Quest(QuestSO questData)
        {
            QuestData = questData;

            foreach (var objective in QuestData.QuestObjectives)
            {
                objective.OnObjectiveIsCompleted += EvaluateAllQuestObjectives;
            }
        }

        private void EvaluateAllQuestObjectives()
        {
            foreach (var objective in QuestData.QuestObjectives)
            {
                if(!objective.IsObjectiveCompleted)
                {
                    return;
                }
            }

            // Complete quest
            IsQuestCompleted = true;
            OnQuestIsCompleted?.Invoke();

            foreach (var objective in QuestData.QuestObjectives)
            {
                objective.OnObjectiveIsCompleted -= EvaluateAllQuestObjectives;
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
        public Objective[] QuestObjectives;
    }    
}