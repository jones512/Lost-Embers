using UnityEngine;

using AdventureKit.ScreensManagement.BaseClaseAndUtils;

namespace AdventureKit.Kernel.Tasks
{
    public class TaskInitCanvas : Task.Task
    {       
        [SerializeField]
        ScreensManager m_ScreensManager;

        [SerializeField]
        GameObject m_CanvasGameObject;

        protected override void DoStart()
        {
            base.DoStart();
            if (m_ScreensManager != null)
            {
                m_ScreensManager.gameObject.SetActive(true);
                m_ScreensManager.Init();
            }

            m_CanvasGameObject.SetActive(true);

            Complete("TaskInitCanvas::Complete");
        }        
    }
}


