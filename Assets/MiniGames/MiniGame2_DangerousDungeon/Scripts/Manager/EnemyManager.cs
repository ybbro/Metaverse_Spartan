using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MiniGame2
{
    public class EnemyManager : MonoBehaviour
    {
        Coroutine waveRoutine;
        [SerializeField] List<GameObject> enemyPrefabs;

        [SerializeField] List<Rect> spawnAreas;
        [SerializeField] Color gizmoColor = new Color(1, 0, 0, .3f);
        List<EnemyController> activeEnemies = new List<EnemyController>();

        bool enemySpawnComplite;

        [SerializeField] float timeBetSpawns = .2f;
        [SerializeField] float timeBetWaves = 1f;

        public void StartWave(int monsterCount)
        {
            if (monsterCount <= 0)
            {
                GameManager.Instance.EndWave();
                return;
            }
            if (waveRoutine != null)
                StopCoroutine(waveRoutine);
            waveRoutine = StartCoroutine(SpawnWave(monsterCount));
        }

        public void StopWave()
        {
            StopAllCoroutines();
        }

        IEnumerator SpawnWave(int monsterCount)
        {
            enemySpawnComplite = false;
            // 웨이브 간 대기
            yield return new WaitForSeconds(timeBetWaves);

            // 몬스터 생성
            for (int i = 0; i < monsterCount; i++)
            {
                yield return new WaitForSeconds(timeBetSpawns);
                SpawnRandomEnemy();
            }

            enemySpawnComplite = true;
        }

        void SpawnRandomEnemy()
        {
            if (enemyPrefabs.Count == 0 || spawnAreas.Count == 0)
            {
                Debug.LogWarning("적 혹은 스폰 포인트가 할당되지 않음");
            }

            // 적 랜덤
            GameObject randomPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];

            // 랜덤 생성 위치
            Rect randomArea = spawnAreas[Random.Range(0, spawnAreas.Count)];
            Vector2 randomPos = new Vector2(
                Random.Range(randomArea.xMin, randomArea.xMax),
                Random.Range(randomArea.yMin, randomArea.yMax));

            // 생성
            GameObject spawnEnemy = Instantiate(randomPrefab, new Vector3(randomPos.x, randomPos.y), Quaternion.identity);

            // 등록? >> 오브젝트 풀링도 아닌데 왜?
            EnemyController enemyController = spawnEnemy.GetComponent<EnemyController>();
            activeEnemies.Add(enemyController);
        }

        // 몬스터 등장 가능한 사각형 영역에 빨간 기즈모로 씬 뷰에서만 표시 >> 작업 편의용
        void OnDrawGizmosSelected()
        {
            if (spawnAreas == null) return;

            Gizmos.color = gizmoColor;
            foreach (var area in spawnAreas)
            {
                Vector3 center = new Vector3(area.x + area.width / 2, area.y + area.height / 2);
                Vector3 size = new Vector3(area.width, area.height);

                Gizmos.DrawCube(center, size);
            }
        }

        public void RemoveEnemy(EnemyController enemy)
        {
            activeEnemies.Remove(enemy);
            if (enemySpawnComplite && activeEnemies.Count == 0)
                GameManager.Instance.EndWave();
        }
    }
}