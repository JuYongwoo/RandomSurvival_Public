using UnityEngine;

namespace JYW.RandomSurvival.SOs
{

    [CreateAssetMenu(fileName = "NewGameModeData", menuName = "Game/GameModeData")]
    public class GameModeSO : ScriptableObject //게임 관련 데이터 SO
    {
        public float CameraPositionDistance;
        public Vector3 CameraOffset;
        public float CameraRenderDisableDistance;
        public int GameTime;

    }
}