using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//using AdventureKit.Scenarios;

namespace AdventureKit.Common
{
    public class Player : Utils.MonoBehaviour
    {
        //[SerializeField]
        //public GameObject m_RayCastPointer;
        //public GameObject RayCastPointer { get { return m_RayCastPointer; } }

        //public LevelSlot AssignedSlot { get; private set; }
        //public GameObject StartSlot { get; set; }
        //public Vector3 OriginalRotation { get; private set; }

        //private void Start()
        //{
        //    K.GameManager.Player = this;
        //    OriginalRotation = transform.eulerAngles;

        //    Color color = new Color();
        //    if (K.GameManager.PlayerGender == GameManager.Gender.Male)
        //    {
        //        if (ColorUtility.TryParseHtmlString("#008898", out color))
        //            this.GetComponent<MeshRenderer>().material.color = color;
        //    }
        //    else
        //    {
        //        if (ColorUtility.TryParseHtmlString("#910098", out color))
        //            this.GetComponent<MeshRenderer>().material.color = color;
        //    }
        //}

        //public void AssignToSlot(GameObject slot)
        //{
        //    Debug.Log("Player assined to slot " + slot.name);

        //    this.transform.parent = slot.transform;
        //    this.transform.localPosition = Vector3.zero + new Vector3(0, 1.5f, 0);

        //    AssignedSlot = slot.GetComponent<LevelSlot>();
        //}

        //public void ActivateSlot()
        //{
        //    if (AssignedSlot.SlotType == LevelSlot.SlotTypes.TARGET)
        //    {
        //        AssignedSlot.SetAsActivated();
        //    } 
        //}
    }
}

