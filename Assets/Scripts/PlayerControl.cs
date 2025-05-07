using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// �÷��̾�� ��ġ�Ⱑ �ǳ�.. Ʈ������ �� ����� �ѵ�
// ��ֹ����� �ɸ��� �� ���� �� �ݶ��̴� ���� �κ��� ����
// ��ҿ��� Ʈ���ſ��ٰ� ��ֹ� ������ ���� �ϸ� �ݶ��̴��� ���ִ°ɱ�?
// >> ������ ������ �ʼ������� �����⿡ �������� ���� !!!!!

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

        // ���� ������Ʈ�� �ʱ� ���� ��ǥ
        sprite_origin = spriteRenderer.transform.localPosition;
        // ���� ������Ʈ�� ���� �ִ� ���̸�ŭ �ö��� ���� ���� ��ǥ
        sprite_jumpMax = sprite_origin + jumpHeight * Vector3.up;

        // jumpTime�� ���� �ڷ�ƾ������ ���� ���� Ƚ��(jumpTime �� ���� ���� ����)
        int loopCount = (int)(jumpTime / Time.fixedDeltaTime);
        // �� ������ ������ �ö� ��/������ �� ����� ���� Ƚ��
        half = loopCount / 2;
    }

    // Ű �Է¿� ���� �Ʒ� �޼���� ȣ��

    // WASD, Ű���� ȭ��ǥ �Է¿� �����Ͽ� �̵�
    void OnMove(InputValue inputValue)
    {
        // �Է°��� ���� �̵�
        moveDir = inputValue.Get<Vector2>().normalized * speed;
        animationHandler.Move(moveDir);
        rigid.velocity = moveDir;

        // �̵� ���⿡ ���� ĳ���� ��������Ʈ �¿� ����
        // 0�� ���� �����ؼ� ������ �ٶ󺸴� ������ �����ϵ���
        if (moveDir.x < 0)
            spriteRenderer.flipX = true;
        else if(moveDir.x > 0)
            spriteRenderer.flipX = false;
    }

    // �����̽��� ������ ���� ȣ��
    void OnJump()
    {
        if (!isJumping)
            StartCoroutine(Jump());
    }

    // �� ����: �����ص� �ٷ� ���� ��ֹ��� ���� ����
    // ������ ��������Ʈ�� ���Ʒ��� ��� �Դٰ��� �ϰԲ�(������ٵ� ���Ʒ��� �̵����� ����)
    IEnumerator Jump()
    {
        isJumping = true;

        // �����Ͽ� ���� �ö󰡱�
        for (int i = 1; i <= half; i++)
        {
            spriteRenderer.transform.localPosition = Vector3.Lerp(sprite_origin, sprite_jumpMax, (float)i / half);
            yield return new WaitForFixedUpdate();
        }

        // �ٽ� �Ʒ��� �������� >> ���� ��ǥ��
        for (int i = 1; i <= half; i++)
        {
            spriteRenderer.transform.localPosition = Vector3.Lerp(sprite_jumpMax, sprite_origin, (float)i / half);
            yield return new WaitForFixedUpdate();
        }
        
        isJumping = false;
    }

    // ��ȣ�ۿ� Ű(F)�� ���� �� ȣ��
    void OnInteraction()
    {
        // ��ȣ�ۿ� ����� null�� �ƴ϶�� ��ȣ�ۿ� ��� ����
        if (interactable)
            interactable.InteracionResult();
    }

    // ��ȣ�ۿ� ���� ���� �ȿ� �� ��/���� �� ȣ��
    public void SetInteractable(Interactable target)
    {
        // ��ȣ�ۿ� ��� ���/����
        interactable = target;
        // �� �� ��ȣ�ۿ� Ű �ȳ� UI ����/ ���� �� UI ��Ȱ��ȭ
        interacionInfo.SetActive(target);
    }

    //void DontFire(InputValue inputValue)
    //{
    //    // UI�� ���콺�� �ö� ���� ���� �������� �ʵ���(���콺 Ŭ�� �Է°� �Բ� ���� ����)
    //    if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
    //        return;

    //    bool isAction = inputValue.isPressed;
    //}
}
