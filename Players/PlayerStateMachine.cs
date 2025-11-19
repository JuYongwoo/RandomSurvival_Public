using JYW.RandomSurvival.Commmons;
using JYW.RandomSurvival.Items;
using JYW.RandomSurvival.Managers;
using JYW.RandomSurvival.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JYW.RandomSurvival.Players
{

    public class PlayerStateMachine : MonoBehaviour
    {
        private PlayerState state;


        private GameObject currentTarget;
        private bool isAttacking;

        private GameObject currentMoveMark;
        private Vector3 currentDestination;

        private Coroutine moveCoroutine;
        private Coroutine attackCoroutine;

        private float moveSpeed = 10f; // 1칸/초



        private void Start()
        {

            EventManager.Instance.SetAttackTargetEvent -= BindSetAttackTarget;
            EventManager.Instance.SetAttackTargetEvent += BindSetAttackTarget;
            EventManager.Instance.SetDestinationEvent -= BindSetDestination;
            EventManager.Instance.SetDestinationEvent += BindSetDestination;
            EventManager.Instance.SetPlayerStateEvent -= SetPlayerState;
            EventManager.Instance.SetPlayerStateEvent += SetPlayerState;

            SetPlayerState(PlayerState.Idle);
        }

        private void BindSetAttackTarget(GameObject go)
        {
            currentTarget = go;
        }

        private void BindSetDestination(Vector3 destPos)
        {
            currentDestination = destPos;
        }


        private void OnDestroy()
        {
            EventManager.Instance.SetAttackTargetEvent -= BindSetAttackTarget;
            EventManager.Instance.SetDestinationEvent -= BindSetDestination;
            EventManager.Instance.SetPlayerStateEvent -= SetPlayerState;

        }
        private void Update()
        {
            //Debug.Log(state);
            switch (state)
            {
                case PlayerState.Idle:
                    var enemies = DetectEnemies();
                    if (enemies.Count > 0)
                    {
                        currentTarget = GetClosestThreatEnemy(enemies)?.gameObject;
                        SetPlayerState(PlayerState.Attack_Attacking);
                    }
                    break;
                case PlayerState.MoveStart:
                    MoveStop();
                    MoveStart(currentDestination);
                    SetPlayerState(PlayerState.Moving);
                    break;
                case PlayerState.Moving:
                    if (Vector3.Distance(this.gameObject.transform.position, currentDestination) < 0.1f)
                    {
                        MoveStop();
                        SetPlayerState(PlayerState.Idle);
                    }
                    break;
                case PlayerState.AttackStart:
                    MoveStop();

                    if (currentTarget == null)
                    {
                        SetPlayerState(PlayerState.Idle);
                    }
                    else if (IsInAttackRange(currentTarget))
                    {
                        SetPlayerState(PlayerState.Attack_Attacking);
                    }
                    else
                    {
                        SetPlayerState(PlayerState.Attack_MoveStart);
                    }
                    break;
                case PlayerState.Attack_MoveStart:
                    MoveStop();
                    if (currentTarget == null)
                    {
                        SetPlayerState(PlayerState.Idle);
                    }
                    else
                    {
                        MoveStart(currentTarget.transform.position);
                        SetPlayerState(PlayerState.Attack_Moving);
                    }
                    break;
                case PlayerState.Attack_Moving:

                    if (currentTarget == null)
                    {
                        SetPlayerState(PlayerState.Idle);
                        return;
                    }


                    if (IsInAttackRange(currentTarget))
                    {
                        MoveStop();
                        SetPlayerState(PlayerState.Attack_Attacking);
                    }
                    break;
                case PlayerState.Attack_Attacking:
                    if (currentTarget == null)
                    {
                        SetPlayerState(PlayerState.Idle);
                    }
                    else if (!IsInAttackRange(currentTarget))
                    {
                        SetPlayerState(PlayerState.Attack_MoveStart);
                    }
                    else if (!isAttacking)
                    {
                        MoveStop();
                        StartCoroutine(Attack());
                    }
                    break;
                case PlayerState.AttackMove_MoveStart:
                    MoveStop();
                    MoveStart(currentDestination);
                    SetPlayerState(PlayerState.AttackMove_Moving);
                    break;
                case PlayerState.AttackMove_Moving:
                    if (Vector3.Distance(this.gameObject.transform.position, currentDestination) < 0.1f)
                    {
                        MoveStop();
                        SetPlayerState(PlayerState.Idle);
                    }

                    var detected = DetectEnemies();
                    if (detected.Count > 0)
                    {
                        currentTarget = GetClosestThreatEnemy(detected)?.gameObject;
                        SetPlayerState(PlayerState.AttackMove_Attacking);
                    }
                    break;
                case PlayerState.AttackMove_Attacking:
                    if (currentTarget == null)
                    {
                        SetPlayerState(PlayerState.AttackMove_MoveStart);
                    }
                    else if (!isAttacking)
                    {
                        MoveStop();
                        StartCoroutine(Attack());
                    }
                    break;
            }
        }

        private void SetPlayerState(PlayerState s)
        {
            state = s;

            int animState = (int)s;
            if (s == PlayerState.AttackMove_Moving || s == PlayerState.Attack_Moving)
                animState = (int)PlayerState.Moving;
            if (s == PlayerState.AttackMove_Attacking)
                animState = (int)PlayerState.Attack_Attacking;

            EventManager.Instance.OnGetAnimator()?.SetInteger("State", animState);
        }

        private IEnumerator Attack()
        {
            isAttacking = true;

            if (currentTarget != null)
                transform.LookAt(currentTarget.transform.position);

            EventManager.Instance.OnPlayAudioClip(ResourceManager.Instance.GetWeaponData(EventManager.Instance.OnGetGetPlayerStat().currentWeapon).GetShootSound(), 0.5f, false);
            var particle = Instantiate(ResourceManager.Instance.GetWeaponData(EventManager.Instance.OnGetGetPlayerStat().currentWeapon).GetProjectilePrefab(), transform.position + Vector3.up * 1.2f, Quaternion.identity);
            var attackParticle = particle.GetComponent<AttackProjectile>();
            if (attackParticle != null && currentTarget != null)
                attackParticle.SetTarget(currentTarget.transform);

            yield return new WaitForSeconds(ResourceManager.Instance.GetWeaponData(EventManager.Instance.OnGetGetPlayerStat().currentWeapon).GetReloadTime());

            isAttacking = false;
        }

        public List<EnemyBase> DetectEnemies()
        {
            var detectedEnemies = new List<EnemyBase>();
            var hitColliders = Physics.OverlapSphere(transform.position, 10f, LayerMask.GetMask("EnemyClick"));

            foreach (var collider in hitColliders)
            {
                var enemy = collider.GetComponent<EnemyBase>();
                if (enemy != null)
                    detectedEnemies.Add(enemy);
            }

            return detectedEnemies;
        }

        private EnemyBase GetClosestThreatEnemy(List<EnemyBase> enemies)
        {
            EnemyBase closestThreat = null;
            EnemyBase closestAny = null;
            float minThreatSqrDist = float.MaxValue;
            float minAnySqrDist = float.MaxValue;
            Vector3 origin = transform.position;

            foreach (var enemy in enemies)
            {
                if (enemy == null) continue;

                float sqrDist = (enemy.transform.position - origin).sqrMagnitude;

                if (enemy.power > 0f && sqrDist < minThreatSqrDist)
                {
                    minThreatSqrDist = sqrDist;
                    closestThreat = enemy;
                }

                if (sqrDist < minAnySqrDist)
                {
                    minAnySqrDist = sqrDist;
                    closestAny = enemy;
                }
            }

            return closestThreat ?? closestAny;
        }


        private void MoveStart(Vector3 dest)
        {
            currentMoveMark = Instantiate(ResourceManager.Instance.GetMoveMark(), dest + Vector3.up * 0.1f, Quaternion.Euler(90, 0, 0));

            moveCoroutine = StartCoroutine(GridUtil.goToIEnu(EventManager.Instance.OnGetMap().map, this.gameObject.transform, dest, moveSpeed, () =>
            {
                SetPlayerState(PlayerState.Idle); //도착했으므로 기본상태
                MoveStop();
            }));
        }

        private void MoveStop()
        {
            DestroyCurrentMoveMark();

            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
                moveCoroutine = null;
            }
        }
        private bool IsInAttackRange(GameObject target)
        {
            if (target == null) return false;
            return (transform.position - target.transform.position).sqrMagnitude <= Math.Pow(ResourceManager.Instance.GetWeaponData(EventManager.Instance.OnGetGetPlayerStat().currentWeapon).GetRange(), 2);
        }


        private void DestroyCurrentMoveMark()
        {
            if (currentMoveMark != null)
            {
                Destroy(currentMoveMark);
                currentMoveMark = null;
            }
        }

    }
}
