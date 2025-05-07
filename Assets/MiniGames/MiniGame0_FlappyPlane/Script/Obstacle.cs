using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MiniGame0
{
    public class Obstacle : MonoBehaviour
    {
        [SerializeField] Transform spawnPoint; // �÷��̾� �ڽ� ������Ʈ�� �÷��̾��� �̵����� �׻� �÷��̾� ������ ȭ�� �ۿ� �ִ� ��ġ�� ������ �ִ� Ʈ������
        [SerializeField] Transform upper, lower; // ��, �Ʒ� ��ֹ��� Ʈ������

        // ����Ⱑ ����Ҹ��� �ּ�, �ִ� ���� ����
        [SerializeField, Range(-14f, -20f)] float diff_y_max = -15f;  // �ʹ� �γ��ϰ� �ϸ� ���尨�� ������

        const string tag_player = "Player";

        const float
            // �� ��ֹ� ��ġ ����
            upper_y_min = 4.5f, // 4���� ������ õ�����κ��� �������� ���ڿ������� ���� >> 4�� �����ϴ� �ٴڰ��� �Ÿ��� �ʹ� ª�� ����ϱ� ���� ����(+�ٴ� ��ö) >> 4.5f
            upper_y_max = 10f, // 10���� ȭ�鿡 ������ ����

            // �� ��ֹ��� �Ʒ� ��ֹ� �� ����
            diff_y_min = -14f; // �Ƹ� -13.5���� �� ������ �÷��̾� ��� �Ұ�. �ణ�� �̰��� �ּ� 14�� Ÿ��



        private void Start()
        {
            Randomize();
        }

        public void Init_Obstacle()
        {
            // ������ ȭ�� ���� ���� �������� �ٽ� ����
            transform.position = Vector3.right * spawnPoint.position.x;

            Randomize();
        }

        void Randomize()
        {
            // �� ��ֹ� ���Ʒ� �̵����� �ٸ� ��ֹ�ó�� ���̰Բ� + �÷����� �پ缺
            upper.localPosition = Vector3.up * Random.Range(upper_y_min, upper_y_max);
            lower.localPosition = Vector3.up * (upper.position.y + Random.Range(diff_y_min, diff_y_max));
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(tag_player) && !GameManager.instance.isDie)
            {
                GameManager.instance.AddScore();
                //Debug.Log($"���� ȹ�� : {GameManager.instance.GetScore()}");
            }
        }
    }
}
