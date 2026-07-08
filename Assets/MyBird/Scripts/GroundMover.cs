using UnityEngine;

namespace MyBird
{
    public class GroundMover : MonoBehaviour
    {
        [Header("Scroll Settings")]
        public float scrollSpeed = 5f;

        private Transform groundA;
        private Transform groundB;
        private float tileWidth = 1f;

        void Start()
        {
            // 이 방식은 바닥 조각이 딱 2개일 때 최적화된 로직입니다.
            if (transform.childCount >= 2)
            {
                groundA = transform.GetChild(0);
                groundB = transform.GetChild(1);

                // 첫 번째 조각의 너비를 측정 (SpriteRenderer 기준)
                if (groundA.TryGetComponent<SpriteRenderer>(out var sr))
                {
                    tileWidth = sr.bounds.size.x;
                }
                else if (groundA.TryGetComponent<Renderer>(out var r))
                {
                    tileWidth = r.bounds.size.x;
                }
            }
            else
            {
                Debug.LogError("GroundMover: 이 로직은 자식(바닥) 오브젝트가 최소 2개 필요합니다!");
            }
        }

        void Update()
        {
            if (groundA == null || groundB == null) return;

            float moveDistance = scrollSpeed * Time.deltaTime;

            // 1. 두 조각을 모두 왼쪽으로 이동
            groundA.localPosition += Vector3.left * moveDistance;
            groundB.localPosition += Vector3.left * moveDistance;

            // 2. A가 화면 밖(-너비)으로 완전히 나가면, B의 바로 오른쪽 끝으로 순간이동
            if (groundA.localPosition.x <= -tileWidth)
            {
                groundA.localPosition = groundB.localPosition + new Vector3(tileWidth, 0, 0);
            }

            // 3. B가 화면 밖으로 나가면, A의 바로 오른쪽 끝으로 순간이동
            if (groundB.localPosition.x <= -tileWidth)
            {
                groundB.localPosition = groundA.localPosition + new Vector3(tileWidth, 0, 0);
            }
        }
    }
}
