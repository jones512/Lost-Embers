using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using AdventureKit.UI.Core.OpenCloseFX;
using AdventureKit.ScreensManagement.BaseClaseAndUtils;

namespace AdventureKit.Kernel.Loading
{
    public class LoadingScreen : ScreenBase
    {

        [SerializeField]
        AlphaBlendOpenCloseFX m_alphaBlendFX;

        [SerializeField]
        Text m_loadingText;

        public void Init(string loadingText)
        {
            m_loadingText.text = loadingText;
        }

        public void OverrideFadeTimes(float fadeIn, float fadeOut)
        {
            m_alphaBlendFX.SetFadeTimes(fadeIn, fadeOut);
        }

        protected override void OnCloseFXFinished()
        {
            base.OnCloseFXFinished();
            gameObject.SetActive(false);
        }
    }
}


