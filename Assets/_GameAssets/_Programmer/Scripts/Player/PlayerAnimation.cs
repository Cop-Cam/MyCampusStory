//----------------------------------------------------------------------
// Author   : "Ananta Miyoru Wijaya"
//----------------------------------------------------------------------

using UnityEngine;

namespace MyCampusStory.Player
{
    /// <summary>
    /// Class for handling player animation
    /// </summary>
    public class PlayerAnimation : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        [field:SerializeField] public string Idle_anim { get; private set; } = "Idle";
        [field:SerializeField] public string Walk_anim { get; private set; } = "Walk";
        [field:SerializeField] public string Run_anim { get; private set; } = "Run";
        [field:SerializeField] public string Jump_anim { get; private set; } = "Jump";
        [field:SerializeField] public string Fall_anim { get; private set; } = "Fall";

        public void SetAnimBool(string id, bool value)
        {
            if (!string.IsNullOrEmpty(id))
                _animator.SetBool(id, value);
        }

        public void SetAnimFloat(string id, float value)
        {
            if (!string.IsNullOrEmpty(id))
                _animator.SetFloat(id, value);
        }

        public void SetAnimTrigger(string id)
        {
            if (!string.IsNullOrEmpty(id))
                _animator.SetTrigger(id);
        }
    }
}
