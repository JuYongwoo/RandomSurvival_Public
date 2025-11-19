using JYW.RandomSurvival.Managers;
using UnityEngine;

namespace JYW.RandomSurvival.Items
{
    public class Spawner : MonoBehaviour
    {
        private bool isActive = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !isActive)
            {
                isActive = true;
                InvokeRepeating("spawnEnemy", 0f, 5f);
            }
        }


        private void spawnEnemy()
        {
            Vector3 rd_position = transform.position;
            GameObject go = Instantiate(ResourceManager.Instance.GetEnemyPrefab(), rd_position, Quaternion.identity);
        }
    }
}