using System.Collections.Generic;
using UnityEngine;

namespace MyCampusStory.EffectSystem
{
    public enum EffectState
    {
        ACTIVATED,
        DEACTIVATED
    }

    public abstract class Effect : ScriptableObject
    {
        [SerializeField] protected string _effectId;
        [SerializeField] protected string _effectName;

        protected EffectState _effectState;

        public abstract void ApplyEffect();
    }

}
