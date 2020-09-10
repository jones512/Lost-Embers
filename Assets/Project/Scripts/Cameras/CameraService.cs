using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AdventureKit.Utils;
using AdventureKit.Camera.Controller;

namespace AdventureKit.Camera
{
    public class CameraService : SingletonMonoBehaviour<CameraService>
    {
        private CamerasLevelController mCameraLeveController;
        public CamerasLevelController CameraLevelController { get { return mCameraLeveController; } private set { } }

        public void RegisterCameraLevelController(CamerasLevelController clc)
        {
            mCameraLeveController = clc;
        }

        public bool CurrentCameraEnabled()
        {
            return mCameraLeveController.CurrentCamera.activeInHierarchy;
        }

        public bool IsCurrentCamera(GameObject camera)
        {
            return mCameraLeveController.CurrentCamera == camera;
        }
    }
}

