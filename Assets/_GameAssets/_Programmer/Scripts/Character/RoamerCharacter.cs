//----------------------------------------------------------------------
// Author   : "Ananta Miyoru Wijaya"
//----------------------------------------------------------------------
using System.Collections.Generic;
using UnityEngine;

using MyCampusStory.BuildingSystem;


namespace MyCampusStory.Character
{
    /// <summary>
    /// 
    /// </summary>
    public class RoamerCharacter : Character
    {
        private void Start()
        {
            _currentState = IdleState;
            _currentState.EnterState(this);
            // SwitchState(IdleState);
        }

        private void Update()
        {
            _currentState.UpdateState(this);
        }
        
        #region IdleState
        [Header("Idle Settings")]
        [SerializeField] private float _idleMaxTime = 3f;
        private RoamerIdleState IdleState = new RoamerIdleState();
        public float GetIdleMaxTime() => _idleMaxTime;
        public string Idle_Anim = "IDLE";
        public class RoamerIdleState : CharacterState
        {
            private float _idleTimeRemaining;
            private RoamerCharacter _roamerCharacter;

            public override void EnterState(Character character)
            {
                _roamerCharacter = (RoamerCharacter)character;
                character.SetAnimBool(_roamerCharacter.Idle_Anim, true);
                _idleTimeRemaining = 0f;
            }

            public override void UpdateState(Character character)
            {
                _idleTimeRemaining += Time.deltaTime;

                if (_idleTimeRemaining > _roamerCharacter.GetIdleMaxTime())
                {
                    character.SwitchState(_roamerCharacter.WalkState);
                }
            }

            public override void ExitState(Character character)
            {
                character.SetAnimBool(_roamerCharacter.Idle_Anim, false);
                _roamerCharacter = null;
                _idleTimeRemaining = 0f;
            }
        }
        #endregion


        #region WalkState
        private RoamerWalkState WalkState = new RoamerWalkState();
        [HideInInspector] public Building CurrentBuildingToMove;
        public string Walk_Anim = "WALK";
        // [HideInInspector] public Transform CurrentReservedInteractPoint;
        public class RoamerWalkState : CharacterState
        {
            private Building _previousMovedToBuilding;
            private List<Building> _buildingToMoveList = new List<Building>();
            private RoamerCharacter _roamerCharacter;
            private float _randomDistance;

            public override void EnterState(Character character)
            {
                _roamerCharacter = (RoamerCharacter)character;

                // Get the list of buildings to move
                if(_buildingToMoveList.Count <= 0 || _buildingToMoveList == null)
                    _buildingToMoveList = Building.GetAllBuildings(character.GetSceneGroup());

                _randomDistance = Random.Range(1f, 1.5f);
                SelectObjectToMoveAt();
            }

            public override void UpdateState(Character character)
            {
                // if(character.GetNavMeshAgent().remainingDistance <= character.GetNavMeshAgent().stoppingDistance)
                // if((character.transform.position - _roamerCharacter.CurrentBuildingToMove.GetBuildingEntryPoint().position).magnitude <= 1f)
                // {
                //     _roamerCharacter.CurrentBuildingToMove.OnInteract(character.gameObject);
                //     character.transform.position = _roamerCharacter.CurrentReservedInteractPoint.position;

                //     _roamerCharacter.CurrentInteractedBuilding = _roamerCharacter.CurrentBuildingToMove;
                //     character.SwitchState(_roamerCharacter.InteractState);
                // }
                if((character.transform.position - _roamerCharacter.CurrentBuildingToMove.GetInteractPoint().position).magnitude <= _randomDistance)
                {
                    _roamerCharacter.CurrentBuildingToMove.OnInteract(character.gameObject);
                    // character.transform.position = _roamerCharacter.CurrentReservedInteractPoint.position;

                    _roamerCharacter.CurrentInteractedBuilding = _roamerCharacter.CurrentBuildingToMove;
                    character.SwitchState(_roamerCharacter.InteractState);
                }
            }

            public override void ExitState(Character character)
            {
                character.SetAnimBool(_roamerCharacter.Walk_Anim, false);
                _randomDistance = 0f;

                _roamerCharacter = null;
            }

            private void SelectObjectToMoveAt()
            {
                Building objectToMove = _buildingToMoveList.Find(obj => obj.GetInteractPoint() != null && obj.IsBuildingUnlocked() && obj != _previousMovedToBuilding);

                //If there is an object to move
                if (objectToMove != null)
                {
                    _roamerCharacter.MoveTo(objectToMove.GetInteractPoint());
                    _previousMovedToBuilding = objectToMove;

                    // //Reserve the interact points
                    // var randomIndex = Random.Range(0, objectToMove.GetInteractPoints().Count);
                    // objectToMove.ReserveInteractPoints(objectToMove.GetInteractPoints()[randomIndex]);
                    _roamerCharacter.CurrentBuildingToMove = objectToMove;
                    _roamerCharacter.SetAnimBool(_roamerCharacter.Walk_Anim, true);
                }
                //If there is no object to move
                else
                {
                    // SelectObjectToMoveAt();
                    _roamerCharacter.SwitchState(_roamerCharacter.IdleState);
                }   
            }
        }
        #endregion


        #region InteractState
        [Header("Interact Settings")]
        private RoamerInteractState InteractState = new RoamerInteractState();
        [SerializeField] private float _interactMaxTime = 3f;
        public float GetInteractMaxTime() => _interactMaxTime;
        public Building CurrentInteractedBuilding;
        public class RoamerInteractState : CharacterState
        {
            private float _interactTimeRemaining;
            private RoamerCharacter _roamerCharacter;

            public override void EnterState(Character character)
            {
                character.SetAnimBool(_roamerCharacter.CurrentInteractedBuilding.GetInteractAnimName(), true);
                _interactTimeRemaining = 0f;
                _roamerCharacter = (RoamerCharacter)character;
            }

            public override void UpdateState(Character character)
            {
                _interactTimeRemaining += Time.deltaTime;

                if (_interactTimeRemaining > _roamerCharacter.GetInteractMaxTime())
                {
                    character.SwitchState(_roamerCharacter.IdleState);
                    _roamerCharacter.CurrentBuildingToMove.OnStopInteract(character.gameObject);
                    // character.transform.position = _roamerCharacter.CurrentBuildingToMove.GetBuildingEntryPoint().position;
                    // _roamerCharacter.CurrentBuildingToMove.UnReserveInteractPoints(_roamerCharacter.CurrentReservedInteractPoint);
                }
            }

            public override void ExitState(Character character)
            {
                character.SetAnimBool(_roamerCharacter.CurrentInteractedBuilding.GetInteractAnimName(), false);
                _roamerCharacter = null;
                _interactTimeRemaining = 0f;
            }
        }
        #endregion
    }
}
