using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;
using UnityEngine;
using JYW.RandomSurvival.Managers;

namespace JYW.RandomSurvival.Players
{

    public class DistanceRenderController : MonoBehaviour
    {
        [SerializeField]
        private float CameraRenderDisableDistance;
        private List<Renderer> targetRenderers = new List<Renderer>();
        private float disableDistanceSqr;

        private void Start()
        {
            disableDistanceSqr = CameraRenderDisableDistance * CameraRenderDisableDistance;

            Renderer[] allRenderers = FindObjectsByType<Renderer>(FindObjectsSortMode.None);
            foreach (var r in allRenderers)
            {
                if (r.gameObject.layer != LayerMask.NameToLayer("UI")) // UI 레이어는 제외
                    targetRenderers.Add(r);
            }
            InvokeRepeating(nameof(RenderUpdate), 0, 1); //1초마다 렌더 업데이트 TODO: 플레이어 이동 거리로 변경? // 이동 거리를 매번 체크하는게 더 비용이 클 수도 // 입력에 따라 업데이트도 가능
        }

        private void RenderUpdate()
        {
            if (targetRenderers.Count == 0) return;

            Vector3 camPos = EventManager.Instance.OnGetPlayerPosition();

            int count = targetRenderers.Count;
            NativeArray<Vector3> positions = new NativeArray<Vector3>(count, Allocator.TempJob);
            NativeArray<bool> results = new NativeArray<bool>(count, Allocator.TempJob);

            for (int i = 0; i < count; i++)
            {
                if (targetRenderers[i] != null)
                    positions[i] = targetRenderers[i].transform.position;
            }

            DistanceCheckJob job = new DistanceCheckJob //2. 묶은 struct를 선언하고 내부에 값들을 넣는다.
            {
                camPos = camPos,
                disableDistanceSqr = disableDistanceSqr,
                positions = positions,
                results = results
            };

            job.Schedule(count, 64).Complete(); // 3. 위에서 선언한 구조체 변수.Schedule(전체를 몇개 씩 쪼개서 병렬로 실행할 것인가).Complete()를 호출하여 잡을 실행한다.


            /////////////////병렬로 계산
            for (int i = 0; i < count; i++)
            {
                var rend = targetRenderers[i];
                if (rend == null) continue;

                bool shouldEnable = results[i];
                if (rend.enabled != shouldEnable)
                    rend.enabled = shouldEnable;
            }

            positions.Dispose();
            results.Dispose();
        }

        [BurstCompile]
        private struct DistanceCheckJob : IJobParallelFor //1. 병렬로 처리할 것들을 struct로 묶는다.
        {

            //잡에 필요한 값들, 내부에서 쓰이는 모든 변수는 이 안에 선언되어야 한다. 밖에서 공용으로 사용할 수 있는 변수더라도 반드시 이 안에 선언
            [ReadOnly] public Vector3 camPos;
            [ReadOnly] public float disableDistanceSqr;
            [ReadOnly] public NativeArray<Vector3> positions;
            [WriteOnly] public NativeArray<bool> results;


            //잡이 병렬로 처리할 함수 Execute
            public void Execute(int index)
            {
                Vector3 objPos = positions[index];
                float sqrDist = (camPos - objPos).sqrMagnitude;
                results[index] = sqrDist <= disableDistanceSqr; //disableDistanceSqr보다 작으면 true, 렌더 켠다
            }
        }
    }
}