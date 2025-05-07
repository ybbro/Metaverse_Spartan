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
            // 플레이어의 체력 리소스 컨트롤러 설정
            resourceController_player = player.GetComponent<ResourceController>();
            // 체력 변경 이벤트를 UI에 연결
            // 중복 등록 방지를 위해 먼저 제거한 뒤 다시 등록
            resourceController_player.Remove_HPChange_Event(uiManager.ChangePlayerHP);
            resourceController_player.Add_HPChange_Event(uiManager.ChangePlayerHP);
        }

        private void Start()
        {
            // 첫 로딩이면 대기 상태로 유지 (타이틀 화면에서 버튼으로 시작하도록)
            if (!isFirstLoading)
            {
                StartGame(); // 두 번째 이후 씬 로딩 시 자동 시작
            }
            else
            {
                isFirstLoading = false; // 첫 로딩 플래그 해제
            }
        }

        public void StartGame()
        {
            uiManager.SetPlayGame(); // UI 상태를 게임 상태로 전환
            StartNextWave();
        }
        void StartNextWave()
        {
            enemyManager.StartWave(1 + (++wave_current) / 5);
            uiManager.ChangeWave(wave_current); // UI에 현재 웨이브 표시
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