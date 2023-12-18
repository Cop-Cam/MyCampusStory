using UnityEngine;
using MyCampusStory.InputSystem;
using Cinemachine;
using System.Collections;

namespace MyCampusStory.CameraSystem
{
    public class CameraController : MonoBehaviour
    {
        private InputManager _inputManager;
        public CinemachineVirtualCamera virtualCamera;
        public float panSpeed = 2f;
        public float zoomSpeed = 0.5f;
        public float minZoom = 5f;
        public float maxZoom = 15f;

        private void Awake()
        {
            _inputManager = InputManager.Instance;
        }

        private void OnEnable()
        {
            _inputManager.OnScrollOrPinchEvent += HandleScrollOrPinch;
            _inputManager.OnDragOrSwipeEventStarted += HandleSwipeStarted;
            _inputManager.OnDragOrSwipeEventEnded += HandleSwipeEnded;
        }

        private void OnDisable()
        {
            _inputManager.OnScrollOrPinchEvent -= HandleScrollOrPinch;
            _inputManager.OnDragOrSwipeEventStarted -= HandleSwipeStarted;
            _inputManager.OnDragOrSwipeEventEnded -= HandleSwipeEnded;
        }

        private void HandleScrollOrPinch(Vector2 input)
        {
            float zoomAmount = input.y * zoomSpeed;
            // Implement zoom logic here
            // Example: Clamp the zoom between minZoom and maxZoom
            var zoom = virtualCamera.m_Lens.FieldOfView - zoomAmount;
            zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
            virtualCamera.m_Lens.FieldOfView = zoom;

            // For pan/scroll, you might use X and Y components of the input vector
            float panX = input.x * panSpeed;
            float panY = input.y * panSpeed;

            // Implement pan logic here
            // Example: Move the camera on its XZ plane
            var forward = virtualCamera.transform.forward.normalized;
            var right = virtualCamera.transform.right.normalized;
            virtualCamera.transform.position += (forward * panY + right * panX) * Time.deltaTime;
        }

        private void HandleSwipeStarted(Vector2 input)
        {
           
            Debug.Log("swiping");
            var x = virtualCamera.transform.position.x - input.x;
            var y = virtualCamera.transform.position.y - input.y;
            Vector3 newCameraReversePosition = new Vector3(x, y, virtualCamera.transform.position.z);

            virtualCamera.transform.position = newCameraReversePosition; 
            
        }

        private void HandleSwipeEnded(Vector2 input)
        {
            
        }


        private IEnumerator StartDragOrSwipe()
        {
            
            yield return null;
        }
    }
}
