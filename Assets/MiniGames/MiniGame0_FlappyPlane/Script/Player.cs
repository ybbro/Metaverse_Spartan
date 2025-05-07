using UnityEngine;

namespace MiniGame0
{
    public class Player : MonoBehaviour
    {
        Animator animator; // ��ֹ��� �ɷ��� �� �ִϸ��̼� ��ü
        Rigidbody2D rigid2D; // ������ �� ���� ����ؿ�

        Transform model;

        [SerializeField, Tooltip("���� Ű�� ������ ���� ��� �ݿ� �ӵ�")]
        float jumpVelocity = 5;

        [SerializeField, Tooltip("�÷��̾��� X�� ���� �ӵ�")]
        float speed_X = 5;

        [SerializeField, Tooltip("���� �� ȸ����")]
        float rotation_max = 30f;

        [SerializeField, Tooltip("ȸ���� �ּ�ġ")]
        float rotation_min = -30f;

        [SerializeField, Tooltip("ȸ���� ���� �ӵ�")]
        float rotation_vel = -5f;

        bool isInputEntered = false; // ���� Ű �Է� ����

        void Start()
        {
            animator = GetComponentInChildren<Animator>();
            rigid2D = GetComponentInChildren<Rigidbody2D>();
            model = GetComponentInChildren<SpriteRenderer>().transform;
        }

        void Update()
        {
            if (GameManager.instance.isDie || !GameManager.instance.isReady)
                return;

            // Ű �Է��� Update����! �Ƚ��忡�� ������ Ű �Է��� ���� �ʴ� ���� �߻�
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0))
                isInputEntered = true;
        }

        private void FixedUpdate()
        {
            if (GameManager.instance.isDie || !GameManager.instance.isReady)
                return;

            if (isInputEntered)
            {
                // �̷��� �������� �ӵ��� ���� ���� ������ �ʳ�? �÷��� ü�� �� �����غ���
                // >> �������� �ӵ��� �־� ����� ������ �ȵǴ� ��찡 ���� >> �̿� ���� ��ü
                rigid2D.velocity = new Vector2(speed_X, jumpVelocity);
                model.rotation = Quaternion.Euler(Vector3.forward * rotation_max);
                isInputEntered = false;
            }

            // ���� �Ʒ� �������� ��������
            model.rotation = Quaternion.Euler(Vector3.forward * Mathf.Clamp(model.rotation.eulerAngles.z + rotation_vel * Time.fixedDeltaTime, rotation_min, rotation_max));
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (GameManager.instance.isDie)
                return;
            GameManager.instance.GameOver(true);
            animator.SetBool("isDie", true);
        }

        public void PressStart()
        {
            // x�� �������� �ε����� �ʴ� �� ���� ���� �ʱ⿡ �ʱ⿡ x�� ��� ����
            rigid2D.velocity = Vector2.right * speed_X;
            // �߷� ���� ����
            rigid2D.gravityScale = 1;
        }
    }
}
