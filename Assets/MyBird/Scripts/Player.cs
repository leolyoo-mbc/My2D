using UnityEngine;
using UnityEngine.InputSystem;

namespace MyBird
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour
    {
        private enum State { Ready, Playing, Dead }

        Rigidbody2D rb;

        [Header("Jump")]
        [SerializeField] private float jumpVelocity = 5f;

        [Header("Movement")]
        // 오른쪽으로 이동하는 속도 (유닛/초)
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float defaultGravityScale;

        [Header("Rotation")]
        // When moving up, rotate toward this angle (degrees)
        public float maxUpAngle = 30f;
        // When falling, rotate toward this angle (degrees)
        public float maxDownAngle = -90f;
        // 고개를 최대로 꺾기 위한 기준 속도 (물리 이동 속도 제한 아님)
        public float speedForMaxUpAngle = 5f;
        public float speedForMaxDownAngle = -5f;
        // How fast the bird rotates to the target angle
        public float rotationSpeed = 5f;

        [Header("State")]
        [SerializeField] private State currentState = State.Ready;

        [SerializeField] private GameObject readyUI;
        [SerializeField] private GameObject gameOverUI;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            defaultGravityScale = rb.gravityScale;
            rb.gravityScale = 0f;
        }

        private void Start()
        {
            gameOverUI.SetActive(false);
        }

        void FixedUpdate()
        {
            switch (currentState)
            {
                case State.Ready:
                    break;
                case State.Playing:
                    rb.linearVelocityX = moveSpeed;
                    ApplyRotation();
                    break;
                case State.Dead:
                    break;
            }
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            switch (currentState)
            {
                case State.Ready:
                    break;
                case State.Playing:
                    if (collision.collider != null && collision.collider.CompareTag("Pipe"))
                    {
                        currentState = State.Dead;
                        // 게임 매니저에 게임오버 알림
                        if (GameManager.Instance != null)
                        {
                            GameManager.Instance.GameOver();
                        }
                        gameOverUI.SetActive(true);
                    }
                    break;
                case State.Dead:
                    break;
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            switch (currentState)
            {
                case State.Ready:
                    break;
                case State.Playing:
                    if (other != null && other.CompareTag("Point"))
                    {
                        if (GameManager.Instance != null)
                        {
                            GameManager.Instance.AddPoint(1);
                        }
                    }
                    break;
                case State.Dead:
                    break;
            }
        }

        void ApplyRotation()
        {
            if (rb == null) return;

            // 물리적인 속도가 기준 속도에 도달했을 때의 비율(0.0 ~ 1.0)을 구함
            float normalizedVelocity = Mathf.InverseLerp(speedForMaxDownAngle, speedForMaxUpAngle, rb.linearVelocityY);

            // 각도 중에 더 절댓값이 큰 값의 각도를 기준으로 대칭(Max Magnitude) 설정
            float maxAngleMagnitude = maxDownAngle + maxUpAngle < 0 ? Mathf.Abs(maxDownAngle) : Mathf.Abs(maxUpAngle);

            // 속도가 0일 때 0도를 바라보도록 대칭 계산
            float target = Mathf.Lerp(-maxAngleMagnitude, maxAngleMagnitude, normalizedVelocity);

            // 최종 계산된 각도가 기존 설정한 한계치(-90도 ~ 30도)를 벗어나지 않게 제한
            target = Mathf.Clamp(target, maxDownAngle, maxUpAngle);

            // 현재 z 각도는 0..360이므로 -180..180 범위로 변환
            float current = transform.eulerAngles.z;
            if (current > 180f) current -= 360f;

            float angle = Mathf.Lerp(current, target, Time.fixedDeltaTime * rotationSpeed);
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            switch (currentState)
            {
                case State.Ready:
                    currentState = State.Playing;
                    rb.gravityScale = defaultGravityScale;
                    readyUI.SetActive(false);
                    GameManager.Instance.StartGame();
                    rb.linearVelocityY = jumpVelocity;
                    break;
                case State.Playing:
                    rb.linearVelocityY = jumpVelocity;
                    break;
                case State.Dead:
                    break;
            }
        }
    }
}