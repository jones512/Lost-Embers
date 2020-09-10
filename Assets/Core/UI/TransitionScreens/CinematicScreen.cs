using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using AdventureKit.ScreensManagement.BaseClaseAndUtils;

namespace AdventureKit.UI.TransitionScreens
{
    public class CinematicScreen : ScreenBase
    {
        [SerializeField]
        protected Animator m_animator;

        [SerializeField]
        Text m_text; // TODO: text writter

        [SerializeField]
        CanvasGroup m_continueBtnCanvas;

        public System.Action OnContinueCallback;        

        protected bool mAlreadySkipped;

        public virtual void Init(string text)
        {                        
            m_text.text = text;
        }                

        public virtual void Skip()
        {
            if(!mAlreadySkipped)
            {
                mAlreadySkipped = true;

                // Show Continue button
                ShowContinueButton();

                // TODO: textWritter.SpeedUp            
            }
        }        

        protected void ShowContinueButton()
        {
            if (!mAlreadySkipped)            
                K.DoFade(m_continueBtnCanvas, 1, 1, () => { m_continueBtnCanvas.interactable = true; });
        }

        public void DoContinue()
        {
            m_animator.SetTrigger("Continue");
            if (OnContinueCallback != null)
                OnContinueCallback();
            StartCoroutine(_WaitForFadeOut());                          
        }

        IEnumerator _WaitForFadeOut()
        {
            yield return new WaitForSeconds(0.1f);
            while (m_animator.GetCurrentAnimatorStateInfo(m_animator.GetLayerIndex("Fades")).normalizedTime < 1f)
                yield return new WaitForSeconds(0.1f);

            CloseScreen();            
        }

        protected override void OnOpenFXFinished()
        {
            base.OnOpenFXFinished();

            m_animator.enabled = true;
            Invoke("ShowContinueButton", 2f);
            // TODO: textWritter.Write( onFinish( ShowContinueButton() );
            // TODO:
        }

        /// <summary>
        /// Called automatically onces the screen close FX has finished.
        /// It will destroy current content and deactivate the blackscreen
        /// </summary>
        protected override void OnCloseFXFinished()
        {            
            base.OnCloseFXFinished();            
            DestroyImmediate(gameObject);            
        }
    }
}
