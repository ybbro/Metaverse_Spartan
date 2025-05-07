using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    // ���ڿ� �񱳺��� ���� �񱳰� ������!
    // �׷��� �ִϸ��̼� �Ķ���� �̸��� �ش� �ִϸ��̼ǿ��� ���� �Ķ���͵��� id�� ���� >> StringToHash
    static readonly int IsMoving = Animator.StringToHash("isMove");

    protected Animator animator;

    protected virtual void Awake() => animator = GetComponentInChildren<Animator>();

    public void Move(Vector2 dir) => animator.SetBool(IsMoving, dir.magnitude > .5f);
}
