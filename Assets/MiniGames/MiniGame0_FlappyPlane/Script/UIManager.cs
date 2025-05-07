using UnityEngine;
using TMPro;

namespace MiniGame0
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI scoreTxt;
        [SerializeField] Transform gameOverUI;

        public void RenewalScore(int score) => scoreTxt.text = score.ToString();

        public void GameOverUIAppear(bool isDie)
        {
            gameOverUI.gameObject.SetActive(isDie);
        }
    }
}