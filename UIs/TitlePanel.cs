using JYW.RandomSurvival.Managers;
using JYW.RandomSurvival.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace JYW.RandomSurvival.UIs
{
    public class TitlePanel : MonoBehaviour
    {
        private enum TitlePanelObjs
        {
            StartBtn,
            EditorBtn,
            QuitBtn
        }
        private Dictionary<TitlePanelObjs, GameObject> titleCanvasMap;

        // Start is called before the first frame update
        private void Awake()
        {
            titleCanvasMap = Util.MapEnumChildObjects<TitlePanelObjs, GameObject>(this.gameObject);
            titleCanvasMap[TitlePanelObjs.StartBtn].GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
            {
                EventManager.Instance.ChangeScene(Commmons.Scenes.Main);
            });

            titleCanvasMap[TitlePanelObjs.EditorBtn].GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
            {
                EventManager.Instance.ChangeScene(Commmons.Scenes.MapEditor);
            });

            titleCanvasMap[TitlePanelObjs.QuitBtn].GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
            {
                Application.Quit();
            });
        }

    }
}