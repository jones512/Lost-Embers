using UnityEngine;
using System.Collections;

namespace AdventureKit.Kernel.Tasks
{
    /// <summary>
    /// This tasks will ensure to hide the LoadingBlackScreen    
    /// </summary>
    public class TaskCloseLoading : AdventureKit.Task.Task
    {
        [Header("Wait before close")]
        [SerializeField]
        private bool m_WaitBeforeClose;

        protected override void DoStart()
        {
            base.DoStart();

            K.LoadingScreen.OnScreenCloseDone = () => Complete("TaskCloseLoading::Complete");

            if (!m_WaitBeforeClose)
                K.LoadingScreen.CloseScreen();
            else
                StartCoroutine("_WaitBeforeClose");       
        } 
        
        private IEnumerator _WaitBeforeClose()
        {
            yield return new WaitForSeconds(1f);
            K.LoadingScreen.CloseScreen();

        }  
    }
}


