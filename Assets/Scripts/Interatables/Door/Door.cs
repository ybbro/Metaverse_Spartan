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
        // 여는 건 즉시 해도 괜찮네요
        if (door.activeSelf)
        {
            DoorOpenClose();
        }
        else
        {
            // 문이 닫힐 때 문 콜라이더가 생기면서 그 영역 안의 플레이어가 바깥으로 충돌하여 중심 방향으로부터 밀려남
            // >> 벽에 끼이는 현상 발생 !!!
            // 문의 콜라이더 영역 >> 트리거 오브젝트의 로컬 좌표 x = -1~1, y = 0~2
            // 문을 닫기 전에 해당 영역안에 있는 콜라이더들을 전부 벽과 겹치지 않을 바깥으로 빼내면 되지 않을까?
            // 된다아..
            exiler.Command_Exile();
            Invoke("DoorOpenClose", .05f);
        }
    }

    void DoorOpenClose()
    {
        // 문 열기/닫기 상태 전환
        door.SetActive(!door.activeSelf);
    }
}
