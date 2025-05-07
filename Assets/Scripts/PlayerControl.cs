using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// 플레이어끼리 겹치기가 되네.. 트리거인 것 같기는 한데
// 장애물한테 걸리는 걸 보면 또 콜라이더 같은 부분이 있음
// 평소에는 트리거였다가 장애물 영역에 들어가려 하면 콜라이더를 켜주는걸까?
// >> 생각은 했지만 필수과제가 먼저기에 구현하지 않음 !!!!!

public class PlayerControl : MonoBehaviour
{
    Rigidbody2D rigid;
    AnimationHandler animationHandler;
    SpriteRenderer spriteRenderer;
    Interactable interactable;

    [SerializeField] GameObject interacionInfo;
    [SerializeField] float speed = 5f;
    float jumpHeight = 2f, jumpTime = 0.5f;
    Vector3 sprite_origin, sprite_jumpMax;

    Vector2 moveDir;
    bool isJumping = false;
    int half; 

    private void Awake()
    {
        TryGetComponent(out animationHandler);
        TryGetComponent(out rigid);
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        // 외형 오브젝트의 초기 로컬 좌표
        sprite_origin = spriteRenderer.transform.localPosition;
        // 외형 오브젝트의 점프 최대 높이만큼 올랐을 때의 로컬 좌표
        sprite_jumpMax = sprite_origin + jumpHeight * Vector3.up;

        // jumpTime에 따른 코루틴에서의 루프 실행 횟수(jumpTime 초 동안 점프 수행)
        int loopCount = (int)(jumpTime / Time.fixedDeltaTime);
        // 그 절반을 나누어 올라갈 때/내려갈 때 사용할 루프 횟수
        half = loopCount / 2;
    }

    // 키 입력에 따라 아래 메서드들 호출

    // WASD, 키보드 화살표 입력에 대응하여 이동
    void OnMove(InputValue inputValue)
    {
        // 입력값에 따른 이동
        moveDir = inputValue.Get<Vector2>().normalized * speed;
        animationHandler.Move(moveDir);
        rigid.velocity = moveDir;

        // 이동 방향에 따라 캐릭터 스프라이트 좌우 반전
        // 0일 때는 제외해서 직전에 바라보던 방향을 유지하도록
        if (moveDir.x < 0)
            spriteRenderer.flipX = true;
        else if(moveDir.x > 0)
            spriteRenderer.flipX = false;
    }

    // 스페이스를 누르면 점프 호출
    void OnJump()
    {
        if (!isJumping)
            StartCoroutine(Jump());
    }

    // 잽 참고: 점프해도 바로 옆의 장애물을 넘지 못함
    // 점프는 스프라이트만 위아래로 잠시 왔다갔다 하게끔(리지드바디가 위아래로 이동하지 않음)
    IEnumerator Jump()
    {
        isJumping = true;

        // 점프하여 위로 올라가기
        for (int i = 1; i <= half; i++)
        {
            spriteRenderer.transform.localPosition = Vector3.Lerp(sprite_origin, sprite_jumpMax, (float)i / half);
            yield return new WaitForFixedUpdate();
        }

        // 다시 아래로 떨어지기 >> 원래 좌표로
        for (int i = 1; i <= half; i++)
        {
            spriteRenderer.transform.localPosition = Vector3.Lerp(sprite_jumpMax, sprite_origin, (float)i / half);
            yield return new WaitForFixedUpdate();
        }
        
        isJumping = false;
    }

    // 상호작용 키(F)를 누를 때 호출
    void OnInteraction()
    {
        // 상호작용 대상이 null이 아니라면 상호작용 기능 수행
        if (interactable)
            interactable.InteracionResult();
    }

    // 상호작용 가능 영역 안에 들어갈 때/나갈 때 호출
    public void SetInteractable(Interactable target)
    {
        // 상호작용 대상 등록/해제
        interactable = target;
        // 들어갈 때 상호작용 키 안내 UI 출현/ 나갈 때 UI 비활성화
        interacionInfo.SetActive(target);
    }

    //void DontFire(InputValue inputValue)
    //{
    //    // UI에 마우스가 올라가 있을 때는 동작하지 않도록(마우스 클릭 입력과 함께 쓰기 좋음)
    //    if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
    //        return;

    //    bool isAction = inputValue.isPressed;
    //}
}
