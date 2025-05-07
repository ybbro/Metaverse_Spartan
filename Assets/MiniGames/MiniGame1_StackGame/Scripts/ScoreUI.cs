using TMPro;
using UnityEngine.UI;
using UnityEngine;
namespace MiniGame1
{
    public class ScoreUI : BaseUI
    {
        [Header("Texts"), SerializeField] TextMeshProUGUI scoreText;
        [SerializeField] TextMeshProUGUI comboText, bestScoreText, bestComboText;

        [Space, Header("Buttons"), SerializeField] Button startButton;
        [SerializeField] Button exitButton;

        protected override UIState GetUIState() => UIState.Score;

        public override void Init(UIManager uiManager)
        {
            base.Init(uiManager);

            startButton.onClick.AddListener(OnClickStartEvenet);
            exitButton.onClick.AddListener(OnClickExitEvenet);
        }

        public void SetUI(int score, int combo, int bestScore, int BestCombo)
        {
            scoreText.text = score.ToString();
            comboText.text = combo.ToString();
            bestScoreText.text = bestScore.ToString();
            bestComboText.text = BestCombo.ToString();
        }

        void OnClickStartEvenet() => uiManager.OnClickStart();
        void OnClickExitEvenet() => uiManager.OnClickExit();
    }
}