using JYW.RandomSurvival.Managers;
using JYW.RandomSurvival.Maps;
using JYW.RandomSurvival.Utils;
using UnityEngine;

namespace JYW.RandomSurvival.Scenes
{

    public class EditorScene : MonoBehaviour
    {
        private MapManager mapManager = new MapManager();

        private float moveSpeed = 5f;

        private bool rightMouseHeld = false;

        private void Awake()
        {
            mapManager.OnAwake(ResourceManager.Instance.GetMapCSV(), ResourceManager.Instance.GetTileDict());
            EventManager.Instance.GetMapEvent -= GetMap;
            EventManager.Instance.GetMapEvent += GetMap;
        }
        private void OnDestroy()
        {
            EventManager.Instance.GetMapEvent -= GetMap;
        }

        private MapManager GetMap()
        {
            return mapManager;
        }

        private void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            LayerMask mask = LayerMask.GetMask("MapEditor");

            if (Physics.Raycast(ray, out RaycastHit hit, 100, mask))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    //TODO JYW 타일들 라벨로 지정해서 맵으로 만든 후, 누를때마다 맵 다음 인덱스로 넘어가면서 종류 바뀌게 해야
                    mapManager.map[GridHelper.WorldToGrid(hit.transform.position).y, GridHelper.WorldToGrid(hit.transform.position).x] = "gw"; //sMap 내용 바꿔주고
                    GameObject go = MonoBehaviour.Instantiate(ResourceManager.Instance.GetTileDict("gw").GetPreFab(), hit.transform.position, Quaternion.identity); //비주얼 업데이트
                    go.name = "gw";
                    MonoBehaviour.Destroy(hit.transform.gameObject); //기존 보이던 것 제거


                }

            }


            // 우클릭 상태 체크
            if (Input.GetMouseButtonDown(1))
            {
                rightMouseHeld = true;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else if (Input.GetMouseButtonUp(1))
            {
                rightMouseHeld = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            if (rightMouseHeld)
            {
                HandleMouseMove();
            }
        }

        private void HandleMouseMove()
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            // 마우스 이동량을 월드 기준 좌/우, 앞/뒤 이동으로 변환
            Camera.main.transform.position += Camera.main.transform.right * mouseX;
            Camera.main.transform.position += Camera.main.transform.up * mouseY;
            //*Time.deltaTime
        }


    }
}