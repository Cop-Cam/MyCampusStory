//----------------------------------------------------------------------
// Author   : "Ananta Miyoru Wijaya"
//----------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using MyCampusStory.BuildingSystem;

namespace MyCampusStory.Character
{
    // public interface IState 
    // {
    //     void Init();
    //     void Enter();
    //     void HandleState();
    //     void HandleLogic();
    // }

    public abstract class CharacterState
    {
        public abstract void EnterState(Character character);
        public abstract void UpdateState(Character character);
        public abstract void ExitState(Character character);
    }
    
    // /// <summary>
    // /// 
    // /// </summary>
    // public abstract class CharacterBaseState
    // {
    //     public abstract void EnterState(CharacterStateManager characterStateManager);
    //     public abstract void UpdateState(CharacterStateManager characterStateManager);
    //     public abstract void ExitState(CharacterStateManager characterStateManager);
    // }

    // public class CharacterIdleState : CharacterBaseState
    // {
    //     private float _idleTime = 3f;
    //     private float _idleTimeRemaining = 0f;

    //     public override void EnterState(CharacterStateManager characterStateManager)
    //     {
    //         characterStateManager.CharacterAnimation.SetAnimCrossFade(characterStateManager.CharacterAnimation.Idle_anim, 0.1f);
    //         _idleTimeRemaining = 0f;
    //     }

    //     public override void UpdateState(CharacterStateManager characterStateManager)
    //     {
    //         _idleTimeRemaining += Time.deltaTime;
    //         if (_idleTimeRemaining > _idleTime)
    //         {
    //             characterStateManager.SwitchState(characterStateManager.WalkState);
    //         }
    //     }

    //     public override void ExitState(CharacterStateManager characterStateManager)
    //     {
    //         characterStateManager.CharacterAnimation.SetAnimBool(characterStateManager.CharacterAnimation.Idle_anim, false);
    //         _idleTimeRemaining = 0f;
    //     }
    // }

    // public class CharacterWalkState : CharacterBaseState
    // {
    //     private NavMeshAgent _navMeshAgent;
    //     private int _buildingToMoveIndex;

    //     public class BuildingToMove
    //     {
    //         public Building BuildingInstance;
    //         public bool HasBeenMovedTo;
    //     } 

    //     private List<BuildingToMove> _buildingToMoveList = new List<BuildingToMove>();


    //     public override void EnterState(CharacterStateManager characterStateManager)
    //     {
    //         _navMeshAgent = characterStateManager.GetComponent<NavMeshAgent>();  

    //         if(_buildingToMoveList.Count == 0 || _buildingToMoveList == null)
    //         {
    //             GetObjectToMoveAt(characterStateManager);
    //         }

    //         SelectObjectToMoveAt();

    //         // Set the NavMeshAgent destination
    //         if (_navMeshAgent != null)
    //         {
    //             _navMeshAgent.SetDestination(_buildingToMoveList[_buildingToMoveIndex].BuildingInstance.transform.position);
    //         }
    //     }

    //     public override void UpdateState(CharacterStateManager characterStateManager)
    //     {
    //         if(_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
    //         {
    //             characterStateManager.SwitchState(characterStateManager.IdleState);
    //         }
    //     }

    //     public override void ExitState(CharacterStateManager characterStateManager)
    //     {
    //         characterStateManager.CharacterAnimation.SetAnimBool(characterStateManager.CharacterAnimation.Walk_anim, false);
    //         _navMeshAgent = null;

    //         _buildingToMoveList[_buildingToMoveIndex].HasBeenMovedTo = true;
    //     }

    //     private void GetObjectToMoveAt(CharacterStateManager characterStateManager)
    //     {
    //         Building[] buildingObject = Building.GetAllBuildings(characterStateManager.CharacterSceneGroup).ToArray();
    //         foreach (var building in buildingObject)
    //         {
    //             _buildingToMoveList.Add(new BuildingToMove { BuildingInstance = building, HasBeenMovedTo = false });
    //         }
    //     }

    //     private void SelectObjectToMoveAt()
    //     {
    //         var objectToMove = _buildingToMoveList.Find(obj => !obj.HasBeenMovedTo && !obj.BuildingInstance.IsBuildingUsed());

    //         if (objectToMove != null)
    //         {
    //             _buildingToMoveIndex = _buildingToMoveList.IndexOf(objectToMove);
    //         }
    //         else
    //         {
    //             foreach (var obj in _buildingToMoveList)
    //             {
    //                 obj.HasBeenMovedTo = false;
    //             }
    //             _buildingToMoveIndex = -1;

    //             SelectObjectToMoveAt();
    //         }
    //     }
    // }

    // public class CharacterInteractState : CharacterBaseState
    // {
    //     private Collider _targetCollider;
    //     private Collider _characterCollider;

    //     public override void EnterState(CharacterStateManager characterStateManager)
    //     {
    //         _characterCollider = characterStateManager.GetComponent<Collider>();
    //     }

    //     public override void UpdateState(CharacterStateManager characterStateManager)
    //     {

    //     }

    //     public override void ExitState(CharacterStateManager characterStateManager)
    //     {

    //     }

    //     private void OnTriggerEnter(Collider other)
    //     {
    //         if (other.gameObject.tag == "Interactable")
    //         {
    //             _targetCollider = other;
    //         }
    //     }
    // }
}
