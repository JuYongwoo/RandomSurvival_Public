using JYW.RandomSurvival.Managers;
using JYW.RandomSurvival.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JYW.RandomSurvival.UIs
{
    public class PortraitPanel : MonoBehaviour
    {

        private enum PortraitPanelObj
        {
            HPBar,
            HPTxt,
            Remaining
        }
        private Dictionary<PortraitPanelObj, GameObject> PortraitPanelObjDict;
        private const float SliderVisualOffset = 0.01f;

        // Start is called before the first frame update
        private void Awake()
        {
            PortraitPanelObjDict = Util.MapEnumChildObjects<PortraitPanelObj, GameObject>(this.gameObject);

            EventManager.Instance.UpdateHPUIEvent -= BindRefershHPBar;
            EventManager.Instance.UpdateHPUIEvent += BindRefershHPBar;
        }

        private void BindRefershHPBar(float HP, float MaxHP)
        {
            if (PortraitPanelObjDict[PortraitPanelObj.HPBar] == null
        || PortraitPanelObjDict[PortraitPanelObj.HPTxt] == null) return;
            PortraitPanelObjDict[PortraitPanelObj.HPBar].GetComponent<Slider>().value = HP / MaxHP + SliderVisualOffset;
            PortraitPanelObjDict[PortraitPanelObj.HPTxt].GetComponent<Text>().text = $"{(int)HP}/{MaxHP}";
        }

        private void OnDestroy()
        {
            EventManager.Instance.UpdateHPUIEvent -= BindRefershHPBar;

        }
    }
}