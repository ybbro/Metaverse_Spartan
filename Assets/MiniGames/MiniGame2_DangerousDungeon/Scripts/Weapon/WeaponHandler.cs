using UnityEngine;

namespace MiniGame2
{
    public class WeaponHandler : MonoBehaviour
    {
        [Header("Attack Info")]
        [SerializeField, Tooltip("공격 딜레이")] private float delay = 1f;
        public float Delay { get => delay; set => delay = value; }

        [SerializeField, Tooltip("공격 크기")] private float weaponSize = 1f;
        public float WeaponSize { get => weaponSize; set => weaponSize = value; }

        [SerializeField, Tooltip("공격력")] private float power = 1f;
        public float Power { get => power; set => power = value; }

        [SerializeField, Tooltip("공격속도")] private float speed = 1f;
        public float Speed { get => speed; set => speed = value; }

        [SerializeField, Tooltip("공격범위")] private float attackRange = 10f;
        public float AttackRange { get => attackRange; set => attackRange = value; }


        [Header("Knock Back Info")]
        [SerializeField, Tooltip("해당 무기가 넉백을 가하는지 여부")] private bool isOnKnockback = false;
        public bool IsKnockBack { get => isOnKnockback; set => isOnKnockback = value; }

        [SerializeField, Tooltip("넉백 힘")] private float knockbackPower = 0.1f;
        public float KnockbackPower { get => knockbackPower; set => knockbackPower = value; }

        [SerializeField, Tooltip("넉백 경직 시간")] private float knockbackTime = 0.5f;
        public float KnockbackTime { get => knockbackTime; set => knockbackTime = value; }


        public LayerMask target;
        private static readonly int IsAttack = Animator.StringToHash("isAttack");

        public BaseController Controller { get; private set; }

        private Animator animator;
        private SpriteRenderer weaponRenderer;

        public AudioClip attackSoundClip;

        protected virtual void Awake()
        {
            Controller = GetComponentInParent<BaseController>();
            animator = GetComponentInChildren<Animator>();
            weaponRenderer = GetComponentInChildren<SpriteRenderer>();

            animator.speed = 1.0f / delay;// 공격 속도를 대입하는 게 아니라 딜레이를 나눠???? >> 이건 진짜 모르겠네.. 
                                          // 무기 사이즈
            transform.localScale = Vector3.one * weaponSize;
        }

        protected virtual void Start()
        {

        }

        public virtual void Attack()
        {
            AttackAnimation();
            if (attackSoundClip)
                GameManager.Instance.soundManager.PlaySfx(attackSoundClip);
        }

        public void AttackAnimation()
        {
            animator.SetTrigger(IsAttack);
        }

        // 캐릭터 회전에 맞춰 무기 방향도 플립
        public virtual void Rotate(bool isLeft)
        {
            weaponRenderer.flipY = isLeft; // 플립y? >> 마우스와 플레이어가 이루는 방향에 따라 무기고가 회전하는데 이에 맞춰 위아래 플립
        }
    }
}