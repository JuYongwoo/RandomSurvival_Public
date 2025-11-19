using UnityEngine;

namespace JYW.RandomSurvival.Utils
{
    public static class GridHelper
    {
        public const int MAP_MIN = -1000;
        public const int MAP_MAX = 1000;
        public const int MAP_SIZE = MAP_MAX - MAP_MIN + 1;

        /// <summary>
        /// 월드 좌표 → 그리드 좌표
        /// -0.5~0.4999 → 0, 0.5~1.4999 → 1, ...
        /// </summary>
        public static Vector2Int WorldToGrid(Vector3 worldPos)
        {
            int gx = Mathf.RoundToInt(worldPos.x);
            int gy = -Mathf.RoundToInt(worldPos.z);
            return new Vector2Int(gx, gy);
        }

        /// <summary>
        /// 그리드 좌표 → 월드 좌표
        /// 그리드 좌표 (n)을 그대로 중심점으로 사용
        /// </summary>
        public static Vector3 GridToWorld(Vector2Int gridPos)
        {
            return new Vector3(gridPos.x, 0, -gridPos.y);
        }

        public static int ToIndexX(int gx) => gx - MAP_MIN;
        public static int ToIndexY(int gy) => gy - MAP_MIN;

        public static bool InBounds(Vector2Int g)
        {
            return g.x >= MAP_MIN && g.x <= MAP_MAX &&
                   g.y >= MAP_MIN && g.y <= MAP_MAX;
        }
    }
}