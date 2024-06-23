//----------------------------------------------------------------------
// Author   : "Ananta Miyoru Wijaya"
//----------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

using MyCampusStory.ResourceSystem;

namespace MyCampusStory.BuildingSystem
{
    public class BuildingUpgradeManager
    {
        public bool TryUpgradingBuilding(Dictionary<int, BuildingStat> buildingStatsPerLevelDictionary, BuildingStat currentBuildingStat, 
        int currentBuildingLevel, ResourceManager resourceManager)
        {
            //Check if the resource is enough to upgrade the building
            foreach (var buildingUpgradeRequirement in currentBuildingStat.BuildingUpgradeRequirements)
            {
                if (resourceManager.GetResourceAmount(buildingUpgradeRequirement.RequiredResource.ResourceId) < buildingUpgradeRequirement.RequiredResourceAmount)
                {
                    return false;
                }
            }
            
            //Check if there is a building stat for the next level
            if(buildingStatsPerLevelDictionary.ContainsKey(currentBuildingLevel + 1))
            {
                currentBuildingStat = buildingStatsPerLevelDictionary[currentBuildingLevel + 1];
            }
            else //If there is none, assign stat using the formula
            {
                // currentBuildingStat = GetNewBuildingStatByFormula();
            }

            return true;
        }
    }
}
