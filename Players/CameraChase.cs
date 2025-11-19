using JYW.RandomSurvival.Managers;
using UnityEngine;

namespace JYW.RandomSurvival.Players
{
    public class CameraChase : MonoBehaviour
    {

        private void Update()
        {
            Vector3 playerPosition = EventManager.Instance.OnGetPlayerPosition();
            playerPosition += new Vector3(0, 13, -10.25f);
            transform.position = playerPosition;
        }

    }
}