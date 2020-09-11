using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AdventureKit.Config;
using AdventureKit.Utils;

namespace AdventureKit.UI
{
    public class LevelUIController : AdventureKit.Utils.MonoBehaviour
    {
        [SerializeField] private CanvasGroup m_PausePanelCanvasGroup;
        [SerializeField] private CanvasGroup m_LevelCompleteCanvasGroup;
        [SerializeField] private Button m_LevelCompleteContinueButton;
        [SerializeField] private CanvasGroup m_YouDiedCanvasGroup;
        [SerializeField] private Button m_YouDiedContinueButton;

        public void ShowPauseMenu()
        {
            m_PausePanelCanvasGroup.alpha = 1;
        }

        public void HidePauseMenu()
        {
            m_PausePanelCanvasGroup.alpha = 0;
        }

        public void ShowLevelCompletedMenu(System.Action onContinue)
        {
            m_LevelCompleteContinueButton.onClick.RemoveAllListeners();
            m_LevelCompleteContinueButton.onClick.AddListener(() => onContinue());
            m_LevelCompleteCanvasGroup.alpha = 1;
        }

        public void HideLevelCompletedMenu()
        {
            m_LevelCompleteCanvasGroup.alpha = 0;
        }

        public void ShowYouDiedMenu(System.Action onContinue)
        {
            m_YouDiedContinueButton.onClick.RemoveAllListeners();
            m_YouDiedContinueButton.onClick.AddListener(() => onContinue());
            m_YouDiedCanvasGroup.alpha = 1;
        }
    }
}