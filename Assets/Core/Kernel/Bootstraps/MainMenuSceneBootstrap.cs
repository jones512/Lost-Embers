using UnityEngine;
using System.Collections;

using AdventureKit.Task;
using AdventureKit.Kernel.Context;

namespace AdventureKit.Kernel.Bootstraps
{
    public class MainMenuSceneBootstrap : TaskDependencyManager, iSceneBootstrap
    {

        public void Init(BaseContext context)
        {
            Debug.Log(this.Log("MainMenu Scene Init"));

            StartTasks();
        }

        public void InitFromKernel()
        {
            Debug.Log(this.Log("MainMenu Scene Initialized from Kernel"));
            Init(Kernel.Instance.MainMenuContext);
        }

        protected override void OnBootTasksCompleted()
        {
            Debug.Log(this.Log("MainMenu Scene Boot Tasks Completed"));
        }
    }
}
