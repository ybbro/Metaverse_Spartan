using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace MiniGame0
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI Text_BestScore;

        const string key_BestScore = "Best_MiniGame0";
        private void OnEnable()
        {
            // 최고점 경신
            int previousBest = PlayerPrefs.GetInt(key_BestScore, 0);
            int score_Current = GameManager.instance.GetScore();
            if (score_Current > previousBest)
            {
                PlayerPrefs.SetInt(key_BestScore, score_Current);
                previousBest = score_Current;
            }

            // 최고점 표시
            Text_BestScore.text = previousBest.ToString();
        }

        public void Button_Retry()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); // 현재 씬 다시 불러오기
        }

        public void Button_Exit()
        {
            SceneManager.LoadScene("SpartaMetaverse"); // 메타버스 씬 다시 불러오기
        }
    }
}
