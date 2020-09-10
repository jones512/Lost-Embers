using UnityEngine;
using System.Collections;

using AdventureKit.Task;
using AdventureKit.Kernel.Context;

namespace AdventureKit.Kernel.Bootstraps
{
    public class FirstWorldSceneBootstrap : TaskDependencyManager, iSceneBootstrap
    {

        public void Init(BaseContext context)
        {
            Debug.Log(this.Log("FirstWorld Scene Init"));

            StartTasks();
        }

        public void InitFromKernel()
        {
            Debug.Log(this.Log("FirstWorld Scene Initialized from Kernel"));
            //Init(Kernel.Instance.FirstWorldContext);
        }

        protected override void OnBootTasksCompleted()
        {
            Debug.Log(this.Log("FirstWorld Scene Boot Tasks Completed"));
        }
    }
}
