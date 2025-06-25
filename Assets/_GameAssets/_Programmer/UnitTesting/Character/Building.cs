using System.Collections.Generic;
using UnityEngine;

namespace MyCampusStory.StatePatternTest
{
    public class Building : MonoBehaviour
    {
        private static List<Building> _allBuildings = new List<Building>();

        public void Awake()
        {
            if (!_allBuildings.Contains(this))
                _allBuildings.Add(this);
        }

        public static List<Building> GetAllBuildings(object unused) => _allBuildings;

        public Transform GetInteractPoint() => transform;

        public virtual void OnInteract(GameObject obj) { }
        public virtual void OnStopInteract(GameObject obj) { }
    }
}
