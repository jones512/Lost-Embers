﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AdventureKit.Utils;

namespace AdventureKit.AI
{
    public class DragonController : AdventureKit.Utils.MonoBehaviour
    {
        //The target player
        public Transform player;
        //At what distance will the enemy walk towards the player?
        public float walkingDistance = 10.0f;
        //In what time will the enemy complete the journey between its position and the players position
        public float smoothTime = 10.0f;
        //Vector3 used to store the velocity of the enemy
        private Vector3 smoothVelocity = Vector3.zero;

        private Animator mAnimator;

        private bool mIsDead = false;

        private void Awake()
        {
            mAnimator = GetComponent<Animator>();
            mAnimator.SetBool("Breath_Gs", true);
            
        }

        void Update()
        {
            if (!mIsDead)
            {
                //Look at the player
                transform.LookAt(player);
                //Calculate distance between player
                float distance = Vector3.Distance(transform.position, player.position);
                //K.SoundManager.PlayFXSound(K.SoundManager.GetSFXByName("dragon_growl"));
                //If the distance is smaller than the walkingDistance
                if (distance < walkingDistance)
                {
                    
                    //Move the enemy towards the player with smoothdamp
                    transform.position = Vector3.SmoothDamp(transform.position, player.position, ref smoothVelocity, smoothTime);

                }
            }

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag.Equals("Player"))
            {
                Inventory inventory = other.GetComponent<Inventory>();
                if (inventory.GetCurrentItem() == ItemsIds.Items.SWORD)
                    Dead();
                else
                    LevelController.Instance.CompleteLevel(true);
            }
        }

        private void Dead()
        {
            mIsDead = true;
            mAnimator.SetBool("Breath_Gs", false);
            mAnimator.SetBool("Dead", true);
            //this.gameObject.SetActive(false);
        }
    }
}

