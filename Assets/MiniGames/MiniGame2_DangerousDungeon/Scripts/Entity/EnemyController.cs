using UnityEngine;
namespace MiniGame2
{
    public class EnemyController : BaseController
    {
        Transform target;

        [SerializeField] float followRange = 15f;

        // ���Ͱ� ��Ÿ�� ���� �÷��̾ ������ �ð� �ʿ�
        // �ش� �κ��� ������ ��� �߻�
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

        // Ÿ�ٱ����� �Ÿ�
        protected float DistanceToTarget()
        {
            return Vector3.Distance(transform.position, target.position);
        }

        // Ÿ�� ����
        protected Vector2 DirectionToTarget()
        {
            return (target.position - transform.position).normalized;
        }

        protected override void HandleAction()
        {
            // ���� or Ÿ���� ���ٸ� �����!
            if (weaponHandler == null || target == null || isSummonDelay)
            {
                moveDir = Vector2.zero;
                return;
            }

            // Ÿ��(�÷��̾�) ������ �Ÿ�, ���� ���
            float distance = DistanceToTarget();
            Vector2 dir = DirectionToTarget();

            // �������� �ʴ� ����
            isAttacking = false;

            // �÷��̾ ���� ���� �ȿ� ������
            // �߰� ����
            if (distance <= followRange)
            {
                // �÷��̾� �ٶ󺸱�
                lookDir = dir;
                // ���� ���� �ȿ� �÷��̾ ���Դٸ�
                if (distance < weaponHandler.AttackRange)
                {
                    // �׷���.. �̹� �÷��̾� ������Ʈ�� Ÿ������ �ϰ� �ְ�
                    // �� ��ġ�� Transform target; �̰ɷ� ������ �޾ƿ� �� �ִµ� �� ���̾����??
                    // �ߺ����� �ʿ���� ����� �ϴ� ��
                    // ����ĳ��Ʈ �������̶� �ĵ� ���� ���� ����� ��

                    //int layerMaskTarget = weaponHandler.target;
                    // ����ĳ��Ʈ�� �÷��̾� �������� �߻��Ͽ� ���� ���� �ִ��� ����
                    // �׷��� Level�� �� �ִ°�?? >> ���� ������ �ν���..
                    //RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, weaponHandler.AttackRange * 1.5f, LayerMask.GetMask("Level") + layerMaskTarget);

                    //if(hit.collider != null && layerMaskTarget == (layerMaskTarget | 1 << hit.collider.gameObject.layer))
                    //{
                    //    isAttacking = true;
                    //}

                    // ���� ����
                    isAttacking = true;
                    // �� �ڸ��� ���߱�
                    moveDir = Vector2.zero;
                    return;
                }
                // ���� ������ ������Ƿ� Ÿ���� ���� �̵�
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