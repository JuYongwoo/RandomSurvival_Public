using JYW.RandomSurvival.Managers;
using JYW.RandomSurvival.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JYW.RandomSurvival.UIs
{

    public class CountPanel : MonoBehaviour
    {
        private enum CountPanelEnum
        {
            EnemyCountTxt
        }
        private Dictionary<CountPanelEnum, GameObject> countPanelMap;
        private Text enemyCountTxt;
        private int enemyCount = 0;

        private void Awake()
        {
            countPanelMap = Util.MapEnumChildObjects<CountPanelEnum, GameObject>(this.gameObject);
            enemyCountTxt = countPanelMap[CountPanelEnum.EnemyCountTxt].GetComponent<Text>();
            EventManager.Instance.DeltaEnemyCountEvent -= DeltaEnemyCountUI;
            EventManager.Instance.DeltaEnemyCountEvent += DeltaEnemyCountUI;
        }

        private void Start()
        {
            DeltaEnemyCountUI(0);
        }

        private void OnDestroy()
        {
            EventManager.Instance.DeltaEnemyCountEvent -= DeltaEnemyCountUI;

        }

        private void DeltaEnemyCountUI(int delta)
        {
            if (enemyCountTxt == null) return;
            enemyCount += delta;
            enemyCountTxt.text = $"Enemy {enemyCount}";
        }
    }
}