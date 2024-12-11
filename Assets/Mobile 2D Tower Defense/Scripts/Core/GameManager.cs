using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MobileTowerDefense
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }


        [Header("UI")]
        public Text goldDisplay;
        public Text waveCountDisplays;
        public Text livesDisplay;

        public int gold;
        public int lives;

        private int lastGold = -1;
        private int lastLives = -1;
        private string lastWaveCount = "";

        public GameObject startWaveButton;
        public GameObject gameOverMenu;
        public GameObject winnerMenu;
        [HideInInspector] public bool win = false;

        public GameObject waveSpawnerGameObject;

        public BuildingPlace[] buildingPlaces;
        public WaveSpawner waveSpawnerScript;

        [SerializeField] GameObject[] PreloadPrefabs;
        [SerializeField] GameObject PreloadPrefab;
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        void Start()
        {       
            Time.timeScale = 1f;
            waveSpawnerGameObject.SetActive(false);
            gameOverMenu.SetActive(false);
            winnerMenu.SetActive(false);

            ObjectPool.Instance.PreloadObjects(PreloadPrefab,2);
        }

        void Update()
        {
            UpdateUI();

            if (lives == 0)
            {
                gameOverMenu.SetActive(true);
                Time.timeScale = 0f;
            }

            if (waveSpawnerScript.IsFinalClusterOfWave() && !waveSpawnerScript.EnemyIsAlive())
            {
                Invoke("YouWin", 3);
            }
        }

        private void UpdateUI()
        {
            if (gold != lastGold)
            {
                goldDisplay.text = gold.ToString();
                lastGold = gold;
            }

            if (lives != lastLives)
            {
                livesDisplay.text = lives.ToString();
                lastLives = lives;
            }

            string currentWaveCount = "Wave " + (waveSpawnerScript.waveCounter + 1) + "/" + waveSpawnerScript.waves.Length;
            if (currentWaveCount != lastWaveCount)
            {
                waveCountDisplays.text = currentWaveCount;
                lastWaveCount = currentWaveCount;
            }
        }

        public void OnCancelAlertButton()
        {
            if  (waveSpawnerScript.waveCounter == 0)
            {
                StartWaveButton();
            }
            else
            {
                gold += (int)(15 * waveSpawnerScript.waveCountDown);
                waveSpawnerScript.waveCountDown = 0;
            }
            waveSpawnerScript.alertCanvas.SetActive(false);
        }

        private void YouWin()
        {
            winnerMenu.SetActive(true);
            Time.timeScale = 0f;
        }

        public void StartWaveButton()
        {
            waveSpawnerGameObject.SetActive(true);
            startWaveButton.SetActive(false);
        }

        public void RestartButton()
        {
            SceneManager.LoadScene(0);
        }

        public void ResetBuildingPlaces()
        {
            foreach (BuildingPlace place in buildingPlaces)
            {
                place.ResetThisPlace();
            }
        }
    }
}
