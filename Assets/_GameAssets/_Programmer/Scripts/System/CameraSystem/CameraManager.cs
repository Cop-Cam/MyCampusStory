//----------------------------------------------------------------------
// Author   : "Ananta Miyoru Wijaya"
//----------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace MyCampusStory.CameraSystem
{
    /// <summary>
    /// Class summary
    /// </summary>
    public class CameraManager : MonoBehaviour
    {
        private HashSet<CinemachineVirtualCamera> _virtualCameras = new HashSet<CinemachineVirtualCamera>();
        
        private CinemachineBrain cinemachineBrain;
        
        public Camera MainCamera { get; private set; }
        public ICinemachineCamera ActiveVirtualCamera { get; private set; } = null;

        private void Awake()
        {
            MainCamera = Camera.main;

            cinemachineBrain = MainCamera.transform.GetComponent<CinemachineBrain>();

            // Get all virtual cameras
            CinemachineVirtualCamera[] cinemachineVirtualCameras = FindObjectsOfType<CinemachineVirtualCamera>();

            foreach (CinemachineVirtualCamera vCam in cinemachineVirtualCameras)
            {
                _virtualCameras.Add(vCam);
            }

            // Get the currently active virtual camera
            ActiveVirtualCamera = cinemachineBrain.ActiveVirtualCamera;
        }

        public void AddVirtualCamera(CinemachineVirtualCamera virtualCamera)
        {
            _virtualCameras.Add(virtualCamera);
        }

        public void SwithCameras(CinemachineVirtualCamera virtualCamera)
        {
            virtualCamera.Priority = 20;

            foreach (CinemachineVirtualCamera vCam in _virtualCameras)
            {
                vCam.Priority = 5;
            }

            ActiveVirtualCamera = cinemachineBrain.ActiveVirtualCamera;
        }
    }
}
