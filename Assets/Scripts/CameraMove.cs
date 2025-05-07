using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraMove : MonoBehaviour
{
    [SerializeField, Tooltip("카메라가 따라다닐 대상")] Transform target;
    [SerializeField, Tooltip("경계로 삼을 타일맵\n카메라 시야가 해당 타일맵 영역을 벗어나지 않음")] Tilemap mapEdge;

    float zPos; // 카메라 Z 좌표

    // 카메라 이동 한계 좌표
    // 0: 왼쪽 아래
    // 1: 오른쪽 위
    Vector2[] posLimits;

    void Start()
    {
        // 카메라가 보고 있는 영역의 크기 받아오기
        // 2D일 때, 카메라 컴포넌트의 Size가 화면 직교 경계까지의 길이
        float camera_half_height = Camera.main.orthographicSize;
        // 화면 비율에 따라 height로부터 width 산출
        float camera_half_width = camera_half_height * Camera.main.aspect;

        // 맵: 타일맵 경계 구하기
        Bounds bounds = mapEdge.localBounds;
        // 타일맵 왼쪽 아래 끝 좌표
        Vector2 bounds_min = bounds.min;
        // 타일맵 오른쪽 위 끝 좌표
        Vector2 bounds_max = bounds.max;

        // 카메라가 이동할 수 있는 좌표의 한계값 산출
        posLimits = new Vector2[2]
        { 
            // 맵 경계로부터 카메라가 보여줄 수 있는 너비, 높이의 절반만큼 이격을 준 값
            new Vector2(bounds_min.x + camera_half_width, bounds_min.y + camera_half_height), 
            new Vector2(bounds_max.x - camera_half_width, bounds_max.y - camera_half_height)
        };

        // 카메라 z축 초기 위치
        zPos = transform.position.z;
    }

    // 이동은 보통 Update() 혹은 FixedUpdate()에서 처리하기에
    // 타겟의 이동이 모두 끝난 뒤에 카메라를 움직이기 위해 LateUpdate() 사용
    private void LateUpdate()
    {
        // 타겟의 x,y 좌표에 clamp로 계산해둔 경계값만큼 최소, 최대 제한을 걸어 경계를 벗어나는 이동은 하지 않도록
        // 각 줄은 x,y,z 좌표
        transform.position = new Vector3
        (
            Mathf.Clamp(target.position.x, posLimits[0].x, posLimits[1].x),
            Mathf.Clamp(target.position.y, posLimits[0].y, posLimits[1].y),
            zPos
        );
    }
}
