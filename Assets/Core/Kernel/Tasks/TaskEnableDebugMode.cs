using UnityEngine;
using System.Collections;

namespace AdventureKit.Kernel.Tasks
{
    public class TaskEnableDebugMode : Task.Task
    {
        public enum ContextType { MAIN, MAIN_MENU, FIRST_WORLD}
        [Header("Target Context")]
        [SerializeField]
        private ContextType m_TargetContext;

        [Header("Debug Scene")]
        [SerializeField]
        private bool m_DebugScene;

        protected override void DoStart()
        {
            Debug.Log("TaskEnableDebugMode:Initialized");

            if (m_DebugScene)
                StartCoroutine("_EnableDebugMode");
            else
                Complete("TaskEnableDebugMode:Completed");
        }

        private IEnumerator _EnableDebugMode()
        {
            while (Kernel.Instance.KernelTaskManager != null && !Kernel.Instance.KernelTaskManager.BootCompleted)
                yield return null;

            switch(m_TargetContext)
            {
                case ContextType.MAIN:
                    K.EnterMainContext();
                    break;

                case ContextType.MAIN_MENU:
                    K.EnterMainMenuContext();
                    break;

                //case ContextType.FIRST_WORLD:
                //    K.EnterFirstWorldContext();
                //    break;
            }

            Complete("TaskEnableDebugMode:Completed");
        }
    }

    
}
