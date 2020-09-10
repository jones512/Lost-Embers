using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

using AdventureKit.Kernel.Loading;

namespace AdventureKit.Kernel.Context
{
	/// <summary>
	/// Wraps the lifecycle of a Scene.
    /// Provides information about his current state
    /// and provides "callback" methods for OnEnter and 
    /// OnExit
	/// </summary>
	public class BaseContext : Utils.MonoBehaviour 
    {
        public bool ExitCompleted { get; protected set; }					
		public bool EnterCompleted { get; protected set; }

        public iSceneBootstrap SceneController { get; protected set; }

        /// <summary>
        /// Called before Entering on a new Scene
        /// </summary> 
		public virtual void Enter() 
        { 
            // if overriden remember to set the flag
            EnterCompleted = true; 
        }

        public virtual void Enter(string level)
        {
            // if overriden remember to set the flag
            EnterCompleted = true;
        }

        /// <summary>
        /// Called before Exit from a Scene
        /// </summary>
        public virtual void Exit() 
        {
            // if overriden remember to set the flag
            ExitCompleted = true; 
        }
        

        /// <summary>
        /// Async load of a scene, when done calls OnSceneLoaded, initialize
        /// the SceneController of the loaded scene and sets the EnterCompleted flag      
        /// </summary>        
        protected void LoadScene(string sceneName, bool additive=false)
        {
            StartCoroutine(_LoadSceneTask(sceneName, additive));
        }

        IEnumerator _LoadSceneTask(string sceneName, bool additive)
        {
            DefaultLoadingScreen loading = Kernel.Instance.DefaultLoadingScreen;
            loading.PlayClosening();
            while (!loading.CloseningFinished)
                yield return null;

            float progress;

            AsyncOperation loadOp = additive? SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive) : SceneManager.LoadSceneAsync(sceneName);            
            while (!loadOp.isDone)
            {
                yield return null;
                progress = loadOp.progress;
                loading.UpdateProgress(progress);
            }
            progress = 1f;            

            //OnSceneLoaded();

            //// Init SceneController of the loaded scene
            //SendInit();

            loading.PlayOpening();
            while (!loading.OpeningFinished)
                yield return null;

            EnterCompleted = true;
        }

        /// <summary>
        /// Inherit to let the Context to perform something
        /// straight after scene has been loaded
        /// </summary>
        protected virtual void OnSceneLoaded() { }

        /// <summary>
        /// Searchs the SceneController for the active Scene
        /// and calls its Init method
        /// </summary>
        protected void SendInit()
        {
            GameObject sceneController = GameObject.Find("_SceneBootstrap");
            if (sceneController == null) 
            {
                Debug.LogError(this.Log("ERROR: No se puede enviar SendInit porque no existe el objeto _SceneController en la escena"));
                return;
            }

            iSceneBootstrap sc = sceneController.GetComponent<iSceneBootstrap>();            
            Tester.ASSERT(sc != null, "Missing SceneController component on " + sceneController.name);
            SceneController = sc;
            sc.Init(this);            
        }       
	}

}

