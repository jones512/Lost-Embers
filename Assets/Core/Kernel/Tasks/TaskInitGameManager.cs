using UnityEngine;
using System.Collections;

namespace AdventureKit.Kernel.Tasks
{
    public class TaskInitGameManager : Task.Task
    {

        protected override void DoStart()
        {
            base.DoStart();

            K.GameManager.Init();

            Complete("TaskInitGameManager::Complete");
        }

    }
}
