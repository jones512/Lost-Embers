using UnityEngine;
using System.Collections;

using AdventureKit.Task;
using AdventureKit.Kernel.Context;

namespace AdventureKit.Kernel.Bootstraps
{
    public class MainSceneBootstrap : TaskDependencyManager, iSceneBootstrap
    {

        public void Init(BaseContext context)
        {
            Debug.Log(this.Log("Main Scene Init"));

            StartTasks();
        }

        public void InitFromKernel()
        {
            Debug.Log(this.Log("Main Scene Initialized from Kernel"));
            Init(Kernel.Instance.MainContext);
        }

        protected override void OnBootTasksCompleted()
        {
            Debug.Log(this.Log("Main Scene Boot Tasks Completed"));
        }
    }
}
