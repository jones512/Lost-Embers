using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AdventureKit.Task
{
    public class TaskDependencyManager : Utils.MonoBehaviour
    {
        [SerializeField]
        protected GameObject m_taskContainer;

        [SerializeField]
        protected bool m_autoStart = true;

        protected List<Task> mTasks = new List<Task>();

        bool mAlreadyStarted;

        public bool BootCompleted { get; protected set; }

        // Use this for initialization
        void Start()
        {
            Debug.Log(this.Log("start task dependency manager"));
            if (m_autoStart)
                StartTasks();
        }

        void OnDestroy()
        {

            Debug.Log(this.Log("on destroy taskdependency manager " + this.gameObject.name));
            StopCoroutine("_WaitBootCompleted");

            if (mTasks != null)
            {
                foreach (Task task in mTasks)
                {
                    task.Complete("");
                }
                mTasks.Clear();
            }

            m_taskContainer = null;

            mAlreadyStarted = false;
        }


        public void StartTasks()
        {
            if (mAlreadyStarted)
                return;

            mAlreadyStarted = true;

            Debug.Log(this.Log("StartingTasks"));

            // Recoge las tareas de los hijos
            if (m_taskContainer != null)
            {
                foreach (Transform child in m_taskContainer.transform)
                {
                    Task t = child.GetComponent<Task>();
                    if (t != null)
                    {
                        Tester.ASSERT(t != null, "Task object not containing a task");
                        mTasks.Add(t);
                    }

                }
            }
            // Inicializa todas las tareas			
            foreach (Task task in mTasks)
            {
                task.WaitDependencies();
            }

            // Espera a que las tareas boot terminen
            StartCoroutine("_WaitBootCompleted");
        }

        // Espera a que las tareas boot hayan terminado
        IEnumerator _WaitBootCompleted()
        {
            while (true)
            {
                bool tasksRunning = false;
                yield return new WaitForSeconds(AdventureKit.Config.AppConfig.CHECK_COMPLETED_TASK_INTERVAL);
                foreach (Task t in mTasks)
                {
                    if (t.WaitForCompletion && !t.Completed)
                    {
                        //Debug.Log (t.name+" running....");
                        tasksRunning = true;
                    }
                }
                if (!tasksRunning)
                    break;
            }

            Debug.Log(this.Log("Taskdependency manager completed all tasks= " + this.gameObject.name));
            BootCompleted = true;
            mAlreadyStarted = false;
            mTasks.Clear();
            OnBootTasksCompleted();
        }

        // Devuelve el progreso con un valor entre 0 y 1
        public float GetBootProgress()
        {
            float actual = 0f, max = 0f;
            foreach (Task t in mTasks)
            {
                if (t.WaitForCompletion)
                {
                    actual += t.ValueProgress;
                    max += t.ValueMaxProgress;
                }
            }

            return actual / max;
        }

        // Para heredar, se la llama cuando las tareas boot han terminado
        protected virtual void OnBootTasksCompleted() { }

    }
}

