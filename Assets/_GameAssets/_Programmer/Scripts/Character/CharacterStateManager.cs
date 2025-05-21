// //----------------------------------------------------------------------
// // Author   : "Ananta Miyoru Wijaya"
// //----------------------------------------------------------------------

// using System.Collections.Generic;
// using UnityEngine;


// namespace MyCampusStory.Character
// {
//     /// <summary>
//     /// 
//     /// </summary>
//     public class CharacterStateManager : MonoBehaviour
//     {
//         [field:Header("Fill or generate id and its scene id only on placed object in scene and not on prefab asset!")]
//         [field:SerializeField] public GroupData CharacterSceneGroup { get; private set; }
//         private CharacterBaseState _currentState;
//         public CharacterAnimation CharacterAnimation { get; private set; } 
        
//         public CharacterIdleState IdleState { get; private set; } = new CharacterIdleState();
//         public CharacterWalkState WalkState { get; private set; } = new CharacterWalkState();
//         public CharacterInteractState InteractState { get; private set; } = new CharacterInteractState();

//         private void Awake()
//         {
//             CharacterAnimation = GetComponent<CharacterAnimation>();
//         }

//         private void Start()
//         {
//             _currentState = IdleState;
//             _currentState.EnterState(this);
//         }

//         private void Update()
//         {
//             _currentState.UpdateState(this);
//         }

//         public void SwitchState(CharacterBaseState newState)
//         {
//             _currentState.ExitState(this);
//             _currentState = newState;
//             _currentState.EnterState(this);
//         }
//     }
// }
