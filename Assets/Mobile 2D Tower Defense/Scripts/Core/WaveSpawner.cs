using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace MobileTowerDefense
{
    public class WaveSpawner : MonoBehaviour
    {
        [System.Serializable]
        public class EnemyGroup
        {
            public GameObject[] enemies;
        }

        [System.Serializable]
        public class Wave
        {
            public EnemyGroup[] EnemyCluster;
            public float timeBetweenEnemysSpawn;
        }

        public enum SpawnState { spawning, waiting, counting };

        public Wave[] waves;
        public PathWay wayPoints;

        [SerializeField] public GameObject alertCanvas;
        public UnityEngine.UI.Image alertFiller;

        public float timeBetweenWaves = 5f;

        private GameManager gameManager;

        [HideInInspector] public SpawnState state = SpawnState.counting;

        [HideInInspector] public int waveCounter = 0;
        [HideInInspector] public int clusterCounter = 0;
        [HideInInspector] public float waveCountDown;
        private float searchCountdown = 1f;

        void Start()
        {
            waveCountDown = timeBetweenWaves;
            waveCountDown = 0;
        }

        void Update()
        {
            if (state == SpawnState.waiting)
            {
                if (clusterCounter == waves[waveCounter].EnemyCluster.Length && waveCounter != waves.Length - 1)
                {
                    WaveCompleted();
                }
                return;
            }
            if (waveCountDown <= 0 && waveCounter != waves.Length)
            {
                if (state != SpawnState.spawning)
                {
                    //Start spawning wave
                    alertCanvas.SetActive(false);
                    StartCoroutine(SpawnWave(waves[waveCounter]));
                }
            }
            else
            {
                alertFiller.fillAmount = waveCountDown / timeBetweenWaves;
                waveCountDown -= Time.deltaTime;
            }
        }

        public bool IsFinalClusterOfWave()
        {
            return clusterCounter == waves[waveCounter].EnemyCluster.Length && waveCounter == waves.Length - 1;
        }

        void WaveCompleted()
        {
            state = SpawnState.counting;
            waveCountDown = timeBetweenWaves;

            waveCounter++;
            clusterCounter = 0;
            alertCanvas.SetActive(true);
        }

        public bool EnemyIsAlive()
        {
            searchCountdown -= Time.deltaTime;
            if (searchCountdown > 0) return true;

            searchCountdown = 1f;
            return GameObject.FindGameObjectWithTag("Enemy") != null;
        }

        IEnumerator SpawnWave(Wave _wave)
        {
            state = SpawnState.spawning;
            foreach (var cluster in waves[waveCounter].EnemyCluster)
            {
                foreach (var enemy in cluster.enemies)
                {
                    SpawnEnemy(enemy, wayPoints.paths[0].spawnPoint);
                    yield return new WaitForSeconds(Random.Range(0.8f, 2.0f));
                }
                clusterCounter++;
                yield return new WaitForSeconds(Random.Range(1.4f, 2.0f));
            }
            state = SpawnState.waiting;
        }

        private void SpawnEnemy(GameObject enemyPrefab, Transform spawnPoint)
        {
            var enemyObject = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            var enemyScript = enemyObject.GetComponent<Enemy>();
            enemyScript.wayIndex = Random.Range(0, wayPoints.paths[0].path.ways.Length);
        }
    }
}
