using System.Collections.Generic;
using UnityEngine;
using MyCampusStory.EffectSystem;

namespace MyCampusStory.ActionSystem
{
    public abstract class Action : ScriptableObject
    {
        [SerializeField] protected List<Effect> _actionEffects;
        [SerializeField] protected List<Effect> _actionCost;

        public abstract void ExecuteAction();
        protected void ApplyEffects()
        {
            if(_actionEffects == null) return;
            

            foreach(Effect effect in _actionEffects)
            {
                effect.ApplyEffect();
            }
        }
        protected void ApplyCosts()
        {
            if(_actionCost == null) return;


            foreach(Effect effect in _actionEffects)
            {
                effect.ApplyEffect();
            }
        }
    }

}
