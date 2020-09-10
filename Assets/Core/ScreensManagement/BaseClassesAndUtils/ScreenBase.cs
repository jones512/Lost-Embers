using UnityEngine;
using UnityEngine.UI;
using System;

using AdventureKit.UI.Core.OpenCloseFX;

namespace AdventureKit.ScreensManagement.BaseClaseAndUtils
{
    public class ScreenBase : Utils.MonoBehaviour
    {
        /// <summary>
        /// Event released when screen is opened
        /// </summary>
        public EventHandler ScreenOpened;

        /// <summary>
        /// Event released when screen is closed
        /// </summary>
        public EventHandler ScreenClosed;

        // Callback for when the screen has finish opening
        public Action OnScreenOpenDone;

        // Callback for when the screen is about to close
        public Action OnScreenClosing;

        // Callback for when the screen is finally closed
        public Action OnScreenCloseDone;

        // Properties
        public string Id { get { return gameObject.name; } }
        public bool IsOpen { get; protected set; }

        // Attrs
        IOpenCloserFX mOpenCloseFX;

        void Setup()
        {
            mOpenCloseFX = GetComponent<IOpenCloserFX>();
            if (mOpenCloseFX == null)
                mOpenCloseFX = gameObject.AddComponent<DefaultOpenCloseFX>();
        }

        #region open/close screen
        public void OpenScreen()
        {
            // check GO has been activated first
            if (mOpenCloseFX == null)
                Setup();

            mOpenCloseFX.DoOpenFX(OnOpenFXFinished);
        }

        public void CloseScreen()
        {
            if (ScreenClosed != null)
                ScreenClosed(this, EventArgs.Empty);

            if (OnScreenClosing != null)
                OnScreenClosing();

            //just a quick fix for the old login scenes, just remove this when we have the steam scenes in place
            mOpenCloseFX = GetComponent<IOpenCloserFX>();
            if (mOpenCloseFX == null)
                mOpenCloseFX = gameObject.AddComponent<DefaultOpenCloseFX>();
            mOpenCloseFX.DoCloseFX(OnCloseFXFinished);
        }

        protected virtual void OnOpenFXFinished()
        {
            if (ScreenOpened != null)
                ScreenOpened(this, EventArgs.Empty);

            IsOpen = true;
            if (OnScreenOpenDone != null)
                OnScreenOpenDone();
            OnScreenOpenDone = null;
        }

        protected virtual void OnCloseFXFinished()
        {
            IsOpen = false;
            if (OnScreenCloseDone != null)
                OnScreenCloseDone();
            OnScreenCloseDone = null;
        }

        #endregion

        public void ButtonEnter(GameObject button)
        {
            button.GetComponent<Text>().color = Color.cyan;
        }

        public void ButtonExit(GameObject button)
        {
            button.GetComponent<Text>().color = Color.white;
        }
    }

}

