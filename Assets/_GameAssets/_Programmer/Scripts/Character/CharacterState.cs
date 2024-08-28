//----------------------------------------------------------------------
// Author   : "Ananta Miyoru Wijaya"
//----------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using MyCampusStory.BuildingSystem;

namespace MyCampusStory.Character
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class CharacterBaseState
    {
        public abstract void EnterState(CharacterStateManager CharacterStateManager);
        public abstract void UpdateState(CharacterStateManager CharacterStateManager);
        public abstract void ExitState(CharacterStateManager CharacterStateManager);
    }

    public class CharacterIdleState : CharacterBaseState
    {
        [SerializeField] private float _idleTime = 3f;
        private float _idleTimeRemaining = 0f;

        public override void EnterState(CharacterStateManager CharacterStateManager)
        {
            CharacterStateManager.CharacterAnimation.SetAnimCrossFade(CharacterStateManager.CharacterAnimation.Idle_anim, 0.1f);
            _idleTimeRemaining = 0f;
        }

        public override void UpdateState(CharacterStateManager CharacterStateManager)
        {
            _idleTimeRemaining += Time.deltaTime;
            if (_idleTimeRemaining > _idleTime)
            {
                CharacterStateManager.SwitchState(CharacterStateManager.WalkState);
            }
        }

        public override void ExitState(CharacterStateManager CharacterStateManager)
        {
            CharacterStateManager.CharacterAnimation.SetAnimBool(CharacterStateManager.CharacterAnimation.Idle_anim, false);
            _idleTimeRemaining = 0f;
        }
    }

    [RequireComponent(typeof(NavMeshAgent))]
    public class CharacterWalkState : CharacterBaseState
    {
        private NavMeshAgent _navMeshAgent;
        private int _objectToMoveIndex;

        public class ObjectToMove
        {
            public GameObject TargetToMove;
            public bool HasBeenMovedTo;
        } 

        private List<ObjectToMove> _objectsToMove = new List<ObjectToMove>();


        public override void EnterState(CharacterStateManager CharacterStateManager)
        {
            _navMeshAgent = CharacterStateManager.GetComponent<NavMeshAgent>();  

            if(_objectsToMove.Count == 0 || _objectsToMove == null)
            {
                GetObjectToMoveAt();
            }

            SelectObjectToMoveAt();

            // Set the NavMeshAgent destination
            if (_navMeshAgent != null)
            {
                _navMeshAgent.SetDestination(_objectsToMove[_objectToMoveIndex].TargetToMove.transform.position);
            }
        }

        public override void UpdateState(CharacterStateManager CharacterStateManager)
        {
            if(_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
            {
                CharacterStateManager.SwitchState(CharacterStateManager.IdleState);
            }
        }

        public override void ExitState(CharacterStateManager CharacterStateManager)
        {
            CharacterStateManager.CharacterAnimation.SetAnimBool(CharacterStateManager.CharacterAnimation.Walk_anim, false);
            _navMeshAgent = null;

            _objectsToMove[_objectToMoveIndex].HasBeenMovedTo = true;
        }

        private void GetObjectToMoveAt()
        {
            GameObject[] buildingObject = Building.GetAllBuildings().ToArray();
            foreach (GameObject obj in buildingObject)
            {
                _objectsToMove.Add(new ObjectToMove { TargetToMove = obj, HasBeenMovedTo = false });
            }
        }

        private void SelectObjectToMoveAt()
        {
            var objectToMove = _objectsToMove.Find(obj => !obj.HasBeenMovedTo);
            if (objectToMove != null)
            {
                _objectToMoveIndex = _objectsToMove.IndexOf(objectToMove);
            }
            else
            {
                foreach (var obj in _objectsToMove)
                {
                    obj.HasBeenMovedTo = false;
                }
                _objectToMoveIndex = -1;

                SelectObjectToMoveAt();
            }
        }
    }

    [RequireComponent(typeof(Collider))]
    public class CharacterInteractState : CharacterBaseState
    {
        private Collider _targetCollider;
        private Collider _characterCollider;

        public override void EnterState(CharacterStateManager CharacterStateManager)
        {
            _characterCollider = CharacterStateManager.GetComponent<Collider>();
        }

        public override void UpdateState(CharacterStateManager CharacterStateManager)
        {

        }

        public override void ExitState(CharacterStateManager CharacterStateManager)
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Interactable")
            {
                _targetCollider = other;
            }
        }
    }
}
