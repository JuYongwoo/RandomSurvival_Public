using JYW.RandomSurvival.Managers;
using JYW.RandomSurvival.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPanel : MonoBehaviour
{

    private enum LoadingPanelObjs
    {
        LoadingSlider
    }

    private Dictionary<LoadingPanelObjs, GameObject> loadingPanelObjsDict = new Dictionary<LoadingPanelObjs, GameObject>();

    private void Awake()
    {
        loadingPanelObjsDict = Util.MapEnumChildObjects<LoadingPanelObjs, GameObject>(gameObject);
    }

    private void Start()
    {

        EventManager.Instance.UpdateLoadingGaugeEvent -= UpdateGauge;
        EventManager.Instance.UpdateLoadingGaugeEvent += UpdateGauge;

    }

    private void OnDestroy()
    {
        EventManager.Instance.UpdateLoadingGaugeEvent -= UpdateGauge;
    }

    private void UpdateGauge(float percentage)
    {
        loadingPanelObjsDict[LoadingPanelObjs.LoadingSlider].GetComponent<Slider>().value = percentage;
    }
}