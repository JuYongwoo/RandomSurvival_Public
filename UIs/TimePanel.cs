using JYW.RandomSurvival.Managers;
using JYW.RandomSurvival.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JYW.RandomSurvival.UIs
{
    public class TimePanel : MonoBehaviour
    {
        private enum TimePanelEnum
        {
            RemainingText
        }
        private Dictionary<TimePanelEnum, GameObject> TimePanelmap;

        private void Awake()
        {
            TimePanelmap = Util.MapEnumChildObjects<TimePanelEnum, GameObject>(this.gameObject);
            EventManager.Instance.RefreshLeftTimeUIEvent -= RefreshLeftTimeUI;
            EventManager.Instance.RefreshLeftTimeUIEvent += RefreshLeftTimeUI;
        }

        private void OnDestroy()
        {
            EventManager.Instance.RefreshLeftTimeUIEvent -= RefreshLeftTimeUI;

        }

        private void RefreshLeftTimeUI(int lefttime)
        {
            string minutesString = lefttime / 60 < 10 ? $"0{(lefttime / 60)}" : $"{(lefttime / 60)}"; // 분이 10보다 작으면 앞에 0을 붙임
            string seconsdString = lefttime % 60 < 10 ? $"0{(lefttime % 60)}" : $"{(lefttime % 60)}"; // 초가 10보다 작으면 앞에 0을 붙임
            string timeString = $"{minutesString}:{seconsdString}";
            TimePanelmap[TimePanelEnum.RemainingText].GetComponent<Text>().text = timeString;
        }
    }
}