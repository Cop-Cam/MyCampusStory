//----------------------------------------------------------------------
// Author   : "Ananta Miyoru Wijaya"
//----------------------------------------------------------------------

using UnityEngine;

namespace MyCampusStory.DesignPatterns
{
    /// <summary>
    /// For singleton that should live forever
    /// </summary>
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        // create a private reference to T instance
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();

                    if (_instance == null)
                    {
                        Debug.LogError("There is no " + typeof(T) + " in the scene.");
                    }
                }

                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Debug.LogWarning("There is already a " + typeof(T) + " in the scene.");
                Destroy(gameObject);
            }
        }
    }


    /// <summary>
    /// For singleton that should be destroyed when changing scene
    /// </summary>
    public class SceneSingleton<T> : MonoBehaviour where T : Component
    {
        // create a private reference to T instance
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();

                    if (_instance == null)
                    {
                        Debug.LogError("There is no " + typeof(T) + " in the scene.");
                    }
                }

                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
            }
            else
            {
                Debug.LogWarning("There is already a " + typeof(T) + " in the scene.");

                Destroy(gameObject);
            }
        }
    }

}
