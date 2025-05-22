using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

using MyCampusStory.InputSystem;
using MyCampusStory.StandaloneManager;

namespace MyCampusStory.CameraSystem
{
    public class FrontViewCameraController : MonoBehaviour
    {
        [Header("Camera & Input")]
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private LayerMask clickableLayerMask;

        [Header("Swipe Settings")]
        [SerializeField] private bool enableHorizontalSwipe = true;
        [SerializeField] private float minSwipeDistance = 2f; // More sensitive
        [SerializeField] private float swipeSpeed = 8f;
        [SerializeField] private float dampTime = 0.15f;

        [Header("Inertia Settings")]
        [SerializeField] private float inertiaDuration = 1f;
        [SerializeField] private float inertiaMultiplier = 1f;
        [SerializeField] private float maxInertiaSpeed = 5f;
        [SerializeField] private AnimationCurve inertiaCurve = new AnimationCurve(
            new Keyframe(0f, 1f),
            new Keyframe(0.2f, 0.8f),
            new Keyframe(0.5f, 0.5f),
            new Keyframe(1f, 0f)
        );

        [Header("Swipe Boundaries")]
        [SerializeField] private Transform leftBoundary;
        [SerializeField] private Transform rightBoundary;

        private Vector2 startTouchPos;
        private Vector2 lastTouchPos;
        private Vector2 lastSwipeDelta;

        private bool isTouching;
        private bool isSwiping;

        private Coroutine inertiaCoroutine;
        private Vector3 smoothVelocity = Vector3.zero;
        private List<IClickable> activeClickables = new();

        private InputManager input;

        private void OnEnable()
        {
            input = GameManager.Instance?.InputManager;
            if (input == null) return;

            input.OnTouchPressPerformedEvent += HandleTouchStart;
            input.OnTouchPressCanceledEvent += HandleTouchEnd;
            input.OnTouchPositionEvent += HandleTouchMove;
        }

        private void OnDisable()
        {
            if (input == null) return;

            input.OnTouchPressPerformedEvent -= HandleTouchStart;
            input.OnTouchPressCanceledEvent -= HandleTouchEnd;
            input.OnTouchPositionEvent -= HandleTouchMove;
        }

        private void HandleTouchStart(Vector2 position)
        {
            startTouchPos = lastTouchPos = position;
            isTouching = true;
            isSwiping = false;

            StopInertia();
        }

        private void HandleTouchEnd(Vector2 position)
        {
            isTouching = false;

            if (isSwiping)
            {
                lastSwipeDelta = position - lastTouchPos;

                if (lastSwipeDelta.sqrMagnitude > 0.001f)
                    inertiaCoroutine = StartCoroutine(ApplyInertia());

                isSwiping = false;
            }
            else
            {
                HandleTap(position);
            }
        }

        private void HandleTouchMove(Vector2 position)
        {
            if (!isTouching) return;

            Vector2 totalDelta = position - startTouchPos;

            if (!isSwiping && totalDelta.sqrMagnitude < minSwipeDistance * minSwipeDistance)
                return;

            isSwiping = true;

            if (!enableHorizontalSwipe) return;

            Vector2 swipeDelta = position - lastTouchPos;
            lastSwipeDelta = swipeDelta;

            float swipeAmount = -swipeDelta.x / Screen.dpi * swipeSpeed; // DPI-aware movement
            Vector3 targetPos = virtualCamera.transform.position + new Vector3(swipeAmount, 0f, 0f);
            virtualCamera.transform.position = ClampPosition(targetPos);

            lastTouchPos = position;
        }

