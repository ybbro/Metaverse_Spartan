using UnityEngine;
namespace MiniGame2
{
    public class ProjectileManager : MonoBehaviour
    {
        private static ProjectileManager instance;
        public static ProjectileManager Instance { get { return instance; } }

        [SerializeField] private GameObject[] projectilePrefabs;

        [SerializeField] private ParticleSystem impact;

        private void Awake()
        {
            instance = this;
        }

        // 원거리 탄을 시작점에서 해당 방향으로 발사
        public void ShootBullet(RangeWeaponHandler rangeWeaponHandler, Vector2 startPostiion, Vector2 direction)
        {
            GameObject origin = projectilePrefabs[rangeWeaponHandler.BulletIndex];
            GameObject obj = Instantiate(origin, startPostiion, Quaternion.identity);

            ProjectileController projectileController = obj.GetComponent<ProjectileController>();
            projectileController.Init(direction, rangeWeaponHandler, this);
        }

        public void CreateImpactParticle(Vector3 pos, RangeWeaponHandler rangeWeaponHandler)
        {
            impact.transform.position = pos;

            // 탄 크기에 따라 시작 속도
            ParticleSystem.EmissionModule em = impact.emission;
            em.SetBurst(0, new ParticleSystem.Burst(0, Mathf.Ceil(rangeWeaponHandler.BulletSize * 5)));

            ParticleSystem.MainModule mainModule = impact.main;
            mainModule.startSpeedMultiplier = rangeWeaponHandler.BulletSize * 10f;

            impact.Play();
        }
    }
}