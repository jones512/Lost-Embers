using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace AdventureKit.Kernel
{
    public class ExitInsideCastleTrigger : Utils.MonoBehaviour
    {
        [Header("Inside CastleR Room")]
        [SerializeField]
        private GameObject m_InsideCastleRoom;

        [SerializeField]
        private GameObject m_OutsidePlayerSpawnPivot;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag.Equals("Player"))
            {

                StartCoroutine(_ExitInsideCastle());
            }
        }

        private IEnumerator _ExitInsideCastle()
        {
            CharacterController player = FindObjectOfType<CharacterController>();
            player.gameObject.SetActive(false);
            player.transform.SetParent(m_OutsidePlayerSpawnPivot.transform);
            player.transform.localPosition = Vector3.zero;
            K.DefaultLoadingScreen.Show(true, "Cargando...");
            yield return new WaitForSecondsRealtime(2);
            K.DefaultLoadingScreen.Show(false);
            m_InsideCastleRoom.SetActive(false);
            player.gameObject.SetActive(true);



        }
    }

}
