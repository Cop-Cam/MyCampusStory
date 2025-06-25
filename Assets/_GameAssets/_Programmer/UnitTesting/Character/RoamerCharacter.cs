using UnityEngine;

namespace MyCampusStory.StatePatternTest
{
    public class RoamerCharacter : Character
    {
        private void Start()
        {
            SwitchState(IdleState);
        }

        private void Update()
        {
            _currentState?.UpdateState(this);
        }

        [SerializeField] private float _idleMaxTime = 3f;
        [SerializeField] private float _interactMaxTime = 2f;

        public float GetIdleMaxTime() => _idleMaxTime;
        public float GetInteractMaxTime() => _interactMaxTime;

        private readonly RoamerIdleState IdleState = new RoamerIdleState();
        private readonly RoamerWalkState WalkState = new RoamerWalkState();
        private readonly RoamerInteractState InteractState = new RoamerInteractState();

        public Building CurrentBuildingToMove;

        #region Roamer States

        public class RoamerIdleState : CharacterState
        {
            private float _timer;
            private RoamerCharacter _roamer;

            public override void EnterState(Character character)
            {
                _roamer = (RoamerCharacter)character;
                _timer = 0f;
            }

            public override void UpdateState(Character character)
            {
                _timer += Time.deltaTime;
                if (_timer >= _roamer.GetIdleMaxTime())
                {
                    character.SwitchState(_roamer.WalkState);
                }
            }

            public override void ExitState(Character character)
            {
                _timer = 0f;
            }
        }

        public class RoamerWalkState : CharacterState
        {
            private RoamerCharacter _roamer;
            private float _timer;
            private const float SimulatedWalkTime = 1.5f;

            public override void EnterState(Character character)
            {
                _roamer = (RoamerCharacter)character;
                _timer = 0f;

                var buildings = Building.GetAllBuildings(null);
                if (buildings.Count > 0)
                {
                    _roamer.CurrentBuildingToMove = buildings[0]; // Always pick the first for test simplicity
                }
            }

            public override void UpdateState(Character character)
            {
                _timer += Time.deltaTime;
                if (_timer >= SimulatedWalkTime)
                {
                    _roamer.CurrentBuildingToMove?.OnInteract(character.gameObject);
                    character.SwitchState(_roamer.InteractState);
                }
            }

            public override void ExitState(Character character)
            {
                _timer = 0f;
            }
        }

        public class RoamerInteractState : CharacterState
        {
            private RoamerCharacter _roamer;
            private float _timer;

            public override void EnterState(Character character)
            {
                _roamer = (RoamerCharacter)character;
                _timer = 0f;
            }

            public override void UpdateState(Character character)
            {
                _timer += Time.deltaTime;
                if (_timer >= _roamer.GetInteractMaxTime())
                {
                    _roamer.CurrentBuildingToMove?.OnStopInteract(character.gameObject);
                    character.SwitchState(_roamer.IdleState);
                }
            }

            public override void ExitState(Character character)
            {
                _timer = 0f;
            }
        }

        #endregion
    }
}
