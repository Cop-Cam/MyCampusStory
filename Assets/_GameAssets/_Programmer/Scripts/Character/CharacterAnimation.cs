// //----------------------------------------------------------------------
// // Author   : "Ananta Miyoru Wijaya"
// //----------------------------------------------------------------------

// using System.Collections.Generic;
// using UnityEngine;

// namespace MyCampusStory.Character
// {
//     /// <summary>
//     /// Class for handling character animation
//     /// </summary>
//     public class CharacterAnimation : MonoBehaviour
//     {
//         [SerializeField] private Animator _animator;

//         [field:SerializeField] public string Idle_anim { get; private set; } = "Idle";
//         [field:SerializeField] public string Walk_anim { get; private set; } = "Walk";
//         [field:SerializeField] public string Interact_anim { get; private set; } = "Interact";

//         public void SetAnimBool(string id, bool value)
//         {
//             if (!string.IsNullOrEmpty(id))
//                 _animator.SetBool(id, value);
//         }

//         public void SetAnimFloat(string id, float value)
//         {
//             if (!string.IsNullOrEmpty(id))
//                 _animator.SetFloat(id, value);
//         }

//         public void SetAnimTrigger(string id)
//         {
//             if (!string.IsNullOrEmpty(id))
//                 _animator.SetTrigger(id);
//         }

//         public void SetAnimCrossFade(string id, float time)
//         {
//             if (!string.IsNullOrEmpty(id))
//                 _animator.CrossFade(id, time);
//         }
//     }
// }
