using UnityEngine;
using UnityEngine.UI;
namespace MiniGame1
{
    public class HomeUI : BaseUI
    {
        [SerializeField] Button startButton, exitButton;

        protected override UIState GetUIState() => UIState.Home;

        public override void Init(UIManager uiManager)
        {
            base.Init(uiManager);

            // Start 버튼의 OnClick()이 실행할 메서드 추가
            startButton.onClick.AddListener(OnclickStartButton);
            exitButton.onClick.AddListener(OnclickExitButton);
        }

        void OnclickStartButton()
        {
            uiManager.OnClickStart();
        }

        void OnclickExitButton()
        {
            uiManager.OnClickExit();
        }
    }
}
