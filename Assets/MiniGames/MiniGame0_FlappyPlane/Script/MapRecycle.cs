using UnityEngine;
namespace MiniGame0
{
    public class MapRecycle : MonoBehaviour
    {
        int recycleInterval = 32; // Ʈ���ſ� ���� �� ��Ұ� �̵��Ͽ� �ٽ� ������ �� ���� �ٰ� �� x ��ǥ
        string targetTag = "Map";

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (GameManager.instance.isDie)
                return;

            if (collision.CompareTag(targetTag))
            {
                Transform parent = collision.transform.parent;
                // ��ֹ� ���ġ
                if (parent.TryGetComponent(out Obstacle obstacle))
                    obstacle.Init_Obstacle();
                // �� ���ġ
                else
                    parent.transform.position += recycleInterval * Vector3.right;
            }
        }
    }
}
