using UnityEngine;
namespace MiniGame2
{
    public class ProjectileController : MonoBehaviour
    {
        // �浹 ������ �� ���̾�?
        [SerializeField] private LayerMask levelCollisionLayer;

        private RangeWeaponHandler rangeWeaponHandler;

        private float currentDuration;
        private Vector2 direction;
        private bool isReady;
        private Transform pivot; // �ǹ�?? �̰� �� ������ ���� >> �׳� źȯ ������Ʈ ������ �θ��̸� �� ������Ʈ��.. >> �� ������ �ְ�����

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
            // �ӵ� �ο�.. �̰� �߷�, ���� ���� ����(kinematic �ɼ��̶� �� �ʿ� ������) ó�� �ѹ��� ���ָ� �Ǵ� �� �ƴѰ�? >> �³�.. ��� ���̱� ���� ��ġ �̵�
            _rigidbody.velocity = direction * rangeWeaponHandler.Speed;
        }

        private void Update()
        {
            if (!isReady)
            {
                return;
            }

            // ������ ������ �ð��� ī��Ʈ
            currentDuration += Time.deltaTime;
            // ���� �ð��� �����ٸ� ź �ı�
            if (currentDuration > rangeWeaponHandler.Duration)
            {
                DestroyProjectile(transform.position, false);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // ClosestPoint�� ó�� ���°ſ��� �޸�
            // levelCollisionLayer �̰� �׳� �� �ݶ��̴��� ���̾� ������ >> �¾Ҿ�!
            // >> ���� ������ �浹 ��ġ ��¦ ����ü �ݴ� ���� ���ʿ� ź �ı� �� ����Ʈ ����(���� �΋H�� ��ó�� ���̰Բ�)
            if (levelCollisionLayer.value == (levelCollisionLayer.value | (1 << collision.gameObject.layer)))
            {
                DestroyProjectile(collision.ClosestPoint(transform.position) - direction * .2f, fxOnDestory);
            }
            // Ÿ���� ����ٸ� �� ��ġ���� ź �ı� �� ����Ʈ ����
            else if (rangeWeaponHandler.target.value == (rangeWeaponHandler.target.value | (1 << collision.gameObject.layer)))
            {
                if (collision.TryGetComponent(out ResourceController resourceController))
                {
                    // �� ü�� ����
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

            // �Ѿ��� x���� �߻� ���⿡ �����ֱ�
            transform.right = this.direction;

            // �ǹ���.. �����ΰ�����
            // ���⿡ ���� �ø�
            if (this.direction.x < 0)
                pivot.localRotation = Quaternion.Euler(180, 0, 0);
            else
                pivot.localRotation = Quaternion.Euler(0, 0, 0);
            // �غ� ��
            isReady = true;
        }

        // ź �ı�
        private void DestroyProjectile(Vector3 position, bool createFx)
        {
            if (createFx)
                projectileManager.CreateImpactParticle(position, rangeWeaponHandler);

            Destroy(this.gameObject);
        }
    }
}