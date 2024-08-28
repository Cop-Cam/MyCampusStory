//----------------------------------------------------------------------
// Author   : "Ananta Miyoru Wijaya"
//----------------------------------------------------------------------

using UnityEngine;
using TMPro;

using MyCampusStory.StandaloneManager;

namespace MyCampusStory.ResourceSystem
{
    public class ResourceUIManager : MonoBehaviour
    {
        [System.Serializable]
        public struct ResourceUI
        {
            public ResourceSO ResourceSO;
            public TextMeshProUGUI ResourceText;
        }
        [SerializeField] private ResourceUI[] _resourceUIs;

        private void OnEnable()
        {
            if(LevelManager.Instance.ResourceManager != null)
            {
                LevelManager.Instance.ResourceManager.OnResourceChanged += UpdateResourceDisplayAmount;
            }
        }

        private void OnDisable()
        {
            if(LevelManager.Instance.ResourceManager != null)
            {
                LevelManager.Instance.ResourceManager.OnResourceChanged -= UpdateResourceDisplayAmount;
            }
        }

        private void UpdateResourceDisplayAmount()
        {
            foreach (ResourceUI resourceUI in _resourceUIs)
            {
                resourceUI.ResourceText.text = LevelManager.Instance.ResourceManager.GetResourceAmount(resourceUI.ResourceSO.ResourceId).ToString();
            }
        }
    }
}