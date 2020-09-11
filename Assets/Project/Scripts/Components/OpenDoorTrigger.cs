using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace AdventureKit.Kernel
{
    public class OpenDoorTrigger : Utils.MonoBehaviour
    {
        [Header("Required Item")]
        [SerializeField]
        private ItemsIds.Items m_ItemId;

        [Header("Door")]
        [SerializeField]
        private Transform m_Door;

        //[Header("Inside Room")]
        //[SerializeField]
        //private GameObject m_InsideRoom;
        //[SerializeField]
        //private GameObject m_InsidePlayerSpawnPivot;

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
                        mDoorOpened = true;
                        playerInventory.HideCurrentItem();
                        K.SoundManager.PlayFXSound(K.SoundManager.GetSFXByName("open_door"));
                        m_Door.transform.DOLocalMoveY(m_Door.transform.localPosition.y + 2.8f, 3).OnComplete(() => player.EnableInput());
                    }

                }
            }
        }
    }

}
