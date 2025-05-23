//----------------------------------------------------------------------
// Author   : "Ananta Miyoru Wijaya"
//----------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



namespace MyCampusStory.Character
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Character : MonoBehaviour
    {
        [Header("Unique Instance Fields")]
        [Tooltip("Scene group for this character!")]
        [SerializeField] protected GroupData _sceneGroup;

        [Header("References")]
        [SerializeField] protected NavMeshAgent _navMeshAgent;
        [SerializeField] protected Animator _animator;
        protected string _currentAnimState;
        
        protected CharacterState _currentState;

        public void SwitchState(CharacterState newState)
        {
            _currentState.ExitState(this);
            _currentState = newState;
            _currentState.EnterState(this);
        }

        public void MoveTo(Transform targetPos)
        {
            _navMeshAgent.SetDestination(targetPos.position);
        }

        
        #region Getter
        public GroupData GetSceneGroup() => _sceneGroup;
        public NavMeshAgent GetNavMeshAgent() => _navMeshAgent;
        #endregion


        #region Animator
        public float GetAnimDuration(string animName)
        {
            foreach (var clip in _animator.runtimeAnimatorController.animationClips)
            {
                if (clip.name == animName)
                    return clip.length;
            }
            return 0.5f; // fallback if not found
        }

        public void SetAnimBool(string id, bool value)
        {
            if (!string.IsNullOrEmpty(id))
                _animator?.SetBool(id, value);
        }

        public void SetAnimFloat(string id, float value)
        {
            if (!string.IsNullOrEmpty(id))
                _animator?.SetFloat(id, value);
        }

        public void SetAnimTrigger(string id)
        {
            if (!string.IsNullOrEmpty(id))
                _animator?.SetTrigger(id);
        }

        public void SetAnimCrossFade(string id, float time)
        {
            if (!string.IsNullOrEmpty(id) && _currentAnimState != id)
            {
                _animator?.CrossFade(id, time);
                _currentAnimState = id;
            }
        }
        #endregion


    }
}
