using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Pinocchio
{
    public class PinocchioEnemy : MonoBehaviour
    {
        private Rigidbody2D rigidbody;
        private bool isMove = false;
        private float moveSpeed;
        private float feverSpeed;

        private float pushSkillRandom_MinTime = 3f;
        private float pushSkillRandom_MaxTime = 6f;

        public float FORECE_SET_OFFSET = 0.1f;

        public Transform startPos;
        public Transform deathBoxCollPos;
        public Vector2 deathBoxSize;

        [SerializeField] private GameObject sadGrandFather;
        [SerializeField] private GameObject goodGrandFather;

        [SerializeField] private GameObject sadImg;


        private void Start()
        {
            rigidbody = GetComponent<Rigidbody2D>();
        }
        private void FixedUpdate()
        {
            if (!PinocchioGame.Instance.isGamePlay) return;
            if (isMove)
            {
                Debug.Log(moveSpeed + "움직임");
                rigidbody.AddForce((Vector2.left * this.moveSpeed) * FORECE_SET_OFFSET, ForceMode2D.Force);
            }
        }

        private void Update()
        {
            if (!PinocchioGame.Instance.isGamePlay) return;

            var hits = Physics2D.BoxCastAll(deathBoxCollPos.position, deathBoxSize, 0, Vector2.up);
            foreach (var hit in hits)
            {
                // AnimationColl
                if (hit.transform.CompareTag("GoodGrandFatherAnime"))
                {
                    sadGrandFather.SetActive(false);
                    goodGrandFather.SetActive(true);
                    sadImg.SetActive(false);
                }
                else if (hit.transform.CompareTag("SadGrandFatherAnime"))
                {
                    sadGrandFather.SetActive(true);
                    goodGrandFather.SetActive(false);
                    sadImg.SetActive(true);
                }

                // GameColl
                if (hit.transform.CompareTag("DeathLine"))
                {
                    Debug.Log("GAME OVER");
                    PinocchioGame.Instance.StageFailed();
                }
                if (hit.transform.CompareTag("ClearLine"))
                {
                    Debug.Log("GAME CLEAR");
                    PinocchioGame.Instance.StageClear();
                    //PinocchioGame.Instance.possibleClick = false;
                }
            }
        }

        public IEnumerator PushSkill(float hitValue)
        {
            //while (PinocchioGame.Instance.gameState == GameState.Play)
            {
                yield return new WaitForSeconds(Random.Range(pushSkillRandom_MinTime, pushSkillRandom_MaxTime));
                AddForcePower(hitValue);
                SoundManagers.Instance.PlaySFX("PushSkill");
                CameraController.Instance.OnShake(3, 3, true, true);
            }
        }

        public void StageInit(StageDifficulty stageInfo)
        {
            moveSpeed = stageInfo.tickHitValue;
            feverSpeed = stageInfo.feverHitValue;

            transform.position = startPos.position;
        }

        /// <summary>
        /// 순간적으로 힘을 줌
        /// </summary>
        public void AddForcePower(float hitValue)
        {
            rigidbody.AddForce((Vector2.right * hitValue) / 2, ForceMode2D.Force);
        }

        /// <summary>
        /// 스피드를 받고 움직이기 시작합니다.
        /// </summary>
        public void OnMove()
        {
            // StartCoroutine(EnemyPowerTick());
            isMove = true;
        }
        public void OnMove(int moveSpeed)
        {
            isMove = true;
            this.moveSpeed = moveSpeed;
            // StopCoroutine(EnemyPowerTick());
        }

        public void StopMove()
        {
            isMove = false;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawCube(deathBoxCollPos.position, deathBoxSize);
        }
#endif
    }
}