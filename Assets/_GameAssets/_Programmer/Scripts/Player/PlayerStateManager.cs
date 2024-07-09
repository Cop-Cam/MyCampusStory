//----------------------------------------------------------------------
// Author   : "Ananta Miyoru Wijaya"
//----------------------------------------------------------------------

using UnityEngine;

namespace MyCampusStory.Player
{
    /// <summary>
    /// 
    /// </summary>
    public class PlayerStateManager : MonoBehaviour
    {
        private PlayerBaseState _currentState;

        public PlayerIdleState IdleState = new PlayerIdleState();
        public PlayerWalkState WalkState = new PlayerWalkState();

        private void Start()
        {

        }

        private void Update()
        {

        }

        public void SwitchState(PlayerBaseState newState)
        {
            _currentState.ExitState(this);
            _currentState = newState;
            _currentState.EnterState(this);
        }
    }
}
