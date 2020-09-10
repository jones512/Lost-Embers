using UnityEngine;
using System;

using AdventureKit.Task;

namespace AdventureKit.ContextHandlers
{
    public abstract class LevelContextHandler : TaskDependencyManager
    {
        Action OnLevelContextHandlingComplete;

        LevelContextHandler mSuccessor;

        public void SetSuccessor(LevelContextHandler successor)
        {
            mSuccessor = successor;
        }

        public void Handle(Action onCompleteCallback)
        {
            if (CheckCanHandleContext())
            {
                DoHandle(onCompleteCallback);
            }
            else
            {
                if (mSuccessor != null)
                    mSuccessor.Handle(onCompleteCallback);
                else
                {
                    Debug.LogError("No one knows how to handle this situation");
                    onCompleteCallback();
                }
            }
        }

        protected abstract bool CheckCanHandleContext();

        void DoHandle(Action onCompleteCallback)
        {
            gameObject.SetActive(true);
            OnLevelContextHandlingComplete = onCompleteCallback;
            StartTasks();
        }
    }
}    
