#define PRINT_DEBUG

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace AdventureKit.Utils
{
    public enum DebugLogColor { LevelController };
    public class DebugHandler : MonoBehaviour
    {
        public static DebugHandler instance;

        // Start is called before the first frame update
        void Start()
        {
            instance = this;
        }

        public void ColoredDebugLog(DebugLogColor logType, string message)
        {
            switch (logType)
            {
                case DebugLogColor.LevelController:
                   
                    Debug.Log("<color=cyan>Level Controller: </color>" + message);
                    break;
            }
        }
    }
}