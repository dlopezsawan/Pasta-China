using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MobileTowerDefense
{
    public class WaveSpawner : MonoBehaviour
    {
        public enum SpawnState {spawning, waiting, counting};

        [System.Serializable]
        public class Wave
        {
            public GameObject[] enemyPrefabs;
            public float timeBetweenEnemysSpawn;
            public Transform spawnPoint;
        }
        public Wave[] waves;
        [HideInInspector]public int nextWave = 0;
        [HideInInspector]public int nextEnemy = 0;
        private int nextWay;

        public float timeBetweenWaves = 5f;
        [HideInInspector]public  float waveCountDown;
        private float searchCountdown = 1f;

        [HideInInspector]public SpawnState state = SpawnState.counting;

        public WayPoints wayPoints;
        private GameManager gameManager;

        void Start()
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            waveCountDown = timeBetweenWaves;
            WhichWay();
            waveCountDown = 0;
        }

            void Update()
            {
                if(state == SpawnState.waiting)
                {
                    if(!EnemyIsAlive())
                    {
                        WaveCompleted();
                    }
                    else
                    {
                        return;
                    }
                }
                if(waveCountDown <= 0 && nextWave != waves.Length)
                {
                    if(state != SpawnState.spawning)
                    {
                        //Start spawning wave
                        StartCoroutine(SpawnWave(waves[nextWave]));
                        gameManager.win = false;
                    }
                }
                else
                {
                    waveCountDown -= Time.deltaTime;
                }
            }
            
            public void WhichWay()
            {
                for(int i = 0; i < wayPoints.ways.Length; i++)
                {
                    if(waves[nextWave].spawnPoint == wayPoints.ways[i].spawnPoint)
                    {
                        nextWay = i;
                        break;
                    }
                }
            }

            void WaveCompleted()
            {
                Debug.Log("Wave completed");
                state = SpawnState.counting;
                waveCountDown = timeBetweenWaves;

                if(nextWave + 1 > waves.Length - 1)
                {
                    //Result after waves
                    Debug.Log("All waves completed!!!");
                    return;
                }
                else
                {
                    nextWave++;
                    nextEnemy = 0;
                    WhichWay();
                }
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

            IEnumerator SpawnWave (Wave _wave)
            {
                state = SpawnState.spawning;

                //spawn
                for(int i = 0; i < _wave.enemyPrefabs.Length; i++)
                {
                    SpawnEnemy(_wave.enemyPrefabs, _wave.spawnPoint);
                    yield return new WaitForSeconds( _wave.timeBetweenEnemysSpawn );
                }

                state = SpawnState.waiting;
                if(nextWave == waves.Length-1)
                {
                    gameManager.win = true;
                }

                yield break;
            }
            void SpawnEnemy(GameObject[] _enemy, Transform _spawn)
            {
                GameObject enemyObject = Instantiate(_enemy[nextEnemy], _spawn.position, _spawn.rotation);

                Enemy enemyScript = enemyObject.GetComponent<Enemy>();
                enemyScript.wayIndex = nextWay;

                nextEnemy++;
            }
    }
}
