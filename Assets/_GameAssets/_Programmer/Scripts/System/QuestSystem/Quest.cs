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
    public class Quest : IObserver
    {
        private List<IObserver> _onQuestStateChangedEventObservers = new List<IObserver>();

        public QuestSO QuestData { get; private set; }
        public bool IsQuestCompleted { get; private set; } = false;
        

        public Quest(QuestSO questData, IObserver questStateObserver)
        {
            QuestData = questData;
        }

        ~Quest()
        {
            QuestData = null;
        }


        private void NotifyOnQuestStateChangedEventObservers()
        {
            foreach (var observer in _onQuestStateChangedEventObservers)
            {
                observer.OnNotify();
            }
        }
        public void OnObjectiveStateChangedEventRegister(IObserver observer)
        {
            _onQuestStateChangedEventObservers.Add(observer);
        }
        public void OnObjectiveStateChangedEventUnregister(IObserver observer)
        {
            _onQuestStateChangedEventObservers.Remove(observer);
        }

        public void OnNotify()
        {
            EvaluateAllObjectives();
        }

        private void EvaluateAllObjectives()
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
            NotifyOnQuestStateChangedEventObservers();
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