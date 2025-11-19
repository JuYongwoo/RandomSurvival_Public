using Unity.Hierarchy;

namespace JYW.RandomSurvival.Commmons
{
    public enum Scenes
    {
        Load,
        Title,
        Main,
        MapEditor
    }

    public enum Weapons
    {
        Hand,
        Bow,
        Magic,
        Gun
    }

    public enum Sounds
    {
        BGM,
        hitsound,
        coinsound


    }


    public enum PlayerState
    {
        Idle,
        MoveStart,
        Moving,
        AttackStart,
        Attack_Attacking,
        Attack_MoveStart,
        Attack_Moving,
        AttackMove_MoveStart,
        AttackMove_Moving,
        AttackMove_Attacking
    }


    public enum EnemyType
    {
        Chase,
        Hold,
        NonAttack
    }

    public class PlayerCurrentStat// 서버로부터 정보를 받은 모든 플레이어는 이 정보를 가지고 있도록 한다.
    {
        public PlayerCurrentStat(float hp, float maxHP, float exp, float maxEXP, float moveSpeed, int hpUpgrade, int weaponUpgrade, Weapons weapon) //SO 값을 토대로 시작 값 초기화
        {
            this.currentHP = hp;
            this.currentMaxHP = maxHP;
            this.currentEXP = exp;
            this.currentMaxEXP = maxEXP;
            this.currentMoveSpeed = moveSpeed;
            this.currentHPUpgrade = hpUpgrade;
            this.currentWeaponUpgrade = weaponUpgrade;
            this.currentWeapon = weapon;
        }


        public void UpdateHP(float hp)
        {
            currentHP += hp;
        }
        public void UpdateEXP(float exp)
        {
            currentEXP += exp;
        }
        public void UpdateCurrentWeapon(Weapons weapon)
        {
            currentWeapon = weapon;
        }

        public Weapons GetWeapon()
        {
            return currentWeapon;
        }


        public float currentMaxHP;
        public float currentHP;
        public float currentMoveSpeed;
        public float currentEXP;
        public float currentMaxEXP;
        public Weapons currentWeapon; // 현재 장착된 무기
        public int currentWeaponUpgrade; // 업그레이드 레벨
        public int currentHPUpgrade; // 업그레이드 레벨

    }
}