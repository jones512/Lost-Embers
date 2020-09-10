using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

namespace AdventureKit.Kernel.Tasks
{
    public class TaskLaunchDialogue : Task.Task
    {
        [SerializeField]
        private string m_DialogueId;

        protected override void DoStart()
        {
            base.DoStart();


            StartCoroutine(_ExecuteTask());
        }

        private IEnumerator _ExecuteTask()
        {
            DialogueManager.StartConversation(m_DialogueId);

            while (DialogueManager.IsConversationActive)
                yield return null;

            Complete("TaskLaunchDialogue::Completed");
        }
    }
}

