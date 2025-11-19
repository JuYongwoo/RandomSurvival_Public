using JYW.RandomSurvival.Managers;
using UnityEngine;

namespace JYW.RandomSurvival.Items
{
    public class AttackProjectile : MonoBehaviour
    {

        private Transform target;

        [SerializeField] private Vector3 rotationOffset;
        // 발사체 기본 메쉬 방향 보정용 (x,y,z 회전 오프셋)

        public void SetTarget(Transform t)
        {
            target = t;
        }

        void Update()
        {
            if (target == null)
            {
                Destroy(gameObject);
                return;
            }

            // 이동 처리
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * EventManager.Instance.OngetProjectileSpeed() * Time.deltaTime;

            // 회전 처리: 목표를 바라보도록
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                Quaternion offsetRotation = Quaternion.Euler(rotationOffset);
                transform.rotation = lookRotation * offsetRotation;
            }

            // 타격 판정
            if (Vector3.Distance(transform.position, target.position) < 0.5f)
            {
                EnemyBase attackable = target.GetComponentInParent<EnemyBase>();
                if (attackable != null)
                    attackable.TakeDamage(EventManager.Instance.OngetCurrentPlayerDamage());

                Destroy(gameObject);
            }
        }
    }
}
