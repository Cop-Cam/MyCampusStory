//----------------------------------------------------------------------
// Author   : "Ananta Miyoru Wijaya"
//----------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

using MyCampusStory.ResourceSystem;
using MyCampusStory.DesignPatterns;


namespace MyCampusStory.QuestSystem
{
    public abstract class Objective
    {
        protected List<IObserver> _onObjectiveStateChangedEventObservers = new List<IObserver>();
        public bool IsObjectiveCompleted { get; protected set; }
        protected virtual void NotifyOnResourceChangedEventObservers()
        {
            foreach (var observer in _onObjectiveStateChangedEventObservers)
            {
                observer.OnNotify();
            }
        }
        public virtual void OnObjectiveStateChangedEventRegister(IObserver observer)
        {
            _onObjectiveStateChangedEventObservers.Add(observer);
        }
        public virtual void OnObjectiveStateChangedEventUnregister(IObserver observer)
        {
            _onObjectiveStateChangedEventObservers.Remove(observer);
        }
    }


    #region GatherResourcesObjective
    public class GatherResourcesObjective : Objective, IObserver
    {
        private ResourceManager _resourceManager;
        private GatherResourcesObjectiveSO _gatherResourcesObjectiveSO;
        
        public GatherResourcesObjective(ResourceManager resourceManager)
        {
            _resourceManager = resourceManager;

            ResourceManager.OnResourceChangedEventRegister(this);
        }

        ~GatherResourcesObjective()
        {
            ResourceManager.OnResourceChangedEventUnregister(this);
        }

        public void OnNotify()
        {
            if(_resourceManager.GetResourceAmount(_gatherResourcesObjectiveSO.ResourceRequired.ResourceId) >= _gatherResourcesObjectiveSO.AmountRequired)
            {
                IsObjectiveCompleted = true;
                NotifyOnResourceChangedEventObservers();
            }
        }
    }

    [CreateAssetMenu(fileName = "Name_GatherResourcesObjectiveSO", menuName = "ScriptableObjects/ObjectiveSO/GatherResourcesObjectiveSO")]
    public class GatherResourcesObjectiveSO : ScriptableObject
    {
        public string ObjectiveId;
        public string ObjectiveName;
        public ResourceSO ResourceRequired;
        public int AmountRequired;
    }
    #endregion
}