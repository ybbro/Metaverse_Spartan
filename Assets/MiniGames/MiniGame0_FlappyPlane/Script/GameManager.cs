using UnityEngine;

namespace MiniGame0
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        [SerializeField] Player player;
        [SerializeField] UIManager uiManager;
        [SerializeField] GameObject readyText;
        public Player Getplayer { get { return player; } }

        int score;
        public void AddScore() => uiManager.RenewalScore(++score);
        public int GetScore() => score;

        public bool isDie { get; private set; }

        public bool isReady { get; private set; }

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            score = 0;
            uiManager.RenewalScore(score);
            GameOver(false);
            isReady = false;
        }

        void Update()
        {
            // Ű �Է��� Update����! �Ƚ��忡�� ������ Ű �Է��� ���� �ʴ� ���� �߻�
            if(!isReady)
            {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0))
                    GameStart();
            }
        }

        public void GameOver(bool isGameOver)
        {
            isDie = isGameOver;
            uiManager.GameOverUIAppear(isDie);
        }

        void GameStart()
        {
            isReady = true;
            readyText.SetActive(false);
            player.PressStart();
        }
    }
}