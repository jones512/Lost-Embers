using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureKit.Kernel.Tasks
{
    public class TaskChangeContext : Task.Task
    {
        public enum GameContext { MAIN, MAIN_MENU, WORLD_SELECTION, FIRST_WORLD }
        [SerializeField]
        private GameContext m_TargetContext;

        protected override void DoStart()
        {
            base.DoStart();

            Complete("TaskLoadStartScene::Complete");

            switch(m_TargetContext)
            {
                case GameContext.MAIN:
                    Kernel.Instance.EnterMainContext();
                    break;

                case GameContext.MAIN_MENU:
                    Kernel.Instance.EnterMainMenuContext();
                    break;

                //case GameContext.FIRST_WORLD:
                //    Kernel.Instance.EnterFirstWorldContext();
                //    break;
            }
        }
    }
}
