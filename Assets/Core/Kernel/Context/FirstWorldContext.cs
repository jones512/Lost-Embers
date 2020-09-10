using UnityEngine;
using System.Collections;

using AdventureKit.Config;

namespace AdventureKit.Kernel.Context
{
    public class FirstWorldContext : BaseContext
    {

        public override void Enter()
        {
            Debug.Log("Level" +
                "::Enter()");
            StartCoroutine("_WaitAndEnter");
        }

        IEnumerator _WaitAndEnter()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(AppConfig.FIRST_LEVEL_SCENE);

            yield return null;

            K.DefaultLoadingScreen.Show(false);
            SendInit();

            base.Enter();

        }

        public override void Exit()
        {
            Debug.Log("FirstWorldContext::Exit()");
            base.Exit();
        }
    }
}