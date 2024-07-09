using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace MyCampusStory.InputSystem
{
    public class InputManager : MonoBehaviour
    {
        private GameInputActionAsset _gameInputAction;

        #region Events
        public delegate void TouchPressEventHandler(Vector2 position);
        public static event TouchPressEventHandler OnTouchPressPerformedEvent;
        public static event TouchPressEventHandler OnTouchPressCanceledEvent;

        public delegate void TouchPositionEventHandler(Vector2 position);
        public static event TouchPositionEventHandler OnTouchPositionEvent;
        #endregion

        private bool IsPointerOverUI;

        private void Awake()
        {
            _gameInputAction = new GameInputActionAsset();
        }

        private void Update()
        {
            if(EventSystem.current.IsPointerOverGameObject() && !IsPointerOverUI)
            {
                IsPointerOverUI = true;
            }
            else if(!EventSystem.current.IsPointerOverGameObject() && IsPointerOverUI)
            {
                IsPointerOverUI = false;
            }
        }

        private void OnEnable()
        {
            _gameInputAction.Enable();

            _gameInputAction.Gameplay.TouchPress.performed += ctx => OnTouchPressPerformed(ctx);
            _gameInputAction.Gameplay.TouchPress.canceled += ctx => OnTouchPressCanceled(ctx);

            _gameInputAction.Gameplay.TouchPosition.performed += ctx => OnTouchPosition(ctx);
        }
        
        private void OnDisable()
        {
            _gameInputAction.Disable();

            _gameInputAction.Gameplay.TouchPress.performed -= ctx => OnTouchPressPerformed(ctx);
            _gameInputAction.Gameplay.TouchPress.canceled -= ctx => OnTouchPressCanceled(ctx);

            _gameInputAction.Gameplay.TouchPosition.performed -= ctx => OnTouchPosition(ctx);
        }

        private void OnTouchPressPerformed(InputAction.CallbackContext context)
        {
            if(IsPointerOverUI) return;

            OnTouchPressPerformedEvent?.Invoke(_gameInputAction.Gameplay.TouchPosition.ReadValue<Vector2>());
        }
        
        private void OnTouchPressCanceled(InputAction.CallbackContext context) 
        {
            if(IsPointerOverUI) return;

            OnTouchPressCanceledEvent?.Invoke(_gameInputAction.Gameplay.TouchPosition.ReadValue<Vector2>());
        }

        private void OnTouchPosition(InputAction.CallbackContext context) 
        {
            if(IsPointerOverUI) return;

            OnTouchPositionEvent?.Invoke(context.ReadValue<Vector2>());
        }

        
    }
}

