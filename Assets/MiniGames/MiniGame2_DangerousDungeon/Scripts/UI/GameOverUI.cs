using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
namespace MiniGame2
{
    public class GameOverUI : BaseUI
    {
        const string key_BestScore = "Best_MiniGame2";


        [SerializeField] TextMeshProUGUI killCountText;
        [SerializeField] Button startButton, exitButton;

        private void Start()
        {
            startButton.onClick.AddListener(OnClickStartBtn);
            exitButton.onClick.AddListener(OnClickExitBtn);
        }
        public void UpdateKillText(int killCount)
        {
            killCountText.text = killCount.ToString();
            if (killCount > PlayerPrefs.GetInt(key_BestScore, 0))
                PlayerPrefs.SetInt(key_BestScore, killCount);
        }
        public void OnClickStartBtn() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        public void OnClickExitBtn() => SceneManager.LoadScene("SpartaMetaverse"); // 메타버스 씬 다시 불러오기 //Application.Quit();

        protected override UIState GetUIState()
        {
            return UIState.GameOver;
        }
    }
}