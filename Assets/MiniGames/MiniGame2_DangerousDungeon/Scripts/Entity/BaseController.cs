using UnityEngine;

namespace MiniGame2
{
    public class BaseController : MonoBehaviour
    {
        protected Rigidbody2D _rigid;

        [SerializeField] SpriteRenderer charRenderer;
        [SerializeField] Transform weaponPivot;


        protected Vector2 moveDir,    // ĳ���� �̵� ����
                          lookDir;    // ĳ���Ͱ� �ٶ󺸴� ����

        Vector2 knockBack;// �˹��� ��

        float knockBack_Duration = 0f; // �˹� ���� �ð�

        public Vector2 GetDir_Move { get => moveDir; }
        public Vector2 GetDir_Look { get => lookDir; }
        public Vector2 GetDir_KnockBack { get => knockBack; }

        protected AnimationHandler animationHandler;
        protected StatHandler statHandler;

        [SerializeField] WeaponHandler weaponPrefab;
        // ������ �����ؼ� ���� ���� ����
        protected WeaponHandler weaponHandler;
        // ���� ������ ����
        protected bool isAttacking;
        // ���� �������κ����� �ð�(������ ���)
        private float timeSinceLastAttack = float.MaxValue;

        protected virtual void Awake()
        {
            TryGetComponent(out _rigid);
            TryGetComponent(out animationHandler);
            TryGetComponent(out statHandler);

            if (!charRenderer)
                charRenderer = GetComponentInChildren<SpriteRenderer>();

            if (!weaponPivot)
                weaponPivot = transform.GetChild(1);

            // �ν����Ϳ��� weaponPrefab�� �Ҵ�� ���� �������� �ִٸ�, �����ؼ� ����� �ֱ�
            if (weaponPrefab)
                weaponHandler = Instantiate(weaponPrefab, weaponPivot);
            // �������� ���ٸ�, �̹� ���⸦ �����ϰ� �ִ��� Ȯ���ϰ� ��������
            else
                weaponHandler = GetComponentInChildren<WeaponHandler>();
        }

        protected virtual void Start()
        {

        }

        protected virtual void Update()
        {
            HandleAction();
            Rotate(lookDir);
            HandleAttackDelay();
        }

        protected virtual void FixedUpdate()
        {
            Movement(moveDir);
            if (knockBack_Duration > 0)
                knockBack_Duration -= Time.fixedDeltaTime;
        }

        protected virtual void HandleAction()
        {

        }

        void Movement(Vector2 dir)
        {
            // �˹� �����̸� �̵� ������ ������ ���� �˹�
            if (knockBack_Duration > 0)
            {
                dir += knockBack;
            }
            else
            {
                dir *= statHandler.Speed;
            }

            _rigid.velocity = dir;
            animationHandler.Move(dir);
        }

        void Rotate(Vector2 dir)
        {
            // Z�� ȸ����
            float rotZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            // zȸ������ ���밪�� 90�� ������ ĳ���͸� �������� �ϴ� ��и鿡�� 2,3 ��и� ��, ������ �ٶ󺸰� �ִٴ� ��
            bool isLeft = Mathf.Abs(rotZ) > 90;

            // �÷��̾ ��� ���� ���� �ִ����� ���� �¿� ����
            charRenderer.flipX = isLeft;

            // ���� �ǹ��� Z�� ȸ������ ���� ȸ��(�ش� �������� �߻��ؾ� �ϱ� ����)
            if (weaponPivot)
                weaponPivot.rotation = Quaternion.Euler(0, 0, rotZ);

            // ���� ���⿡ ���� ���� ���� �ø�
            weaponHandler?.Rotate(isLeft);
        }

        public void ApplyKnockBack(Transform other, float power, float duration)
        {
            knockBack_Duration = duration;
            // �°� �ڷ� ���ư��� ��
            knockBack = (transform.position - other.position).normalized * power;
        }

        private void HandleAttackDelay()
        {
            // ���Ⱑ ���ٸ� �۵����� ����
            if (weaponHandler == null)
                return;

            // ������ �߿��� ī����
            if (timeSinceLastAttack <= weaponHandler.Delay)
            {
                timeSinceLastAttack += Time.deltaTime;
            }

            // ���� ��ȣ�� ������ �� 1����
            if (isAttacking && timeSinceLastAttack > weaponHandler.Delay)
            {
                // ī���� �ʱ�ȭ(�̰ɷ� ���� if���� ���� �� 1���� ����ϰԲ�)
                timeSinceLastAttack = 0;
                // ����!
                Attack();
            }
        }

        // ���� ������ ���� �� ����!
        protected virtual void Attack()
        {
            if (lookDir != Vector2.zero)
                weaponHandler?.Attack();
        }

        public virtual void Death()
        {
            _rigid.velocity = Vector3.zero;
            foreach (SpriteRenderer renderer in transform.GetComponentsInChildren<SpriteRenderer>())
            {
                Color color = renderer.color;
                color.a = 0.3f;
                renderer.color = color;
            }

            // Behaviour : ��� ��ũ��Ʈ�� Ȱ��/��Ȱ�� ��ɿ� ���� �������� �κ�
            foreach (Behaviour component in transform.GetComponentsInChildren<Behaviour>())
            {
                component.enabled = false;
            }

            Destroy(gameObject, .5f);
        }
    }
}