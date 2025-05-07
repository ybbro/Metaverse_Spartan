using UnityEngine;

namespace MiniGame0
{
    public class Player : MonoBehaviour
    {
        Animator animator; // 장애물에 걸렸을 때 애니메이션 교체
        Rigidbody2D rigid2D; // 점프할 때 힘을 줘야해요

        Transform model;

        [SerializeField, Tooltip("점프 키를 눌렀을 때의 즉시 반영 속도")]
        float jumpVelocity = 5;

        [SerializeField, Tooltip("플레이어의 X축 방향 속도")]
        float speed_X = 5;

        [SerializeField, Tooltip("점프 시 회전각")]
        float rotation_max = 30f;

        [SerializeField, Tooltip("회전각 최소치")]
        float rotation_min = -30f;

        [SerializeField, Tooltip("회전각 감소 속도")]
        float rotation_vel = -5f;

        bool isInputEntered = false; // 점프 키 입력 여부

        void Start()
        {
            animator = GetComponentInChildren<Animator>();
            rigid2D = GetComponentInChildren<Rigidbody2D>();
            model = GetComponentInChildren<SpriteRenderer>().transform;
        }

        void Update()
        {
            if (GameManager.instance.isDie || !GameManager.instance.isReady)
                return;

            // 키 입력은 Update에서! 픽스드에서 받으면 키 입력이 되지 않는 현상 발생
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0))
                isInputEntered = true;
        }

        private void FixedUpdate()
        {
            if (GameManager.instance.isDie || !GameManager.instance.isReady)
                return;

            if (isInputEntered)
            {
                // 이러면 떨어지는 속도가 빠를 때는 별로지 않나? 플레이 체감 후 변경해보기
                // >> 떨어지는 속도가 있어 제대로 점프가 안되는 경우가 생김 >> 이에 따라 교체
                rigid2D.velocity = new Vector2(speed_X, jumpVelocity);
                model.rotation = Quaternion.Euler(Vector3.forward * rotation_max);
                isInputEntered = false;
            }

            // 점점 아래 방향으로 기울어지게
            model.rotation = Quaternion.Euler(Vector3.forward * Mathf.Clamp(model.rotation.eulerAngles.z + rotation_vel * Time.fixedDeltaTime, rotation_min, rotation_max));
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (GameManager.instance.isDie)
                return;
            GameManager.instance.GameOver(true);
            animator.SetBool("isDie", true);
        }

        public void PressStart()
        {
            // x축 방향으로 부딪히지 않는 한 힘을 받지 않기에 초기에 x축 등속 지정
            rigid2D.velocity = Vector2.right * speed_X;
            // 중력 적용 시작
            rigid2D.gravityScale = 1;
        }
    }
}
