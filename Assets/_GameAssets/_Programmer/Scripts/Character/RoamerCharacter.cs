//----------------------------------------------------------------------
// Author   : "Ananta Miyoru Wijaya"
//----------------------------------------------------------------------
using System.Collections.Generic;
using System.Linq;
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
        public string Idle_Anim_StateName = "IDLE";
        public class RoamerIdleState : CharacterState
        {
            private float _idleTimeRemaining;
            private RoamerCharacter _roamerCharacter;

            public override void EnterState(Character character)
            {
                _roamerCharacter = (RoamerCharacter)character;
                // character.SetAnimBool(_roamerCharacter.Idle_Anim_Param, true);
                // _roamerCharacter.ChangeAnimation(_roamerCharacter.Idle_Anim_Param);
                character.SetAnimCrossFade(_roamerCharacter.Idle_Anim_StateName, 0.1f);
                

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
                // character.SetAnimBool(_roamerCharacter.Idle_Anim_Param, false);
                _roamerCharacter = null;
                _idleTimeRemaining = 0f;
            }
        }
        #endregion


        #region WalkState
        private RoamerWalkState WalkState = new RoamerWalkState();
        [HideInInspector] public Building CurrentBuildingToMove;
        public string Walk_Anim_StateName = "WALK";
        // [HideInInspector] public Transform CurrentReservedInteractPoint;
        public class RoamerWalkState : CharacterState
        {
            private Building _previousMovedToBuilding;
            private List<Building> _buildingToMoveList = new List<Building>();
            private RoamerCharacter _roamerCharacter;

            public override void EnterState(Character character)
            {
                _roamerCharacter = (RoamerCharacter)character;

                // Get the list of buildings to move
                if(_buildingToMoveList.Count <= 0 || _buildingToMoveList == null)
                    _buildingToMoveList = Building.GetAllBuildings(character.GetSceneGroup());

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

                Vector3 velocity = character.GetNavMeshAgent().velocity;

                if (velocity.x > 0.1f)
                {
                    // Face right
                    character.transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                else if (velocity.x < -0.1f)
                {
                    // Face left
                    character.transform.rotation = Quaternion.Euler(0, 0, 0);
                }


                // if((character.transform.position - _roamerCharacter.CurrentBuildingToMove.GetInteractPoint().position).magnitude <= 1f)
                // {
                //     _roamerCharacter.CurrentBuildingToMove.OnInteract(character.gameObject);
                //     // character.transform.position = _roamerCharacter.CurrentReservedInteractPoint.position;

                //     _roamerCharacter.CurrentInteractedBuilding = _roamerCharacter.CurrentBuildingToMove;
                //     character.SwitchState(_roamerCharacter.InteractState);
                // }

                if (!_roamerCharacter._navMeshAgent.pathPending && _roamerCharacter._navMeshAgent.remainingDistance <= _roamerCharacter._navMeshAgent.stoppingDistance)
                {
                    if (!_roamerCharacter._navMeshAgent.hasPath || _roamerCharacter._navMeshAgent.velocity.sqrMagnitude == 0f)
                    {
                        _roamerCharacter.CurrentBuildingToMove.OnInteract(character.gameObject);
                        // character.transform.position = _roamerCharacter.CurrentReservedInteractPoint.position;

                        character.SwitchState(_roamerCharacter.InteractState);
                        
                    }
                }
            }

            

            public override void ExitState(Character character)
            {
                // character.SetAnimBool(_roamerCharacter.Walk_Anim_Param, false);
                // character.ChangeAnimation(_roamerCharacter.Idle_Anim_Param);

                _roamerCharacter = null;
            }

            private void SelectObjectToMoveAt()
            {
                List<Building> validBuildings = _buildingToMoveList
                    .Where(obj => obj.GetInteractPoint() != null && obj.IsBuildingUnlocked() && obj != _previousMovedToBuilding)
                    .OrderBy(b => Random.value)
                    .ToList();

                if (validBuildings.Count > 0)
                {
                    Building objectToMove = validBuildings[0];

                    _roamerCharacter.MoveTo(objectToMove.GetInteractPoint());
                    _previousMovedToBuilding = objectToMove;
                    _roamerCharacter.CurrentBuildingToMove = objectToMove;
                    _roamerCharacter.SetAnimCrossFade(_roamerCharacter.Walk_Anim_StateName, 0.1f);
                }
                else
                {
                    _roamerCharacter.SwitchState(_roamerCharacter.IdleState);
                }

                // Building objectToMove = _buildingToMoveList.Find(obj => obj.GetInteractPoint() != null && obj.IsBuildingUnlocked() && obj != _previousMovedToBuilding);

                // //If there is an object to move
                // if (objectToMove != null)
                // {
                //     _roamerCharacter.MoveTo(objectToMove.GetInteractPoint());
                //     _previousMovedToBuilding = objectToMove;

                //     // //Reserve the interact points
                //     // var randomIndex = Random.Range(0, objectToMove.GetInteractPoints().Count);
                //     // objectToMove.ReserveInteractPoints(objectToMove.GetInteractPoints()[randomIndex]);
                //     _roamerCharacter.CurrentBuildingToMove = objectToMove;
                //     // _roamerCharacter.SetAnimBool(_roamerCharacter.Walk_Anim_Param, true);
                //     // _roamerCharacter.ChangeAnimation(_roamerCharacter.Walk_Anim_Param);
                //     _roamerCharacter.SetAnimCrossFade(_roamerCharacter.Walk_Anim_Param, 0.1f);

                // }
                // //If there is no object to move
                // else
                // {
                //     // SelectObjectToMoveAt();
                //     _roamerCharacter.SwitchState(_roamerCharacter.IdleState);
                // }
            }
        }
        #endregion


        #region InteractState
        [Header("Interact Settings")]
        private RoamerInteractState InteractState = new RoamerInteractState();
        [SerializeField] private float _interactMaxTime = 3f;
        public float GetInteractMaxTime() => _interactMaxTime;
        public string Interact_Anim_StateName = "INTERACT";
        public string StopInteract_Anim_StateName = "STOPINTERACT";
        public class RoamerInteractState : CharacterState
        {
            private float _interactTimeRemaining;
            private RoamerCharacter _roamerCharacter;

            private bool _isExiting;
            private float _exitTimer;
            private float _stopInteractAnimDuration;


            public override void EnterState(Character character)
            {
                _roamerCharacter = (RoamerCharacter)character;

                _stopInteractAnimDuration = _roamerCharacter.GetAnimDuration(_roamerCharacter.StopInteract_Anim_StateName);
                // character.SetAnimBool(_roamerCharacter.Interact_Anim_Param, true);
                character.SetAnimCrossFade(_roamerCharacter.Interact_Anim_StateName, 0.1f);
                // character.ChangeAnimation(_roamerCharacter.Interact_Anim_Param);
                _interactTimeRemaining = 0f;
                _exitTimer = 0f;
                _isExiting = false;
            }

            public override void UpdateState(Character character)
            {
                if (_isExiting)
                {
                    _exitTimer -= Time.deltaTime;
                    if (_exitTimer <= 0f)
                    {
                        _roamerCharacter.SwitchState(_roamerCharacter.IdleState); // or WalkState, etc.
                    }
                    return;
                }

                _interactTimeRemaining += Time.deltaTime;

                if (_interactTimeRemaining > _roamerCharacter.GetInteractMaxTime())
                {
                    _roamerCharacter.CurrentBuildingToMove.OnStopInteract(character.gameObject);
                    character.SetAnimCrossFade(_roamerCharacter.StopInteract_Anim_StateName, 0.1f);

                    _isExiting = true;
                    _exitTimer = _stopInteractAnimDuration;
                }
            }

            public override void ExitState(Character character)
            {
                _interactTimeRemaining = 0f;
                _roamerCharacter = null;
            }
        }
        #endregion
    }
}
