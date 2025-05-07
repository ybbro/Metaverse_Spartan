using UnityEngine;
namespace MiniGame2
{
    public class EnemyController : BaseController
    {
        Transform target;

        [SerializeField] float followRange = 15f;

        // 몬스터가 나타난 이후 플레이어가 반응할 시간 필요
        // 해당 부분이 없으면 억까 발생
        float delayAfterSummon = 1;
        bool isSummonDelay = true;


        protected override void Start()
        {
            target = GameManager.Instance.player.transform;
            Invoke("BattleReady", delayAfterSummon);
        }

        void BattleReady()
        {
            isSummonDelay = false;
        }

        // 타겟까지의 거리
        protected float DistanceToTarget()
        {
            return Vector3.Distance(transform.position, target.position);
        }

        // 타겟 방향
        protected Vector2 DirectionToTarget()
        {
            return (target.position - transform.position).normalized;
        }

        protected override void HandleAction()
        {
            // 무기 or 타겟이 없다면 멈춰라!
            if (weaponHandler == null || target == null || isSummonDelay)
            {
                moveDir = Vector2.zero;
                return;
            }

            // 타겟(플레이어) 까지의 거리, 방향 계산
            float distance = DistanceToTarget();
            Vector2 dir = DirectionToTarget();

            // 공격하지 않는 상태
            isAttacking = false;

            // 플레이어가 색적 범위 안에 들어오면
            // 추격 시작
            if (distance <= followRange)
            {
                // 플레이어 바라보기
                lookDir = dir;
                // 공격 범위 안에 플레이어가 들어왔다면
                if (distance < weaponHandler.AttackRange)
                {
                    // 그런데.. 이미 플레이어 오브젝트를 타겟으로 하고 있고
                    // 그 위치를 Transform target; 이걸로 언제든 받아올 수 있는데 왜 레이어까지??
                    // 중복으로 필요없는 계산을 하는 듯
                    // 레이캐스트 교육용이라 쳐도 좋지 않은 방법인 듯

                    //int layerMaskTarget = weaponHandler.target;
                    // 레이캐스트를 플레이어 방향으로 발사하여 맞은 것이 있는지 판정
                    // 그런데 Level은 왜 넣는가?? >> 정말 설명이 부실해..
                    //RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, weaponHandler.AttackRange * 1.5f, LayerMask.GetMask("Level") + layerMaskTarget);

                    //if(hit.collider != null && layerMaskTarget == (layerMaskTarget | 1 << hit.collider.gameObject.layer))
                    //{
                    //    isAttacking = true;
                    //}

                    // 공격 시작
                    isAttacking = true;
                    // 그 자리에 멈추기
                    moveDir = Vector2.zero;
                    return;
                }
                // 공격 범위를 벗어났으므로 타겟을 향해 이동
                moveDir = dir;
            }
        }

        public override void Death()
        {
            GameManager.Instance.AddKillCount();
            GameManager.Instance.enemyManager.RemoveEnemy(this);
            base.Death();
        }
    }
}