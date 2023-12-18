using UnityEngine;

using MyCampusStory.Player;

namespace MyCampusStory.EffectSystem
{
    [CreateAssetMenu(fileName = "ChangeStat_", menuName = "Scriptable Objects/Effect Assets/New ChangeStat")]
    public class ChangeStat : Effect
    {
        [Tooltip("Select the Status to be changed")]
        [SerializeField] private PlayerStatType _changedStatType;


        [Tooltip("Change value of selected Status, can be minus or plus")]
        [SerializeField] private int _changeValue;


        public override void ApplyEffect()
        {
            PlayerData.Instance.ChangeStat(_changedStatType, _changeValue);
        }
    }

}
