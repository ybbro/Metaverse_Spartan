using UnityEngine;
namespace MiniGame2
{
    // 각각의 UI 상태를 enum으로 구분
    public enum UIState
    {
        Home,
        Game,
        GameOver,
    }

    public class UIManager : MonoBehaviour
    {
        // 현재 UI 상태
        private UIState currentState;

        // 각 UI의 동작을 위한 스크립트
        HomeUI homeUI;
        GameUI gameUI;
        GameOverUI gameOverUI;
        private void Awake()
        {
            // 자식 오브젝트에서 각각의 UI를 찾아 초기화
            homeUI = GetComponentInChildren<HomeUI>(true);
            gameUI = GetComponentInChildren<GameUI>(true);
            gameOverUI = GetComponentInChildren<GameOverUI>(true);

            // 초기 상태를 홈 화면으로 설정
            ChangeState(UIState.Home);
        }

        // UI 전환
        public void SetPlayGame() => ChangeState(UIState.Game);
        public void SetGameOver()
        {
            ChangeState(UIState.GameOver);
            gameOverUI.UpdateKillText(GameManager.Instance.killCount);
        }

        // GameUI 에서 게임 중 웨이브, HP바 변경 표시를 위한 메서드
        public void ChangeWave(int waveIndex) => gameUI.UpdateWaveText(waveIndex);
        public void ChangePlayerHP(float currentHP, float maxHP) => gameUI.UpdateHPBar(currentHP / maxHP);

        // 현재 UI 상태를 변경하고, 각 UI 오브젝트에 상태를 전달
        public void ChangeState(UIState state)
        {
            // 각 UI의 표시 여부 결정 >> state에 해당하는 하나만 표시
            homeUI.SetState(state);
            gameUI.SetState(state);
            gameOverUI.SetState(state);
        }
    }
}