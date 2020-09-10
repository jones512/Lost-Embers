using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AdventureKit.Kernel.Tasks
{
    public class TaskLoadProductionScene : Task.Task
    {
        [SerializeField]
        string m_ScenePath = "Scenes/Production/";
        [SerializeField]
        Transform m_ParentTransform;
        [SerializeField]
        bool m_ActivateRootGameObject = false;

        private Scene mMainScene;

        protected override void DoStart()
        {
            base.DoStart();
            mMainScene = SceneManager.GetActiveScene();
            StartCoroutine(LoadScene(m_ScenePath));
        }

        private IEnumerator LoadScene(string sceneName)
        {
            AsyncOperation loadSceneOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            while (loadSceneOperation.isDone == false)
                yield return new WaitForEndOfFrame();

            Scene scene = SceneManager.GetSceneByName(sceneName);

            GameObject[] vRootGameObjects = scene.GetRootGameObjects();
            Debug.Log(vRootGameObjects.Length);
            for (int i = 0; i < vRootGameObjects.Length; i++)
            {
                vRootGameObjects[i].transform.SetParent(m_ParentTransform);
                vRootGameObjects[i].SetActive(m_ActivateRootGameObject);
            }

            Complete("TaskLoadProductionScene::Complete");
        }
    }
}
