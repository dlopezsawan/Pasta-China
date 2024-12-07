using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MobileTowerDefense
{
    public class GameManager : MonoBehaviour
    {
        [Header("UI")]
        public int gold;
        public Text goldDisplay;

        public int lives;
        public Text livesDisplay;

        private string waveCount;
        public Text waveCountDisplays;

        public GameObject startWaveButton;
        public GameObject gameOverMenu;
        public GameObject winnerMenu;
        [HideInInspector]public bool win = false;

        public GameObject waveSpawnerGameObject;


        public BuildingPlace[] buildingPlaces;
        [HideInInspector] public BuildingPlace currentBuildingPlace;
        public WaveSpawner waveSpawnerScript;

        void Start()
        {
            Time.timeScale = 1f;
            waveSpawnerGameObject.SetActive(false);
            gameOverMenu.SetActive(false);
            winnerMenu.SetActive(false);

            DisplayGoldText();
            DisplayLivesText();
            UpdateWaveCountText();
        }

        void Update()
        {
            //Check canvas enable or disable
            if (currentBuildingPlace != null)
            {
                currentBuildingPlace.ClosedOpenBuildingCanvas();
            }
        }

        public void UpdateWaveCountText()
        {
            waveCount = "Wave " + (waveSpawnerScript.nextWave + 1) + "/" + waveSpawnerScript.waves.Length;
            waveCountDisplays.text = waveCount;
        }

        public void DisplayLivesText()
        {
            livesDisplay.text = lives.ToString();
        }

        public void DisplayGoldText()
        {
            goldDisplay.text = gold.ToString();
        }

        public void OnWinGame()
        {
            if (waveSpawnerScript.nextWave == waveSpawnerScript.waves.Length - 1 && win == true)
            {
                if (GameObject.FindGameObjectWithTag("Enemy") == null)
                {
                    Invoke("YouWin", 3);
                }
            }
        }

        public void OnGameOver()
        {
            gameOverMenu.SetActive(true);
            Time.timeScale = 0f;
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
