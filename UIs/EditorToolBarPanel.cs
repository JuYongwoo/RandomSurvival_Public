using JYW.RandomSurvival.Managers;
using JYW.RandomSurvival.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JYW.RandomSurvival.UIs
{
    public class EditorToolBarPanel : MonoBehaviour
    {
        private enum EditorToolBarObjs
        {
            EditorToolBarSaveBtn,
            EditorToolBarCancelBtn,
            EditorToolBarContents
        }

        private Dictionary<EditorToolBarObjs, GameObject> editorToolBarMap;



        private void Awake()
        {
            editorToolBarMap = Util.MapEnumChildObjects<EditorToolBarObjs, GameObject>(this.gameObject);
            //에디터 툴바 버튼 생성
            foreach (var tile in ResourceManager.Instance.GetTileDict())
            {
                AddTileBtn(tile.Key.ToString()); //모든 타일맵의 키를 인자로 하여 개수만큼 반복한다
            }
            //


            editorToolBarMap[EditorToolBarObjs.EditorToolBarSaveBtn].GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
            {
                Debug.Log("Save Button Clicked");
                GridUtil.SaveCsv(EventManager.Instance.OnGetMap().map);
            });

            editorToolBarMap[EditorToolBarObjs.EditorToolBarCancelBtn].GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
            {
                Debug.Log("Cancel Button Clicked");
                UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
            });
        }

        private void AddTileBtn(string tileName)
        {
            GameObject go = Instantiate(ResourceManager.Instance.GetTileBtnBasePrefab());
            go.transform.GetChild(0).GetComponent<Image>().sprite = ResourceManager.Instance.GetTileDict(tileName).GetIcon();
            go.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = tileName.ToString();
            go.transform.SetParent(editorToolBarMap[EditorToolBarObjs.EditorToolBarContents].transform, false);
            //TODO JYW 여기서 버튼에 리스너 등록, 누르면 현재 에디터에서 마우스에 올려져있는 프리팹 변경되도록

        }

    }
}