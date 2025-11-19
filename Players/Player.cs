using JYW.RandomSurvival.Commmons;
using JYW.RandomSurvival.Managers;
using System.Collections;
using UnityEngine;

namespace JYW.RandomSurvival.Players
{


    public class Player : MonoBehaviour
    {
        private Animator animator;

        private bool canWarp = true;

        private PlayerCurrentStat playerCurrentStat;

        private void Awake()
        {
            //플레이어 실제 스탯 초기화
            var data = ResourceManager.Instance.GetPlayerSO();
            playerCurrentStat = new PlayerCurrentStat(data.GetCurrentHP(),
                data.GetCurrentMaxHP(),
                data.GetCurrentEXP(),
                data.GetCurrentMaxEXP(),
                data.GetCurrentMoveSpeed(),
                data.GetCurrentHPUpgrade(),
                data.GetCurrentWeaponUpgrade(),
                data.GetCurrentWeapon()
                );

            animator = GetComponentInChildren<Animator>();

            EventManager.Instance.SetIsWarpOnEvent -= SetWarpable;
            EventManager.Instance.SetIsWarpOnEvent += SetWarpable;
            EventManager.Instance.HitPlayerEvent -= GetDamaged;
            EventManager.Instance.HitPlayerEvent += GetDamaged;
            EventManager.Instance.GetIsPlayerWarpReadyEvent -= BindIsPlayerWarpReady;
            EventManager.Instance.GetIsPlayerWarpReadyEvent += BindIsPlayerWarpReady;
            EventManager.Instance.GetAnimatorEvent -= BindAnimator;
            EventManager.Instance.GetAnimatorEvent += BindAnimator;

            EventManager.Instance.GetPlayerStatEvent -= GetStat;
            EventManager.Instance.GetPlayerStatEvent += GetStat;

            EventManager.Instance.GetCurrentPlayerDamageEvent -= BindGetCurrentPlayerDamage;
            EventManager.Instance.GetCurrentPlayerDamageEvent += BindGetCurrentPlayerDamage;

            EventManager.Instance.GetProjectileSpeedEvent -= BindGetProjectileSpeed;
            EventManager.Instance.GetProjectileSpeedEvent += BindGetProjectileSpeed;

            EventManager.Instance.GetPlayerPositionEvent -= GetPlayerPosition;
            EventManager.Instance.GetPlayerPositionEvent += GetPlayerPosition;
        }

        private Vector3 GetPlayerPosition() => gameObject.transform.position;

        private float BindGetCurrentPlayerDamage() => playerCurrentStat.currentWeaponUpgrade * ResourceManager.Instance.GetWeaponData(playerCurrentStat.currentWeapon).GetUpgradeDamageDelta() + ResourceManager.Instance.GetWeaponData(playerCurrentStat.currentWeapon).GetBaseDamage();
        private float BindGetProjectileSpeed() => ResourceManager.Instance.GetWeaponData(playerCurrentStat.currentWeapon).GetProjectileSpeed();

        private void Start()
        {

            canWarp = true;

            StartCoroutine(SendPositionCoroutine()); // 코루틴 시작

            //플레이어 스탯 UI 초기화
            EventManager.Instance.OnUpdateHPUI(playerCurrentStat.currentHP, playerCurrentStat.currentMaxHP);
            EventManager.Instance.OnUpdateEXPUI(playerCurrentStat.currentEXP, playerCurrentStat.currentMaxEXP);
            EventManager.Instance.OnUpdateWeaponUI(playerCurrentStat.currentWeapon);


        }

        private void OnDestroy()
        {
            EventManager.Instance.SetIsWarpOnEvent -= SetWarpable;
            EventManager.Instance.HitPlayerEvent -= GetDamaged;
            EventManager.Instance.GetIsPlayerWarpReadyEvent -= BindIsPlayerWarpReady;
            EventManager.Instance.GetAnimatorEvent -= BindAnimator;

            EventManager.Instance.GetPlayerStatEvent -= GetStat;

            EventManager.Instance.GetCurrentPlayerDamageEvent -= BindGetCurrentPlayerDamage;
            EventManager.Instance.GetProjectileSpeedEvent -= BindGetProjectileSpeed;
            EventManager.Instance.GetPlayerPositionEvent -= GetPlayerPosition;
        }

        private PlayerCurrentStat GetStat() => playerCurrentStat;

        private bool BindIsPlayerWarpReady()
        {
            return canWarp;
        }

        private Animator BindAnimator()
        {
            return animator;
        }


        private void SetWarpable(bool enable)
        {
            canWarp = enable;
        }

        public void GetDamaged(float damage)
        {

            playerCurrentStat.UpdateHP(-damage);
            EventManager.Instance.OnFaceDamaged();
            EventManager.Instance.OnPlayAudioClip(ResourceManager.Instance.GetSound(Sounds.hitsound), 0.5f, false);


        }

        private IEnumerator SendPositionCoroutine()
        {
            yield return new WaitForSeconds(1f); // 최초 1초 대기
            while (true)
            {
                // 위치 이벤트 전송
                EventManager.Instance.OnSend($"POSITION:{(int)gameObject.transform.position.x},{(int)gameObject.transform.position.y},{(int)gameObject.transform.position.z}");

                // 1초마다 반복
                yield return new WaitForSeconds(1f);
            }
        }


    }
}