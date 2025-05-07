using UnityEngine;

[System.Serializable]
public enum VehicleType
{
    none,
    plane,
}

public class PlayerVehicle : MonoBehaviour
{
    [SerializeField] Vehicle[] vehicle;
    Animator animator;

    private void Awake()
    {
        TryGetComponent(out animator);
    }

    public float ChangeVehicle_outlook(VehicleType vehicleType)
    {
        int i = 0;
        for (; i < vehicle.Length; i++)
        {
            if (vehicleType == vehicle[i].vehicleType)
                break;
        }
        // 바꿀 탈것 외형(애니메이터 컨트롤러)로 교체
        animator.runtimeAnimatorController = vehicle[i].animatorController;

        // 속도 반환
        return vehicle[i].speed;
    }
}

[System.Serializable]
public class Vehicle
{
    public VehicleType vehicleType;
    public RuntimeAnimatorController animatorController;
    public float speed;
}
