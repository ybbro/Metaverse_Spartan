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
            // ���̺� �� ���
            yield return new WaitForSeconds(timeBetWaves);

            // ���� ����
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
                Debug.LogWarning("�� Ȥ�� ���� ����Ʈ�� �Ҵ���� ����");
            }

            // �� ����
            GameObject randomPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];

            // ���� ���� ��ġ
            Rect randomArea = spawnAreas[Random.Range(0, spawnAreas.Count)];
            Vector2 randomPos = new Vector2(
                Random.Range(randomArea.xMin, randomArea.xMax),
                Random.Range(randomArea.yMin, randomArea.yMax));

            // ����
            GameObject spawnEnemy = Instantiate(randomPrefab, new Vector3(randomPos.x, randomPos.y), Quaternion.identity);

            // ���? >> ������Ʈ Ǯ���� �ƴѵ� ��?
            EnemyController enemyController = spawnEnemy.GetComponent<EnemyController>();
            activeEnemies.Add(enemyController);
        }

        // ���� ���� ������ �簢�� ������ ���� ������ �� �信���� ǥ�� >> �۾� ���ǿ�
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