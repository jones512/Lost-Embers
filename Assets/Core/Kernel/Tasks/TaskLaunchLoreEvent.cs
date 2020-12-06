using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace AdventureKit.Kernel.Tasks
{
    /// <summary>
    /// This tasks will ensure to hide the LoadingBlackScreen    
    /// </summary>
    public class TaskLaunchLoreEvent : AdventureKit.Task.Task
    {
        [SerializeField]
        private Transform m_PlayerCharacter;

        protected override void DoStart()
        {
            base.DoStart();

            Tween moveTween = m_PlayerCharacter.DOMove(new Vector3(0, 0.91f, 0), 3f).OnComplete(() => {

                //TODO: Launch Dialogue
                Complete("TaskLaunchLoreEvent::Complete");

            });
            moveTween.Play();
        }
    }
}


