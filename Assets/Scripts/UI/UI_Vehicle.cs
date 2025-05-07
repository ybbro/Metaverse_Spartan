using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Vehicle : MonoBehaviour
{
    [SerializeField] PlayerControl player;
    int vehicleIndex_selected = -1;
    ScrollRect scrollRect;

    private void Awake()
    {
        scrollRect = GetComponentInChildren<ScrollRect>(true);
    }

    private void OnEnable()
    {
        // ���� ��ũ���� ���� ����
        scrollRect.verticalNormalizedPosition = 1;
    }

    public void Button_SelectVehicle(int index)
    {
        // �ٽ� �����ϸ� �ش� �̴ϰ��� ���� ����
        if (index == vehicleIndex_selected)
        {
            // ���� ���õ� UI ��� ������ ���� ����
            EventSystem.current.SetSelectedGameObject(null);
            vehicleIndex_selected = -1;
        }
        else
        {
            // ������ Ż �� ���
            vehicleIndex_selected = index;
        }
    }

    // ������ Ż �� ž��
    public void Button_ChangeConfirm()
    {
        if (vehicleIndex_selected >= 0)
            player.ChangeVehicle(vehicleIndex_selected);

        gameObject.SetActive(false);
    }
}
