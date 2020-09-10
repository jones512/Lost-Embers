using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;

namespace AdventureKit.Kernel
{
    public class SaveLoad : Utils.MonoBehaviour
    {
        public enum DataId {  World, Level, Gender, Audio }

        private const string sWorld = "World";
        private const string sLevel = "Level";
        private const string sGender = "Gender";

        private const string sAudio = "Audio";

        public int World { get { return PlayerPrefs.GetInt(DataId.World.ToString());  } }
        public int Level { get { return PlayerPrefs.GetInt(DataId.Level.ToString()); } }
        public int Gender { get { return PlayerPrefs.GetInt(DataId.Gender.ToString()); } }
        public bool LoadedDefaultData { get { return PlayerPrefs.GetInt("LoadedDefaultData") == 0 ? false : true; } }
        public bool LoadedDefaultConfigData { get { return PlayerPrefs.GetInt("LoadedDefaultConfigData") == 0 ? false : true; } }

        public bool Audio { get { return PlayerPrefs.GetInt(DataId.Audio.ToString()) == 0 ? false : true; } }

        public void LoadDefaultData()
        {
            PlayerPrefs.SetInt("LoadedDefaultData", 1);

            SaveCommonData(DataId.World, 0);
            K.GameManager.CurrentWorld = 0;

            SaveCommonData(DataId.World, 0);
            K.GameManager.CurrentLevel = 0;

            SaveWorldUnlocked(0, 1);
            SaveLevelUnlocked(0, 0, 1);

            for (int i=0 ; i < K.GameManager.MAX_WORLDS; i++)
            {
                SaveWorldCompleted(i, 0);
                for (int j=0; j < K.GameManager.MAX_LEVELS; j++)
                {
                    SaveLevelCompleted(i, j, 0);
                }
            }
        }

        public void SaveCommonData(DataId id, int value)
        {
            PlayerPrefs.SetInt(id.ToString(), value);
        }

        public void SaveWorldUnlocked(int world, int value)
        {
            Debug.Log(DataId.World.ToString() + "_" + world + "_locked:" + value);
            PlayerPrefs.SetInt(DataId.World.ToString() + "_" + world + "_locked", value);
        }

        public void SaveWorldCompleted(int world, int value)
        {
            Debug.Log(DataId.World.ToString() + "_" + world + ": " + value);
            PlayerPrefs.SetInt(DataId.World.ToString() + "_" + world, value);

        }

        public bool CheckWorldCompleted(int world)
        {
            return PlayerPrefs.GetInt(DataId.World.ToString() + "_" + world) == 0 ? false : true;
        }

        public bool CheckWordlUnlocked(int world)
        {
            return PlayerPrefs.GetInt(DataId.World.ToString() + "_" + world + "_locked") == 0 ? false : true;
        }

        public void SaveLevelUnlocked(int world, int level, int value)
        {
            Debug.Log("Set " + DataId.Level.ToString() + "_" + world + "_" + level + " to " + (value == 0 ? "locked" : "unlocked"));
            PlayerPrefs.SetInt(DataId.Level.ToString() + "_" + world + "_" + level + "_locked", value);
        }

        public void SaveLevelCompleted(int world, int level, int value)
        {
            Debug.Log("Set " + DataId.Level.ToString() + "_" + world + "_" + level + " to " + value);
            PlayerPrefs.SetInt(DataId.Level.ToString() + "_" + world + "_" + level, value);
        }

        public bool CheckLevelCompleted(int world, int level)
        {
            return PlayerPrefs.GetInt(DataId.Level.ToString() + "_" + world + "_" + level) == 0 ? false : true;
        }

        public bool CheckLevelUnlocked(int world, int level)
        {
            return PlayerPrefs.GetInt(DataId.Level.ToString() + "_" + world + "_" + level + "_locked") == 0 ? false : true;
        }

#if UNITY_EDITOR
        [UnityEditor.MenuItem("MyTools/Clean PlayerPrefs")]
        public static void CleanSaveGames()
        {
            PlayerPrefs.DeleteAll();
        }
#endif

    }
}

