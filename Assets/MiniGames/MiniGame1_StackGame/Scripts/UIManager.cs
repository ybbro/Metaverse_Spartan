using UnityEngine;
using UnityEngine.SceneManagement;
namespace MiniGame1
{
    public enum UIState
    {
        Home,
        Game,
        Score,
    }

    public class UIManager : MonoBehaviour
    {
        static UIManager instance;

        public static UIManager Instance { get => instance; }

        UIState currentState = UIState.Home;
        HomeUI homeui = null;
        GameUI gameui = null;
        ScoreUI scoreui = null;

        Spawner spawner = null;

        private void Awake()
        {
            instance = this;
            spawner = FindObjectOfType<Spawner>();
            homeui = GetComponentInChildren<HomeUI>(true);
            homeui?.Init(this);

            gameui = GetComponentInChildren<GameUI>(true);
            gameui?.Init(this);

            scoreui = GetComponentInChildren<ScoreUI>(true);
            scoreui?.Init(this);

            ChangeState(UIState.Home);
        }

        public void ChangeState(UIState state)
        {
            currentState = state;
            homeui?.SetActive(currentState);
            gameui?.SetActive(currentState);
            scoreui?.SetActive(currentState);
        }

        public void OnClickStart()
        {
            spawner.Restart();
            ChangeState(UIState.Game);
        }

        public void OnClickExit()
        {
            SceneManager.LoadScene("SpartaMetaverse"); // 메타버스 씬 다시 불러오기

            // 환경마다 다른 동작을 하게끔
            // 플레이 종료
            //#if UNITY_EDITOR
            //        UnityEditor.EditorApplication.isPlaying = false; 
            //#else
            //        Application.Quit();
            //#endif
        }

        public void UpdateScore()
        {
            gameui.SetUI(spawner.Score, spawner.Combo, spawner.MaxCombo);
        }

        public void SetScoreUI()
        {
            scoreui.SetUI(spawner.Score, spawner.Combo, spawner.BestScore, spawner.MaxCombo);
            ChangeState(UIState.Score);
        }
    }
}