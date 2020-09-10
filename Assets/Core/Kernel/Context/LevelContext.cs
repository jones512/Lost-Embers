using UnityEngine;
using System.Collections;

using AdventureKit.Config;

namespace AdventureKit.Kernel.Context
{
    public class LevelContext : BaseContext
    {
        private string mLevelName = "";
        public void SetLevelName(string level)
        {
            mLevelName = level;
        }
        public override void Enter()
        {
            Debug.Log("LevelContext::Enter()");
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
            Debug.Log("FirstWorldContext::Exit()");
            base.Exit();
        }
    }
}