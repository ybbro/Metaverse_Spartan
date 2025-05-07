using TMPro;
using UnityEngine;
namespace MiniGame1
{
    public class GameUI : BaseUI
    {
        [SerializeField] TextMeshProUGUI scoreText, comboText, maxComboText;
        protected override UIState GetUIState() => UIState.Game;

        public override void Init(UIManager uiManager)
        {
            base.Init(uiManager);
        }

        public void SetUI(int score, int combo, int maxCombo)
        {
            scoreText.text = score.ToString();
            comboText.text = combo.ToString();
            maxComboText.text = maxCombo.ToString();
        }
    }
}