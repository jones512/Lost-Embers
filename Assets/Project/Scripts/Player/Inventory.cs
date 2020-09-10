using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Inventory : MonoBehaviour
{
    [Header("Items Root")]
    [SerializeField]
    private GameObject m_ItemsRoot;

    private ItemsIds.Items mCurrentItem;
    private GameObject mItemObject;

    public ItemsIds.Items GetCurrentItem()
    {
        return mCurrentItem;
    }

    public void PickItem(ItemsIds.Items itemId, GameObject item)
    {
        if(mCurrentItem != ItemsIds.Items.NONE)
            DropItem(mItemObject);

        mCurrentItem = itemId;
        mItemObject = item;
        item.transform.SetParent(this.transform);
        item.transform.DOLocalMove(new Vector3(0, 2, 0), 0f);
    }

    public void DropItem(GameObject item)
    {
        mCurrentItem = ItemsIds.Items.NONE;
        item.transform.position = this.transform.position;
        item.transform.SetParent(m_ItemsRoot.transform);
        item.GetComponent<PickableItem>().SetAsPickable();
    }

    public void DropCurrentItem(bool setAsPickable = true)
    {
        mCurrentItem = ItemsIds.Items.NONE;
        mItemObject.transform.position = this.transform.position;
        mItemObject.transform.SetParent(m_ItemsRoot.transform);

        if(setAsPickable)
            mItemObject.GetComponent<PickableItem>().SetAsPickable();
        else
            mItemObject.GetComponent<PickableItem>().SetAsNotPickable();
    }

    public void HideCurrentItem()
    {
        mCurrentItem = ItemsIds.Items.NONE;
        mItemObject.transform.SetParent(m_ItemsRoot.transform);
        mItemObject.transform.position = Vector3.zero;
        mItemObject.GetComponent<PickableItem>().SetAsNotPickable();
        mItemObject.SetActive(false);
    }
}
