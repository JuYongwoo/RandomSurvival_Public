using JYW.RandomSurvival.Commmons;
using UnityEngine;

namespace JYW.RandomSurvival.SOs
{

    [CreateAssetMenu(fileName = "NewPlayerData", menuName = "Game/PlayerData")]
    public class PlayerDataSO : ScriptableObject //기획에 따라 플레이어 시작 스탯 수정 필요
    {
        [SerializeField]
        private float CurrentMoveSpeed;
        [SerializeField]
        private float CurrentMaxHP;
        [SerializeField]
        private float CurrentHP;
        [SerializeField]
        private int CurrentEXP;
        [SerializeField]
        private int CurrentMaxEXP;
        [SerializeField]
        private int CurrentHPUpgrade;
        [SerializeField]
        private int CurrentWeaponUpgrade;
        [SerializeField]
        private Weapons CurrentWeapon;

        public float GetCurrentMoveSpeed()
        {
            return CurrentMoveSpeed;
        }
        public float GetCurrentMaxHP()
        {
            return CurrentMaxHP;
        }
        public float GetCurrentHP()
        {
            return CurrentHP;
        }
        public int GetCurrentEXP()
        {
            return CurrentEXP;
        }
        public int GetCurrentMaxEXP()
        {
            return CurrentMaxEXP;
        }
        public int GetCurrentHPUpgrade()
        {
            return CurrentHPUpgrade;
        }
        public int GetCurrentWeaponUpgrade()
        {
            return CurrentWeaponUpgrade;
        }
        public Weapons GetCurrentWeapon()
        {
            return CurrentWeapon;
        }
    }
}