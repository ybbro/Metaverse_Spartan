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

        // 문 위,아래의 좌표
        Vector3 
            spawn_upper = transform.position + Vector3.up*1.5f,
            spawn_lower = transform.position + Vector3.down*1.5f;
        spawnpoints = new Vector3[2] { spawn_upper, spawn_lower };
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 부자연스러움을 최대한 없애기 위해 좌우 좌표는 최대한 유지
        if(collision.CompareTag(tag_Player))
        {
            if (collision.transform.position.y > transform.position.y)
                collision.transform.position = collision.transform.position.x * Vector3.right + spawnpoints[0];
            else
                collision.transform.position = collision.transform.position.x * Vector3.right + spawnpoints[1];
        }
    }

    // 콜라이더(트리거)를 활성화 했을 때
    // 아주 잠시라도 추방 기능이 발동하게끔 켜져 있어야 하기에
    // 인보크를 이용하여 짧은 시간 활성화
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
