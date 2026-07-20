using UnityEngine;

namespace MyBird
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("References")]
        public PipeSpawner pipeSpawner;

        [SerializeField] private bool isGameOver = false;
        public int Score { get; private set; }

        [SerializeField] private Player player;

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
            if (pipeSpawner == null)
            {
                pipeSpawner = FindFirstObjectByType<PipeSpawner>();
            }
        }

        private void Update()
        {
            if (isGameOver) return;

            switch (player.CurrentState)
            {
                case Player.State.Ready:
                    break;
                case Player.State.Playing:
                    if (pipeSpawner != null) pipeSpawner.StartSpawning();
                    break;
                case Player.State.Dead:
                    isGameOver = true;
                    Debug.Log("GameManager: GameOver triggered.");
                    // 파이프 스폰 중지
                    if (pipeSpawner != null) pipeSpawner.StopSpawning();
                    break;
            }
        }

        public void AddPoint(int amount = 1)
        {
            if (isGameOver) return;
            Score += amount;
            Debug.Log($"GameManager: Score = {Score}");
        }
    }
}
