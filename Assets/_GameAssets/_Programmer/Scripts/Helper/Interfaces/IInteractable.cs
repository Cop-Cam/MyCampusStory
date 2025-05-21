//----------------------------------------------------------------------
// Author   : "Ananta Miyoru Wijaya"
//----------------------------------------------------------------------

using UnityEngine;

/// <summary>
/// Interface for defining contract to implement interactable behaviour in class
/// </summary>
public interface IInteractable
{
    void OnInteract(GameObject interactedObject);
    void OnStopInteract(GameObject interactedObject);
}

