using UnityEngine;
using AdventureKit.ScreensManagement.BaseClaseAndUtils;

namespace AdventureKit.UI.Core.Screens
{
    public class BasicTransitionScreen : ScreenBase
    {
        System.Action OnPreClose;     

        [SerializeField]
        protected float m_timeOnScreen;

        [SerializeField]
        protected bool m_AutoComplete = true;

        public virtual void Init(float timeOnScreen)
        {
            m_timeOnScreen = timeOnScreen;            
        }          
                

        protected override void OnOpenFXFinished()
        {
            base.OnOpenFXFinished();
            if(m_AutoComplete)
                Invoke("CloseScreen", m_timeOnScreen);
        }

        protected override void OnCloseFXFinished()
        {
            base.OnCloseFXFinished();
            gameObject.SetActive(false);
        }
    }

}


