using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using AdventureKit.ScreensManagement.BaseClaseAndUtils;

namespace AdventureKit.Kernel.Loading
{
    public class DefaultLoadingScreen : ScreenBase
    {
        // TODO: define here the set of widgets required for any LoadingScreen
        // LoadingWidget: 
        // TipLabel: space to write tips 
        // ProgressionBar: bar to fill based on loading progression
        // Alpha Tweener: FadeIn/Out of the screen             
        [SerializeField]
        Text m_loadingText;

        public bool CloseningFinished { get; private set; }
        public bool OpeningFinished { get; private set; }
        public bool Loading { get; private set; }

        public void Show(bool show, string message = null)
        {
            Loading = show;
            gameObject.SetActive(show);
            m_loadingText.text = message;
            //Debug.Log("Show:" + show + " loading: " + message);
        }

        public void UpdateMessage(string message)
        {
            m_loadingText.text = message;
            Debug.Log("Loading message updated to: " + message);
        }

        public void PlayClosening()
        {
            CloseningFinished = false;
            Invoke("Close", 0.15f);

            // TODO: implement closening: execute closening FX
            //CloseningFinished = true;
        }

        void Close()
        {
            CloseningFinished = true;
        }

        public void UpdateProgress(float progress)
        {
            Debug.Log(this.Log("Loading: " + (progress * 100f) + "%"));
        }

        public void PlayOpening()
        {
            // TODO: implement opening: execute opening FX
            //OpeningFinished = true;

            OpeningFinished = false;
            Invoke("Open", 0.15f);
        }

        void Open()
        {
            OpeningFinished = true;
        }
    }
}