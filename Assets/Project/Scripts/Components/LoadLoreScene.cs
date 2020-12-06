using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using AdventureKit.Utils;
using AdventureKit.Config;

public class LoadLoreScene : AdventureKit.Utils.MonoBehaviour
{
    [Header("Item Type")]
    [SerializeField]
    private ItemsIds.Items m_ItemId;

    [SerializeField]
    private GameObject  m_Root;

    private bool mCanLoad = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player") && !mCanLoad)
        {
            Inventory playerInventory = other.GetComponent<Inventory>();
            PlayerController player = other.GetComponent<PlayerController>();
            if (m_ItemId.Equals(playerInventory.GetCurrentItem()))
            {
                player.DisableInput();
                K.SoundManager.PlayFXSound(K.SoundManager.GetSFXByName("open_door"));
                playerInventory.HideCurrentItem();
                m_Root.transform.DOLocalMoveZ(1.25f, 3).OnComplete(() => K.EnterLoreContext());
            }
        }
    }
}
