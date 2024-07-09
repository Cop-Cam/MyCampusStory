//----------------------------------------------------------------------
// Author   : "Ananta Miyoru Wijaya"
//----------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

using MyCampusStory.DesignPatterns;

namespace MyCampusStory.QuestSystem
{
    /// <summary>
    /// Class for Questline initialization and runtime
    /// </summary>
    public class Questline : IObserver
    {
        private List<IObserver> _onQuestlineStateChangedEventObservers = new List<IObserver>();
        public QuestlineSO QuestlineData { get; private set; }

        public bool IsQuestlineCompleted { get; private set; } = false;

        public Quest CurrentActiveQuestInQuestline { get; private set; }
        private int _currentActiveQuestIndex = 0;


        //Constructor
        public Questline(QuestlineSO questlineSo)
        {
            QuestlineData = questlineSo;

            CurrentActiveQuestInQuestline = QuestlineData.QuestsInQuestline[_currentActiveQuestIndex];
        }

        public virtual void OnQuestlineStateChangedEventRegister(IObserver observer)
        {
            _onQuestlineStateChangedEventObservers.Add(observer);
        }
        public virtual void OnQuestlineStateChangedEventUnregister(IObserver observer)
        {
            _onQuestlineStateChangedEventObservers.Remove(observer);
        }
        private void NotifyOnResourceChangedEventObservers()
        {
            foreach (var observer in _onQuestlineStateChangedEventObservers)
            {
                observer.OnNotify();
            }
        }

        #region IObserver
        public void OnNotify()
        {
            if(CurrentActiveQuestInQuestline != null)
            {
                EvaluateCurrentActiveQuestInQuestline();
            }
        }
        #endregion

        private void EvaluateCurrentActiveQuestInQuestline()
        {
            if(!CurrentActiveQuestInQuestline.IsQuestCompleted) 
                return;

            if(_currentActiveQuestIndex < QuestlineData.QuestsInQuestline.Length - 1)
            {
                _currentActiveQuestIndex++;
                CurrentActiveQuestInQuestline = QuestlineData.QuestsInQuestline[_currentActiveQuestIndex];
            }
            else
            {
                IsQuestlineCompleted = true;
                NotifyOnResourceChangedEventObservers();
            }
        }
    }
    
    /// <summary>
    /// Class for Questline data only or template purpose
    /// </summary>
    
    [CreateAssetMenu(fileName = "Name_QuestSO", menuName = "ScriptableObjects/QuestSO")]
    public class QuestlineSO : ScriptableObject
    {
        public string QuestlineId;
        public string QuestlineName;
        public Quest[] QuestsInQuestline;
    }
}