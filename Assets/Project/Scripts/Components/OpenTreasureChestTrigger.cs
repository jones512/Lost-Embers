using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using AdventureKit.Utils;

public class OpenTreasureChestTrigger : AdventureKit.Utils.MonoBehaviour
{
    //[Header("Item Type")]
    //[SerializeField]
    //private ItemsIds.Items m_ItemId;

    [Header("Open Root")]
    [SerializeField]
    private GameObject m_OpenRoot;

    [Header("Close Root")]
    [SerializeField]
    private GameObject m_CloseRoot;

    [Header("Item Object")]
    [SerializeField]
    private GameObject m_ItemObject;


    private bool mCanPickUp = true;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player") && mCanPickUp)
        {
            m_CloseRoot.SetActive(false);
            m_ItemObject.SetActive(true);
           
            mCanPickUp = false;

            //K.SoundManager.PlayFXSound(K.SoundManager.GetSFXByName("pick_item"));
            //other.GetComponent<Inventory>().PickItem(m_ItemId, m_ItemObject);
        }
    }
}
