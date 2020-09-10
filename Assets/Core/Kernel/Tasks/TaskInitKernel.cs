using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureKit.Kernel.Tasks
{
    public class TaskInitKernel : Task.Task
    {
        public enum GameContext { MAIN, MAIN_MENU, WORLD_SELECTION, LEVEL }
        [SerializeField]
        private GameContext m_TargetContext;

        protected override void DoStart()
        {
            base.DoStart();

            Complete("TaskLoadStartScene::Complete");

            switch (m_TargetContext)
            {
                case GameContext.MAIN:
                    Kernel.Instance.EnterMainContext();
                    break;

                case GameContext.MAIN_MENU:
                    Kernel.Instance.EnterMainMenuContext();
                    break;

                //case GameContext.LEVEL:
                //    Kernel.Instance.EnterLevelContext();
                //    break;
            }
        }
    }
}
