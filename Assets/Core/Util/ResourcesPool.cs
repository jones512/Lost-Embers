using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace AdventureKit.Utils
{

    /// <summary>
    /// Pool of resources (NOT RESOURCES FOLDER) to have preloaded, all assets used by the game outside the Resources folder should be here.
    /// </summary>
    public class ResourcesPool : SingletonMonoBehaviour<ResourcesPool>
    {
        /**
		* Prefabs de los que se va a hacer pool
		* CUIDADO ¡EL ORDEN ES IMPORTANTE!
		*/
        [SerializeField]
        GameObject[] m_prefabs;

        /**
		* Cantidad mínima que siempre ha de estar disponible
		* CUIDADO ¡EL ORDEN ES IMPORTANTE!
		*/
        [SerializeField]
        int[] m_minAmount;


        /// <summary>
        /// Sprites to access from the game, not in resources
        /// </summary>
        [SerializeField]
        List<Sprite> m_sprites;

        /// <summary>
        /// Dictionary to access the sprites by name
        /// </summary>
        protected Dictionary<string, Sprite> mSpritesByName = new Dictionary<string, Sprite>();

        /// <summary>
        /// Dictionary to access the UI Images by name
        /// </summary>
        protected Dictionary<string, Image> mImagesByName = new Dictionary<string, Image>();


        /**
		* Los objetos creados.
		*/
        protected Dictionary<int, List<GameObject>> mGameObjects = new Dictionary<int, List<GameObject>>();

        /* Diccionario de objetos por nombre */
        protected Dictionary<string, int> mGameObjectsByName = new Dictionary<string, int>();

        /**
		 * Callback para saber cuando se ha termiando de inicializar
		 * por completo: se ha ejecutado Start() 
		 */
        public System.Action OnPoolLoaded;


        // EDITOR
        // -------------------------------------------

        [SerializeField]
        List<string> m_spritesPaths;
        [ContextMenu("LoadSprites")]
        void LoadSprites()
        {
#if UNITY_EDITOR
            m_sprites = new List<Sprite>();
            foreach (string spritesPath in m_spritesPaths)
            {
                string path = spritesPath;
                string[] assetFiles = System.IO.Directory.GetFiles(Application.dataPath + path);
                foreach (string asset in assetFiles)
                {
                    string assetPath = "Assets" + asset.Replace(Application.dataPath, "").Replace('\\', '/');
                    Sprite s = (Sprite)UnityEditor.AssetDatabase.LoadAssetAtPath(assetPath, typeof(Sprite));
                    if (s != null)
                        m_sprites.Add(s);
                }
            }
#endif
        }

        // -------------------------------------------
        protected override void Initialize()
        {
            Assert.IsTrue(m_minAmount.Length == m_prefabs.Length, "No hay la misma cantidad de prefabs que de definición de mínimos");

            //DontDestroyOnLoad(this.gameObject);

            Debug.Log("-> Loading ResourcesPool");

            /**
			* Inicializa las listas e instancia los objetos
			* Despues los desactiva
			*/
            for (int i = 0; i < m_prefabs.Length; i++)
            {
                mGameObjects[i] = new List<GameObject>();
                mGameObjectsByName.Add(m_prefabs[i].name, i);
                for (int j = 0; j < m_minAmount[i]; j++)
                {
                    GameObject nuevoObjeto = NewInstance(m_prefabs[i]);
                    mGameObjects[i].Add(nuevoObjeto);
                    nuevoObjeto.SetActive(false);
                }
            }

            for (int i = 0; i < m_sprites.Count; ++i)
            {
                mSpritesByName.Add(m_sprites[i].name, m_sprites[i]);
            }

            if (OnPoolLoaded != null)
                OnPoolLoaded();

            Debug.Log("ResourcesPool loaded <-");
        }

        /**
		* Se pide un objeto de un tipo
		* - Recorre el array buscando el objeto desactivado
		* - Si lo encuentra lo activa y lo devuelve
		* - Si no lo encuentra, instancia uno y aumenta la cantidad que hay en la lista
		*/
        public GameObject GetObject(int tipo, bool active = true)
        {
            // Busca un objeto inactivo dentro de la lista
            // una vez lo encuentra lo activa y lo devuelve	 	
            Assert.IsTrue((tipo >= 0 && tipo < mGameObjects.Count), "Se está pidiendo un tipo fuera de los límites, el tamaño del array de tipos es más pequeño");

            for (int i = 0; i < mGameObjects[tipo].Count; i++)
            {
                if (!mGameObjects[tipo][i].activeSelf)
                {
                    mGameObjects[tipo][i].SetActive(active);
                    return mGameObjects[tipo][i];
                }
            }

            // No ha encontrado ninguno, lo crea
            GameObject nuevo = NewInstance(m_prefabs[tipo]);
            mGameObjects[tipo].Add(nuevo);
            return nuevo;
        }

        public GameObject GetObject(string name, bool active = true)
        {
            name = Utils.RemoveCloneSubstring(name);
            return GetObject(mGameObjectsByName[name], active);
        }

        /**
		* Desactiva el objeto y lo devuelve al array
		*/
        public void UnloadObject(GameObject t, bool deactivate = true, float delay = 0f)
        {
            if (delay > 0f)
            {
                StartCoroutine(UnloadObjectTask(t, delay, deactivate));
            }
            else
            {
                if (deactivate)
                    t.SetActive(false);

                // Lo volvemos a poner como hijo de este objeto si necesario
                if (t.transform.parent != transform)
                    t.transform.SetParent(transform);
            }
        }

        IEnumerator UnloadObjectTask(GameObject go, float delay, bool deactivate = true)
        {
            yield return new WaitForSeconds(delay);
            UnloadObject(go, deactivate);
        }


        /** 
		* Devuelve el prefab instanciado
		* Lo pone como hijo de este objeto para control
		*/
        protected GameObject NewInstance(GameObject prefab)
        {
            GameObject nuevo = (GameObject)Instantiate(prefab);
            nuevo.transform.SetParent(transform);

            // Quitamos la coletilla '(Clone)' del nombre de la instancia
            nuevo.name = Utils.RemoveCloneSubstring(nuevo.name);
            return nuevo;
        }

        public Sprite GetSprite(string name)
        {
            Sprite result = null;
            if (mSpritesByName.TryGetValue(name.ToLower(), out result))
                return result;

            Debug.LogWarning("Sprite " + name + " not found. Assign it to ResourcesPool");
            return null;
        }
    }
}
