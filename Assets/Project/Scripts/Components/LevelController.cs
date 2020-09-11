using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using AdventureKit.Utils;
using AdventureKit.UI;

public class LevelController : AdventureKit.Utils.SingletonMonoBehaviour<LevelController>
{
    [SerializeField]private int m_GlobetsToCompleteLevel = 3;
    [SerializeField] private PlayerController m_PlayerController;
    [SerializeField] private DebugHandler m_DebugHandler;
    [SerializeField] private LevelUIController m_UIController;

    [SerializeField] private string m_SceneLevel;
    [SerializeField] private string m_NextSceneLevel;
    [SerializeField] private bool m_IsLastLevel;
    [SerializeField]
    private AudioClip m_SoundClip;


    private int mGlobetsCount;
    private bool mGamePaused = false;

    private void Awake()
    {
        InitLevel();
    }

    private void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            if (!mGamePaused)
                PauseLevel();
            else
                ResumeLevel();
        }
    }

    public void RegisterGlobet(string id)
    {
        mGlobetsCount += 1;
        m_DebugHandler.ColoredDebugLog(DebugLogColor.LevelController, "registered globet " + id);
    }
    public void InitLevel()
    {
        K.SoundManager.PlayMusic(m_SoundClip, true);
        StartCoroutine(_CheckLevelCompleted());
        m_DebugHandler.ColoredDebugLog(DebugLogColor.LevelController, "initialized");
    }

    private IEnumerator _CheckLevelCompleted()
    {
        while (mGlobetsCount != m_GlobetsToCompleteLevel)
            yield return new WaitForEndOfFrame();

        CompleteLevel(false);
    }

    public void PauseLevel()
    {
        m_DebugHandler.ColoredDebugLog(DebugLogColor.LevelController, "paused");
        mGamePaused = true;
        StopCoroutine(_CheckLevelCompleted());
        m_PlayerController.DisableInput();
        m_UIController.ShowPauseMenu();

    }

    public void ResumeLevel()
    {
        m_DebugHandler.ColoredDebugLog(DebugLogColor.LevelController, "resumed");
        m_UIController.HidePauseMenu();
        mGamePaused = false;
        m_PlayerController.EnableInput();
        StartCoroutine(_CheckLevelCompleted());
    }

    public void CompleteLevel(bool died)
    {
        
        StopCoroutine(_CheckLevelCompleted());
        m_PlayerController.DisableInput();
        K.SoundManager.StopMusic();
        K.SoundManager.PlayFXSound(died ? K.SoundManager.GetSFXByName("you_died_1") : K.SoundManager.GetSFXByName("level_complete_1"));
        if (!died)
            m_UIController.ShowLevelCompletedMenu(GoToNextLevel);
        else
            m_UIController.ShowYouDiedMenu(Retry);
    }

    private void GoToNextLevel()
    {
        K.EnterLevelContext(m_NextSceneLevel);
    }

    private void Retry()
    {
        K.EnterLevelContext(m_SceneLevel);
    }
}
