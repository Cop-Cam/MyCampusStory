//----------------------------------------------------------------------
// Author   : "Ananta Miyoru Wijaya"
//----------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;


namespace MyCampusStory.ResourceSystem
{
    public class Resource
    {
        public int ResourceAmount { get; private set; }
        private ResourceSO ResourceSO;
        
        public Resource(ResourceSO resourceSO)
        {
            ResourceSO = resourceSO;
        }

        /// <summary>
        /// Adjusts the amount of a resource by adding a specified value. 
        /// Ensures the resource amount stays within defined upper and lower limits.
        /// </summary>
        /// <param name="amount">The amount to add to the current resource amount. 
        /// Positive values increase the resource amount, while negative values decrease it.</param>
        public void ModifyAmount(int amount)
        {
            if (ResourceAmount + amount > ResourceSO.UpperLimit)
            {
                ResourceAmount = ResourceSO.UpperLimit;
            }
            else if (ResourceAmount + amount < ResourceSO.LowerLimit)
            {
                ResourceAmount = ResourceSO.LowerLimit;
            }
            else
            {
                ResourceAmount += amount;
            }
        }

        /// <summary>
        /// Adjusts the amount of a resource by replacing a specified value. 
        /// Ensures the resource amount stays within defined upper and lower limits.
        /// </summary>
        /// <param name="amount">The amount to replace to the current resource amount.</param>
        public void ReplaceAmount(int amount)
        {
            if (ResourceAmount + amount > ResourceSO.UpperLimit)
            {
                ResourceAmount = ResourceSO.UpperLimit;
            }
            else if (ResourceAmount + amount < ResourceSO.LowerLimit)
            {
                ResourceAmount = ResourceSO.LowerLimit;
            }
            else
            {
                ResourceAmount = amount;
            }
        }
    }

    /// <summary>
    /// Resource scriptableobject for defining purposes only
    /// </summary>
    [CreateAssetMenu(fileName = "Resource_Name", menuName = "ScriptableObjects/Resource")]
    public class ResourceSO : ScriptableObject
    {
        public string ResourceId;
        public string ResourceName;
        public int UpperLimit = int.MaxValue;
        public int LowerLimit = 0;
    }
}
