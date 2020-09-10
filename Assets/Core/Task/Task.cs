using UnityEngine;
using System.Collections;

using AdventureKit.Config;

namespace AdventureKit.Task
{
    public class Task : Utils.MonoBehaviour
    {
        // Dependencias
        [SerializeField]
        Task[] m_dependencies;

        // Do other tasks needs to wait for this one to complete?
        [SerializeField]
        public bool WaitForCompletion = true;

        // Esta tarea está compeltada
        public bool Completed { get; protected set; }
        
        // Progreso, en caso de que tenga
        public float ValueMaxProgress { get; set; }
        public float ValueProgress { get; set; }        

        
        // Use this for initialization
        public virtual void WaitDependencies()
        {
            StartCoroutine(_WaitDependencies());
        }

        // Espera a que las dependencias se cumplan
        IEnumerator _WaitDependencies()
        {
            bool foundActiveTask;
            while (true)
            {
                yield return new WaitForSeconds(AppConfig.CHECK_COMPLETED_TASK_INTERVAL);
                foundActiveTask = false;

                for (int i = 0; i < m_dependencies.Length; i++)
                {
                    Task t = m_dependencies[i];
                    if (!t.Completed)
                    {
                        foundActiveTask = true;
                        break;
                    }
                }
                if (!foundActiveTask)
                {
                    break;
                }
            }

            Debug.Log("Starting task " + name);
            DoStart();
        }

        // Cuando se cumplen los requisitos se cede el control a DoStart()
        protected virtual void DoStart() { }

        protected virtual void DoOnComplete() { }

        // Marca la tarea como completada
        public void Complete(string message)
        {
            Debug.Log(message);
            Completed = true;
            DoOnComplete();
        }
    }
}
