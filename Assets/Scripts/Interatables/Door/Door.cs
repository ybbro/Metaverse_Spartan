using UnityEngine;
public class Door : Interactable
{
    GameObject door;
    Exiler exiler;

    protected override void Start()
    {
        base.Start();
        door = transform.GetChild(0).gameObject;
        exiler = GetComponentInChildren<Exiler>();
    }

    public override void InteracionResult()
    {
        // ���� �� ��� �ص� �����׿�
        if (door.activeSelf)
        {
            DoorOpenClose();
        }
        else
        {
            // ���� ���� �� �� �ݶ��̴��� ����鼭 �� ���� ���� �÷��̾ �ٱ����� �浹�Ͽ� �߽� �������κ��� �з���
            // >> ���� ���̴� ���� �߻� !!!
            // ���� �ݶ��̴� ���� >> Ʈ���� ������Ʈ�� ���� ��ǥ x = -1~1, y = 0~2
            // ���� �ݱ� ���� �ش� �����ȿ� �ִ� �ݶ��̴����� ���� ���� ��ġ�� ���� �ٱ����� ������ ���� ������?
            // �ȴپ�..
            exiler.Command_Exile();
            Invoke("DoorOpenClose", .05f);
        }
    }

    void DoorOpenClose()
    {
        // �� ����/�ݱ� ���� ��ȯ
        door.SetActive(!door.activeSelf);
    }
}
