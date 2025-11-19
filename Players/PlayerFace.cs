using JYW.RandomSurvival.Managers;
using JYW.RandomSurvival.Utils;
using System.Collections;
using UnityEngine;

namespace JYW.RandomSurvival.Players
{

    public class PlayerFace : MonoBehaviour
    {
        private Animator Faceanim;

        private void Awake()
        {
            Faceanim = Util.AddOrGetComponent<Animator>(gameObject);
            Faceanim.runtimeAnimatorController = ResourceManager.Instance.GetPlayerFace();

            EventManager.Instance.FaceDamagedEvent -= ChangeFaceDamage;
            EventManager.Instance.FaceDamagedEvent += ChangeFaceDamage;
        }

        private void Start()
        {
            Faceanim.SetBool("Damaged", false);

        }
        private IEnumerator ResetFace()
        {
            yield return new WaitForSeconds(0.75f);
            Faceanim.SetBool("Damaged", false);
        }

        private void OnDestroy()
        {
            EventManager.Instance.FaceDamagedEvent -= ChangeFaceDamage;
        }

        private void ChangeFaceDamage()
        {
            Faceanim.SetBool("Damaged", true);
            StartCoroutine(ResetFace());
        }
    }
}