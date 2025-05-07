using UnityEngine;

namespace MiniGame2
{
    public class WeaponHandler : MonoBehaviour
    {
        [Header("Attack Info")]
        [SerializeField, Tooltip("���� ������")] private float delay = 1f;
        public float Delay { get => delay; set => delay = value; }

        [SerializeField, Tooltip("���� ũ��")] private float weaponSize = 1f;
        public float WeaponSize { get => weaponSize; set => weaponSize = value; }

        [SerializeField, Tooltip("���ݷ�")] private float power = 1f;
        public float Power { get => power; set => power = value; }

        [SerializeField, Tooltip("���ݼӵ�")] private float speed = 1f;
        public float Speed { get => speed; set => speed = value; }

        [SerializeField, Tooltip("���ݹ���")] private float attackRange = 10f;
        public float AttackRange { get => attackRange; set => attackRange = value; }


        [Header("Knock Back Info")]
        [SerializeField, Tooltip("�ش� ���Ⱑ �˹��� ���ϴ��� ����")] private bool isOnKnockback = false;
        public bool IsKnockBack { get => isOnKnockback; set => isOnKnockback = value; }

        [SerializeField, Tooltip("�˹� ��")] private float knockbackPower = 0.1f;
        public float KnockbackPower { get => knockbackPower; set => knockbackPower = value; }

        [SerializeField, Tooltip("�˹� ���� �ð�")] private float knockbackTime = 0.5f;
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

            animator.speed = 1.0f / delay;// ���� �ӵ��� �����ϴ� �� �ƴ϶� �����̸� ����???? >> �̰� ��¥ �𸣰ڳ�.. 
                                          // ���� ������
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

        // ĳ���� ȸ���� ���� ���� ���⵵ �ø�
        public virtual void Rotate(bool isLeft)
        {
            weaponRenderer.flipY = isLeft; // �ø�y? >> ���콺�� �÷��̾ �̷�� ���⿡ ���� ����� ȸ���ϴµ� �̿� ���� ���Ʒ� �ø�
        }
    }
}