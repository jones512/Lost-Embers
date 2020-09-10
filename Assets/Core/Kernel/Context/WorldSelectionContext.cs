using UnityEngine;
using System.Collections;

using AdventureKit.Config;

namespace AdventureKit.Kernel.Context
{
    public class WorldSelectionContext : BaseContext
    {

        public override void Enter()
        {
            Debug.Log("WorldSelectionContext::Enter()");
            StartCoroutine("_WaitAndEnter");
        }

        IEnumerator _WaitAndEnter()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(AppConfig.WORLD_SELECTION_SCENE);

            yield return null;

            K.DefaultLoadingScreen.Show(false);
            SendInit();

            base.Enter();

        }

        public override void Exit()
        {
            Debug.Log("MainMenuContext::Exit()");
            base.Exit();
        }

    }
}