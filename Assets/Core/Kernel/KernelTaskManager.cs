using System;

using AdventureKit.Task;

namespace AdventureKit.Kernel
{
    public class KernelTaskManager : TaskDependencyManager
    {
        // Kernel Events  
        // ------------------------------------------------
        public EventHandler KernelTasksCompleted;

        protected override void OnBootTasksCompleted()
        {
            base.OnBootTasksCompleted();
            KernelTasksCompleted(this, EventArgs.Empty);
        }
    }
}