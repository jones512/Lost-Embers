using UnityEngine;
using System.Collections;


namespace AdventureKit.ScreensManagement.BaseClaseAndUtils
{ 
    public class ScreensManagerBase : Utils.SingletonMonoBehaviour<ScreensManagerBase> {

        #region init and destroy
        protected override void Initialize() {
            base.Initialize();
            Debug.Log("initializa ScreensManager base"); 
        }
        #endregion
    }

}