        private IEnumerator ApplyInertia()
        {
            if (lastSwipeDelta == Vector2.zero)
                yield break;

            float velocity = -lastSwipeDelta.x / Screen.dpi * swipeSpeed * inertiaMultiplier;
            velocity = Mathf.Clamp(velocity, -maxInertiaSpeed, maxInertiaSpeed);

            float timeElapsed = 0f;

            while (timeElapsed < inertiaDuration && Mathf.Abs(velocity) > 0.001f)
            {
                float t = timeElapsed / inertiaDuration;
                float decay = inertiaCurve.Evaluate(t);
                float movement = velocity * decay * Time.deltaTime;

                Vector3 newPos = virtualCamera.transform.position + new Vector3(movement, 0f, 0f);
                virtualCamera.transform.position = ClampPosition(newPos);

                timeElapsed += Time.deltaTime;
                yield return null;
            }
        }

        private void HandleTap(Vector2 screenPos)
        {
            foreach (var obj in activeClickables)
                obj.OnStopClick();
            activeClickables.Clear();

            Camera cam = Camera.main;
            if (cam == null) return;

            Ray ray = cam.ScreenPointToRay(screenPos);
            if (Physics.Raycast(ray, out RaycastHit hit, cam.farClipPlane, clickableLayerMask))
            {
                foreach (var clickable in hit.collider.GetComponents<IClickable>())
                {
                    clickable.OnClick();
                    activeClickables.Add(clickable);
                }
            }
        }

        private Vector3 ClampPosition(Vector3 position)
        {
            if (leftBoundary != null && rightBoundary != null)
            {
                float minX = leftBoundary.position.x;
                float maxX = rightBoundary.position.x;
                position.x = Mathf.Clamp(position.x, minX, maxX);
            }
            return position;
        }

        private void StopInertia()
        {
            if (inertiaCoroutine != null)
                StopCoroutine(inertiaCoroutine);
        }
    }
}

    /*
    public class FrontViewCameraController : MonoBehaviour
    {
        [SerializeField] private LayerMask _interactableLayerMask;
        [SerializeField] private CinemachineVirtualCamera _virtualCam;

        private GameManager _gameManager;
        private LevelManager _levelManager;

        private Vector2 _lastTouchPressPosition;
        private bool _isPressing = false;
        private bool _isSwiping = false;
        [SerializeField] private float _minimumDeltaPositionForSwipe = 1f;
        [SerializeField] private float _swipeSpeed = 3f;
        [SerializeField] private float _dampTime = .3f;
        [SerializeField] private float _maxCameraPosChange = 10f;
        private Vector3 _smoothVelocity = Vector3.zero;
        [SerializeField] private bool enableSwipeHorizontally = true;

        [SerializeField] private List<IInteractable> _lastInteractedObjects;

        private Vector3 _targetCameraPosition;

        private void Awake()
        {
            _gameManager = GameManager.Instance;
            _levelManager = LevelManager.Instance;
        }

        private void OnEnable()
        {
            if(GameManager.Instance.InputManager != null)
            {
                GameManager.Instance.InputManager.OnTouchPositionEvent += TouchPositionMethodHandler;

                GameManager.Instance.InputManager.OnTouchPressPerformedEvent += TouchPressPerformedMethodHandler;
                GameManager.Instance.InputManager.OnTouchPressCanceledEvent += TouchPressCanceledMethodHandler;
            }
        }

        private void OnDisable()
        {
            if(GameManager.Instance.InputManager != null)
            {
                GameManager.Instance.InputManager.OnTouchPositionEvent -= TouchPositionMethodHandler;

                GameManager.Instance.InputManager.OnTouchPressPerformedEvent -= TouchPressPerformedMethodHandler;
                GameManager.Instance.InputManager.OnTouchPressCanceledEvent -= TouchPressCanceledMethodHandler;
            }
        }

        private void Update()
        {
            // Smoothly move the camera towards the target position
            _virtualCam.transform.position = Vector3.SmoothDamp(_virtualCam.transform.position, _targetCameraPosition, ref _smoothVelocity, _dampTime);
        }

        private void TouchPositionMethodHandler(Vector2 position)
        {
            if (!_isPressing) 
                return;

            if ((_lastTouchPressPosition - position).sqrMagnitude <= _minimumDeltaPositionForSwipe) 
                return;

            _isSwiping = true;

            Vector3 newCameraReversePosition = _virtualCam.transform.position;
            Vector2 deltaSwipePosition = position - _lastTouchPressPosition;

            if (enableSwipeHorizontally)
            {    
                var x = _virtualCam.transform.position.x - deltaSwipePosition.x * _swipeSpeed * Time.deltaTime;
                newCameraReversePosition = new Vector3(x, _virtualCam.transform.position.y, _virtualCam.transform.position.z);
            }

            // Clamp the target position to be within max distance
            _targetCameraPosition = Vector3.ClampMagnitude(newCameraReversePosition - _virtualCam.transform.position, _maxCameraPosChange) + _virtualCam.transform.position;

            _lastTouchPressPosition = position;
        }

        private void TouchPressPerformedMethodHandler(Vector2 position)
        {
            _lastTouchPressPosition = position;
            _isPressing = true;
        }

        private void TouchPressCanceledMethodHandler(Vector2 position)
        {
            _isPressing = false;
            
            if (!_isSwiping)
            {
                if (_lastInteractedObjects != null)
                {
                    foreach (var interactableObject in _lastInteractedObjects)
                    {
                        interactableObject.OnStopInteract();
                    }
                    _lastInteractedObjects = null;
                }

                DrawRayAndTryToInteract(position);
            }
            else
            {
                _isSwiping = false;
            }
        }

        private void DrawRayAndTryToInteract(Vector2 drawPosition)
        {
            Camera mainCamera = _levelManager.CameraManager.MainCamera;
            Ray ray = mainCamera.ScreenPointToRay(drawPosition);

            RaycastHit hit;
            if (!Physics.Raycast(ray, out hit, mainCamera.farClipPlane, _interactableLayerMask) ||
                hit.collider == null || hit.collider.gameObject.GetComponent<IInteractable>() == null)
            {
                return;
            }

            _lastInteractedObjects = new List<IInteractable>(hit.collider.gameObject.GetComponents<IInteractable>());
            foreach (var interactableObject in _lastInteractedObjects)
            {
                interactableObject.OnInteract();
            }
        }
    }
*/

