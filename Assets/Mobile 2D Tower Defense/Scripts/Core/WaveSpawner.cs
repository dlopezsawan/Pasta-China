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

        public enum SpawnState {spawning, waiting, counting};

        public Wave[] waves;
        [HideInInspector]public int nextWave = 0;
        [HideInInspector]public int nextEnemy = 0;
        private int nextWay;
        private int nextCluster=0;

        public float timeBetweenWaves = 5f;
        [HideInInspector]public  float waveCountDown;
        private float searchCountdown = 1f;

        [HideInInspector]public SpawnState state = SpawnState.counting;

        public PathWay wayPoints;
        private GameManager gameManager;

        [SerializeField] GameObject AlertCanvas;
        public UnityEngine.UI.Image alertFiller;

        void Start()
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            waveCountDown = timeBetweenWaves;
            waveCountDown = 0;
        }

        void Update()
        {
            if (state == SpawnState.waiting)
            {
                if (!EnemyIsAlive())
                {
                    if (nextWave + 1 > waves.Length - 1)
                    {
                        gameManager.win = true;
                        gameManager.OnWinGame();
                        return;
                    }
                }
                else if (nextCluster == waves[nextWave].EnemyCluster.Length && nextWave != waves.Length-1)
                {
                    WaveCompleted();
                }
                else
                {
                    return;
                }
            }
            if (waveCountDown <= 0 && nextWave != waves.Length)
            {
                if (state != SpawnState.spawning)
                {
                    //Start spawning wave
                    AlertCanvas.SetActive(false);
                    StartCoroutine(SpawnWave(waves[nextWave]));
                    gameManager.win = false;
                }
            }
            else
            {
                alertFiller.fillAmount = waveCountDown / timeBetweenWaves;
                waveCountDown -= Time.deltaTime;
            }
        }

        void WaveCompleted()
        {
            state = SpawnState.counting;
            waveCountDown = timeBetweenWaves;

            nextWave++;
            nextEnemy = 0;
            nextCluster = 0;
            AlertCanvas.SetActive(true);
            gameManager.UpdateWaveCountText();

        }

        public void OnCancelAlertButton()
        {
            if (nextWave == 0)
            {
                gameManager.StartWaveButton();                          
            }
            else
            {
                gameManager.gold += (int)(15 * waveCountDown);
                gameManager.DisplayGoldText();
                waveCountDown = 0;
            }
            AlertCanvas.SetActive(false);
        }

        bool EnemyIsAlive()
        {
            searchCountdown -= Time.deltaTime;
            if (searchCountdown <= 0f)
            {
                searchCountdown = 1f;
                if (GameObject.FindGameObjectWithTag("Enemy") == null)
                {
                    return false;
                }
            }
            return true;

        }

        IEnumerator SpawnWave(Wave _wave)
        {
            state = SpawnState.spawning;
            foreach (var wave in waves[nextWave].EnemyCluster)
            {
                //spawn
                for (int i = 0; i < wave.enemies.Length; i++)
                {
                    SpawnEnemy(wave.enemies, wayPoints.paths[0].spawnPoint);
                    yield return new WaitForSeconds(Random.Range(0.8f, 2.0f)); // _wave.timeBetweenEnemysSpawn
                }
                nextEnemy = 0;
                nextCluster++;
                yield return new WaitForSeconds(Random.Range(1.4f, 2.0f));
            }
            state = SpawnState.waiting;
            if (nextWave == waves.Length - 1)
            {
                gameManager.win = true;
            }

            yield break;
        }
        void SpawnEnemy(GameObject[] _enemy, Transform _spawn)
        {
            GameObject enemyObject = Instantiate(_enemy[nextEnemy], _spawn.position, _spawn.rotation);

            Enemy enemyScript = enemyObject.GetComponent<Enemy>();
            int randVal = Random.Range(0, 2);
            enemyScript.wayIndex = randVal;

            nextEnemy++;
        }
    }
}
