//using UnityEngine;
//using System.Collections.Generic;


//namespace AdventureKit.Kernel
//{
//    interface ISavable
//    {
//        void Save();
//        void Load();
//    }

//    public static class GameDataFlags
//    {
//        public const string GAME_INTRO_FLAG = "GameIntroCinematic";
//    }

//    [System.Serializable]
//    public class GameData
//    {
//        public PlayerData PlayerData;

//        //Lists of Dialogues Data
//        public List<DialogueData> DialoguesData;

//        //Lists of Levels Data
//        public List<LevelData> LevelsData;

//        //Lists of Items Daata
//        public List<ItemData.ItemsProvider> ItemsData;

//        // Any existing flag is true, not existing is false
//        public List<string> Flags;

//        //Constructor
//        public GameData()
//        {

//            DialoguesData = new List<DialogueData>();
//            LevelsData = new List<LevelData>();
//            ItemsData = new List<ItemData.ItemsProvider>();

//            Flags = new List<string>();
//        }

//        public void SetFlag(string flag)
//        {
//            if (!ContainsFlag(flag))
//                Flags.Add(flag);
//        }

//        public bool ContainsFlag(string flag)
//        {
//            return Flags.Contains(flag);
//        }

//        public static GameData LoadDefaultGameData()
//        {
//            // Load PlayerData from defaults
//            TextAsset txt = Resources.Load("Data/PlayerData", typeof(TextAsset)) as TextAsset;

//            Debug.Log(txt.text);

//            // Create the GameData instance we will fill
//            GameData gdInstance = new GameData();

//            //covert the string read from disk into a class
//            gdInstance.PlayerData = JsonUtility.FromJson<PlayerData>(txt.text);

//            //Get dialogues data -> Should be move to a dialogues manager
//            for (int i = 0; i < Resources.LoadAll<TextAsset>("Data/Dialogues/").Length; i++)
//            {
//                TextAsset asset = Resources.LoadAll<TextAsset>("Data/Dialogues/")[i];

//                DialogueData dialogueData = JsonUtility.FromJson<DialogueData>(asset.text);
//                gdInstance.DialoguesData.Add(dialogueData);

//                Debug.Log("LOADED DIALOGUE: " + dialogueData.id);
//            }

//            //Get levels data
//            for (int i = 0; i < Resources.LoadAll<TextAsset>("Data/Levels/").Length; i++)
//            {
//                TextAsset asset = Resources.LoadAll<TextAsset>("Data/Levels/")[i];

//                LevelData levelData = JsonUtility.FromJson<LevelData>(asset.text);
                
//                gdInstance.LevelsData.Add(levelData);

//                Debug.Log("LOADED LEVEL: " + levelData.temple_id);
//            }

//            //Get items data
//            for (int i = 0; i < Resources.LoadAll<TextAsset>("Data/Items/").Length; i++)
//            {
//                TextAsset asset = Resources.LoadAll<TextAsset>("Data/Items/")[i];

//                ItemData.ItemsProvider items = JsonUtility.FromJson<ItemData.ItemsProvider>(asset.text);

//                gdInstance.ItemsData.Add(items);

//                Debug.Log("LOADED ITEMS DATA");
//            }

//            return gdInstance;
//        }

//        //Return a dialogue data -> Should be move to a dialogues manager
//        public DialogueData GetDialogue(string id)
//        {
//            for(int i=0; i<DialoguesData.Count; i++)
//            {
//                if (DialoguesData[i].id.Equals(id))
//                    return DialoguesData[i];
//            }

//            return null;
//        }

//        public ItemData GetItem(string id)
//        {
//            ItemData.ItemsProvider itemsList = ItemsData[0];

//            for (int i = 0; i < itemsList.items.Count; i++)
//            {
//                if (itemsList.items[i].id.Equals(id))
//                    return itemsList.items[i];
//            }

//            return null;
//        }
//    }
//}

