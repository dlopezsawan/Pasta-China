using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using static Path;

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

        [SerializeField]GameObject healthBar;

        void Start()
        {
            gameManager = GameManager.Instance;
            pathway = GameObject.Find("PathWay").GetComponent<PathWay>();
            currentHealth = startHealth;
        }

        void Update()
        {
            var target = pathway.paths[0].path.ways[wayIndex].wayPoints[wayPointIndex];
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, target.position) < 0.1f)
            {
                if (wayPointIndex < pathway.paths[0].path.ways[wayIndex].wayPoints.Length - 1)
                {
                    wayPointIndex++;
                }
                else
                {
                    gameManager.lives--;
                    //gameManager.
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
            Destroy(healthBar);
            Destroy(gameObject, 0.2f);
            gameManager.gold += coinsAfterDeath;
        }
    }
}
