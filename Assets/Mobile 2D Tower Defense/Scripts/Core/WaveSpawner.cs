using System.Collections;
using UnityEngine;

namespace MobileTowerDefense
{
    public class WaveSpawner : MonoBehaviour
    {
        [System.Serializable]
        public class EnemyGroup
        {
            public GameObject[] enemies;
            public bool hasDialogue;
        }

        [System.Serializable]
        public class Wave
        {
            public EnemyGroup[] EnemyCluster;
            public float timeBetweenEnemysSpawn;
            public int[] pathIndex;
        }

        public enum SpawnState { spawning, waiting, counting };

        public Wave[] waves;
        public PathWay wayPoints;

        public UnityEngine.UI.Image[] alertFillers;
        public float timeBetweenWaves = 5f;
        private GameManager gameManager;

        [HideInInspector] public SpawnState state = SpawnState.counting;

        [HideInInspector] public int waveCounter = 0;
        [HideInInspector] public int clusterCounter = 0;
        [HideInInspector] public float waveCountDown = 0;
        private float searchCountdown = 1f;

        public GameObject[] enemyPrefabs;
        public GameObject bossPrefab;
        public GameObject dialoguePanel;
        ObjectPool objectPool;
        private void Start()
        {
            objectPool = GameObject.Find("ObjectPool").GetComponent<ObjectPool>();
            foreach (var item in enemyPrefabs)
            {
                objectPool.PreloadObjects(item,4);
            }
            objectPool.PreloadObjects(bossPrefab, 1);
        }
        void Update()
        {
            if (state == SpawnState.waiting)
            {
                if (IsWaveCompleted())
                    WaveCompleted();
                return;
            }

            if (waveCountDown <= 0 && waveCounter != waves.Length)
            {
                if (state != SpawnState.spawning)
                {
                    OnEnableAlertCanvas(false);
                    //Choose Which EnemySpawner is select
                    StartCoroutine(SpawnWave(waves[waveCounter]));
                }
            }
            else
            {
                UpdateWaveCountdown();
            }
        }

        public void OnEnableAlertCanvas(bool bEnable)
        {
            foreach (var index in waves[waveCounter].pathIndex)
            {
                alertFillers[index].transform.parent.gameObject.gameObject.SetActive(bEnable); 
            }
        }

        private void UpdateWaveCountdown()
        {
            waveCountDown -= Time.deltaTime;
     
            foreach (var alertfiller in alertFillers)
            {
                alertfiller.fillAmount = waveCountDown / timeBetweenWaves;
            }        
        }

        private bool IsWaveCompleted()
        {
            return clusterCounter >= waves[waveCounter].EnemyCluster.Length &&
                   waveCounter < waves.Length - 1;
        }

        private void WaveCompleted()
        {
            state = SpawnState.counting;
            waveCountDown = timeBetweenWaves;
            clusterCounter = 0;
            waveCounter++;
            OnEnableAlertCanvas(true);
        }

        public bool IsFinalClusterOfWave()
        {
            return waveCounter == waves.Length - 1 &&
                   clusterCounter >= waves[waveCounter].EnemyCluster.Length;
        }

        public bool EnemyIsAlive()
        {
            searchCountdown -= Time.deltaTime;
            if (searchCountdown > 0) return true;

            searchCountdown = 1f;
            return GameObject.FindGameObjectWithTag("Enemy") != null;
        }

        private IEnumerator SpawnWave(Wave wave)
        {
            state = SpawnState.spawning;

            foreach (var cluster in wave.EnemyCluster)
            {
                foreach (var enemy in cluster.enemies)
                {
                    int rand = Random.Range(0, wave.pathIndex.Length);
                    SpawnEnemy(enemy, wayPoints.paths[wave.pathIndex[rand]].spawnPoint, wave.pathIndex[rand]);
                    yield return new WaitForSeconds(Random.Range(0.8f, 2.0f));
                }
                clusterCounter++;
                if (cluster.hasDialogue)
                {
                    dialoguePanel.SetActive(true);
                }
                yield return new WaitForSeconds(Random.Range(1.4f, 2.0f));
            }

            state = SpawnState.waiting;
        }

        private void SpawnEnemy(GameObject enemyPrefab, Transform spawnPoint, int pathIndex)
        {          
            var enemyObject = objectPool.GetFromPool(enemyPrefab, spawnPoint);
            var enemyScript = enemyObject.GetComponent<Enemy>();
            enemyScript.enemyPrefab = enemyPrefab;
            enemyScript.Reset();
            enemyScript.currentPathIndex = pathIndex;
            enemyScript.wayIndex = Random.Range(0, wayPoints.paths[pathIndex].path.ways.Length);
        }
    }
}
