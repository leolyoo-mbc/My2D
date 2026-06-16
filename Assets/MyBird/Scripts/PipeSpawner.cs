using UnityEngine;

namespace MyBird
{
    public class PipeSpawner : MonoBehaviour
    {
        [Header("Spawn Settings")]
        // 생성할 파이프 프리팹
        public GameObject pipePrefab;
        // 스폰 포인트(없으면 이 오브젝트 위치 사용)
        public Transform spawnPoint;

        [Header("Timing")]
        // 최소/최대 스폰 간격(초)
        public float minInterval = 0.95f;
        public float maxInterval = 1.05f;

        [Header("Height")]
        // 파이프 Y 위치 범위
        public float minY = 0f;
        public float maxY = 3f;

        [Header("Difficulty")]
        // 난이도 계수: 값이 클수록 더 자주 생성 (기본 1)
        public float difficulty = 1f;

        Coroutine spawnRoutine;

        void Start()
        {
            if (spawnPoint == null) spawnPoint = transform;
        }

        public void StartSpawning()
        {
            if (pipePrefab == null)
            {
                Debug.LogError("PipeSpawner: pipePrefab is not assigned.");
                return;
            }

            if (spawnRoutine == null)
                spawnRoutine = StartCoroutine(SpawnLoop());
        }

        public void StopSpawning()
        {
            if (spawnRoutine != null)
            {
                StopCoroutine(spawnRoutine);
                spawnRoutine = null;
            }
        }

        System.Collections.IEnumerator SpawnLoop()
        {
            while (true)
            {
                float interval = Random.Range(minInterval, maxInterval);
                // 난이도 적용 (음수/0 방지)
                float d = Mathf.Max(0.0001f, difficulty);
                interval /= d;

                yield return new WaitForSeconds(interval);

                float y = Random.Range(minY, maxY);
                Vector3 pos = spawnPoint.position + Vector3.up * y;

                Instantiate(pipePrefab, pos, Quaternion.identity);
            }
        }
    }
}
