using UnityEngine;

public class NPC_MiniGame : Interactable
{
    [SerializeField] GameObject MiniGameUI;

    public override void InteracionResult()
    {
        MiniGameUI.SetActive(true);
    }
}
