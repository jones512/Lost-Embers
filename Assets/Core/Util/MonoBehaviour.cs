using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

namespace AdventureKit.Utils
{
    public class MonoBehaviour : UnityEngine.MonoBehaviour
    {   

        // Direct access to Kernel through K        
        protected Kernel.Kernel K {
            get { return Kernel.Kernel.Instance; }
        }

        public void DoFade(GameObject canvasGO, float to, float time, System.Action onAlphaBlendFinishedCallback = null)
        {
            CanvasGroup cg = canvasGO.GetComponentInChildren<CanvasGroup>();
            Assert.IsTrue(cg != null, "The provided GO " + canvasGO.name + " doesnt contains a CanvasGroup");
            DoFade(cg, to, time, onAlphaBlendFinishedCallback);
        }
        public void DoFade(CanvasGroup canvasGroup, float to, float time, System.Action onAlphaBlendFinishedCallback = null)
        {
            StartCoroutine(_DoFadeRoutine(canvasGroup, to, time, onAlphaBlendFinishedCallback));
        }

        IEnumerator _DoFadeRoutine(CanvasGroup canvasGroup, float to, float time, System.Action onAlphaBlendFinishedCallback)
        {
            float from = canvasGroup.alpha;
            float acumTime = 0f;
            while (acumTime < time)
            {
                acumTime += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(from, to, acumTime);
                yield return null;
            }

            canvasGroup.alpha = to;
            if (onAlphaBlendFinishedCallback != null)
                onAlphaBlendFinishedCallback();

            onAlphaBlendFinishedCallback = null;
            yield return null;
        }

    }

}
