#if !UNITY_EDITOR
#define DEBUG_LOG_WRAP
#endif

using System;
using UnityEngine;
   
/*
public static class Debug
{
    
#if DEBUG_LOG_WRAP
    static public void Break ()
    {
        if (IsDebugBuild ())
        {
            UnityEngine.Debug.Break ();
        }
    }
 
    static public void Log (object message)
    {        
        if (IsDebugBuild ()) {
            UnityEngine.Debug.Log ( message);
        }
    }
 
    static public void Log (object message, UnityEngine.Object context)
    {
        if (IsDebugBuild ()) {
            UnityEngine.Debug.Log (message, context);
        }
    }
 
    static public void LogWarning (object message)
    {
        if (IsDebugBuild ()) {
            UnityEngine.Debug.LogWarning (message);
        }
    }
 
    static public void LogWarning (object message, UnityEngine.Object context)
    {
        if (IsDebugBuild ()) {
            UnityEngine.Debug.LogWarning (message, context);
        }
    }
 
    static public void LogError (object message)
    {
        if (IsDebugBuild ()) {
            UnityEngine.Debug.LogError (message);
        }
    }

    static public void LogError(object message, UnityEngine.Object context)
    {
        if (IsDebugBuild ()) {
            UnityEngine.Debug.LogError (message, context);
        }
    }
 
    static public void DrawLine (Vector3 start, Vector3 end, Color color, float duration = 0.0F, bool depthTest = true)
    {
        UnityEngine.Debug.DrawLine(start, end, color, duration, depthTest);
    }
 
    static bool IsDebugBuild ()
    {
        return UnityEngine.Debug.isDebugBuild;
    }

#endif


    static public void ASSERT(bool test, string message)
    {
        if (!test)
            throw new Exception(message);
    }
}
 * */
 
public static class GameObjectExtensions
{
    public static string Log(this MonoBehaviour mb, object messageString)
    {
        return string.Format("[{0}] {1}::{3}\n{2}\n",
            Time.frameCount,
            mb.name,
            mb.GetType().ToString(),
            messageString.ToString());        
    }
    public static string Log(this object mb, object messageString=null)
    {
        string objName = mb.ToString(); 
        return string.Format("[{0}] {1}::{2}\n{3}\n",
            Time.frameCount,
            objName.Contains("+")? objName.Split('+')[1] : objName, //.Split('.')[objName.LastIndexOf(".")+1],
            messageString.ToString(),
            mb.GetType().ToString());                
    }    

    //public static sunburned.kernel.Kernel GetKernel(this object o)
    //{
    //    return sunburned.kernel.Kernel.Instance;
    //}
}
public static class Tester
{
    public static void ASSERT(bool test, string message)
    {
        if (!test)
            throw new Exception(message);
    }
}


