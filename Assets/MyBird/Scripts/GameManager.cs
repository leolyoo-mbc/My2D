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

            // 파이프 스폰 시작 명령
            if (pipeSpawner != null)
            {
                pipeSpawner.StartSpawning();
            }
        }
    }
}
