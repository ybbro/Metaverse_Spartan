using UnityEngine;

namespace MiniGame0
{
    public class CameraMove : MonoBehaviour
    {
        [SerializeField] float gap_x = 5; // 플레이어가 화면 왼쪽에 위치하게끔 이격을 주기 위한 값
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
