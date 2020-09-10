using UnityEngine;
using System.Collections;

namespace AdventureKit.Kernel.Tasks
{
    public class TaskHideLoadingScreen : Task.Task
    {
        [SerializeField]
        private float m_TimeOnScreen = 1f;

        protected override void DoStart()
        {
            Debug.Log("TaskHideLoadingScreen:Initialized");

            StartCoroutine("_HideLoadingScreen");
        }

        private IEnumerator _HideLoadingScreen()
        {
            yield return new WaitForSeconds(m_TimeOnScreen);

            K.DefaultLoadingScreen.Show(false);

            Complete("TaskHideLoadingScreen::Complete");
        }
    }

}
