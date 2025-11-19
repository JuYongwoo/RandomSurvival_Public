using JYW.RandomSurvival.Managers;
using JYW.RandomSurvival.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace JYW.RandomSurvival.UIs
{
    public class HowToPlayPanel : MonoBehaviour
    {
        private enum PortraitPanelEnum
        {
            HowToPlayImg
        }
        private Dictionary<PortraitPanelEnum, GameObject> HowToPlaymap;

        private void Awake()
        {
            HowToPlaymap = Util.MapEnumChildObjects<PortraitPanelEnum, GameObject>(this.gameObject);

            EventManager.Instance.EnterEvent -= BindEnterEvent;
            EventManager.Instance.EnterEvent += BindEnterEvent;
        }
        private void Start()
        {
            HowToPlaymap[PortraitPanelEnum.HowToPlayImg].SetActive(true);
            Time.timeScale = 0;
        }

        private void BindEnterEvent()
        {
            HowToPlaymap[PortraitPanelEnum.HowToPlayImg].SetActive(false);
            Time.timeScale = 1;

        }
        private void OnDestroy()
        {
            EventManager.Instance.EnterEvent -= BindEnterEvent;

        }
    }
}