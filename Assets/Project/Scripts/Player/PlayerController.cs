using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController characterController;
    Inventory mInventory;

    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;

    private Vector3 moveDirection = Vector3.zero;
    private bool mEnableInput = true;
    void Start()
    {
        mInventory = GetComponent<Inventory>();
        characterController = GetComponent<CharacterController>();
    }

    public void EnableInput()
    {
        mEnableInput = true;
    }

    public void DisableInput()
    {
        mEnableInput = false;
    }

    void Update()
    {
        if(mEnableInput)
        {
            if (characterController.isGrounded)
            {
                if(Input.GetKeyDown("space") && mInventory.GetCurrentItem() != ItemsIds.Items.NONE)
                    mInventory.DropCurrentItem(true);

                // We are grounded, so recalculate
                // move direction directly from axes

                moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
                moveDirection *= speed;

                //if (Input.GetButton("Jump"))
                //{
                //    moveDirection.y = jumpSpeed;
                //}
            }

            // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
            // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
            // as an acceleration (ms^-2)
            moveDirection.y -= gravity * Time.deltaTime;

            // Move the controller
            characterController.Move(moveDirection * Time.deltaTime);
        }
    }
}
