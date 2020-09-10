using System.Collections.Generic;
using UnityEngine;

using AdventureKit.ContextHandlers;

namespace AdventureKit.Kernel.Tasks
{
    public class TaskLoadLevel : Task.Task
    {
        [Header("Level Handle Context")]
        [SerializeField]
        private List<LevelContextHandler> m_levelContextHandlers;

        protected override void DoStart()
        {
            base.DoStart();

            for (int i = 0; i < m_levelContextHandlers.Count - 1; ++i)
            {
                LevelContextHandler lch = m_levelContextHandlers[i];
                lch.SetSuccessor(m_levelContextHandlers[i + 1]);
            }
            m_levelContextHandlers[0].Handle(OnLevelContextHandlerComplete);
        }

        private void OnLevelContextHandlerComplete()
        {
            Complete("TaskLoadLevel::COMPLETED");
        }
    }
}

