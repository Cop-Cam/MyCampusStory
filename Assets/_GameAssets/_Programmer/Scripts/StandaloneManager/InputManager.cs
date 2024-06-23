using UnityEngine;
using UnityEngine.InputSystem;

namespace MyCampusStory.InputSystem
{
    public class InputManager : SingletonMonoBehaviour<InputManager>
    {
        private GameInputActionAsset _gameInputAction;

        #region Events
        public delegate void ClickOrPressEventHandler(Vector2 position);
        public event ClickOrPressEventHandler OnClickOrPressEvent;

        public delegate void ScrollOrPinchEventHandler(Vector2 position);
        public event ScrollOrPinchEventHandler OnScrollOrPinchEvent;

        public delegate void DragOrSwipeEventHandler (Vector2 position);
        public event DragOrSwipeEventHandler OnDragOrSwipeEventStarted;
        public event DragOrSwipeEventHandler OnDragOrSwipeEventEnded;
        
        #endregion


        protected override void Awake()
        {
            base.Awake();

            _gameInputAction = new GameInputActionAsset();
        }

        private void Start()
        {
            _gameInputAction.Gameplay.ClickOrPress.performed += ctx => ClickOrTap(ctx);
            _gameInputAction.Gameplay.ScrollOrPinch.performed += ctx => ScrollOrPinch(ctx);
            _gameInputAction.Gameplay.DragOrSwipe.started += ctx => DragOrSwipeStarted(ctx);
            _gameInputAction.Gameplay.DragOrSwipe.performed += ctx => DragOrSwipeEnded(ctx);
        }

        private void ClickOrTap(InputAction.CallbackContext context)
        {
            OnClickOrPressEvent?.Invoke(context.ReadValue<Vector2>());
        }

        private void ScrollOrPinch(InputAction.CallbackContext context)
        {
            OnScrollOrPinchEvent?.Invoke(context.ReadValue<Vector2>());
        }

        private void DragOrSwipeStarted(InputAction.CallbackContext context)
        {

            OnDragOrSwipeEventStarted?.Invoke(context.ReadValue<Vector2>());
        }
        private void DragOrSwipeEnded(InputAction.CallbackContext context)
        {

            OnDragOrSwipeEventEnded?.Invoke(context.ReadValue<Vector2>());
        }


        private void OnEnable()
        {
            _gameInputAction.Enable();
        }
        private void OnDisable()
        {
            _gameInputAction.Disable();
        }
    }
}

