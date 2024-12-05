using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MobileTowerDefense
{
    public class Enemy : MonoBehaviour
    {
        public float startHealth;
        private float health;
        public Image healthBar;
        public int coinsAfterDeath;


        private GameManager gameManager;
        public float speed;
        private WayPoints points;
            
        private int wayPointIndex;
        [HideInInspector]public int wayIndex;
        public GameObject deathObject;
        void Start()
        {
            points = GameObject.Find("WayPoints").GetComponent<WayPoints>();
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            health = startHealth;
        }

            void Update()
            {
                transform.position = Vector2.MoveTowards(transform.position, points.ways[wayIndex].wayPoints[wayPointIndex].position, speed * Time.deltaTime);
                if(Vector2.Distance(transform.position, points.ways[wayIndex].wayPoints[wayPointIndex].position) < 0.1f)
                {
                    if(wayPointIndex < points.ways[wayIndex].wayPoints.Length - 1)
                    {
                        wayPointIndex++;
                    }
                    else
                    {
                        gameManager.lives -= 1;
                        Die();
                    }
                }
            }

            public void TakeDamage(float amount)
            {
                health -= amount;
                healthBar.fillAmount = health / startHealth;

                if(health <= 0)
                {
                    Die();
                }
            }
            void Die()
            {
                GameObject obj = (GameObject)Instantiate(deathObject, transform.position, transform.rotation);
                Destroy(obj.gameObject, 1);
                Destroy(gameObject);
                gameManager.gold += coinsAfterDeath;
            }
    }
}
