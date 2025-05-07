using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraMove : MonoBehaviour
{
    [SerializeField, Tooltip("ī�޶� ����ٴ� ���")] Transform target;
    [SerializeField, Tooltip("���� ���� Ÿ�ϸ�\nī�޶� �þ߰� �ش� Ÿ�ϸ� ������ ����� ����")] Tilemap mapEdge;

    float zPos; // ī�޶� Z ��ǥ

    // ī�޶� �̵� �Ѱ� ��ǥ
    // 0: ���� �Ʒ�
    // 1: ������ ��
    Vector2[] posLimits;

    void Start()
    {
        // ī�޶� ���� �ִ� ������ ũ�� �޾ƿ���
        // 2D�� ��, ī�޶� ������Ʈ�� Size�� ȭ�� ���� �������� ����
        float camera_half_height = Camera.main.orthographicSize;
        // ȭ�� ������ ���� height�κ��� width ����
        float camera_half_width = camera_half_height * Camera.main.aspect;

        // ��: Ÿ�ϸ� ��� ���ϱ�
        Bounds bounds = mapEdge.localBounds;
        // Ÿ�ϸ� ���� �Ʒ� �� ��ǥ
        Vector2 bounds_min = bounds.min;
        // Ÿ�ϸ� ������ �� �� ��ǥ
        Vector2 bounds_max = bounds.max;

        // ī�޶� �̵��� �� �ִ� ��ǥ�� �Ѱ谪 ����
        posLimits = new Vector2[2]
        { 
            // �� ���κ��� ī�޶� ������ �� �ִ� �ʺ�, ������ ���ݸ�ŭ �̰��� �� ��
            new Vector2(bounds_min.x + camera_half_width, bounds_min.y + camera_half_height), 
            new Vector2(bounds_max.x - camera_half_width, bounds_max.y - camera_half_height)
        };

        // ī�޶� z�� �ʱ� ��ġ
        zPos = transform.position.z;
    }

    // �̵��� ���� Update() Ȥ�� FixedUpdate()���� ó���ϱ⿡
    // Ÿ���� �̵��� ��� ���� �ڿ� ī�޶� �����̱� ���� LateUpdate() ���
    private void LateUpdate()
    {
        // Ÿ���� x,y ��ǥ�� clamp�� ����ص� ��谪��ŭ �ּ�, �ִ� ������ �ɾ� ��踦 ����� �̵��� ���� �ʵ���
        // �� ���� x,y,z ��ǥ
        transform.position = new Vector3
        (
            Mathf.Clamp(target.position.x, posLimits[0].x, posLimits[1].x),
            Mathf.Clamp(target.position.y, posLimits[0].y, posLimits[1].y),
            zPos
        );
    }
}
