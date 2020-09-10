using UnityEngine;
using System;

namespace AdventureKit.UI.Core.OpenCloseFX
{    
    public class AlphaBlendOpenCloseFX : Utils.MonoBehaviour, IOpenCloserFX
    {
        [SerializeField]
        CanvasGroup m_canvasGroup;

        [SerializeField]
        float m_fadeInTime = 1f;

        [SerializeField]
        float m_fadeOutTime = 1f;      
        
        public void SetFadeTimes(float fadeIn, float fadeOut)
        {
            m_fadeInTime = fadeIn;
            m_fadeOutTime = fadeOut;
        }

        public void DoOpenFX(Action onOpenFXFinished)
        {
            gameObject.SetActive(true);
            m_canvasGroup.alpha = 0f;
            DoFade(m_canvasGroup, 1f, m_fadeInTime, onOpenFXFinished);                 
        }

        public void DoCloseFX(Action onCloseFXFinished)
        {
            // early out if object is already inactive
            if (!gameObject.activeInHierarchy)
            {
                if (onCloseFXFinished != null)
                    onCloseFXFinished();
                return;
            }

            DoFade(m_canvasGroup, 0f, m_fadeOutTime, onCloseFXFinished);            
        }
    }
}

