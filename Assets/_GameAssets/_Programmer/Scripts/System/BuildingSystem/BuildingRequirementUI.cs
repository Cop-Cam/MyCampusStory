//----------------------------------------------------------------------
// Author   : "Ananta Miyoru Wijaya"
//----------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using TMPro;

using MyCampusStory.ResourceSystem;
using MyCampusStory.StandaloneManager;

namespace MyCampusStory.BuildingSystem
{
    public class BuildingRequirementUI : MonoBehaviour
    {
        private string _resourceId;
        private int _resourceAmountInInt;
        [SerializeField] private Image _resourceIcon;
        [SerializeField] private TextMeshProUGUI _resourceAmount;
        [SerializeField] private Animator _animator;

        // public string Activated_Anim_Param = "ISACTIVATED";

        public string Activated_Anim_State = "ACTIVATED";
        public string DeActivated_Anim_State = "DEACTIVATED";
        public string Fulfilled_Anim_State = "FULFILLED";
        public string UnFulfilled_Anim_State = "UNFULFILLED";

        public string _currentAnimState;

        private void Update()
        {
            if(_currentAnimState != Activated_Anim_State) return;

            if(_resourceAmountInInt <= LevelManager.Instance.ResourceManager.GetResourceAmount(_resourceId))
            {
                SetAnimCrossFade(Fulfilled_Anim_State, 0.1f);
            }
            else
            {
                SetAnimCrossFade(UnFulfilled_Anim_State, 0.1f);
            }
        }

        public void Init(ResourceSO resourceSO, int resourceAmount)
        {
            _resourceIcon.sprite = resourceSO.ResourceIcon;
            _resourceAmount.text = resourceAmount.ToString();
            _resourceAmountInInt = resourceAmount;
            _resourceId = resourceSO.ResourceId;
            // _animator.SetBool(Activated_Anim_Param, true);
            SetAnimCrossFade(Activated_Anim_State, 0.1f);
        }

        public void DeInit()
        {
            // _animator.SetBool(Activated_Anim_Param, false);
            _resourceIcon.sprite = null;
            _resourceAmount.text = string.Empty;
        }

        public void SetAnimCrossFade(string id, float value)
        {
            if (!string.IsNullOrEmpty(id) && _currentAnimState != id)
            {
                _animator?.CrossFade(id, value);
                _currentAnimState = id;
            }
        }
    }
}
