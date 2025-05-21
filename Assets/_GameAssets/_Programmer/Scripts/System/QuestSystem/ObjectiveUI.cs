//----------------------------------------------------------------------
// Author   : "Ananta Miyoru Wijaya"
//----------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using TMPro;

using MyCampusStory.ResourceSystem;

namespace MyCampusStory.QuestSystem
{
    public class ObjectiveUI : MonoBehaviour
    {
        [SerializeField] private Image _resourceIcon;
        [SerializeField] private TextMeshProUGUI _resourceAmount;

        public void Init(ResourceSO resourceSO, int resourceAmount)
        {
            _resourceIcon.sprite = resourceSO.ResourceIcon;
            _resourceAmount.text = resourceAmount.ToString();
        }

        public void DeInit()
        {
            _resourceIcon.sprite = null;
            _resourceAmount.text = string.Empty;
        }
    }
}