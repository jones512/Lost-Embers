using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class PickableItem : MonoBehaviour
{
    [Header("Item Type")]
    [SerializeField]
    private ItemsIds.Items m_ItemId;

    private Tween mTween;
    private bool mCanPickUp = true;
    void Start()
    {
        mTween = this.transform.DOLocalMoveY(this.transform.localPosition.y + 0.5f, 0.25f).SetLoops(10000000, LoopType.Yoyo);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player") && mCanPickUp)
        {
            other.GetComponent<Inventory>().PickItem(m_ItemId, this.gameObject);
            SetAsNotPickable();
        }
    }

    public void SetAsPickable()
    {
        StartCoroutine(_SetAsPickable());
    }

    public void SetAsNotPickable()
    {
        mTween.Complete();
        mCanPickUp = false;
    }

    IEnumerator _SetAsPickable()
    {
        mTween = this.transform.DOLocalMoveY(this.transform.localPosition.y + 0.5f, 0.25f).SetLoops(10000000, LoopType.Yoyo);
        
        yield return new WaitForSecondsRealtime(2);

        mCanPickUp = true;
    }
}
