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
            // ��ư �̺�Ʈ ���
            startButton.onClick.AddListener(OnClickStartBtn);
            exitButton.onClick.AddListener(OnClickExitBtn);
        }

        // ��ư ���� ���� ����
        public void OnClickStartBtn() => GameManager.Instance.StartGame();
        public void OnClickExitBtn() => SceneManager.LoadScene("SpartaMetaverse"); // ��Ÿ���� �� �ٽ� �ҷ�����

        protected override UIState GetUIState()
        {
            return UIState.Home;
        }
    }
}