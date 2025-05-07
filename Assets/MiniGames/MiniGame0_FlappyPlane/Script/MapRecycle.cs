using UnityEngine;
namespace MiniGame0
{
    public class MapRecycle : MonoBehaviour
    {
        int recycleInterval = 32; // 트리거에 닿은 맵 요소가 이동하여 다시 오른쪽 맵 끝에 붙게 할 x 좌표
        string targetTag = "Map";

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (GameManager.instance.isDie)
                return;

            if (collision.CompareTag(targetTag))
            {
                Transform parent = collision.transform.parent;
                // 장애물 재배치
                if (parent.TryGetComponent(out Obstacle obstacle))
                    obstacle.Init_Obstacle();
                // 맵 재배치
                else
                    parent.transform.position += recycleInterval * Vector3.right;
            }
        }
    }
}