//     public class FrontViewCameraController : MonoBehaviour
//     {
//         [SerializeField] private LayerMask _clickableLayerMask;
//         [SerializeField] private CinemachineVirtualCamera _virtualCam;

//         private Vector2 _lastTouchPressPosition;
//         private bool _isPressing = false;
//         private bool _isSwiping = false;
//         [SerializeField] private float _minimumDeltaPositionForSwipe = 1f;
//         [SerializeField] private float _swipeSpeed = 3f;
//         [SerializeField] private float _dampTime = .3f;
//         [SerializeField] private float _maxCameraPosChange = 10f;
//         private Vector3 _smoothVelocity = Vector3.zero;
//         [SerializeField] private bool enableSwipeHorizontally = true; //in case we will want to have swipe in other direction

//         [SerializeField] private List<IClickable> _lastclickedObjects;

//         private Coroutine MoveCameraCoroutine;
        

//         private void Awake()
//         {
        
//         }

//         private void OnEnable()
//         {
//             if(GameManager.Instance.InputManager != null)
//             {
//                 GameManager.Instance.InputManager.OnTouchPositionEvent += TouchPositionMethodHandler;

//                 GameManager.Instance.InputManager.OnTouchPressPerformedEvent += TouchPressPerformedMethodHandler;
//                 GameManager.Instance.InputManager.OnTouchPressCanceledEvent += TouchPressCanceledMethodHandler;
//             }
//         }

//         private void OnDisable()
//         {
//             if(GameManager.Instance.InputManager != null)
//             {
//                 GameManager.Instance.InputManager.OnTouchPositionEvent -= TouchPositionMethodHandler;

