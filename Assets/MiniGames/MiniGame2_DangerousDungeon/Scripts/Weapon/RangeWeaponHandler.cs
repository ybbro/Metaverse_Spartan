using UnityEngine;
using Random = UnityEngine.Random;
namespace MiniGame2
{
    public class RangeWeaponHandler : WeaponHandler
    {
        private ProjectileManager projectileManager;

        [Header("Ranged Attack Data")]
        [SerializeField, Tooltip("����ü ���� ��ġ")] private Transform projectileSpawnPosition;

        [SerializeField, Tooltip("�Ѿ� ��ȣ")] private int bulletIndex;
        public int BulletIndex { get { return bulletIndex; } }

        [SerializeField, Tooltip("�Ѿ� ũ��")] private float bulletSize = 1;
        public float BulletSize { get { return bulletSize; } }

        [SerializeField, Tooltip("���� �ð�")] private float duration;
        public float Duration { get { return duration; } }

        [SerializeField, Tooltip("ź ���� ����")] private float spread;
        public float Spread { get { return spread; } }

        [SerializeField, Tooltip("��Ƽ�� ź�� ��")] private int numberofProjectilesPerShot;
        public int NumberofProjectilesPerShot { get { return numberofProjectilesPerShot; } }

        [SerializeField, Tooltip("��Ƽ�� ź ���� ����")] private float multipleProjectilesAngel;
        public float MultipleProjectilesAngel { get { return multipleProjectilesAngel; } }

        [SerializeField, Tooltip("����ü ����")] private Color projectileColor;
        public Color ProjectileColor { get { return projectileColor; } }

        protected override void Start()
        {
            base.Start();
            projectileManager = ProjectileManager.Instance;
        }

        public override void Attack()
        {
            // ���� �ִϸ��̼� ���
            base.Attack();

            float projectilesAngleSpace = multipleProjectilesAngel;
            int numberOfProjectilesPerShot = numberofProjectilesPerShot;

            // ���� ���� ������ �߻�Ǵ� ź�� �߻簢
            float minAngle = -(numberOfProjectilesPerShot / 2f) * projectilesAngleSpace;

            for (int i = 0; i < numberOfProjectilesPerShot; i++)
            {
                // �ش� ź�� ����
                float angle = minAngle + projectilesAngleSpace * i;
                // ź ������ ���� ������ ���� �߰�
                float randomSpread = Random.Range(-spread, spread);
                angle += randomSpread;

                //�÷��̾�� ���콺�� �̷�� ������ ����. angle ��밢�� ���� ź ����
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
