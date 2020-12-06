using UnityEngine;
using System.Collections;

using AdventureKit.Config;

namespace AdventureKit.Kernel.Context
{
    public class LoreContext : BaseContext
    {
        private string mLevelName = AppConfig.LORE_LEVEL_SCENE;
        
        public override void Enter()
        {
            Debug.Log("LoreContext::Enter()");
            StartCoroutine(_WaitAndEnter());
        }

        IEnumerator _WaitAndEnter()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(mLevelName);

            yield return null;

            K.DefaultLoadingScreen.Show(false);
            SendInit();

            base.Enter();

        }

        public override void Exit()
        {
            Debug.Log("LoreContext::Exit()");
            base.Exit();
        }
    }
}