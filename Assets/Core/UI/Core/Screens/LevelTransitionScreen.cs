using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace AdventureKit.UI.Core.Screens
{
    public class LevelTransitionScreen : BasicTransitionScreen
    {
        override public void Init(float timeOnScreen)
        {
            base.Init(timeOnScreen);
        }

        protected override void OnOpenFXFinished()
        {
            base.OnOpenFXFinished();
        }

        protected override void OnCloseFXFinished()
        {
            base.OnCloseFXFinished();
            gameObject.SetActive(false);
        }
    }
}
