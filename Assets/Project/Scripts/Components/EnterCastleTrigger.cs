using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace AdventureKit.Kernel
{
    public class EnterCastleTrigger : Utils.MonoBehaviour
    {
        [Header("Required Item")]
        [SerializeField]
        private ItemsIds.Items m_ItemId;

        [Header("Door")]
        [SerializeField]
        private Transform m_Door;

        [Header("Inside Room")]
        [SerializeField]
        private GameObject m_InsideRoom;
        [SerializeField]
        private GameObject m_InsidePlayerSpawnPivot;

        private bool mDoorOpened = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag.Equals("Player"))
            {
                Inventory playerInventory = other.GetComponent<Inventory>();
                PlayerController player = other.GetComponent<PlayerController>();
                if (!mDoorOpened)
                {
                    if (m_ItemId.Equals(playerInventory.GetCurrentItem()))
                    {
                        
                        player.DisableInput();
                        playerInventory.HideCurrentItem();
                        K.SoundManager.PlayFXSound(K.SoundManager.GetSFXByName("open_door"));
                        m_Door.transform.DOLocalMoveY(m_Door.transform.localPosition.y + 1.5f, 3).OnComplete(() => { player.EnableInput(); mDoorOpened = true; });
                    }
                   
                }
                else
                {
                    StartCoroutine(_EnterInsideCastle());

                }

            }
        }

        private IEnumerator _EnterInsideCastle()
        {
            PlayerController player = FindObjectOfType<PlayerController>();
            player.transform.SetParent(m_InsidePlayerSpawnPivot.transform);
            player.transform.localPosition = Vector3.zero;
            K.DefaultLoadingScreen.Show(true, "Cargando...");
            yield return new WaitForSecondsRealtime(1);
            m_InsideRoom.SetActive(true);
            yield return new WaitForSecondsRealtime(1);
            K.DefaultLoadingScreen.Show(false);
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.tag.Equals("Player"))
            {
                Inventory playerInventory = other.GetComponent<Inventory>();
                PlayerController player = other.GetComponent<PlayerController>();
                if (mDoorOpened)
                {
                    StartCoroutine(_EnterInsideCastle());

                }
            }
        }
    }

   

}
