//----------------------------------------------------------------------
// Author   : "Ananta Miyoru Wijaya"
//----------------------------------------------------------------------

using System.Collections;
using UnityEngine;


namespace MyCampusStory.InteractionBehavior
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Transform))]
    public class RescaleOnInteraction : MonoBehaviour, IInteractable
    {
        [SerializeField] private Vector3 _targetScale = new Vector3(2f, 2f, 2f);
        [SerializeField] private float _animationDuration = 0.1f;
        
        private Vector3 _originalScale;
        private Coroutine _resizeObjectAnimationCoroutine;

        private void Awake()
        {
            _originalScale = transform.localScale;
        }

        public void OnInteract()
        {
            _resizeObjectAnimationCoroutine = StartCoroutine(ResizeObject(true));
        }

        public void OnStopInteract()
        {
            _resizeObjectAnimationCoroutine = StartCoroutine(ResizeObject(false));
        }

        private IEnumerator ResizeObject(bool scalingUp)
        {
            float currentTime = 0f;

            if(scalingUp)
            {
                // Scale up
                while (currentTime < _animationDuration)
                {
                    transform.localScale = Vector3.Lerp(_originalScale, _targetScale, currentTime / _animationDuration);
                    currentTime += Time.deltaTime;
                    yield return null;
                }
                transform.localScale = _targetScale;
            }
            else
            {
                currentTime = 0f;
                while (currentTime < _animationDuration)
                {
                    transform.localScale = Vector3.Lerp(_targetScale, _originalScale, currentTime / _animationDuration);
                    currentTime += Time.deltaTime;
                    yield return null;
                }
                transform.localScale = _originalScale;
            }
        }
    }
}
