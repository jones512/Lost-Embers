using System;

namespace AdventureKit.UI.Core.OpenCloseFX
{
    public class DefaultOpenCloseFX : Utils.MonoBehaviour, OpenCloseFX.IOpenCloserFX
    {
        // Callbacks to call once the open/close FX is done
        Action OnOpenFXFinished;
        Action OnCloseFXFinished;

        public void DoOpenFX(Action onOpenFXFinished)
        {
            // Register callback
            OnOpenFXFinished = onOpenFXFinished;

            // Do Open FX
            gameObject.SetActive(true);

            // Done
            TriggerOpenFinishedEvent();
        }

        public void DoCloseFX(Action onCloseFXFinished)
        {
            // Register callback
            OnCloseFXFinished = onCloseFXFinished;

            // Do Close FX
            gameObject.SetActive(false);

            // Done
            TriggerCloseFinishedEvent();
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
            OnCloseFXFinished = null;
        }
    }

}

