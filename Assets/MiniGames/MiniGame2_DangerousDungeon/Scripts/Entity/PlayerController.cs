using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MiniGame2
{
    public class PlayerController : BaseController
    {
        protected override void HandleAction()
        {
            // Ű �Է��� ���� �̵� ����
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            moveDir = new Vector2(horizontal, vertical).normalized;

            // ���콺 ��ġ�� ĳ���� ��ġ�� ���� ���� ����
            Vector2 mousePos = Input.mousePosition;
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            lookDir = (worldPos - (Vector2)transform.position);

            // 1���� ũ�Ⱑ ������ 0.. �� �κ��� �ʿ��Ѱ�??? ���콺�� ĳ���Ϳ� ���� �����ϸ� ������ �˱Ⱑ ������� �׳� �̷��� ó���ϴ� ��
            if (lookDir.magnitude < .9f)
                lookDir = Vector2.zero;
            else
                lookDir = lookDir.normalized;

            // ���콺 ��Ŭ������ ���� ��ȣ
            isAttacking = Input.GetMouseButton(0);
        }

        public override void Death()
        {
            base.Death();
            GameManager.Instance.GameOver();
        }
    }
}