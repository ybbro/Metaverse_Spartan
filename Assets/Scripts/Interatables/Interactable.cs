using UnityEngine;

// zep 생각해보면 메타버스에는 여러 사람이 들어와서 동시에 활동할 수 있어야 함
// 상호작용 가능한 오브젝트에는 각각의 플레이어가 접근할 수 있어야 함
// 해당 상호작용 오브젝트를 근접한(트리거 내) 플레이어들에게 등록 >> 각자 상호작용 조작할 수 있도록

[RequireComponent(typeof(Collider2D))]
public abstract class Interactable : MonoBehaviour
{
    // 상호작용 가능한 트리거 내에 플레이어가 들어와 있는지를 체크
    protected virtual void Start()
    {
        if (TryGetComponent(out Collider2D col))
            col.isTrigger = true;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerControl player))
            player.SetInteractable(this);
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerControl player))
            player.SetInteractable(null);
    }

    // 상호작용으로 생기는 결과
    public abstract void InteracionResult();
}
