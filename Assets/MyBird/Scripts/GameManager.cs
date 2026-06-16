using UnityEngine;

namespace MyBird
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("References")]
        public Player player;
        public PipeSpawner pipeSpawner;

        public bool IsGameOver { get; private set; }
        public int Score { get; private set; }

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        void Start()
        {
            if (player == null)
            {
                player = FindFirstObjectByType<Player>();
            }
            if (pipeSpawner == null)
            {
                pipeSpawner = FindFirstObjectByType<PipeSpawner>();
            }
        }

        public void GameOver()
        {
            if (IsGameOver) return;
            IsGameOver = true;

            Debug.Log("GameManager: GameOver triggered.");

            // 멈출 동작: 플레이어 앞으로 이동 중지
            if (player != null)
            {
                player.isPlaying = false;
                // 선택적으로 플레이어 물리도 멈춤
                //if (player.GetComponent<Rigidbody2D>() is Rigidbody2D prb)
                //{
                //    prb.linearVelocity = Vector2.zero;
                //    prb.bodyType = RigidbodyType2D.Static;
                //}
            }

            // 파이프 스폰 중지
            if (pipeSpawner != null)
            {
                pipeSpawner.StopSpawning();
            }
        }

        public void AddPoint(int amount = 1)
        {
            if (IsGameOver) return;
            Score += amount;
            Debug.Log($"GameManager: Score = {Score}");
        }

        public void StartGame()
        {
            if (IsGameOver) return;

            // 플레이어 상태 활성화
            player.isPlaying = true;

            // 파이프 스폰 시작 명령
            if (pipeSpawner != null)
            {
                pipeSpawner.StartSpawning();
            }
        }
    }
}
