using UnityEngine;

namespace JYW.RandomSurvival.Players
{

    public class EditorCamera : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKey(KeyCode.A))
            {
                transform.position += Vector3.left * Time.deltaTime * 10;
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.position += Vector3.right * Time.deltaTime * 10;
            }
            if (Input.GetKey(KeyCode.W))
            {
                transform.position += Vector3.forward * Time.deltaTime * 10;
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.position += Vector3.back * Time.deltaTime * 10;
            }
        }
    }
}