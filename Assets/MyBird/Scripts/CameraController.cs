using UnityEngine;

namespace MyBird
{
    public class CameraController : MonoBehaviour
    {
        [Header("Target")]
        // 따라갈 대상(없으면 씬에서 Player 컴포넌트를 찾아 사용)
        public Transform target;

        [Header("Follow Settings")]
        // 카메라와 대상 사이의 기본 오프셋
        public Vector3 offset = new Vector3(0f, 0f, -10f);
        // 보간 속도 (클수록 빠르게 따라감)
        public float smoothSpeed = 5f;
        // 기본적으로 X축(오른쪽)만 따라감. Y축도 같이 따라가려면 true로 변경
        public bool followY = false;

        void Start()
        {
            if (target == null)
            {
                // Player 타입을 가진 오브젝트를 찾아서 타겟으로 사용
                var player = FindFirstObjectByType<Player>();
                if (player != null)
                {
                    target = player.transform;
                }
            }
        }

        void LateUpdate()
        {
            if (target == null) return;

            // 목표 위치 계산: 대상 위치 + 오프셋
            Vector3 desired = target.position + offset;

            // 기본적으로 카메라 Z는 유지
            desired.z = offset.z;

            // Y 축을 고정하려면 현재 카메라 Y값 유지
            if (!followY)
            {
                desired.y = transform.position.y;
            }

            //// 부드럽게 보간하여 이동
            //transform.position = Vector3.Lerp(transform.position, desired, Time.deltaTime * smoothSpeed);
            transform.position = desired;
        }
    }
}
