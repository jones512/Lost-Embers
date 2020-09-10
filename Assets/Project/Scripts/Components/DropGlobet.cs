using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace AdventureKit.Kernel
{
    public class DropGlobet : Utils.MonoBehaviour
    {
        [Header("Required Item")]
        [SerializeField]
        private ItemsIds.Items m_ItemId;

        [SerializeField]
        private GameObject m_Fire;

        private bool mUnlocked = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag.Equals("Player"))
            {
                Inventory playerInventory = other.GetComponent<Inventory>();
                if (!mUnlocked)
                {
                    if (m_ItemId.Equals(playerInventory.GetCurrentItem()))
                    {
                        m_Fire.SetActive(true);
                        playerInventory.HideCurrentItem();
                    }
                }
            }
        }
    }
}
