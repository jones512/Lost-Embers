using UnityEngine;
using System;

namespace AdventureKit.UI.Core.OpenCloseFX
{
    public class TweenedOpenCloseFX : Utils.MonoBehaviour, IOpenCloserFX
    {
        public enum TweenType { Scale, Move}

        [SerializeField]
        TweenType m_tweenType = TweenType.Scale;

        [SerializeField]
        float m_openTime = 0.5f;

        [SerializeField]
        float m_closeTime = 0.5f;

        [SerializeField]
        float m_onOpenMoveTo;
        [SerializeField]
        float m_onCloseMoveTo;

        [SerializeField]
        LeanTweenType m_easeType;

        // Callbacks to call once the open/close FX is done
        Action OnOpenFXFinished;
        Action OnCloseFXFinished;

        public void DoOpenFX(Action onOpenFXFinished)
        {
            OnOpenFXFinished = onOpenFXFinished;
            
            gameObject.SetActive(true);

            switch(m_tweenType)
            {
                case TweenType.Scale:
                    LeanTween.scale(gameObject, Vector3.one, m_openTime).setEase(LeanTweenType.easeInOutSine).setOnComplete(TriggerOpenFinishedEvent);
                    break;
                case TweenType.Move:
                    LeanTween.moveLocalX(gameObject, m_onOpenMoveTo, m_openTime).setEase(LeanTweenType.easeInOutQuad).setOnComplete(TriggerOpenFinishedEvent);
                    break;
            }                        
        }

        public void DoCloseFX(Action onCloseFXFinished)
        {
            OnCloseFXFinished = onCloseFXFinished;

            switch (m_tweenType)
            {
                case TweenType.Scale:
                    LeanTween.scale(gameObject, Vector3.zero, m_closeTime).setEase(LeanTweenType.easeInOutSine).setOnComplete(TriggerCloseFinishedEvent);
                    break;
                case TweenType.Move:
                    LeanTween.moveLocalX(gameObject,m_onCloseMoveTo, m_closeTime).setEase(LeanTweenType.easeInOutQuad).setOnComplete(TriggerCloseFinishedEvent);
                    break;
            }
            
            // TODO: SoundManager.Instance.PlayFXSound(ButtonSound, fxVolume, false);
        }

        public void TriggerOpenFinishedEvent()
        {
            if (OnOpenFXFinished != null)
                OnOpenFXFinished();

            OnOpenFXFinished = null;            
        }

        public void TriggerCloseFinishedEvent()
        {
            if (OnCloseFXFinished != null)
                OnCloseFXFinished();

            gameObject.SetActive(false);

            OnCloseFXFinished = null;
        }        
    }
}


