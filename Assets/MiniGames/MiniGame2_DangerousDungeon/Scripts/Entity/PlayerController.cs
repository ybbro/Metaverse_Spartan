using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MiniGame2
{
    public class PlayerController : BaseController
    {
        protected override void HandleAction()
        {
            // 키 입력을 통한 이동 방향
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            moveDir = new Vector2(horizontal, vertical).normalized;

            // 마우스 위치와 캐릭터 위치를 통한 보는 방향
            Vector2 mousePos = Input.mousePosition;
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            lookDir = (worldPos - (Vector2)transform.position);

            // 1보다 크기가 작으면 0.. 이 부분이 필요한가??? 마우스가 캐릭터에 아주 근접하면 방향을 알기가 어려워서 그냥 이렇게 처리하는 듯
            if (lookDir.magnitude < .9f)
                lookDir = Vector2.zero;
            else
                lookDir = lookDir.normalized;

            // 마우스 좌클릭으로 공격 신호
            isAttacking = Input.GetMouseButton(0);
        }

        public override void Death()
        {
            base.Death();
            GameManager.Instance.GameOver();
        }
    }
}