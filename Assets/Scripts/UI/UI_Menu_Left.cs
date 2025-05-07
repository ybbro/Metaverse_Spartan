using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Menu_Left : MonoBehaviour
{
    [SerializeField] GameObject UI_Vehicle_obj;

    public void Button_Vehicle()
    {
        UI_Vehicle_obj.SetActive(!UI_Vehicle_obj.activeSelf);
    }
}
