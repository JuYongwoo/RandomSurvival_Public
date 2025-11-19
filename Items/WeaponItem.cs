using JYW.RandomSurvival.Commmons;
using JYW.RandomSurvival.Managers;
using UnityEngine;

namespace JYW.RandomSurvival.Items
{

    public class WeaponItem : MonoBehaviour
    {
        [SerializeField]
        private Weapons weaponType;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                EventManager.Instance.OnGetGetPlayerStat().currentWeapon = weaponType;
                EventManager.Instance.OnUpdateWeaponUI(weaponType);
                Destroy(gameObject);
            }
        }
    }
}