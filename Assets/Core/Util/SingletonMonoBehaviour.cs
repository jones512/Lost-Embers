using UnityEngine;

namespace AdventureKit.Utils
{
    //public to be accessible by Unity engine
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
    {
        private static T m_instance;

        public static T Instance
        {
            get
            {
                if (m_instance == null)
                {
                    //find existing instance (only active ones)                   
                    m_instance = GameObject.FindObjectOfType<T>();
                    
                    //// Support for inactive GO: if not found by type (because its inactive)
                    //if (m_instance == null)
                    //{
                    //    // find it by name (that means GO should be named as the Singleton
                    //    GameObject go = GameObject.Find(typeof(T).Name);
                    //    m_instance = null;
                    //    go.SetActive(true);
                    //    m_instance = go.GetComponent<T>();
                    //}

                    if (m_instance == null)
                    {
                        //create new instance
                        GameObject go = new GameObject(typeof(T).Name);
                        m_instance = go.AddComponent<T>();
                        //DontDestroyOnLoad(go);
                    }
                    
                    
                    //initialize instance if necessary
                    if (!m_instance.initialized)
                    {
                        m_instance.Initialize();
                        m_instance.initialized = true;
                    }
                }
                return m_instance;
            }
        }

        private void Awake()
        {
            //check if instance already exists when reloading original scene
            if (m_instance != null)
            {
                DestroyImmediate(gameObject);
            }
            else
            {
                // auto load
                m_instance = Instance;
            }
        }

        protected bool initialized { get; set; }

        protected virtual void Initialize()
        { }
    }
}
