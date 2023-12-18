using UnityEngine;
using TMPro;

using MyCampusStory.StatSystem;
using MyCampusStory.ResourceSystem;


namespace MyCampusStory.Player
{
    public enum PlayerStatType
    {
        SOCIAL,
        INTELLIGENCE
    }
    public enum PlayerResourceType
    {
        MONEY,
        ENERGY
    }

    public class PlayerData : SingletonMonoBehaviour<PlayerData>
    {
        [Header("Player Stats")]
        public Stat Social;    
        public Stat Intelligence;


        [Header("Player Resources")]
        public Resource Money; 
        public Resource Energy;





        public void ChangeStat(PlayerStatType playerStatType, int value)
        {
            switch(playerStatType)
            {
                case PlayerStatType.SOCIAL:
                    Social.AddToValue(value);
                    break;

                case PlayerStatType.INTELLIGENCE:
                    Intelligence.AddToValue(value);
                    break;

                default:
                    Debug.LogWarning("There is no player stat for type " + playerStatType);
                    break;    
            }
        }

        public void ChangeResource(PlayerResourceType playerResourceType, int value)
        {
            switch(playerResourceType)
            {
                case PlayerResourceType.MONEY:
                    Money.AddToValue(value);
                    break;

                case PlayerResourceType.ENERGY:
                    Energy.AddToValue(value);
                    break;

                default:
                    Debug.LogWarning("There is no player resource for type " + playerResourceType);
                    break;
            }
        }
         
    }
}