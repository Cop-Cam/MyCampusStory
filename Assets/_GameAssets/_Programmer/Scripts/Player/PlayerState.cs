//----------------------------------------------------------------------
// Author   : "Ananta Miyoru Wijaya"
//----------------------------------------------------------------------

using UnityEngine;

namespace MyCampusStory.Player
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class PlayerBaseState
    {
        public abstract void EnterState(PlayerStateManager playerStateManager);
        public abstract void UpdateState(PlayerStateManager playerStateManager);
        public abstract void ExitState(PlayerStateManager playerStateManager);
    }

    public class PlayerIdleState : PlayerBaseState
    {
        PlayerAnimation _playerAnimation;

        public override void EnterState(PlayerStateManager playerStateManager)
        {
            _playerAnimation = playerStateManager.GetComponent<PlayerAnimation>();  
        }

        public override void UpdateState(PlayerStateManager playerStateManager)
        {
            _playerAnimation.SetAnimBool(_playerAnimation.Idle_anim, true);
        }

        public override void ExitState(PlayerStateManager playerStateManager)
        {
            _playerAnimation.SetAnimBool(_playerAnimation.Idle_anim, false);
            _playerAnimation = null;
        }
    }

    public class PlayerWalkState : PlayerBaseState
    {
        public override void EnterState(PlayerStateManager playerStateManager)
        {
            throw new System.NotImplementedException();    
        }

        public override void ExitState(PlayerStateManager playerStateManager)
        {
            throw new System.NotImplementedException();
        }

        public override void UpdateState(PlayerStateManager playerStateManager)
        {
            throw new System.NotImplementedException();
        }
    }
}
