using UnityEngine;
namespace MiniGame2
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public PlayerController player { get; private set; }
        ResourceController resourceController_player;

        [SerializeField] int wave_current = 0;

        public EnemyManager enemyManager { get; private set; }

        public SoundManager soundManager { get; private set; }

        public UIManager uiManager { get; private set; }
        public static bool isFirstLoading = true;

        public int killCount { get; private set; }
        public void AddKillCount() => killCount++;

        private void Awake()
        {
            if (Instance)
                Destroy(gameObject);
            else
                Instance = this;

            player = FindObjectOfType<PlayerController>();
            enemyManager = GetComponentInChildren<EnemyManager>();
            soundManager = GetComponentInChildren<SoundManager>();
            uiManager = FindObjectOfType<UIManager>();
            // �÷��̾��� ü�� ���ҽ� ��Ʈ�ѷ� ����
            resourceController_player = player.GetComponent<ResourceController>();
            // ü�� ���� �̺�Ʈ�� UI�� ����
            // �ߺ� ��� ������ ���� ���� ������ �� �ٽ� ���
            resourceController_player.Remove_HPChange_Event(uiManager.ChangePlayerHP);
            resourceController_player.Add_HPChange_Event(uiManager.ChangePlayerHP);
        }

        private void Start()
        {
            // ù �ε��̸� ��� ���·� ���� (Ÿ��Ʋ ȭ�鿡�� ��ư���� �����ϵ���)
            if (!isFirstLoading)
            {
                StartGame(); // �� ��° ���� �� �ε� �� �ڵ� ����
            }
            else
            {
                isFirstLoading = false; // ù �ε� �÷��� ����
            }
        }

        public void StartGame()
        {
            uiManager.SetPlayGame(); // UI ���¸� ���� ���·� ��ȯ
            StartNextWave();
        }
        void StartNextWave()
        {
            enemyManager.StartWave(1 + (++wave_current) / 5);
            uiManager.ChangeWave(wave_current); // UI�� ���� ���̺� ǥ��
        }

        public void EndWave()
        {
            StartNextWave();
        }

        public void GameOver()
        {

            enemyManager.StopWave();
            uiManager.SetGameOver();
        }
    }
}