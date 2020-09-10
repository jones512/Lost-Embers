using System.Collections;
using UnityEngine;
using System;

using AdventureKit.Task;
using AdventureKit.Kernel.Context;
using AdventureKit.Kernel.Loading;
using AdventureKit.ScreensManagement.BaseClaseAndUtils;
using AdventureKit.Config;
using AdventureKit.Common;
using AdventureKit.Utils;

namespace AdventureKit.Kernel
{
    public class Kernel : TaskDependencyManager
    {
        [Header("Kernel Servicies")]
        [SerializeField]
        private GameManager m_GameManager;
        public GameManager GameManager { get { return m_GameManager; } }
        [SerializeField]
        private SaveLoad m_SaveLoad;
        public SaveLoad SaveLoad { get { return m_SaveLoad; } }
        [SerializeField]
        private SoundManager m_SoundManager;
        public SoundManager SoundManager { get { return m_SoundManager; } }

        [Header("Game Contexts")]
        [SerializeField]
        MainContext m_MainContext;
        public MainContext MainContext { get { return m_MainContext; } }
        [SerializeField]
        MainMenuContext m_MainMenusContext;
        public MainMenuContext MainMenuContext { get { return m_MainMenusContext; } }
        [SerializeField]
        LevelContext m_LevelContext;
        public LevelContext LevelContext { get { return m_LevelContext; } }

        [Header("Loading Screens")]
        [SerializeField]
        DefaultLoadingScreen m_DefaultLoadingScreen;
        public DefaultLoadingScreen DefaultLoadingScreen { get { return m_DefaultLoadingScreen; } }
        [SerializeField]
        LoadingScreen m_LoadingScreen;
        public LoadingScreen LoadingScreen { get { return m_LoadingScreen; } }

        public ScreensManager ScreenManager { get; set; }
        public KernelTaskManager KernelTaskManager { get; private set; }
        
        public BaseContext CurrentContext { get; private set; }
        public BaseContext PreviousContext { get; private set; }
        private BaseContext mContextAfterLoadingScreen;
      
        public bool IsChangingContext { get; private set; }
        private static bool _CleanUpOnDestroy = false;

        #region Singleton
        private static Kernel sInstance;

        private Kernel() { }

        public static Kernel Instance
        {
            get
            {
                if (sInstance == null)
                {
                    sInstance = FindObjectOfType(typeof(Kernel)) as Kernel;
                    if (sInstance == null)
                    {
                        var kernelPrefab = Resources.Load("_Kernel") as GameObject;
                        sInstance = Instantiate(kernelPrefab).GetComponent<Kernel>();
                    }
                }
                return sInstance;
            }
        }
        #endregion

        #region monobehaviour init and destroy
        void Awake()
        {            
            Debug.Log(this.Log("Initialize"));

            KernelTaskManager = GetComponentInChildren<KernelTaskManager>();

            // StartContext is always the default
            if (CurrentContext == null)
                CurrentContext = MainContext;

            // Do not destroy on scene changes
            if (Application.isPlaying)
                DontDestroyOnLoad(gameObject);

        }

        void OnDestroy() {

            Debug.Log("on destroy kernel");

            if(ScreenManager != null)
             ScreenManager = null;

            if (m_SoundManager != null)
                m_SoundManager = null;
        }
        #endregion


        #region Context functions
        public void EnterMainContext() 
        {
            EnterOnContext(MainContext);
        }

        public void EnterMainMenuContext()
        {
            EnterOnContext(MainMenuContext);
        }

        public void EnterLevelContext(string levelName)
        {
            LevelContext.SetLevelName(levelName);
            EnterOnContext(LevelContext);
        }


        public void EnterPreviousContext()
        {
            EnterOnContext(PreviousContext);
        }

        public void SetCurrentContext(BaseContext newContext)
        {
            CurrentContext = newContext;
        }

        private void EnterOnContext(BaseContext baseContext)
        {
            mContextAfterLoadingScreen = baseContext;
            LoadingScreen.ScreenOpened += OnScreenOpened;
            LoadingScreen.OpenScreen();
        }

        private void OnScreenOpened(object sender, EventArgs eventArgs)
        {
            LoadingScreen.ScreenOpened -= OnScreenOpened;
            StartCoroutine(_ExitAndLaunchContext(mContextAfterLoadingScreen));
        }

        public void RestartCurrentScene()
        {
            if (IsChangingContext && PreviousContext != null)
            {
                StopCoroutine("_ExitAndLaunchContext");
                IsChangingContext = false;
                StartCoroutine(_ExitAndLaunchContext(PreviousContext));
            }
            else
            {
                StartCoroutine(_ExitAndLaunchContext(CurrentContext));
            }
        }
          
        public IEnumerator _ExitAndLaunchContext(BaseContext newContext)
        {
            if (!IsChangingContext && newContext != null)
            {
                IsChangingContext = true;
                
                if (CurrentContext != null)
                {                                    
                    PreviousContext = CurrentContext;                    
                    CurrentContext.Exit();

                    // Wait for current context to finish his Exit stuff
                    while (!CurrentContext.ExitCompleted) { 
                        yield return new WaitForSeconds(AppConfig.CHECK_COMPLETED_TASK_INTERVAL); //basically 60fps to wait (0.016ms)
                    }                    

                }

                CurrentContext = newContext;                
                CurrentContext.Enter();


                // Wait for current context to finish shi Enter stuff
                while (!CurrentContext.EnterCompleted) {
                    yield return new WaitForSeconds(AppConfig.CHECK_COMPLETED_TASK_INTERVAL); //basically 60fps to wait (0.016ms)
                }

                IsChangingContext = false;
            }
        }
        #endregion
    }
}