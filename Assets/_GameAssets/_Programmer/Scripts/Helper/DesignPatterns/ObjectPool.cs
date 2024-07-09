using System.Collections.Generic;
using UnityEngine;

namespace MyCampusStory.DesignPatterns
{
    public class ObjectPool : MonoBehaviour
    {
        private List<GameObject> _pool;
        private GameObject _pooledObject;
        private int _initialPoolSize;
        private Transform _poolHolder;

        public void Init(Transform poolHolder, GameObject pooledObject, int initialPoolSize)
        {
            _pooledObject = pooledObject;
            _initialPoolSize = initialPoolSize;
            _poolHolder = poolHolder;

            _pool = new List<GameObject>();
            for (int i = 0; i < _initialPoolSize; i++)
            {
                GameObject obj = Instantiate(_pooledObject);
                obj.transform.SetParent(_poolHolder);                
                obj.SetActive(false);
                _pool.Add(obj);
            }
        }

        public GameObject GetObject()
        {
            foreach (GameObject obj in _pool)
            {
                if (!obj.activeInHierarchy)
                {
                    return obj;
                }
            }

            GameObject newObj = Instantiate(_pooledObject);
            newObj.SetActive(false);
            _pool.Add(newObj);
            return newObj;
        }

        public void ReturnObject(GameObject obj)
        {
            obj.transform.SetParent(_poolHolder);
            obj.SetActive(false);
        }
    }
    
}
