using UnityEngine;

namespace MiniGame2
{
    public class BaseController : MonoBehaviour
    {
        protected Rigidbody2D _rigid;

        [SerializeField] SpriteRenderer charRenderer;
        [SerializeField] Transform weaponPivot;


        protected Vector2 moveDir,    // 캐릭터 이동 방향
                          lookDir;    // 캐릭터가 바라보는 방향

        Vector2 knockBack;// 넉백의 힘

        float knockBack_Duration = 0f; // 넉백 지속 시간

        public Vector2 GetDir_Move { get => moveDir; }
        public Vector2 GetDir_Look { get => lookDir; }
        public Vector2 GetDir_KnockBack { get => knockBack; }

        protected AnimationHandler animationHandler;
        protected StatHandler statHandler;

        [SerializeField] WeaponHandler weaponPrefab;
        // 원본을 복사해서 쓰기 위해 선언
        protected WeaponHandler weaponHandler;
        // 공격 중인지 여부
        protected bool isAttacking;
        // 지난 공격으로부터의 시간(딜레이 사용)
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

            // 인스펙터에서 weaponPrefab에 할당된 무기 프리팹이 있다면, 생성해서 무기고에 넣기
            if (weaponPrefab)
                weaponHandler = Instantiate(weaponPrefab, weaponPivot);
            // 프리팹이 없다면, 이미 무기를 장착하고 있는지 확인하고 가져오기
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
            // 넉백 도중이면 이동 조작의 영향이 적고 넉백
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
            // Z축 회전값
            float rotZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            // z회전값의 절대값이 90을 넘으면 캐릭터를 원점으로 하는 사분면에서 2,3 사분면 즉, 왼쪽을 바라보고 있다는 뜻
            bool isLeft = Mathf.Abs(rotZ) > 90;

            // 플레이어가 어느 쪽을 보고 있는지에 따라 좌우 반전
            charRenderer.flipX = isLeft;

            // 무기 피벗은 Z축 회전값에 따라 회전(해당 방향으로 발사해야 하기 때문)
            if (weaponPivot)
                weaponPivot.rotation = Quaternion.Euler(0, 0, rotZ);

            // 보는 방향에 따라 무기 상하 플립
            weaponHandler?.Rotate(isLeft);
        }

        public void ApplyKnockBack(Transform other, float power, float duration)
        {
            knockBack_Duration = duration;
            // 맞고 뒤로 날아가는 힘
            knockBack = (transform.position - other.position).normalized * power;
        }

        private void HandleAttackDelay()
        {
            // 무기가 없다면 작동하지 않음
            if (weaponHandler == null)
                return;

            // 딜레이 중에는 카운터
            if (timeSinceLastAttack <= weaponHandler.Delay)
            {
                timeSinceLastAttack += Time.deltaTime;
            }

            // 공격 신호가 들어왔을 때 1번만
            if (isAttacking && timeSinceLastAttack > weaponHandler.Delay)
            {
                // 카운터 초기화(이걸로 위의 if문을 공격 시 1번만 통과하게끔)
                timeSinceLastAttack = 0;
                // 공격!
                Attack();
            }
        }

        // 공격 방향이 있을 때 공격!
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

            // Behaviour : 모든 스크립트의 활성/비활성 기능에 대한 공통적인 부분
            foreach (Behaviour component in transform.GetComponentsInChildren<Behaviour>())
            {
                component.enabled = false;
            }

            Destroy(gameObject, .5f);
        }
    }
}