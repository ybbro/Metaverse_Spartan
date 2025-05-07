using UnityEngine;

namespace MiniGame2
{
    public class AnimationHandler : MonoBehaviour
    {
        // ���ڿ� �񱳺��� ���� �񱳰� ������!
        // �׷��� �ִϸ��̼� �Ķ���� �̸��� �ش� �ִϸ��̼ǿ��� ���� �Ķ���͵��� id�� ���� >> StringToHash
        static readonly int IsMoving = Animator.StringToHash("isMove");
        static readonly int IsDamage = Animator.StringToHash("isDamage");

        protected Animator animator;

        protected virtual void Awake() => animator = GetComponentInChildren<Animator>();

        public void Move(Vector2 dir) => animator.SetBool(IsMoving, dir.magnitude > .5f);
        public void Damage() => animator.SetBool(IsDamage, true);
        public void InvincibilityEnd() => animator.SetBool(IsDamage, false);
    }
}
