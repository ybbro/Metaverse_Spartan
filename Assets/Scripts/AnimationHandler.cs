using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    // 문자열 비교보다 숫자 비교가 빠르다!
    // 그래서 애니메이션 파라미터 이름을 해당 애니메이션에서 쓰는 파라미터들의 id로 변경 >> StringToHash
    static readonly int IsMoving = Animator.StringToHash("isMove");

    protected Animator animator;

    protected virtual void Awake() => animator = GetComponentInChildren<Animator>();

    public void Move(Vector2 dir) => animator.SetBool(IsMoving, dir.magnitude > .5f);
}
