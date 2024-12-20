using UnityEngine;
using UnityEngine.UI;

namespace MobileTowerDefense
{
    public class Enemy : MonoBehaviour
    {
        public float startHealth;
        private float currentHealth;
        public Image imgHealthBar;
        public int coinsAfterDeath;

        private GameManager gameManager;
        public float speed;
        private PathWay pathway;
            
        private int wayPointIndex;
        [HideInInspector]public int wayIndex;

        [SerializeField] GameObject healthBar;
        AudioManager audioManager;
        ObjectPool objectPool;

        [HideInInspector] public GameObject enemyPrefab;

        public int currentPathIndex;
        void Start()
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            objectPool = GameObject.Find("ObjectPool").GetComponent<ObjectPool>();
            audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
            pathway = GameObject.Find("PathWay").GetComponent<PathWay>();
            currentHealth = startHealth;
        }

        void Update()
        {
            var target = pathway.paths[currentPathIndex].path.ways[wayIndex].wayPoints[wayPointIndex];
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, target.position) < 0.1f)
            {
                if (wayPointIndex < pathway.paths[currentPathIndex].path.ways[wayIndex].wayPoints.Length - 1)
                {
                    wayPointIndex++;
                }
                else
                {
                    gameManager.lives--;
                    Die();
                }
            }
        }

        public void TakeDamage(float amount)
        {
            currentHealth -= amount;
            imgHealthBar.fillAmount = currentHealth / startHealth;

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        void Die()
        {
            if (audioManager != null) { audioManager.PlaySound("Action", "EnemyDeath", transform.position, false); }
            objectPool.ReturnToPool(enemyPrefab, gameObject);
            healthBar.SetActive(false);
            gameManager.gold += coinsAfterDeath;
        }

        public void Reset()
        {
            healthBar.SetActive(true);
            wayPointIndex = 0;
            currentHealth = startHealth;
            imgHealthBar.fillAmount = 1;
        }
    }
}
