using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace JYW.RandomSurvival.Utils
{
    public class Node : IComparable<Node>
    {
        public Vector2Int pos;
        public float priority; // fScore

        public Node(Vector2Int p, float f)
        {
            pos = p;
            priority = f;
        }

        public int CompareTo(Node other)
        {
            // priority가 낮을수록 우선순위가 높도록 //AStar 계산 시 휴리스틱 계산값이 들어갈 것임
            return -priority.CompareTo(other.priority);
        }
    }


    public class GridUtil
    {



        public static List<List<string>> LoadGrid(string AddressableFileKey)
        {
            var textAsset = Addressables.LoadAssetAsync<TextAsset>(AddressableFileKey).WaitForCompletion();
            if (textAsset == null)
            {
                Debug.LogError($"[Util] Addressable '{AddressableFileKey}'을(를) 찾을 수 없습니다.");
                return new List<List<string>>();
            }

            string[] lines = textAsset.text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            if (lines.Length == 0)
            {
                Debug.LogWarning($"[Util] Addressable '{AddressableFileKey}'의 내용이 비어 있습니다.");
                return new List<List<string>>();
            }

            string[] firstLine = lines[0].Trim().Split(',');
            int cols = firstLine.Length;
            List<List<string>> gridvalue = new List<List<string>>();

            for (int i = 0; i < lines.Length; i++)
            {
                string[] row = lines[i].Trim().Split(',');
                var rowList = new List<string>();
                for (int j = 0; j < cols && j < row.Length; j++)
                {
                    var cell = row[j].Trim();
                    if (!string.IsNullOrEmpty(cell))
                        rowList.Add(cell);
                }
                gridvalue.Add(rowList);
            }
            return gridvalue;
        }

        public static void SaveCsv(string[,] map)
        {
            if (map == null)
                return;

            int rows = map.GetLength(0);
            int cols = map.GetLength(1);

            StringBuilder sb = new StringBuilder();

            for (int r = 0; r < rows; r++)
            {
                string[] row = new string[cols];
                for (int c = 0; c < cols; c++)
                {
                    string value = map[r, c] ?? "";
                    if (value.Contains(",") || value.Contains("\""))
                    {
                        value = "\"" + value.Replace("\"", "\"\"") + "\"";
                    }
                    row[c] = value;
                }
                sb.AppendLine(string.Join(",", row));
            }

            File.WriteAllText(Application.dataPath + "/SavedMap.csv", sb.ToString(), Encoding.UTF8);
        }


        // ====== 여기부터 경로 탐색(대각선 지원) ======

        // 4방향 / 8방향
        static readonly Vector2Int[] DIRS4 = {
        new Vector2Int( 1, 0),
        new Vector2Int(-1, 0),
        new Vector2Int( 0, 1),
        new Vector2Int( 0,-1),
    };

        static readonly Vector2Int[] DIRS8 = {
        new Vector2Int( 1, 0),
        new Vector2Int(-1, 0),
        new Vector2Int( 0, 1),
        new Vector2Int( 0,-1),
        new Vector2Int( 1, 1),
        new Vector2Int( 1,-1),
        new Vector2Int(-1, 1),
        new Vector2Int(-1,-1),
    };

        // 장애물 체크
        private static bool IsInBounds(Vector2Int p, int w, int h)
            => (uint)p.x < (uint)w && (uint)p.y < (uint)h;

        private static bool IsWalkable(string[,] map, Vector2Int p)
            => map[p.y, p.x] != "x";

        // Octile 휴리스틱 (대각선 비용 √2, 직선 1)
        private static float HeuristicOctile(Vector2Int a, Vector2Int b)
        {
            int dx = Mathf.Abs(a.x - b.x);
            int dy = Mathf.Abs(a.y - b.y);
            const float D = 1f;
            const float D2 = 1.41421356237f; // Mathf.Sqrt(2f)
            return D * (dx + dy) + (D2 - 2f * D) * Mathf.Min(dx, dy);
        }

        private static float HeuristicManhattan(Vector2Int a, Vector2Int b)
            => Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);

        /// <summary>
        /// A* 경로탐색 (대각선 지원)
        /// </summary>
        /// <param name="start">그리드 시작</param>
        /// <param name="goal">그리드 목적지</param>
        /// <param name="pMap">타일 맵</param>
        /// <param name="allowDiagonal">대각선 허용 여부 (기본: 허용)</param>
        /// <param name="preventCornerCutting">모서리 끼워들기 금지 (기본: 금지)</param>
        public static List<Vector2Int> FindPathwithAStar(Vector2Int start, Vector2Int goal, string[,] pMap, bool allowDiagonal = true, bool preventCornerCutting = true)
        {
            int width = pMap.GetLength(0);
            int height = pMap.GetLength(1);

            // 시작/목표 유효성
            if (!IsInBounds(start, width, height) || !IsInBounds(goal, width, height))
                return null;
            if (!IsWalkable(pMap, start) || !IsWalkable(pMap, goal))
                return null;

            var dirs = allowDiagonal ? DIRS8 : DIRS4;

            var openSet = new PriorityQueue<Node>();               // (기존 프로젝트의 PriorityQueue 사용)
            var cameFrom = new Dictionary<Vector2Int, Vector2Int>();
            var gScore = new Dictionary<Vector2Int, float>();
            var fScore = new Dictionary<Vector2Int, float>();
            var closed = new HashSet<Vector2Int>();

            gScore[start] = 0f;
            fScore[start] = allowDiagonal ? HeuristicOctile(start, goal) : HeuristicManhattan(start, goal);
            openSet.Push(new Node(start, fScore[start]));

            while (openSet.Count > 0)
            {
                Node currentNode = openSet.Pop();
                Vector2Int current = currentNode.pos;

                if (current == goal)
                    return Reconstruct(cameFrom, current);

                if (closed.Contains(current))
                    continue;
                closed.Add(current);

                foreach (var d in dirs)
                {
                    Vector2Int neighbor = current + d;
                    if (!IsInBounds(neighbor, width, height))
                        continue;
                    if (!IsWalkable(pMap, neighbor))
                        continue;

                    // 대각선 이동 시 모서리 끼워들기 방지
                    bool isDiag = (d.x != 0 && d.y != 0);
                    if (isDiag && preventCornerCutting)
                    {
                        // 현재에서 수평/수직 인접 두 칸 중 하나라도 벽이면 대각선 금지
                        var sideA = new Vector2Int(current.x + d.x, current.y);
                        var sideB = new Vector2Int(current.x, current.y + d.y);

                        if (!IsInBounds(sideA, width, height) || !IsInBounds(sideB, width, height))
                            continue;
                        if (!IsWalkable(pMap, sideA) || !IsWalkable(pMap, sideB))
                            continue;
                    }

                    float stepCost = isDiag ? 1.41421356237f : 1f;
                    float tentativeG = gScore[current] + stepCost;

                    if (gScore.TryGetValue(neighbor, out float oldG) && tentativeG >= oldG)
                        continue;

                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeG;
                    float h = allowDiagonal ? HeuristicOctile(neighbor, goal) : HeuristicManhattan(neighbor, goal);
                    fScore[neighbor] = tentativeG + h;

                    openSet.Push(new Node(neighbor, fScore[neighbor]));
                }
            }
            return null; // 경로 없음
        }

        private static List<Vector2Int> Reconstruct(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current)
        {
            var path = new List<Vector2Int> { current };
            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current]; //도착지점의 부모(골 지점부터 시작 지점으로 반대로 이동)
                path.Insert(0, current);//맨 앞에 삽입
            }
            path.RemoveAt(0); // 시작점 제외
            return path;
        }


        static public IEnumerator goToIEnu(string[,] pMap, Transform trs, Vector3 destination, float moveSpeed, Action afterAction = null)
        {
            List<Vector2Int> path = GridUtil.FindPathwithAStar(GridHelper.WorldToGrid(trs.position), GridHelper.WorldToGrid(destination), pMap);


            foreach (var cell in path) //리스트를 따라 한 칸 한 칸 이동
            {
                Vector3 targetPos = GridHelper.GridToWorld(cell);

                while ((trs.position - targetPos).sqrMagnitude > 0.001f) //다음 칸까지 멀었으면
                {
                    trs.position = Vector3.MoveTowards(trs.position, targetPos, moveSpeed * Time.deltaTime);
                    trs.LookAt(targetPos);
                    yield return null;
                }

                trs.position = targetPos; // 마지막에 딱 맞추기 위 0.001f 오차 방지
            }
            if (afterAction != null) afterAction();
        }
    }
}