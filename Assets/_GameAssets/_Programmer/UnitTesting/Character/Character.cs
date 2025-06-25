using UnityEngine;

namespace MyCampusStory.StatePatternTest
{
    public abstract class Character : MonoBehaviour
    {
        protected CharacterState _currentState;

        public void SwitchState(CharacterState newState)
        {
            _currentState?.ExitState(this);
            _currentState = newState;
            _currentState?.EnterState(this);
        }

        public CharacterState GetCurrentState() => _currentState;

        // For simulation (fake movement completed flag)
        public virtual bool HasArrived => true;
    }
}
