using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

using MyCampusStory.InputSystem;
using MyCampusStory.StandaloneManager;

namespace MyCampusStory.CameraSystem
{
    public class CameraController : MonoBehaviour
    {
        [Header("Camera & Input")]
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private LayerMask clickableLayerMask;

        [Header("Swipe Settings")]
        [SerializeField] private bool enableHorizontalSwipe = true;
        [SerializeField, Range(0.01f, 0.5f)] private float minSwipePercent = 0.02f; // e.g. 2% of screen
        [SerializeField] private float swipeSpeed = 10f;
        [SerializeField] private float inertiaDecay = 4f;

        [Header("Swipe Boundaries")]
        [SerializeField] private Transform leftBoundary;
        [SerializeField] private Transform rightBoundary;

        private Vector2 startTouchPos;
        private Vector2 lastTouchPos;
        private Vector2 swipeDelta;
        private bool isTouching, isSwiping;

        private float inertiaVelocity;
        private InputManager input;

        private List<IClickable> activeClickables = new();

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

        private void Update()
        {
            if (Mathf.Abs(inertiaVelocity) > 0.001f)
            {
                float moveAmount = inertiaVelocity * Time.deltaTime;
                MoveCamera(moveAmount);
                inertiaVelocity = Mathf.Lerp(inertiaVelocity, 0f, inertiaDecay * Time.deltaTime);
            }
        }

        private void HandleTouchStart(Vector2 position)
        {
            startTouchPos = lastTouchPos = position;
            swipeDelta = Vector2.zero;
            isTouching = true;
            isSwiping = false;
            inertiaVelocity = 0f;
        }

        private void HandleTouchEnd(Vector2 position)
        {
            isTouching = false;

            if (isSwiping)
            {
                Vector2 endDelta = position - lastTouchPos;
                inertiaVelocity = -endDelta.x / Screen.width * swipeSpeed;
                isSwiping = false;
            }
            else
            {
                HandleTap(position);
            }
        }

        private void HandleTouchMove(Vector2 position)
        {
            if (!isTouching || !enableHorizontalSwipe) return;

            Vector2 totalDelta = position - startTouchPos;
            float minSwipePixels = minSwipePercent * Screen.width;

            if (!isSwiping && totalDelta.magnitude < minSwipePixels)
                return;

            isSwiping = true;

            swipeDelta = position - lastTouchPos;
            lastTouchPos = position;

            float movement = -swipeDelta.x / Screen.width * swipeSpeed;
            MoveCamera(movement);
        }

        private void MoveCamera(float deltaX)
        {
            Vector3 targetPos = virtualCamera.transform.position + new Vector3(deltaX, 0f, 0f);
            virtualCamera.transform.position = ClampPosition(targetPos);
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
    }
}
