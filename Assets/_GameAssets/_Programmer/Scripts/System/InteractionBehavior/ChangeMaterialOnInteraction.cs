//----------------------------------------------------------------------
// Author   : "Ananta Miyoru Wijaya"
//----------------------------------------------------------------------

using System.Collections;
using UnityEngine;


namespace MyCampusStory.InteractionBehavior
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Material))]
    public class ChangeMaterialOnInteraction : MonoBehaviour, IClickable
    {
        private Material _originalMaterial;
        [SerializeField] private Material _changingMaterial; 

        private void Awake()
        {
            // Store the original material
            _originalMaterial = GetComponent<Renderer>().material;
        }

        public void OnClick()
        {
            // Switch to other material on interaction
            GetComponent<Renderer>().material = _changingMaterial;
        }

        public void OnStopClick()
        {
            // Revert to the original material when stop interacting
            GetComponent<Renderer>().material = _originalMaterial;
        }
    }
}
