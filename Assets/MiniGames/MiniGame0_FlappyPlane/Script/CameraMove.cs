using UnityEngine;

namespace MiniGame0
{
    public class CameraMove : MonoBehaviour
    {
        [SerializeField] float gap_x = 5; // �÷��̾ ȭ�� ���ʿ� ��ġ�ϰԲ� �̰��� �ֱ� ���� ��
        float zPos;
        private void Start()
        {
            zPos = transform.position.z;
        }

        private void FixedUpdate()
        {
            if (GameManager.instance.isDie)
                return;

            transform.position = new Vector3(GameManager.instance.Getplayer.transform.position.x + gap_x, 0, zPos);
        }
    }
}
