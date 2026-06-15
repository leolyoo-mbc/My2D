using UnityEngine;

namespace MyBird
{
    public class GroundMover : MonoBehaviour
    {
        [Header("Scroll")]
        // 왼쪽으로 스크롤되는 속도 (유닛/초)
        public float scrollSpeed = 2f;

        // 자식으로 배치된 그라운드 조각들을 자동으로 수집하여 루핑 처리
        Transform[] pieces;
        float[] widths;
        float totalWidth;

        void Start()
        {
            // 모든 직계 자식들을 그라운드 조각으로 간주
            int count = transform.childCount;
            pieces = new Transform[count];
            widths = new float[count];

            totalWidth = 0f;

            for (int i = 0; i < count; i++)
            {
                var t = transform.GetChild(i);
                pieces[i] = t;

                // SpriteRenderer 또는 Renderer로 넓이 계산
                float w = 0f;
                if (t.TryGetComponent<SpriteRenderer>(out var sr))
                {
                    w = sr.bounds.size.x;
                }
                else
                {
                    if (t.TryGetComponent<Renderer>(out var r)) w = r.bounds.size.x;
                }

                // 너비를 못 구하면 기본값 1 사용
                if (w <= 0f) w = 1f;

                widths[i] = w;
                totalWidth += w;
            }
        }

        void Update()
        {
            if (pieces == null || pieces.Length == 0) return;

            float dx = scrollSpeed * Time.deltaTime;

            // 각 조각을 왼쪽으로 이동시키고, 화면 왼쪽을 벗어나면 오른쪽 끝으로 옮겨 루핑
            for (int i = 0; i < pieces.Length; i++)
            {
                var t = pieces[i];
                Vector3 lp = t.localPosition;
                lp.x -= dx;

                // 해당 조각이 자신의 너비만큼 왼쪽으로 벗어나면 totalWidth만큼 오른쪽으로 옮김
                if (lp.x <= -widths[i])
                {
                    lp.x += totalWidth;
                }

                t.localPosition = lp;
            }
        }
    }
}
