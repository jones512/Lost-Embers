using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            Inventory inventory = other.GetComponent<Inventory>();
            if (inventory.GetCurrentItem() == ItemsIds.Items.SWORD)
                Dead();
            else
                other.gameObject.SetActive(false);
        }
    }

    private void Dead()
    {
        this.gameObject.SetActive(false);
    }
}
