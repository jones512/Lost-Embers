using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AdventureKit.Camera;

namespace AdventureKit.Camera.Controller
{
    public class CamerasLevelController : MonoBehaviour
    {
        [Header("Start Camera")]
        [SerializeField]
        private GameObject m_StartCamera;

        private GameObject mCurrentCamera;
        public GameObject CurrentCamera { get { return mCurrentCamera;  } private set { } }

        private GameObject mPrevCamera;
        public GameObject PrevCamera { get { return mPrevCamera; } private set { } }

        private void Awake()
        {
            Init();
        }
        public void Init()
        {
            CameraService.Instance.RegisterCameraLevelController(this);

            mCurrentCamera = m_StartCamera;
            m_StartCamera.SetActive(true);
        }

        public void ActiveCamera(GameObject camera)
        {
            mPrevCamera = mCurrentCamera;
            mCurrentCamera = camera;
            camera.SetActive(true);
        }

        public void DisableCurrentCamera()
        {
            mCurrentCamera.SetActive(false);
        }

        public void DisablePrevCamera()
        {
            mPrevCamera.SetActive(false);
        }
    }
}

