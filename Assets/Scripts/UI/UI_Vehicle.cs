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
        // 수직 스크롤을 가장 위로
        scrollRect.verticalNormalizedPosition = 1;
    }

    public void Button_SelectVehicle(int index)
    {
        // 다시 선택하면 해당 미니게임 선택 해제
        if (index == vehicleIndex_selected)
        {
            // 현재 선택된 UI 요소 강제로 선택 해제
            EventSystem.current.SetSelectedGameObject(null);
            vehicleIndex_selected = -1;
        }
        else
        {
            // 선택한 탈 것 기억
            vehicleIndex_selected = index;
        }
    }

    // 선택한 탈 것 탑승
    public void Button_ChangeConfirm()
    {
        if (vehicleIndex_selected >= 0)
            player.ChangeVehicle(vehicleIndex_selected);

        gameObject.SetActive(false);
    }
}
