using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace AdventureKit.Common
{
    public class GameManager : Utils.MonoBehaviour
    {
        public enum Gender { Male, Female }
        public Gender PlayerGender { get; set; }

        public int CurrentWorld { get; set; }
        public int CurrentLevel = 0;

        public Player Player { get; set; }

        public int MAX_WORLDS = 3;
        public int MAX_LEVELS = 8;


        public void Init()
        {
            gameObject.SetActive(true);

            SetDefaultConfig();
        }

        private void SetDefaultConfig()
        {
            if(K.SaveLoad.LoadedDefaultConfigData)
            {
                if (K.SaveLoad.Gender > 0)
                    PlayerGender = Gender.Female;
                else
                    PlayerGender = Gender.Male;

                if (K.SaveLoad.Audio == false)
                    K.SoundManager.MuteAudioSource();
                else
                    K.SoundManager.ResumeAudioSource();

            }
            else
            {
                PlayerGender = Gender.Male;
                K.SaveLoad.SaveCommonData(Kernel.SaveLoad.DataId.Gender, 0);
                K.SoundManager.ResumeAudioSource();
                K.SaveLoad.SaveCommonData(Kernel.SaveLoad.DataId.Audio, 1);

                PlayerPrefs.SetInt("LoadedDefaultConfigData", 1);
            }
           
        }
    }
}