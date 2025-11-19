using UnityEngine;

namespace JYW.RandomSurvival.SOs
{
    [CreateAssetMenu(fileName = "NewWeaponData", menuName = "Game/WeaponData")]
    public class WeaponDataSO : ScriptableObject
    {
        [SerializeField]
        private GameObject ProjectilePrefab;
        [SerializeField]
        private AudioClip ShootSound;
        [SerializeField]
        private Sprite WeaponIcon;
        [SerializeField]
        private string WeaponName;
        [SerializeField]
        private float BaseDamage;
        [SerializeField]
        private float UpgradeDamageDelta;
        [SerializeField]
        private float ProjectileSpeed;
        [SerializeField]
        private float ReloadTime;
        [SerializeField]
        private float Range;

        public GameObject GetProjectilePrefab()
        {
            return ProjectilePrefab;
        }
        public AudioClip GetShootSound()
        {
            return ShootSound;
        }
        public Sprite GetWeaponIcon()
        {
            return WeaponIcon;
        }
        public string GetWeaponName()
        {
            return WeaponName;
        }
        public float GetBaseDamage()
        {
            return BaseDamage;
        }
        public float GetUpgradeDamageDelta()
        {
            return UpgradeDamageDelta;
        }
        public float GetProjectileSpeed()
        {
            return ProjectileSpeed;
        }
        public float GetReloadTime()
        {
            return ReloadTime;
        }
        public float GetRange()
        {
            return Range;
        }

    }
}