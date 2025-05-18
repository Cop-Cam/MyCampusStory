using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace MyCampusStory.InputSystem
{
    public class InputManager : MonoBehaviour
    {
        private GameInputActionAsset _gameInputAction;

        #region Events
        public delegate void InputPositionEventHandler(Vector2 position);
        public event InputPositionEventHandler OnTouchPressPerformedEvent;
        public event InputPositionEventHandler OnTouchPressCanceledEvent;
        public event InputPositionEventHandler OnTouchPositionEvent;
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

            _gameInputAction.Gameplay.TouchPress.performed += ctx => OnTouchPress(ctx);
            _gameInputAction.Gameplay.TouchPress.canceled += ctx => OnTouchPress(ctx);
            
            _gameInputAction.Gameplay.TouchPosition.performed += ctx => OnTouchPosition(ctx);
        }
        
        private void OnDisable()
        {
            _gameInputAction.Disable();

            _gameInputAction.Gameplay.TouchPress.performed -= ctx => OnTouchPress(ctx);
            _gameInputAction.Gameplay.TouchPress.canceled -= ctx => OnTouchPress(ctx);

            _gameInputAction.Gameplay.TouchPosition.performed -= ctx => OnTouchPosition(ctx);
        }

        private void OnTouchPress(InputAction.CallbackContext context)
        {
            if(IsPointerOverUI) return;

            if(context.performed)
            {
                OnTouchPressPerformedEvent?.Invoke(_gameInputAction.Gameplay.TouchPosition.ReadValue<Vector2>());
            }

            if(context.canceled)
            {
                OnTouchPressCanceledEvent?.Invoke(_gameInputAction.Gameplay.TouchPosition.ReadValue<Vector2>());
            }
        }

        private void OnTouchPosition(InputAction.CallbackContext context) 
        {
            if(IsPointerOverUI) return;

            OnTouchPositionEvent?.Invoke(context.ReadValue<Vector2>());
        }

        
    }
}

