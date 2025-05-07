using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace MiniGame2
{
    public class HomeUI : BaseUI
    {
        [SerializeField] Button startButton, exitButton;

        private void Start()
        {
            // 버튼 이벤트 등록
            startButton.onClick.AddListener(OnClickStartBtn);
            exitButton.onClick.AddListener(OnClickExitBtn);
        }

        // 버튼 누를 때의 동작
        public void OnClickStartBtn() => GameManager.Instance.StartGame();
        public void OnClickExitBtn() => SceneManager.LoadScene("SpartaMetaverse"); // 메타버스 씬 다시 불러오기

        protected override UIState GetUIState()
        {
            return UIState.Home;
        }
    }
}