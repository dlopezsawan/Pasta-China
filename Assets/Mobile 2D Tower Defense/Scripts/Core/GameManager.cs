using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        public WaveSpawner waveSpawnerScript;
            
            void Start()
            {
                Time.timeScale = 1f;
                waveSpawnerGameObject.SetActive(false);
                gameOverMenu.SetActive(false);
                winnerMenu.SetActive(false);
            }

            void Update()
            {
                goldDisplay.text = gold.ToString();
                livesDisplay.text = lives.ToString();
                waveCount = "Wave " + (waveSpawnerScript.nextWave + 1) + "/" + waveSpawnerScript.waves.Length;
                waveCountDisplays.text = waveCount;

                if(lives == 0)
                {
                    gameOverMenu.SetActive(true);
                    Time.timeScale = 0f;
                }

                if(waveSpawnerScript.nextWave == waveSpawnerScript.waves.Length-1 && win == true)
                {
                    if (GameObject.FindGameObjectWithTag("Enemy") == null)
                    {
                        Invoke("YouWin", 3);
                    }
                }
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
                Application.LoadLevel(Application.loadedLevel);
            }

            public void ResetBuildingPlaces()
            {
                foreach(BuildingPlace place in buildingPlaces)
                {
                    place.ResetThisPlace();
                }
            }
    }
}
