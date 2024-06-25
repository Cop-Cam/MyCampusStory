using UnityEngine;
using Cinemachine;

using MyCampusStory.InputSystem;
using MyCampusStory.StandaloneManager;

namespace MyCampusStory.CameraSystem
{
    public class FrontViewCameraController : MonoBehaviour
    {
        [SerializeField] private LayerMask _interactableLayerMask;
        [SerializeField] private CinemachineVirtualCamera _virtualCam;

        private InputManager _inputManager;
        private Camera _mainCamera;
        private Vector2 _lastTouchPressPosition;
        private bool _isSwiping = false;
        private Vector2 _deltaSwipePosition;
        [SerializeField] private float _swipeSpeed = 8f;
        [SerializeField] private float _swipeLerpingTime = .1f;
        [SerializeField] private bool enableSwipeHorizontally = true; //in case we will want to have swipe in other direction


        private void Awake()
        {
            _inputManager = GameManager.Instance.InputManager;
            _mainCamera = Camera.main;
        }

        private void Start()
        {
        }

        private void OnEnable()
        {
            Debug.Log("CameraController OnEnable");

            _inputManager.OnTouchPositionEvent += SwippingMethodHandler;

            _inputManager.OnTouchPressStartedEvent += TouchPressStartedMethodHandler;
            _inputManager.OnTouchPressPerformedEvent += TouchPressPerformedMethodHandler;
            _inputManager.OnTouchPressCanceledEvent += TouchPressCanceledMethodHandler;
        }

        private void OnDisable()
        {
            Debug.Log("CameraController OnDisable");

            _inputManager.OnTouchPositionEvent -= SwippingMethodHandler;

            _inputManager.OnTouchPressStartedEvent -= TouchPressStartedMethodHandler;
            _inputManager.OnTouchPressPerformedEvent -= TouchPressPerformedMethodHandler;
            _inputManager.OnTouchPressCanceledEvent -= TouchPressCanceledMethodHandler;
        }

        private void SwippingMethodHandler(Vector2 position)
        {
            if(!_isSwiping) return;

            Vector3 newCameraReversePosition = new Vector3(_virtualCam.transform.position.x, _virtualCam.transform.position.y, _virtualCam.transform.position.z);

            if(enableSwipeHorizontally)
            {
                _deltaSwipePosition = position - _lastTouchPressPosition;
                
                var x = _virtualCam.transform.position.x - _deltaSwipePosition.x * _swipeSpeed * Time.deltaTime;
                // var xClamped = Mathf.Clamp(x, -10f, 10f);

                newCameraReversePosition = new Vector3(x, _virtualCam.transform.position.y, _virtualCam.transform.position.z);
            }

            _virtualCam.transform.position = Vector3.Lerp(_virtualCam.transform.position, newCameraReversePosition, _swipeLerpingTime);

            _lastTouchPressPosition = position;
        }


        private void TouchPressStartedMethodHandler(Vector2 touchPosition)
        {
            Debug.Log("TouchEvent Started");
            _lastTouchPressPosition = touchPosition;
            _isSwiping = true;
        }

        private void TouchPressPerformedMethodHandler(Vector2 touchPosition)
        {
            Debug.Log("TouchEvent Performed");
        }

        private void TouchPressCanceledMethodHandler(Vector2 touchPosition)
        {
            Debug.Log("TouchEvent Canceled");
            _isSwiping = false;

            //Check if it wasn't a swipe
            if(_lastTouchPressPosition == touchPosition)
            {
                DrawRayAndTryToInteract(_lastTouchPressPosition);
            }
        }

        private void DrawRayAndTryToInteract(Vector2 drawPosition)
        {
            Ray ray = _mainCamera.ScreenPointToRay(drawPosition);
            
            // Raycast methods can't differentiate between maxDistance and layerMask args
            RaycastHit hit;
            if(!Physics.Raycast(ray, out hit, _mainCamera.farClipPlane, _interactableLayerMask))
            {
                return;
            }

            if(hit.collider == null)
            {
                Debug.LogWarning("The interactable object is detected but doesn't have a collider!");
                return;
            }

            if(hit.collider.gameObject.GetComponent<IInteractable>() == null)
            {
                Debug.LogWarning("The interactable object is detected but doesn't have an IInteractable script!");
                return;
            }

            // IInteractable interactedObj = hit.collider.gameObject.GetComponent<IInteractable>();
            // interactedObj.Interact();
            
            IInteractable[] interactableObjects = hit.collider.gameObject.GetComponents<IInteractable>();
            foreach (var interactableObject in interactableObjects)
            {
                interactableObject.Interact();
            }
        }
    }
}
