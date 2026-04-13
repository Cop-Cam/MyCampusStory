using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyCampusStory
{
    [System.Serializable]
    public class TutorialStep
    {
        [Header("Camera")]
        public Transform cameraTarget;
        public Vector3 cameraOffset = new Vector3(0f, 8f, -10f);
        public bool lookAtTarget = true;

        [Header("Guide")]
        [TextArea(2, 5)]
        public string guideText;
        public float stepDuration = 5f;
        public bool waitForInput = true;

        [Header("Highlight")]
        public Renderer highlightRenderer;
        public Color highlightColor = Color.yellow;
    }

    public class Tutorial : MonoBehaviour
    {
        [Tooltip("Camera used for the tutorial. If empty, uses Camera.main.")]
        public Camera tutorialCamera;

        [Tooltip("UI Text that shows the tutorial instructions.")]
        public Text tutorialText;

        [Tooltip("Optional container to show/hide while the tutorial is active.")]
        public GameObject tutorialPanel;

        [Tooltip("Steps executed in order during the tutorial.")]
        public List<TutorialStep> steps = new List<TutorialStep>();

        [Tooltip("Speed at which the camera moves between step targets.")]
        public float cameraMoveSpeed = 3f;

        [Tooltip("Key to press to advance the tutorial when waitForInput is enabled.")]
        public KeyCode nextStepKey = KeyCode.Space;

        private readonly Dictionary<Renderer, Color[]> originalColors = new Dictionary<Renderer, Color[]>();
        private readonly Dictionary<Renderer, Color[]> originalEmissionColors = new Dictionary<Renderer, Color[]>();
        private readonly HashSet<Renderer> highlightedRenderers = new HashSet<Renderer>();

        private void Start()
        {
            if (tutorialCamera == null)
            {
                tutorialCamera = Camera.main;
            }

            if (tutorialPanel != null)
            {
                tutorialPanel.SetActive(true);
            }

            StartCoroutine(RunTutorial());
        }

        private IEnumerator RunTutorial()
        {
            if (tutorialCamera == null || tutorialText == null)
            {
                Debug.LogWarning("Tutorial requires a camera and a tutorialText UI Text reference.");
                yield break;
            }

            foreach (var step in steps)
            {
                SetHighlight(step.highlightRenderer, step.highlightColor);
                SetText(step.guideText);

                if (step.cameraTarget != null)
                {
                    yield return StartCoroutine(MoveCameraToTarget(step.cameraTarget, step.cameraOffset, step.lookAtTarget));
                }

                if (step.waitForInput)
                {
                    yield return StartCoroutine(WaitForInput());
                }
                else
                {
                    yield return new WaitForSeconds(step.stepDuration);
                }

                ClearHighlight(step.highlightRenderer);
            }

            EndTutorial();
        }

        private IEnumerator MoveCameraToTarget(Transform target, Vector3 offset, bool lookAt)
        {
            Vector3 desiredPosition = target.position + offset;
            Quaternion desiredRotation = tutorialCamera.transform.rotation;

            if (lookAt)
            {
                desiredRotation = Quaternion.LookRotation(target.position - desiredPosition, Vector3.up);
            }

            while (Vector3.Distance(tutorialCamera.transform.position, desiredPosition) > 0.05f || Quaternion.Angle(tutorialCamera.transform.rotation, desiredRotation) > 0.5f)
            {
                tutorialCamera.transform.position = Vector3.Lerp(tutorialCamera.transform.position, desiredPosition, Time.deltaTime * cameraMoveSpeed);
                tutorialCamera.transform.rotation = Quaternion.Slerp(tutorialCamera.transform.rotation, desiredRotation, Time.deltaTime * cameraMoveSpeed);
                yield return null;
            }
        }

        private IEnumerator WaitForInput()
        {
            while (!Input.GetKeyDown(nextStepKey) && !Input.GetMouseButtonDown(0) && Input.touchCount == 0)
            {
                yield return null;
            }
        }

        private void SetText(string text)
        {
            tutorialText.text = text;
        }

        private void SetHighlight(Renderer renderer, Color highlightColor)
        {
            if (renderer == null)
            {
                return;
            }

            if (!highlightedRenderers.Contains(renderer))
            {
                SaveOriginalMaterialColors(renderer);
                highlightedRenderers.Add(renderer);
            }

            foreach (var mat in renderer.materials)
            {
                if (mat.HasProperty("_EmissionColor"))
                {
                    mat.EnableKeyword("_EMISSION");
                    mat.SetColor("_EmissionColor", highlightColor);
                }
                else if (mat.HasProperty("_Color"))
                {
                    mat.SetColor("_Color", highlightColor);
                }
            }
        }

        private void ClearHighlight(Renderer renderer)
        {
            if (renderer == null || !highlightedRenderers.Contains(renderer))
            {
                return;
            }

            if (originalColors.TryGetValue(renderer, out var colors))
            {
                var mats = renderer.materials;
                for (int i = 0; i < mats.Length && i < colors.Length; i++)
                {
                    if (mats[i].HasProperty("_Color"))
                    {
                        mats[i].SetColor("_Color", colors[i]);
                    }
                }
            }

            if (originalEmissionColors.TryGetValue(renderer, out var emissionColors))
            {
                var mats = renderer.materials;
                for (int i = 0; i < mats.Length && i < emissionColors.Length; i++)
                {
                    if (mats[i].HasProperty("_EmissionColor"))
                    {
                        mats[i].SetColor("_EmissionColor", emissionColors[i]);
                    }
                }
            }

            highlightedRenderers.Remove(renderer);
        }

        private void SaveOriginalMaterialColors(Renderer renderer)
        {
            if (renderer == null)
            {
                return;
            }

            var mats = renderer.materials;
            var colorArray = new Color[mats.Length];
            var emissionArray = new Color[mats.Length];

            for (int i = 0; i < mats.Length; i++)
            {
                colorArray[i] = mats[i].HasProperty("_Color") ? mats[i].GetColor("_Color") : Color.white;
                emissionArray[i] = mats[i].HasProperty("_EmissionColor") ? mats[i].GetColor("_EmissionColor") : Color.black;
            }

            originalColors[renderer] = colorArray;
            originalEmissionColors[renderer] = emissionArray;
        }

        private void EndTutorial()
        {
            tutorialText.text = string.Empty;
            if (tutorialPanel != null)
            {
                tutorialPanel.SetActive(false);
            }
        }
    }
}
