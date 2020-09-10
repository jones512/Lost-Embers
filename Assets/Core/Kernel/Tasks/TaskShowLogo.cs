using UnityEngine;

using AdventureKit.UI.Core.Screens;

namespace AdventureKit.Kernel.Tasks
{
    public class TaskShowLogo : Task.Task
    {
        protected override void DoStart()
        {
            base.DoStart();

            BasicTransitionScreen screen = K.ScreenManager.GetScreen<BasicTransitionScreen>("Logo");
            screen.OnScreenCloseDone = () => { Complete("TaskInitSplash::Complete"); };
            screen.OpenScreen();
        }
    }
}