//                 GameManager.Instance.InputManager.OnTouchPressPerformedEvent -= TouchPressPerformedMethodHandler;
//                 GameManager.Instance.InputManager.OnTouchPressCanceledEvent -= TouchPressCanceledMethodHandler;
//             }
//         }

//         private void TouchPositionMethodHandler(Vector2 position)
//         {
//             if(!_isPressing) 
//                 return;

//             if((_lastTouchPressPosition - position).sqrMagnitude <= _minimumDeltaPositionForSwipe) 
//                 return;

//             _isSwiping = true;

//             Vector3 newCameraReversePosition = new Vector3(_virtualCam.transform.position.x, _virtualCam.transform.position.y, _virtualCam.transform.position.z);

//             Vector2 deltaSwipePosition = position - _lastTouchPressPosition;

//             if(enableSwipeHorizontally)
//             {    
//                 var x = _virtualCam.transform.position.x - deltaSwipePosition.x * _swipeSpeed * Time.deltaTime;

//                 newCameraReversePosition = new Vector3(x, _virtualCam.transform.position.y, _virtualCam.transform.position.z);
//             }

//             if(MoveCameraCoroutine != null)
//                 StopCoroutine(MoveCameraCoroutine);

//             MoveCameraCoroutine = StartCoroutine(MoveCamera(newCameraReversePosition));

//             _lastTouchPressPosition = position;
//         }

//         private IEnumerator MoveCamera(Vector3 targetPos)
//         {
//             Vector3 currentPosition = _virtualCam.transform.position;
            
//             // Clamp the target position to be within 10 units of the current position
//             Vector3 clampedTargetPos = Vector3.ClampMagnitude(targetPos - currentPosition, _maxCameraPosChange) + currentPosition;

//             while(_virtualCam.transform.position != clampedTargetPos)
//             {
//                 _virtualCam.transform.position = Vector3.SmoothDamp(_virtualCam.transform.position, clampedTargetPos, ref _smoothVelocity, _dampTime);

//                 // yield return new WaitForEndOfFrame();
//                 yield return null;
//             }
//         }

//         // private IEnumerator MoveCamera(Vector3 targetPos)
//         // {
//         //     while(_virtualCam.transform.position != targetPos)
//         //     {
//         //         _virtualCam.transform.position = Vector3.SmoothDamp(_virtualCam.transform.position, targetPos, ref _smoothVelocity, _dampTime);

//         //         yield return new WaitForEndOfFrame();
//         //     }
//         // }

//         private void TouchPressPerformedMethodHandler(Vector2 position)
//         {
//             _lastTouchPressPosition = position;

//             _isPressing = true;
//         }

//         private void TouchPressCanceledMethodHandler(Vector2 position)
//         {
//             _isPressing = false;
            
//             if(!_isSwiping)
//             {
//                 if(_lastclickedObjects != null)
//                 {
//                     foreach (var clickedObj in _lastclickedObjects)
//                     {
//                         clickedObj.OnStopClick();
//                     }
//                     _lastclickedObjects = null;
//                 }

//                 DrawRayAndTryToInteract(position);
//             }
//             else
//             {
//                 _isSwiping = false;
//             }
//         }

//         private void DrawRayAndTryToInteract(Vector2 drawPosition)
//         {
//             Camera mainCamera = LevelManager.Instance.CameraManager.MainCamera;
//             Ray ray = mainCamera.ScreenPointToRay(drawPosition);
            
//             // Raycast methods can't differentiate between maxDistance and layerMask args
//             RaycastHit hit;
//             if(!Physics.Raycast(ray, out hit, mainCamera.farClipPlane, _clickableLayerMask) ||
//                 hit.collider == null || hit.collider.gameObject.GetComponent<IClickable>() == null)
//             {
//                 return;
//             }
            
//             _lastclickedObjects = new (hit.collider.gameObject.GetComponents<IClickable>());
//             foreach (var clickedObj in _lastclickedObjects)
//             {
//                 clickedObj.OnClick();
//             }
//         }
//     }

// }
