using UnityEngine;
using Random = UnityEngine.Random;
namespace MiniGame2
{
    public class RangeWeaponHandler : WeaponHandler
    {
        private ProjectileManager projectileManager;

        [Header("Ranged Attack Data")]
        [SerializeField, Tooltip("투사체 생성 위치")] private Transform projectileSpawnPosition;

        [SerializeField, Tooltip("총알 번호")] private int bulletIndex;
        public int BulletIndex { get { return bulletIndex; } }

        [SerializeField, Tooltip("총알 크기")] private float bulletSize = 1;
        public float BulletSize { get { return bulletSize; } }

        [SerializeField, Tooltip("지속 시간")] private float duration;
        public float Duration { get { return duration; } }

        [SerializeField, Tooltip("탄 퍼짐 정도")] private float spread;
        public float Spread { get { return spread; } }

        [SerializeField, Tooltip("멀티샷 탄의 수")] private int numberofProjectilesPerShot;
        public int NumberofProjectilesPerShot { get { return numberofProjectilesPerShot; } }

        [SerializeField, Tooltip("멀티샷 탄 간의 각도")] private float multipleProjectilesAngel;
        public float MultipleProjectilesAngel { get { return multipleProjectilesAngel; } }

        [SerializeField, Tooltip("투사체 색상")] private Color projectileColor;
        public Color ProjectileColor { get { return projectileColor; } }

        protected override void Start()
        {
            base.Start();
            projectileManager = ProjectileManager.Instance;
        }

        public override void Attack()
        {
            // 공격 애니메이션 재생
            base.Attack();

            float projectilesAngleSpace = multipleProjectilesAngel;
            int numberOfProjectilesPerShot = numberofProjectilesPerShot;

            // 가장 낮은 각도로 발사되는 탄의 발사각
            float minAngle = -(numberOfProjectilesPerShot / 2f) * projectilesAngleSpace;

            for (int i = 0; i < numberOfProjectilesPerShot; i++)
            {
                // 해당 탄의 각도
                float angle = minAngle + projectilesAngleSpace * i;
                // 탄 퍼짐에 따라 랜덤한 각도 추가
                float randomSpread = Random.Range(-spread, spread);
                angle += randomSpread;

                //플레이어와 마우스가 이루는 각도를 기준. angle 상대각을 가진 탄 생성
                CreateProjectile(Controller.GetDir_Look, angle);
            }
        }

        private void CreateProjectile(Vector2 _lookDirection, float angle)
        {
            projectileManager.ShootBullet(this, projectileSpawnPosition.position, RotateVector2(_lookDirection, angle));
        }

        private static Vector2 RotateVector2(Vector2 v, float degree)
        {
            return Quaternion.Euler(0, 0, degree) * v;
        }
    }
}
