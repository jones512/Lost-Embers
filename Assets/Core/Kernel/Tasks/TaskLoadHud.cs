using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AdventureKit.Kernel.Tasks
{
    public class TaskLoadHud : Task.Task
    {

        protected override void DoStart()
        {
            base.DoStart();

            StartCoroutine(_Execute());

        }

        private IEnumerator _Execute()
        {
            SceneManager.LoadScene(Config.AppConfig.WORLD_HUD_SCENE, LoadSceneMode.Additive);

            //while (K.GameManager.WorldHudController == null)
            //    yield return null;

            Complete("TaskLoadHud::Complete");
            yield return null;
        }
    }
}

