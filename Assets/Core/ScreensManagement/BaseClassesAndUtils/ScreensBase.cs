using UnityEngine;
using System.Collections;

namespace AdventureKit.ScreensManagement.BaseClaseAndUtils
{ 
    public class ScreensBase : Utils.MonoBehaviour
    {

        public Game.MenuNavigation screenNavigation = new Game.MenuNavigation();
        protected GamepadController gamepadController;
        public GameObject go;
        protected bool isOpen = false;


        public System.Action EvntClosedWindow;


        #region init and destroy
        public virtual void Start()
        {
            if (screenNavigation != null) {
                screenNavigation.Init();
                screenNavigation.EventOnClick += OnClickCallback;
            }
        }

        public virtual void OnDestroy()
        {
            if (screenNavigation != null) {
                screenNavigation.EventOnClick -= OnClickCallback;
                screenNavigation.Destroy();
            }

            gamepadController = null;
        }

        public virtual void Init(ref GamepadController _gamepad)
        {
            gamepadController = _gamepad;
        }

        public virtual void Init() {
            gamepadController = null;
        }
        #endregion

        #region open/close screen
        public virtual void OpenScreen() {
        }

        public virtual void CloseScreen() {
        }
        #endregion

        #region input callbacks
        protected virtual void OnClickCallback(int idItem) {
        }

        protected virtual void SetControllerCallbacks() {
        }

        protected virtual void RemoveControllerCallbacks() {

        }
        #endregion

    }

}
