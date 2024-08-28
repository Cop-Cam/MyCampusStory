//----------------------------------------------------------------------
// Author   : "Ananta Miyoru Wijaya"
//----------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

using MyCampusStory.ResourceSystem;
using MyCampusStory.DesignPatterns;
using MyCampusStory.StandaloneManager;


namespace MyCampusStory.QuestSystem
{
    #region ObjectiveBaseClasses
    public abstract class Objective
    {
        public delegate void ObjectiveIsCompletedDelegate();
        public event ObjectiveIsCompletedDelegate OnObjectiveIsCompleted;
        
        public ObjectiveSO ObjectiveData { get; protected set; }
        public bool IsObjectiveCompleted { get; protected set; } = false;

        protected void InvokeOnObjectiveIsCompleted()
        {
            OnObjectiveIsCompleted?.Invoke();
        }

        public Objective(ObjectiveSO objectiveSO)
        {
            ObjectiveData = objectiveSO;
        }
    }

    public abstract class ObjectiveSO : ScriptableObject
    {
        public string ObjectiveId;
        public string ObjectiveName;

        public abstract Objective GetObjectiveInstance();
    }
    #endregion


    #region GatherResourcesObjective
    public class GatherResourcesObjective : Objective
    {
        private ResourceManager _resourceManager;
        private GatherResourcesObjectiveSO _gatherResourcesObjectiveSO;
        
        public GatherResourcesObjective(ObjectiveSO objectiveSO) : base(objectiveSO)
        {
            _resourceManager = LevelManager.Instance.ResourceManager;

            if(LevelManager.Instance.ResourceManager != null)
                LevelManager.Instance.ResourceManager.OnResourceChanged += EvaluateResourceAmount;
        }

        private void EvaluateResourceAmount()
        {
            if(_resourceManager.GetResourceAmount(_gatherResourcesObjectiveSO.ResourceRequired.ResourceId) >= _gatherResourcesObjectiveSO.AmountRequired)
            {
                IsObjectiveCompleted = true;
                InvokeOnObjectiveIsCompleted();
                
                if(LevelManager.Instance.ResourceManager != null)
                    LevelManager.Instance.ResourceManager.OnResourceChanged -= EvaluateResourceAmount;
            }
        }
    }

    [CreateAssetMenu(fileName = "Name_GatherResourcesObjectiveSO", menuName = "ScriptableObjects/ObjectiveSO/GatherResourcesObjectiveSO")]
    public class GatherResourcesObjectiveSO : ObjectiveSO
    {
        public ResourceSO ResourceRequired;
        public int AmountRequired;

        public override Objective GetObjectiveInstance()
        {
            throw new System.NotImplementedException();
        }
    }
    #endregion
}