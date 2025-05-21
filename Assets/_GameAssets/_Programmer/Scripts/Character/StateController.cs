// //----------------------------------------------------------------------
// // Author   : "Ananta Miyoru Wijaya"
// //----------------------------------------------------------------------

// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.PlayerLoop;

// namespace MyCampusStory.Character
// {
//     public enum ActionState
//     {
//         IDLE,
//         WALK,
//         INTERACT   
//     }

//     public abstract class StateController : MonoBehaviour
//     {
        
//     }

//     /// <summary>
//     /// 
//     /// </summary>
//     public class CharacterStateController : StateController
//     {
//         public ActionState _actionState;
//         public IState _currentState;
        
//         public CharacterIdleState IdleState { get; private set; } = new();
//         public CharacterWalkState WalkState { get; private set; } = new();
//         public CharacterInteractState InteractState { get; private set; } = new();

//         public void Init(ActionState actionState, IState currentState)
//         {

//         }

//         public void SwitchState(IState newState)
//         {
//             _currentState = newState;
//             _currentState.Enter();
//         }
//     }
// }

