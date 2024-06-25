using UnityEngine;

namespace MyCampusStory
{
    public class InteractableObjectTest : MonoBehaviour, IInteractable
    {
        public void Interact()
        {
            Debug.Log("Interacted");
        }
    }
}

