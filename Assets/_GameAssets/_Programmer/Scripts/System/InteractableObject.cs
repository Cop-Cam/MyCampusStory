//----------------------------------------------------------------------
// Author   : "Ananta Miyoru Wijaya"
//----------------------------------------------------------------------

using System.Collections;
using UnityEngine;


namespace MyCampusStory.BuildingSystem
{
    public class InteractableObject : MonoBehaviour, IInteractable
    {
        private Vector3 _originalScale;
        [SerializeField] private Vector3 _targetScale = new Vector3(2f, 2f, 2f);
        [SerializeField] private float _animationDuration = 0.1f;
        private Coroutine _resizeObjectAnimationCoroutine;
        private bool _isAnimating;

        private void Awake()
        {
            _originalScale = transform.localScale;
        }

        public void Interact()
        {
            if(_isAnimating) return;
            
            _isAnimating = true;
            _resizeObjectAnimationCoroutine = StartCoroutine(ResizeObject());
        }

        private IEnumerator ResizeObject()
        {
            float currentTime = 0f;

            // Scale up
            while (currentTime < _animationDuration)
            {
                transform.localScale = Vector3.Lerp(_originalScale, _targetScale, currentTime / _animationDuration);
                currentTime += Time.deltaTime;
                yield return null;
            }
            transform.localScale = _targetScale;

            // Scale down
            currentTime = 0f;
            while (currentTime < _animationDuration)
            {
                transform.localScale = Vector3.Lerp(_targetScale, _originalScale, currentTime / _animationDuration);
                currentTime += Time.deltaTime;
                yield return null;
            }
            transform.localScale = _originalScale;
            _isAnimating = false;
        }
    }
}
