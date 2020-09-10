using UnityEngine;
using System.Collections.Generic;
using System;

using AdventureKit.UI.TransitionScreens;
using AdventureKit.UI.Core.Screens;

namespace AdventureKit.ScreensManagement.BaseClaseAndUtils
{
    public class ScreensManager : Utils.MonoBehaviour
    {
        [SerializeField]
        protected List<ScreenBase> m_screens;

        public EventHandler mScreenManagerInitialized;

        public virtual void Init()
        {
            K.ScreenManager = this;

            foreach (var screen in m_screens)
                screen.gameObject.SetActive(false);

            if (mScreenManagerInitialized != null)
                mScreenManagerInitialized(this, new ScreenManagerInitializedEventArgs(m_screens));
        }

        public void AddScreenToScreensList(ScreenBase screen)
        {
            if (!m_screens.Contains(screen))
                m_screens.Add(screen);
        }

        public bool ScreensListContainsScreen(ScreenBase screen)
        {
            if (m_screens.Contains(screen))
                return true;

            return false;
        }

        public void TransitionScreens(ScreenBase screenToClose, ScreenBase screenToOpen)
        {
            screenToClose.OnScreenCloseDone = () => { screenToOpen.OpenScreen(); };
            screenToClose.CloseScreen();
        }


        /// <summary>
        /// Retreive a screen of the specified Type T
        /// </summary>        
        public T GetScreen<T>() where T : ScreenBase
        {
            for (int i = 0; i < m_screens.Count; i++)
            {
                ScreenBase screen = m_screens[i];
                if (screen is T)
                {
                    return screen as T;
                }
            }

            return default(T);
        }

        /// <summary>
        /// Retrieve a screen by name of the specified Type
        /// </summary>        
        public T GetScreen<T>(string screenName) where T : ScreenBase
        {
            for (int i = 0; i < m_screens.Count; i++)
            {
                ScreenBase screen = m_screens[i];
                if (screen is T && screen.Id == screenName)
                {
                    return screen as T;
                }
            }

            return default(T);
        }


        public T GetScreenCinematic<T>(string screenName) where T : CinematicScreen
        {
            for (int i = 0; i < m_screens.Count; i++)
            {
                ScreenBase screen = m_screens[i];
                if (screen is T && screen.Id == screenName)
                {
                    return screen as T;
                }
            }

            return default(T);
        }

        public static T GetCinematicScreen<T>(string cinematicId) where T : CinematicScreen
        {
            GameObject cinematicGO = Instantiate(Resources.Load("Cinematics/" + cinematicId)) as GameObject;
            return cinematicGO.GetComponent<T>();
        }

        #region Stacking Operations

        // Dictionary of stacks
        protected Dictionary<string, Stack<ScreenBase>> mScreenStacks = new Dictionary<string, Stack<ScreenBase>>();

        public void Stack(string stackId, ScreenBase screen)
        {
            Stack<ScreenBase> stack;
            if (!mScreenStacks.TryGetValue(stackId, out stack))
                stack = new Stack<ScreenBase>();
            stack.Push(screen);
        }

        public ScreenBase Unstack(string stackId)
        {
            Stack<ScreenBase> stack;
            if (!mScreenStacks.TryGetValue(stackId, out stack))
                if (stack.Count > 0)
                    return stack.Pop();

            return null;
        }

        public ScreenBase GetTop(string stackId)
        {
            Stack<ScreenBase> stack;
            if (!mScreenStacks.TryGetValue(stackId, out stack))
                if (stack.Count > 0)
                    return stack.Peek();
            return null;
        }
        #endregion Stacking Operations

    }
}



