using UnityEngine;
namespace MiniGame2
{
    // ������ UI ���¸� enum���� ����
    public enum UIState
    {
        Home,
        Game,
        GameOver,
    }

    public class UIManager : MonoBehaviour
    {
        // ���� UI ����
        private UIState currentState;

        // �� UI�� ������ ���� ��ũ��Ʈ
        HomeUI homeUI;
        GameUI gameUI;
        GameOverUI gameOverUI;
        private void Awake()
        {
            // �ڽ� ������Ʈ���� ������ UI�� ã�� �ʱ�ȭ
            homeUI = GetComponentInChildren<HomeUI>(true);
            gameUI = GetComponentInChildren<GameUI>(true);
            gameOverUI = GetComponentInChildren<GameOverUI>(true);

            // �ʱ� ���¸� Ȩ ȭ������ ����
            ChangeState(UIState.Home);
        }

        // UI ��ȯ
        public void SetPlayGame() => ChangeState(UIState.Game);
        public void SetGameOver()
        {
            ChangeState(UIState.GameOver);
            gameOverUI.UpdateKillText(GameManager.Instance.killCount);
        }

        // GameUI ���� ���� �� ���̺�, HP�� ���� ǥ�ø� ���� �޼���
        public void ChangeWave(int waveIndex) => gameUI.UpdateWaveText(waveIndex);
        public void ChangePlayerHP(float currentHP, float maxHP) => gameUI.UpdateHPBar(currentHP / maxHP);

        // ���� UI ���¸� �����ϰ�, �� UI ������Ʈ�� ���¸� ����
        public void ChangeState(UIState state)
        {
            // �� UI�� ǥ�� ���� ���� >> state�� �ش��ϴ� �ϳ��� ǥ��
            homeUI.SetState(state);
            gameUI.SetState(state);
            gameOverUI.SetState(state);
        }
    }
}