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
    [SerializeField] private CinemachineVirtualCamera _virtualCam;
    [SerializeField] private LayerMask _clickableLayerMask;

    [Header("Swipe Settings")]
    [SerializeField] private bool enableSwipeHorizontally = true;
    [SerializeField] private float _minimumSwipeDistance = 5f;
    [SerializeField] private float _swipeSpeed = 5f;
    [SerializeField] private float _dampTime = 0.15f;

    [Header("Inertia Settings")]
    [SerializeField] private float inertiaDuration = 0.5f;
    [SerializeField] private float inertiaMultiplier = 0.5f;
    [SerializeField] private float maxInertiaSpeed = 2f;
    [SerializeField] private AnimationCurve inertiaCurve = new AnimationCurve(
        new Keyframe(0f, 1f),
        new Keyframe(0.2f, 0.6f),
        new Keyframe(0.5f, 0.3f),
        new Keyframe(1f, 0f)
    );

    [Header("Swipe Boundaries")]
    [SerializeField] private Transform leftBoundary;
    [SerializeField] private Transform rightBoundary;

    private Vector2 _startTouchPosition;
    private Vector2 _lastTouchPosition;
    private Vector2 _lastSwipeDelta;
    private bool _isPressing = false;
    private bool _isSwiping = false;
    private Vector3 _smoothVelocity = Vector3.zero;

    private Coroutine _moveCameraCoroutine;
    private List<IClickable> _lastClickedObjects = new();

    private void OnEnable()
    {
        var input = GameManager.Instance.InputManager;

        if (input != null)
        {
            input.OnTouchPressPerformedEvent += OnTouchPressStart;
            input.OnTouchPressCanceledEvent += OnTouchPressEnd;
            input.OnTouchPositionEvent += OnTouchMove;
        }
    }

    private void OnDisable()
    {
        var input = GameManager.Instance.InputManager;

        if (input != null)
        {
            input.OnTouchPressPerformedEvent -= OnTouchPressStart;
            input.OnTouchPressCanceledEvent -= OnTouchPressEnd;
            input.OnTouchPositionEvent -= OnTouchMove;
        }
    }

    private void OnTouchPressStart(Vector2 position)
    {
        _startTouchPosition = position;
        _lastTouchPosition = position;
        _isPressing = true;
        _isSwiping = false;

        if (_moveCameraCoroutine != null)
            StopCoroutine(_moveCameraCoroutine);
    }

    private void OnTouchPressEnd(Vector2 position)
    {
        _isPressing = false;

        if (_isSwiping)
        {
            _lastSwipeDelta = position - _lastTouchPosition;

            if (_lastSwipeDelta.sqrMagnitude > 0.001f)
            {
                if (_moveCameraCoroutine != null)
                    StopCoroutine(_moveCameraCoroutine);

                _moveCameraCoroutine = StartCoroutine(ApplyInertia());
            }

            _isSwiping = false;
        }
        else
        {
            HandleTapInteraction(position);
        }
    }

    private void OnTouchMove(Vector2 position)
    {
        if (!_isPressing) return;

        Vector2 swipeDelta = position - _startTouchPosition;

        if (!_isSwiping && swipeDelta.sqrMagnitude < _minimumSwipeDistance * _minimumSwipeDistance)
            return;

        _isSwiping = true;

        if (enableSwipeHorizontally)
        {
            float swipeAmount = -(position.x - _lastTouchPosition.x) / Screen.width * _swipeSpeed;
            Vector3 targetPos = _virtualCam.transform.position + new Vector3(swipeAmount, 0, 0);
            targetPos = ClampPosition(targetPos);
            _virtualCam.transform.position = targetPos;
        }

        _lastTouchPosition = position;
    }

    private IEnumerator ApplyInertia()
    {
        if (_lastSwipeDelta == Vector2.zero)
            yield break;

        Vector3 startVelocity = new Vector3(_lastSwipeDelta.x, 0f, 0f) / Screen.width;
        Vector3 clampedVelocity = Vector3.ClampMagnitude(startVelocity * inertiaMultiplier, maxInertiaSpeed);

        float elapsedTime = 0f;
        Vector3 startPosition = _virtualCam.transform.position;

        while (elapsedTime < inertiaDuration)
        {
            float t = elapsedTime / inertiaDuration;
            float strength = inertiaCurve.Evaluate(t);

            Vector3 offset = clampedVelocity * strength;
            Vector3 targetPos = ClampPosition(startPosition + offset);

            _virtualCam.transform.position = Vector3.SmoothDamp(
                _virtualCam.transform.position,
                targetPos,
                ref _smoothVelocity,
                _dampTime
            );

            elapsedTime += Time.deltaTime;
            yield return null;
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

    private void HandleTapInteraction(Vector2 screenPosition)
    {
        foreach (var obj in _lastClickedObjects)
            obj.OnStopClick();

        _lastClickedObjects.Clear();

        Camera cam = Camera.main;
        Ray ray = cam.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hit, cam.farClipPlane, _clickableLayerMask))
        {
            var clickables = hit.collider.GetComponents<IClickable>();
            foreach (var clickable in clickables)
            {
                clickable.OnClick();
                _lastClickedObjects.Add(clickable);
            }
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

    // public class FrontViewCameraController : MonoBehaviour
    // {
    //     [SerializeField] private LayerMask _clickableLayerMask;
    //     [SerializeField] private CinemachineVirtualCamera _virtualCam;

    //     private Vector2 _lastTouchPressPosition;
    //     private bool _isPressing = false;
    //     private bool _isSwiping = false;
    //     [SerializeField] private float _minimumDeltaPositionForSwipe = 1f;
    //     [SerializeField] private float _swipeSpeed = 3f;
    //     [SerializeField] private float _dampTime = .3f;
    //     [SerializeField] private float _maxCameraPosChange = 10f;
    //     private Vector3 _smoothVelocity = Vector3.zero;
    //     [SerializeField] private bool enableSwipeHorizontally = true; //in case we will want to have swipe in other direction

    //     [SerializeField] private List<IClickable> _lastclickedObjects;

    //     private Coroutine MoveCameraCoroutine;
        

    //     private void Awake()
    //     {
        
    //     }

    //     private void OnEnable()
    //     {
    //         if(GameManager.Instance.InputManager != null)
    //         {
    //             GameManager.Instance.InputManager.OnTouchPositionEvent += TouchPositionMethodHandler;

    //             GameManager.Instance.InputManager.OnTouchPressPerformedEvent += TouchPressPerformedMethodHandler;
    //             GameManager.Instance.InputManager.OnTouchPressCanceledEvent += TouchPressCanceledMethodHandler;
    //         }
    //     }

    //     private void OnDisable()
    //     {
    //         if(GameManager.Instance.InputManager != null)
    //         {
    //             GameManager.Instance.InputManager.OnTouchPositionEvent -= TouchPositionMethodHandler;

    //             GameManager.Instance.InputManager.OnTouchPressPerformedEvent -= TouchPressPerformedMethodHandler;
    //             GameManager.Instance.InputManager.OnTouchPressCanceledEvent -= TouchPressCanceledMethodHandler;
    //         }
    //     }

    //     private void TouchPositionMethodHandler(Vector2 position)
    //     {
    //         if(!_isPressing) 
    //             return;

    //         if((_lastTouchPressPosition - position).sqrMagnitude <= _minimumDeltaPositionForSwipe) 
    //             return;

    //         _isSwiping = true;

    //         Vector3 newCameraReversePosition = new Vector3(_virtualCam.transform.position.x, _virtualCam.transform.position.y, _virtualCam.transform.position.z);

    //         Vector2 deltaSwipePosition = position - _lastTouchPressPosition;

    //         if(enableSwipeHorizontally)
    //         {    
    //             var x = _virtualCam.transform.position.x - deltaSwipePosition.x * _swipeSpeed * Time.deltaTime;

    //             newCameraReversePosition = new Vector3(x, _virtualCam.transform.position.y, _virtualCam.transform.position.z);
    //         }

    //         if(MoveCameraCoroutine != null)
    //             StopCoroutine(MoveCameraCoroutine);

    //         MoveCameraCoroutine = StartCoroutine(MoveCamera(newCameraReversePosition));

    //         _lastTouchPressPosition = position;
    //     }

    //     private IEnumerator MoveCamera(Vector3 targetPos)
    //     {
    //         Vector3 currentPosition = _virtualCam.transform.position;
            
    //         // Clamp the target position to be within 10 units of the current position
    //         Vector3 clampedTargetPos = Vector3.ClampMagnitude(targetPos - currentPosition, _maxCameraPosChange) + currentPosition;

    //         while(_virtualCam.transform.position != clampedTargetPos)
    //         {
    //             _virtualCam.transform.position = Vector3.SmoothDamp(_virtualCam.transform.position, clampedTargetPos, ref _smoothVelocity, _dampTime);

    //             // yield return new WaitForEndOfFrame();
    //             yield return null;
    //         }
    //     }

    //     // private IEnumerator MoveCamera(Vector3 targetPos)
    //     // {
    //     //     while(_virtualCam.transform.position != targetPos)
    //     //     {
    //     //         _virtualCam.transform.position = Vector3.SmoothDamp(_virtualCam.transform.position, targetPos, ref _smoothVelocity, _dampTime);

    //     //         yield return new WaitForEndOfFrame();
    //     //     }
    //     // }

    //     private void TouchPressPerformedMethodHandler(Vector2 position)
    //     {
    //         _lastTouchPressPosition = position;

    //         _isPressing = true;
    //     }

    //     private void TouchPressCanceledMethodHandler(Vector2 position)
    //     {
    //         _isPressing = false;
            
    //         if(!_isSwiping)
    //         {
    //             if(_lastclickedObjects != null)
    //             {
    //                 foreach (var clickedObj in _lastclickedObjects)
    //                 {
    //                     clickedObj.OnStopClick();
    //                 }
    //                 _lastclickedObjects = null;
    //             }

    //             DrawRayAndTryToInteract(position);
    //         }
    //         else
    //         {
    //             _isSwiping = false;
    //         }
    //     }

    //     private void DrawRayAndTryToInteract(Vector2 drawPosition)
    //     {
    //         Camera mainCamera = LevelManager.Instance.CameraManager.MainCamera;
    //         Ray ray = mainCamera.ScreenPointToRay(drawPosition);
            
    //         // Raycast methods can't differentiate between maxDistance and layerMask args
    //         RaycastHit hit;
    //         if(!Physics.Raycast(ray, out hit, mainCamera.farClipPlane, _clickableLayerMask) ||
    //             hit.collider == null || hit.collider.gameObject.GetComponent<IClickable>() == null)
    //         {
    //             return;
    //         }
            
    //         _lastclickedObjects = new (hit.collider.gameObject.GetComponents<IClickable>());
    //         foreach (var clickedObj in _lastclickedObjects)
    //         {
    //             clickedObj.OnClick();
    //         }
    //     }
    // }
}
