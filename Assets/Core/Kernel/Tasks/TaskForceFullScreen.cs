using UnityEngine;

namespace AdventureKit.Kernel.Tasks
{
    public class TaskForceFullScreen : Task.Task
    {       
       

        protected override void DoStart()
        {

            base.DoStart();

            //by default 60fps with full screen
            Application.targetFrameRate = 60;
            Screen.fullScreen = true;


            Complete("TaskForceFullScreen::Complete");
        }        
    }
}


