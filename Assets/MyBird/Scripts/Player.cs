using UnityEngine;

namespace MyBird
{
    public class Player : MonoBehaviour
    {
        [Header("Jump")]
        public float jumpVelocity = 5f;

        [Header("Rotation")]
        // When moving up, rotate toward this angle (degrees)
        public float maxUpAngle = 30f;
        // When falling, rotate toward this angle (degrees)
        public float maxDownAngle = -90f;
        // Expected vertical speeds used to map velocity -> angle
        public float maxRiseSpeed = 5f;
        public float maxFallSpeed = -5f;
        // How fast the bird rotates to the target angle
        public float rotationSpeed = 5f;

        [Header("Movement")]
        // 오른쪽으로 이동하는 속도 (유닛/초)
        public float moveSpeed = 2f;

        [Header("State")]
        // 게임 시작 전 대기 상태인지 여부
        public bool isPlaying = false;
        // 대기 중에 아래로 떨어질 때마다 받는 위쪽 힘
        public float hoverForce = 10f;

        Rigidbody2D rb;
        bool isDead = false;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                Debug.LogError("Player requires a Rigidbody2D component.");
            }
        }

        void Update()
        {
            // 입력: 스페이스바 또는 마우스 왼쪽 버튼
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                // 대기 상태라면 플레이 상태로 전환하고 첫 점프를 수행
                if (!isPlaying)
                {
                    isPlaying = true;
                    Jump();
                }
                else
                {
                    Jump();
                }
            }
        }

        void FixedUpdate()
        {
            if (rb == null) return;

            if (isPlaying)
            {
                // 플레이 중일 때만 오른쪽으로 이동
                transform.Translate(moveSpeed * Time.fixedDeltaTime * Vector2.right, Space.World);
            }
            else
            {
                // 대기 상태: 아래로 떨어질 때만 아래에서 위로 지속적인 힘을 줘서 제자리에 뜨게 함
                if (rb.linearVelocity.y < 0f)
                {
                    rb.AddForce(Vector2.up * hoverForce);
                }
            }

            ApplyRotation();
        }

        void Jump()
        {
            if (rb == null) return;
            if (isDead) return;

            // Rigidbody2D velocity를 직접 설정하여 즉시 위로 점프시킴
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpVelocity);
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (isDead) return;

            if (collision.collider != null && collision.collider.CompareTag("Pipe"))
            {
                isDead = true;
                // 게임 매니저에 게임오버 알림
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.GameOver();
                }
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (isDead) return;

            if (other != null && other.CompareTag("Point"))
            {
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.AddPoint(1);
                }
            }
        }

        void ApplyRotation()
        {
            if (rb == null) return;

            // rb.linearVelocity.y 값을 maxFallSpeed..maxRiseSpeed 범위로 정규화
            float v = rb.linearVelocity.y;
            float t = Mathf.InverseLerp(maxFallSpeed, maxRiseSpeed, v);

            float target = Mathf.Lerp(maxDownAngle, maxUpAngle, t);

            // 현재 z 각도는 0..360이므로 -180..180 범위로 변환
            float current = transform.eulerAngles.z;
            if (current > 180f) current -= 360f;

            float angle = Mathf.Lerp(current, target, Time.fixedDeltaTime * rotationSpeed);
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }
}