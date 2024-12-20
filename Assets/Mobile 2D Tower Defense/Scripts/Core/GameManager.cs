using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MobileTowerDefense
{
    public class GameManager : MonoBehaviour
    {
        [Header("UI")]
        public Text goldDisplay;
        public Text waveCountDisplays;
        public Text livesDisplay;

        public int gold;
        public int lives;

        private int lastGold = -1;
        private int lastLives = -1;
        private string lastWaveCount = "";

        public GameObject gameOverMenu;
        public GameObject winnerMenu;
        [HideInInspector] public bool win = false;

        public GameObject waveSpawnerGameObject;

        public BuildingPlace[] buildingPlaces;
        public WaveSpawner waveSpawnerScript;

        [SerializeField] GameObject[] PreloadPrefabs;
        [SerializeField] GameObject PreloadPrefab;
        AudioManager audioManager;
        ObjectPool objectPool;

        [SerializeField] GameObject characterDialogue;
        private void Awake()
        {
            objectPool = GameObject.Find("ObjectPool").GetComponent<ObjectPool>();
            audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        }

        void Start()
        {       
            Time.timeScale = 1f;
            waveSpawnerGameObject.SetActive(false);
            gameOverMenu.SetActive(false);
            winnerMenu.SetActive(false);

            if (audioManager != null) { audioManager.PlaySound("Ambient", "Ambient_jungle", transform.position, true); }

            foreach (var item in PreloadPrefabs)
            {
                objectPool.PreloadObjects(item, 2);
            }
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

                characterDialogue.SetActive(true);
                //int currentLevel = GlobalData.Instance.levelCounter++;
                //if (currentLevel <=3) {
                //    SceneManager.LoadSceneAsync(currentLevel);
                //}
                //else
                //{
                //    characterDialogue.SetActive(true);

                //}
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
                if (audioManager != null) { audioManager.PlaySound("Music", "Music", transform.position, true); }
                StartWaveButton();
            }
            else
            {
                gold += (int)(15 * waveSpawnerScript.waveCountDown);
                waveSpawnerScript.waveCountDown = 0;
            }

            waveSpawnerScript.OnEnableAlertCanvas(false);
        }

        private void YouWin()
        {
            winnerMenu.SetActive(true);
            Time.timeScale = 0f;
        }

        public void StartWaveButton()
        {
            waveSpawnerGameObject.SetActive(true);
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
