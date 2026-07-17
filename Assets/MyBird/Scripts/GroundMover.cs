using UnityEngine;

namespace MyBird
{
    public class GroundMover : MonoBehaviour
    {
        [Tooltip("배치된 바닥 조각의 총 개수 (예: 2)")]
        public int totalGroundCount = 2;

        private float tileWidth = 1f;
        private Transform mainCamera;
        private float recycleDistance;

        void Start()
        {
            mainCamera = Camera.main.transform;
            Camera cam = Camera.main;

            if (TryGetComponent<SpriteRenderer>(out var sr))
            {
                tileWidth = sr.bounds.size.x;
            }
            else if (TryGetComponent<Renderer>(out var r))
            {
                tileWidth = r.bounds.size.x;
            }

            // 카메라 화면의 가로 절반 길이 계산
            float halfScreenWidth = cam.orthographicSize * cam.aspect;
            
            // 바닥 중심이 카메라 중심에서 '화면 절반 + 바닥 절반'보다 멀어지면 완전히 화면 밖임
            recycleDistance = halfScreenWidth + (tileWidth / 2f);
        }

        void Update()
        {
            if (mainCamera == null) return;

            // 카메라가 바닥보다 오른쪽으로 지정된 거리(recycleDistance) 이상 진행했다면
            if (mainCamera.position.x - transform.position.x > recycleDistance)
            {
                // 맨 앞으로 이동 (현재 위치 + 바닥 너비 * 전체 개수)
                transform.position += new Vector3(tileWidth * totalGroundCount, 0, 0);
            }
        }
    }
}
