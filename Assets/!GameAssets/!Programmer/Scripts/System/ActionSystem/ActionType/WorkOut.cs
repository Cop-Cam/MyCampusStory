using System.Collections.Generic;
using UnityEngine;

namespace MyCampusStory.ActionSystem
{
    [CreateAssetMenu(fileName = "WorkOut_", menuName = "Scriptable Objects/Action Assets/New WorkOut")]
    public class WorkOut : Action
    {
        public override void ExecuteAction()
        {
            ApplyCosts();

            WorkingOut();
            
            ApplyEffects();
        }

        //Do whatever action needed for workingout
        private void WorkingOut()
        {

        }

        
    }

}
