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
        public BuildingStat GetNewBuildingStat(Building building, ResourceManager resourceManager)
        {
            //Check if there is a building stat for the next level
            if(building.BuildingStatsPerLevelDictionary.ContainsKey(building.CurrentBuildingLevel + 1))
            {
                return building.BuildingStatsPerLevelDictionary[building.CurrentBuildingLevel + 1];
            }
            else //If there is none, assign stat using the formula
            {
                // currentBuildingStat = GetNewBuildingStatByFormula();
                Debug.Log("Get New Building Stat By Formula (Not implemented yet!)");
                return building.CurrentBuildingStat;
            }
        }

        public bool CheckUpgradeEligibility(Building building, ResourceManager resourceManager)
        {
            // Check if the building is at max level
            if(building.CurrentBuildingLevel >= building.BuildingDataSO.MaxBuildingLevel)
            {
                return false;
            }

            var currentBuildingStat = building.CurrentBuildingStat;

            //Check if the resource is enough to upgrade the building
            foreach (var buildingUpgradeRequirement in currentBuildingStat.BuildingUpgradeRequirements)
            {
                if (resourceManager.GetResourceAmount(buildingUpgradeRequirement.ResourceRequired.ResourceId) < buildingUpgradeRequirement.AmountRequired)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
