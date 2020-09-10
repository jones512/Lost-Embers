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
                if (!mDoorOpened)
                {
                    if (m_ItemId.Equals(playerInventory.GetCurrentItem()))
                    {
                        mDoorOpened = true;
                        playerInventory.DropCurrentItem(false);
                        m_Door.transform.DOLocalMoveY(m_Door.transform.localPosition.y + 3f, 1);
                    }

                }
            }
        }
    }

}
