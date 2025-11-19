using JYW.RandomSurvival.Managers;
using System.Collections;
using UnityEngine;

namespace JYW.RandomSurvival.Items
{
    public class Teleport : MonoBehaviour
    {
        public GameObject destination;



        private void OnTriggerEnter(Collider col)
        {
            if (col.tag == "Player")
            {
                if (EventManager.Instance.OnGetIsPlayerWarpReady())
                {
                    col.gameObject.SetActive(false); // 순간적으로 껐다 켜줘야 이동에 문제가 없음

                    col.transform.position = new Vector3(destination.transform.position.x, col.transform.position.y, destination.transform.position.z);

                    col.gameObject.SetActive(true);
                    StartCoroutine(warpOnCoroutine());

                }

            }
        }

        IEnumerator warpOnCoroutine()
        {
            EventManager.Instance.OnSetIsWarpOn(false);
            yield return new WaitForSeconds(3f);
            EventManager.Instance.OnSetIsWarpOn(true);

        }
    }
}