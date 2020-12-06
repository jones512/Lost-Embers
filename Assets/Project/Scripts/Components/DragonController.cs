using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AdventureKit.Utils;
using DG.Tweening;

namespace AdventureKit.AI
{
    public class DragonController : AdventureKit.Utils.MonoBehaviour
    {
        public enum DragonType { GREEN, YELLOW }
        public DragonType m_Type;
        //The target player
        public Transform player;
        public Transform m_EnemyRoot;
        //At what distance will the enemy walk towards the player?
        public float walkingDistance = 10.0f;
        //In what time will the enemy complete the journey between its position and the players position
        public float smoothTime = 10.0f;

        public List<Transform> m_FleePivots;

        //Vector3 used to store the velocity of the enemy
        private Vector3 smoothVelocity = Vector3.zero;

        private MeshRenderer mMeshRenderer;
        private Color mColor;
        //private Animator mAnimator;

        private bool mIsDead = false;
        private bool mDoingAttack;
        private bool mCanAttack;
        private bool mOnFlee;

        private int mFleePivotIndex;

        private PlayerController mPlayerController;
        private Inventory mInventory;

        private void Awake()
        {
            mPlayerController = FindObjectOfType<PlayerController>();
            mInventory = mPlayerController.GetComponent<Inventory>();
            mMeshRenderer = m_EnemyRoot.GetComponent<MeshRenderer>();
            mColor = mMeshRenderer.material.color;

            mCanAttack = true;
            mDoingAttack = false;
            mIsDead = false;
            mOnFlee = false;
            //mAnimator = GetComponent<Animator>();
            // mAnimator.SetBool("Breath_Gs", true);

            StartCoroutine(_DoAttack());
            

        }

        void Update()
        {
            if (!mIsDead)
            {
                //Look at the player
                

                //Calculate distance between player
                float distance = Vector3.Distance(m_EnemyRoot.transform.position, player.position);
                //K.SoundManager.PlayFXSound(K.SoundManager.GetSFXByName("dragon_growl"));
                //If the distance is smaller than the walkingDistance
                if (distance < walkingDistance)
                {
                    m_EnemyRoot.LookAt(player);

                    if (m_Type == DragonType.GREEN)
                    {
                        transform.position = Vector3.SmoothDamp(transform.position, player.position, ref smoothVelocity, smoothTime);
                        if (mCanAttack)
                            StartCoroutine(_DoAttack());

                    }
                    else
                    {
                        if (mInventory.GetCurrentItem() == ItemsIds.Items.SWORD)
                        {
                            if (!mOnFlee)
                            {
                                mOnFlee = true;
                                mPlayerController.DisableInput();
                                this.transform.DOLocalMoveY(50, 5).OnComplete(() =>
                                {

                                    this.transform.SetParent(m_FleePivots[mFleePivotIndex]);
                                    this.transform.localPosition = Vector3.zero;
                                    mOnFlee = false;
                                    mPlayerController.EnableInput();
                                });
                            }
                        }
                        else
                        {
                            if (mCanAttack)
                                StartCoroutine(_DoAttack());
                        }
                    }
                }
            }

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag.Equals("Player") && !mIsDead)
            {
                Inventory inventory = other.GetComponent<Inventory>();
                if (inventory.GetCurrentItem() == ItemsIds.Items.SWORD && !mDoingAttack)
                    Dead();
                else
                    LevelController.Instance.CompleteLevel(true);
            }
        }

        private void Dead()
        {
            mIsDead = true;
            //mAnimator.SetBool("Breath_Gs", false);
            // mAnimator.SetBool("Dead", true);
            StartCoroutine(_DoDead());
        }

        private IEnumerator _DoDead()
        {
            m_EnemyRoot.gameObject.SetActive(false);

            yield return new WaitForSeconds(0.5f);

            m_EnemyRoot.gameObject.SetActive(true);

            yield return new WaitForSeconds(0.5f);

            m_EnemyRoot.gameObject.SetActive(false);


        }

        private IEnumerator _DoAttack()
        {
            mDoingAttack = true;
            mCanAttack = false; 
            mMeshRenderer.material.color = Color.red;

            yield return new WaitForSecondsRealtime(2);

            mMeshRenderer.material.color = m_Type == DragonType.GREEN ? Color.green : Color.yellow ;
            mDoingAttack = false;

            yield return new WaitForSecondsRealtime(2);

            mCanAttack = true;
        }
    }
}

