using UnityEngine;
using System.Collections;

namespace AdventureKit.Kernel.Tasks
{
    public class TaskEnableUI : Task.Task
    {

        public GameObject UIGameobject;

        protected override void DoStart()
        {
            UIGameobject.SetActive(true);
            Complete("TaskEnbleUI::Complete");
        }
    }
}
