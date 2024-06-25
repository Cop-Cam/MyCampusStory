using UnityEngine;
using UnityEngine.InputSystem;

namespace MyCampusStory.InputSystem
{
    public class InputManager : MonoBehaviour
    {
        private GameInputActionAsset _gameInputAction;

        #region Events
        public delegate void TouchPressEventHandler(Vector2 position);
        public event TouchPressEventHandler OnTouchPressStartedEvent;
        public event TouchPressEventHandler OnTouchPressPerformedEvent;
        public event TouchPressEventHandler OnTouchPressCanceledEvent;

        public delegate void TouchPositionEventHandler(Vector2 position);
        public event TouchPositionEventHandler OnTouchPositionEvent;
        #endregion


        private void Awake()
        {
            _gameInputAction = new GameInputActionAsset();

            _gameInputAction.Gameplay.TouchPress.started += ctx => OnTouchPressStarted(ctx);
            _gameInputAction.Gameplay.TouchPress.performed += ctx => OnTouchPressPerformed(ctx);
            _gameInputAction.Gameplay.TouchPress.canceled += ctx => OnTouchPressCanceled(ctx);

            _gameInputAction.Gameplay.TouchPosition.performed += ctx => OnTouchPosition(ctx);
        }

        private void OnEnable()
        {
            _gameInputAction.Enable();
        }
        private void OnDisable()
        {
            _gameInputAction.Disable();
        }

        private void OnTouchPressStarted(InputAction.CallbackContext context)
        {
            OnTouchPressStartedEvent?.Invoke(_gameInputAction.Gameplay.TouchPosition.ReadValue<Vector2>());
        }

        private void OnTouchPressPerformed(InputAction.CallbackContext context)
        {
            OnTouchPressPerformedEvent?.Invoke(_gameInputAction.Gameplay.TouchPosition.ReadValue<Vector2>());
        }
        
        private void OnTouchPressCanceled(InputAction.CallbackContext context) 
        {
            OnTouchPressCanceledEvent?.Invoke(_gameInputAction.Gameplay.TouchPosition.ReadValue<Vector2>());
        }

        private void OnTouchPosition(InputAction.CallbackContext context) 
        {
            OnTouchPositionEvent?.Invoke(context.ReadValue<Vector2>());
        }

        
    }
}

