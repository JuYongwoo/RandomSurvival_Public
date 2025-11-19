using JYW.RandomSurvival.SOs;
using JYW.RandomSurvival.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JYW.RandomSurvival.Maps
{
    public class MapManager
    {

        public string[,] map;
        public Dictionary<string, TileSO> tileMap;
        public void OnAwake(TextAsset csv, Dictionary<string, TileSO> tilemap)
        {

            this.tileMap = tilemap;
            //0915 사용, 라벨단위로 모든 타일 가져온다.

            //csv 텍스트 덩어리를 줄로 쪼갠다 string이 string[]로 변환
            string[] lines = csv.text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries); //줄단위 split

            map = new string[lines.Length, lines[0].Trim().Split(',').Length]; //trim 후 ',' 단위 split

            for (int y = 0; y < map.GetLength(0); y++)
            {
                //텍스트 줄을 콤마 단위로 쪼갠다 string이 string[]로 변환
                string[] line = lines[y].Trim().Split(','); //trim 후 ',' 단위 split (string을 여러개, char 아님)
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    //주의: 이전에는 key값으로만 비교했는데 추후 문제가 생길 수 있어 "Tile"라벨 단위로 미리 불러오는게 나을 수 있음 0915 deprecate, 에디터에서 모든 타일 다루기 위해
                    //if (!TileMap.ContainsKey(line[x])) TileMap.Add(line[x], Addressables.LoadAssetAsync<TileSO>(line[x]).WaitForCompletion());
                    map[y, x] = line[x];
                }
            }
            //////
            ///

            List<GameObject> gos = new List<GameObject>();
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    gos.Add(UnityEngine.Object.Instantiate(tilemap[map[i, j]].GetPreFab(), GridHelper.GridToWorld(new Vector2Int(j, i)), Quaternion.identity));
                    gos[gos.Count - 1].name = map[i, j]; //오브젝트 이름은 키값(마크)과 같게(추후 이름으로 찾을 시를 위해)

                }
            }

            /////////////////////에디터 씬일 경우에만
            if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("MapEditor")) //에디터 씬일 경우 레이어를 바꾸고 카메라를 이동
            {
                foreach (var go in gos)
                {
                    if (go.name == "s")
                    {
                        UnityEngine.Object.Destroy(go.transform.GetChild(0).gameObject); //에디터 씬일 경우 플레이어 오브젝트 파괴(바닥은 놔둬서 타일 클릭 및 변경 가능하게)
                        break;
                    }
                    go.layer = LayerMask.NameToLayer("MapEditor");

                }
            }
        }

    }
}