using UnityEngine;
namespace MiniGame2
{
    public class ProjectileController : MonoBehaviour
    {
        // 충돌 판정에 쓸 레이어?
        [SerializeField] private LayerMask levelCollisionLayer;

        private RangeWeaponHandler rangeWeaponHandler;

        private float currentDuration;
        private Vector2 direction;
        private bool isReady;
        private Transform pivot; // 피벗?? 이걸 왜 가지고 있지 >> 그냥 탄환 오브젝트 외형의 부모이며 빈 오브젝트네.. >> 다 이유가 있겠지요

        private Rigidbody2D _rigidbody;
        private SpriteRenderer spriteRenderer;

        public bool fxOnDestory = true;

        ProjectileManager projectileManager;

        private void Awake()
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _rigidbody = GetComponent<Rigidbody2D>();
            pivot = transform.GetChild(0);
        }

        private void Start()
        {
            // 속도 부여.. 이건 중력, 마찰 영향 끄고(kinematic 옵션이라 끌 필요 없었음) 처음 한번만 해주면 되는 게 아닌가? >> 맞네.. 계산 줄이기 위해 위치 이동
            _rigidbody.velocity = direction * rangeWeaponHandler.Speed;
        }

        private void Update()
        {
            if (!isReady)
            {
                return;
            }

            // 생성된 이후의 시간을 카운트
            currentDuration += Time.deltaTime;
            // 지속 시간이 지났다면 탄 파괴
            if (currentDuration > rangeWeaponHandler.Duration)
            {
                DestroyProjectile(transform.position, false);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // ClosestPoint는 처음 보는거에요 메모
            // levelCollisionLayer 이건 그냥 맵 콜라이더의 레이어 같은데 >> 맞았어!
            // >> 여기 닿으면 충돌 위치 살짝 투사체 반대 방향 뒤쪽에 탄 파괴 및 이펙트 생성(벽에 부딫힌 것처럼 보이게끔)
            if (levelCollisionLayer.value == (levelCollisionLayer.value | (1 << collision.gameObject.layer)))
            {
                DestroyProjectile(collision.ClosestPoint(transform.position) - direction * .2f, fxOnDestory);
            }
            // 타겟을 맞췄다면 그 위치에서 탄 파괴 및 이펙트 생성
            else if (rangeWeaponHandler.target.value == (rangeWeaponHandler.target.value | (1 << collision.gameObject.layer)))
            {
                if (collision.TryGetComponent(out ResourceController resourceController))
                {
                    // 적 체력 감소
                    resourceController.ChangeHealth(-rangeWeaponHandler.Power);
                    if (rangeWeaponHandler.IsKnockBack)
                    {
                        if (collision.TryGetComponent(out BaseController controller))
                            controller.ApplyKnockBack(transform, rangeWeaponHandler.KnockbackPower, rangeWeaponHandler.KnockbackTime);
                    }
                }

                DestroyProjectile(collision.ClosestPoint(transform.position), fxOnDestory);
            }
        }


        public void Init(Vector2 direction, RangeWeaponHandler weaponHandler, ProjectileManager projectileManager)
        {
            this.projectileManager = projectileManager;
            rangeWeaponHandler = weaponHandler;

            this.direction = direction;
            currentDuration = 0;
            transform.localScale = Vector3.one * weaponHandler.BulletSize;
            spriteRenderer.color = weaponHandler.ProjectileColor;

            // 총알의 x축을 발사 방향에 맞춰주기
            transform.right = this.direction;

            // 피벗이.. 외형인가보네
            // 방향에 따라 플립
            if (this.direction.x < 0)
                pivot.localRotation = Quaternion.Euler(180, 0, 0);
            else
                pivot.localRotation = Quaternion.Euler(0, 0, 0);
            // 준비 완
            isReady = true;
        }

        // 탄 파괴
        private void DestroyProjectile(Vector3 position, bool createFx)
        {
            if (createFx)
                projectileManager.CreateImpactParticle(position, rangeWeaponHandler);

            Destroy(this.gameObject);
        }
    }
}