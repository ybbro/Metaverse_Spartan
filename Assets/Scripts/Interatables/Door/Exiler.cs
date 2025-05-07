using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exiler : MonoBehaviour
{
    const string tag_Player = "Player";
    BoxCollider2D col;

    Vector3[] spawnpoints;
    private void Start()
    {
        TryGetComponent(out col);
        col.isTrigger = true;
        col.enabled = false;

        // �� ��,�Ʒ��� ��ǥ
        Vector3 
            spawn_upper = transform.position + Vector3.up*1.5f,
            spawn_lower = transform.position + Vector3.down*1.5f;
        spawnpoints = new Vector3[2] { spawn_upper, spawn_lower };
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ���ڿ��������� �ִ��� ���ֱ� ���� �¿� ��ǥ�� �ִ��� ����
        if(collision.CompareTag(tag_Player))
        {
            if (collision.transform.position.y > transform.position.y)
                collision.transform.position = collision.transform.position.x * Vector3.right + spawnpoints[0];
            else
                collision.transform.position = collision.transform.position.x * Vector3.right + spawnpoints[1];
        }
    }

    // �ݶ��̴�(Ʈ����)�� Ȱ��ȭ ���� ��
    // ���� ��ö� �߹� ����� �ߵ��ϰԲ� ���� �־�� �ϱ⿡
    // �κ�ũ�� �̿��Ͽ� ª�� �ð� Ȱ��ȭ
    public void Command_Exile()
    {
        CancelInvoke();
        col.enabled = true;
        Invoke("OffCollider", .1f);
    }

    void OffCollider()
    {
        col.enabled = false;
    }
}
