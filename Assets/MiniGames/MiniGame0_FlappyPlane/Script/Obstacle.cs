using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MiniGame0
{
    public class Obstacle : MonoBehaviour
    {
        [SerializeField] Transform spawnPoint; // 플레이어 자식 오브젝트로 플레이어의 이동에도 항상 플레이어 오른쪽 화면 밖에 있는 위치를 가지고 있는 트랩스폼
        [SerializeField] Transform upper, lower; // 위, 아래 장애물의 트랜스폼

        // 비행기가 통과할만한 최소, 최대 높이 차이
        [SerializeField, Range(-14f, -20f)] float diff_y_max = -15f;  // 너무 널널하게 하면 긴장감이 떨어짐

        const string tag_player = "Player";

        const float
            // 위 장애물 위치 범위
            upper_y_min = 4.5f, // 4보다 작으면 천장으로부터 떨어져서 부자연스럽게 보임 >> 4로 설정하니 바닥과의 거리가 너무 짧아 통과하기 쉽지 않음(+바닥 요철) >> 4.5f
            upper_y_max = 10f, // 10부터 화면에 보이지 않음

            // 위 장애물과 아래 장애물 간 간격
            diff_y_min = -14f; // 아마 -13.5보다 더 좁으면 플레이어 통과 불가. 약간의 이격을 둬서 14로 타협



        private void Start()
        {
            Randomize();
        }

        public void Init_Obstacle()
        {
            // 오른쪽 화면 밖의 스폰 지점에서 다시 등장
            transform.position = Vector3.right * spawnPoint.position.x;

            Randomize();
        }

        void Randomize()
        {
            // 각 장애물 위아래 이동으로 다른 장애물처럼 보이게끔 + 플레이의 다양성
            upper.localPosition = Vector3.up * Random.Range(upper_y_min, upper_y_max);
            lower.localPosition = Vector3.up * (upper.position.y + Random.Range(diff_y_min, diff_y_max));
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(tag_player) && !GameManager.instance.isDie)
            {
                GameManager.instance.AddScore();
                //Debug.Log($"점수 획득 : {GameManager.instance.GetScore()}");
            }
        }
    }
}
