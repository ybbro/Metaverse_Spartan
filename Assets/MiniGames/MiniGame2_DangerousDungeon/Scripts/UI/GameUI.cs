using UnityEngine;
using TMPro;
using UnityEngine.UI;
namespace MiniGame2
{
    public class GameUI : BaseUI
    {
        [SerializeField] TextMeshProUGUI waveText;
        [SerializeField] Slider hpSlider;

        private void Start()
        {
            UpdateHPBar(1);
        }

        public void UpdateHPBar(float percentage) => hpSlider.value = percentage;
        public void UpdateWaveText(int wave) => waveText.text = wave.ToString();

        protected override UIState GetUIState()
        {
            return UIState.Game;
        }
    }
}