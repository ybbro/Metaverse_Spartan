using UnityEngine;
namespace MiniGame2
{
    public abstract class BaseUI : MonoBehaviour
    {
        protected abstract UIState GetUIState();

        public void SetState(UIState state)
        {
            gameObject.SetActive(GetUIState() == state);
        }
    }
}