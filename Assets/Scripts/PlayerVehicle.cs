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
        // �ٲ� Ż�� ����(�ִϸ����� ��Ʈ�ѷ�)�� ��ü
        animator.runtimeAnimatorController = vehicle[i].animatorController;

        // �ӵ� ��ȯ
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
