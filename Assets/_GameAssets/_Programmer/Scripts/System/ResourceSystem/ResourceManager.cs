//----------------------------------------------------------------------
// Author   : "Ananta Miyoru Wijaya"
//----------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

using MyCampusStory.DataPersistenceSystem;
using MyCampusStory.DesignPatterns;
using MyCampusStory.BuildingSystem;
using System;

namespace MyCampusStory.ResourceSystem
{
    /// <summary>
    /// Class for managing resources in game
    /// </summary>
    public class ResourceManager : MonoBehaviour, IDataPersistence
    {
        [SerializeField] private ResourceSO[] _resourceSOs;
        private Dictionary<string, Resource> _resourcesDictionary;

        public delegate void ResourceChangedEventDelegate();
        public event ResourceChangedEventDelegate OnResourceChanged;

        private void Awake()
        {
            _resourcesDictionary = new Dictionary<string, Resource>();

            //Instantiate resources based on the resource SOs
            foreach (var resourceSO in _resourceSOs)
            {
                var newResource = new Resource(resourceSO);

                if(_resourcesDictionary.ContainsKey(resourceSO.ResourceId))
                {
                    Debug.LogWarning("There are resources with same id found: " + resourceSO.ResourceId);
                }
                else
                {
                    _resourcesDictionary.Add(resourceSO.ResourceId, newResource);
                }
            }
        }

        public int GetResourceAmount(string resourceId)
        {
            if (_resourcesDictionary.ContainsKey(resourceId))
            {
                return _resourcesDictionary[resourceId].ResourceAmount;
            }
            else
            {
                Debug.LogWarning("Resource not found: " + resourceId);
                return 0;
            }
        }

        /// <summary>
        /// Adjusts the amount of a specified resource by adding its current amount with a new value.
        /// </summary>
        /// <param name="resourceId">The identifier of the resource to be modified.</param>
        /// <param name="amount">The new amount to set for the resource. 
        /// Positive values increase the resource amount, while negative values decrease it.</param>
        public void ModifyResourceAmount(string resourceId, int amount)
        {
            if (_resourcesDictionary.ContainsKey(resourceId))
            {
                _resourcesDictionary[resourceId].ModifyAmount(amount);

                OnResourceChanged?.Invoke();
            }
            else
            {
                Debug.LogWarning("Resource not found: " + resourceId);
            }
        }

        /// <summary>
        /// Adjusts the amount of a specified resource by replacing its current amount with a new value.
        /// </summary>
        /// <param name="resourceId">The identifier of the resource to be modified.</param>
        /// <param name="amount">The new amount to set for the resource.</param>
        public void ReplaceResourceAmount(string resourceId, int amount)
        {
            if (_resourcesDictionary.ContainsKey(resourceId))
            {
                _resourcesDictionary[resourceId].ReplaceAmount(amount);

                OnResourceChanged?.Invoke();
            }
            else
            {
                Debug.LogWarning("Resource not found: " + resourceId);
            }
        }

        #region IDataPersistence
        public void LoadData(GameData data)
        {
            foreach (var loadedResources in data.PlayerResourceData)
            {
                if(_resourcesDictionary.ContainsKey(loadedResources.Key))
                {
                    _resourcesDictionary[loadedResources.Key].ReplaceAmount(loadedResources.Value);
                }
                else
                {
                    Debug.LogWarning("There is a different resource found from save data: " + loadedResources.Key);
                }
            }
        }   

        public void SaveData(GameData data)
        {
            foreach (var resource in _resourcesDictionary)
            {
                if(!data.PlayerResourceData.ContainsKey(resource.Key))
                {
                    data.PlayerResourceData.Add(resource.Key, resource.Value.ResourceAmount);
                }
                else
                {
                    data.PlayerResourceData[resource.Key] = resource.Value.ResourceAmount;
                }
            }
        }
        #endregion
    }
}