using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVehicle : MonoBehaviour
{
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        
    }
}

[System.Serializable]
public class Vehicle
{
    RuntimeAnimatorController animatorController;
}
