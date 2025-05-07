using UnityEngine;

// zep �����غ��� ��Ÿ�������� ���� ����� ���ͼ� ���ÿ� Ȱ���� �� �־�� ��
// ��ȣ�ۿ� ������ ������Ʈ���� ������ �÷��̾ ������ �� �־�� ��
// �ش� ��ȣ�ۿ� ������Ʈ�� ������(Ʈ���� ��) �÷��̾�鿡�� ��� >> ���� ��ȣ�ۿ� ������ �� �ֵ���

[RequireComponent(typeof(Collider2D))]
public abstract class Interactable : MonoBehaviour
{
    // ��ȣ�ۿ� ������ Ʈ���� ���� �÷��̾ ���� �ִ����� üũ
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

    // ��ȣ�ۿ����� ����� ���
    public abstract void InteracionResult();
}
