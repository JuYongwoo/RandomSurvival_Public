using JYW.RandomSurvival.Commmons;
using JYW.RandomSurvival.Managers;
using UnityEngine;

namespace JYW.RandomSurvival.Items
{
    public class Coin : MonoBehaviour
    {



        private void OnTriggerEnter(Collider other)
        {

            if (!other.CompareTag("Player")) return;

            EventManager.Instance.OnPlayAudioClip(ResourceManager.Instance.GetSound(Sounds.coinsound), 0.5f, false);

            gameObject.SetActive(false);
        }


        private void Update()
        {
            transform.Rotate(Vector3.forward * Time.deltaTime * 100);

        }
    }
}