using JYW.RandomSurvival.Commmons;
using JYW.RandomSurvival.Managers;
using JYW.RandomSurvival.Maps;
using JYW.RandomSurvival.Utils;
using UnityEngine;

namespace JYW.RandomSurvival.Scenes
{
    public class MainScene : MonoBehaviour
    {
        private MapManager mapManager = new MapManager();
        private Texture2D currentCursor = null;
        private int lefttime;

        private void Awake()
        {
            mapManager.OnAwake(ResourceManager.Instance.GetMapCSV(), ResourceManager.Instance.GetTileDict());
            EventManager.Instance.GetMapEvent -= GetMap;
            EventManager.Instance.GetMapEvent += GetMap;
        }
        private void Start()
        {

            EventManager.Instance.OnPlayAudioClip(ResourceManager.Instance.GetSound(Sounds.BGM), 0.3f, true);

            lefttime = ResourceManager.Instance.GetGameModeData().GameTime;
            InvokeRepeating(nameof(Refreshlefttime), 0f, 1f);
        }
        private void OnDestroy()
        {
            EventManager.Instance.GetMapEvent -= GetMap;
        }

        private MapManager GetMap() => mapManager;

        private void Refreshlefttime()
        {
            EventManager.Instance.OnRefreshLeftTimeUI(lefttime);
            lefttime--;

        }

        private void Update()
        {

            if (Input.GetKeyDown(KeyCode.Return))
                EventManager.Instance.OnEnterEvent();

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            LayerMask mask = LayerMask.GetMask("EnemyClick", "Ground");

            if (Physics.Raycast(ray, out RaycastHit hit, 100, mask))
            {


                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("EnemyClick")) //적 마우스
                {
                    SetCursor(ResourceManager.Instance.GetAttackCursor());

                    if (Input.GetKeyDown(KeyCode.A) || Input.GetMouseButtonDown(1))
                    {
                        EventManager.Instance.OnSetAttackTarget(hit.transform.gameObject);
                        EventManager.Instance.OnSetPlayerState(PlayerState.AttackStart);

                    }
                    return;
                }
                else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground")) //땅 마우스
                {
                    SetCursor(null);
                    if (mapManager.map[GridHelper.WorldToGrid(hit.point).y, GridHelper.WorldToGrid(hit.point).x] == "x") return; //벽이 있는 땅이면 리턴

                    if (Input.GetMouseButtonDown(1))
                    {
                        EventManager.Instance.OnSetDestination(hit.point);
                        EventManager.Instance.OnSetPlayerState(PlayerState.MoveStart);
                    }
                    if (Input.GetKeyDown(KeyCode.A))
                    {
                        EventManager.Instance.OnSetDestination(hit.point);
                        EventManager.Instance.OnSetPlayerState(PlayerState.AttackMove_MoveStart);
                    }
                    return;

                }
                else
                {
                    SetCursor(null);
                    return;

                }
            }
        }

        private void SetCursor(Texture2D attackCursor)
        {
            currentCursor = attackCursor;
            Cursor.SetCursor(currentCursor, Vector2.zero, CursorMode.Auto);
        }
    }
}