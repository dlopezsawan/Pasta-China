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
        }
        public Wave[] waves;
        [HideInInspector]public int nextWave = 0;
        [HideInInspector]public int nextEnemy = 0;
        private int nextWay;

        public float timeBetweenWaves = 5f;
        [HideInInspector]public  float waveCountDown;
        private float searchCountdown = 1f;

        [HideInInspector]public SpawnState state = SpawnState.counting;

        public PathWay wayPoints;
        private GameManager gameManager;

        void Start()
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            waveCountDown = timeBetweenWaves;
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
            
            void WaveCompleted()
            {
                Debug.Log("Wave completed");
                state = SpawnState.counting;
                waveCountDown = timeBetweenWaves;

                if(nextWave + 1 > waves.Length - 1)
                {
                    //Result after waves
                    Debug.Log("All waves completed!!!");
                    gameManager.win = true;
                    gameManager.OnWinGame();
                    return;
                }
                else
                {
                    nextWave++;
                    nextEnemy = 0;
 
                    gameManager.UpdateWaveCountText();
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
                SpawnEnemy(_wave.enemyPrefabs, wayPoints.paths[0].spawnPoint);

                float randVal = Random.Range(0.8f, 2.0f);
                yield return new WaitForSeconds(randVal); // _wave.timeBetweenEnemysSpawn
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
            int randVal = Random.Range(0, 2);
            enemyScript.wayIndex = randVal;

            nextEnemy++;
        }
    }
}
