using UnityEngine;
using System.Collections;

using AdventureKit.Config;

namespace AdventureKit.Kernel.Context
{
	public class MainContext : BaseContext {	        
        
		public override void Enter ()
        {
			Debug.Log ("MainContext::Enter()"); 
			StartCoroutine ("_WaitAndEnter");
		}

		IEnumerator _WaitAndEnter()
        {
			UnityEngine.SceneManagement.SceneManager.LoadScene(AppConfig.MAIN_SCENE);

			yield return null;

            K.DefaultLoadingScreen.Show(false);
			SendInit();

            base.Enter();

        }

        public override void Exit()
        {
			Debug.Log ("StartContext::Exit()");
			base.Exit ();
		}
		
	}
}