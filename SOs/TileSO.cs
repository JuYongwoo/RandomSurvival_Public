using UnityEngine;

namespace JYW.RandomSurvival.SOs
{
    [CreateAssetMenu(fileName = "NewTileData", menuName = "Game/TileData")]
    public class TileSO : ScriptableObject //기획에 따라 플레이어 시작 스탯 수정 필요
    {
        [SerializeField]
        private GameObject preFab;
        [SerializeField]
        private Sprite Icon;

        public GameObject GetPreFab() => preFab;
        public Sprite GetIcon() => Icon;

    }
}