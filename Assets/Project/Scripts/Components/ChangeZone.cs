using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AdventureKit.Camera;

namespace AdventureKit.Level
{
    public class ChangeZone : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_VCamera;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag.Equals("Player"))
            {
                CameraService csInstance = CameraService.Instance;

                if(!csInstance.IsCurrentCamera(m_VCamera))
                {
                    csInstance.CameraLevelController.ActiveCamera(m_VCamera);
                    csInstance.CameraLevelController.DisablePrevCamera();
                }
            }
        }

        //private IEnumerator _Wait(System.Action callback)
        //{
        //    yield return new WaitForSecondsRealtime(2);

        //    callback();
        //}
    }
}


