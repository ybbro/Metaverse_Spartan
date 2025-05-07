using UnityEngine;
namespace MiniGame2
{
    public class MeleeWeaponHandler : WeaponHandler
    {
        protected override void Start()
        {
            base.Start();
            transform.localScale *= WeaponSize;
        }

        public override void Attack()
        {
            base.Attack();
            RaycastHit2D hit = Physics2D.BoxCast(
                transform.position + (Vector3)Controller.GetDir_Look * transform.localScale.x,
                transform.localScale, 0, Vector2.zero, 0, target);

            if (hit.collider)
            {
                if (hit.transform.TryGetComponent(out ResourceController resourceController))
                {
                    resourceController.ChangeHealth(-Power);
                    if (IsKnockBack)
                    {
                        if (hit.transform.TryGetComponent(out BaseController controller))
                            controller.ApplyKnockBack(transform, KnockbackPower, KnockbackTime);
                    }
                }
            }
        }

        public override void Rotate(bool isLeft)
        {
            if (isLeft)
                transform.eulerAngles = new Vector3(0, 180, 0);
            else
                transform.eulerAngles = Vector3.zero;
        }
    }
}