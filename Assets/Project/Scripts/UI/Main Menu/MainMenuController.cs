using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AdventureKit.Config;
using AdventureKit.Utils;

namespace AdventureKit.UI
{
    public class MainMenuController : AdventureKit.Utils.MonoBehaviour
    {
        [SerializeField]
        private Button m_NewGameButton;
        [SerializeField]
        private Button m_QuitGameButton;

        public void NewGame()
        {
            PlayerPrefs.DeleteAll();

            //K.SoundManager.PlayFXSound(K.SoundManager.GetSFXByName("click"));
            //K.SaveLoad.LoadDefaultData();
            K.EnterLevelContext(AppConfig.FIRST_LEVEL_SCENE);
        }

        public void Quit()
        {
            //K.GameManager.CurrentWorld = K.SaveLoad.World;
            //K.GameManager.CurrentLevel = K.SaveLoad.Level;

            Application.Quit();
        }
    }
}

