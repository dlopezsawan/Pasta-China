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
            pathway = GameObject.Find("PathWay").GetComponent<PathWay>();
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            health = startHealth;
        }

        void Update()
        {
            transform.position = Vector2.MoveTowards(transform.position, pathway.paths[0].path.ways[wayIndex].wayPoints[wayPointIndex].position, speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, pathway.paths[0].path.ways[wayIndex].wayPoints[wayPointIndex].position) < 0.1f)
            {
                if (wayPointIndex < pathway.paths[0].path.ways[wayIndex].wayPoints.Length - 1)
                {
                    wayPointIndex++;
                }
                else
                {
                    //we can use event here instead also
                    gameManager.lives -= 1;
                    gameManager.DisplayLivesText();
                    Die();
                }
            }
        }

        public void TakeDamage(float amount)
        {
            health -= amount;
            imgHealthBar.fillAmount = health / startHealth;

            if (health <= 0)
            {
                Debug.Log("hello");
                StartCoroutine(gameManager.currentBuildingPlace.buildingPlaceCanvas.CheckButtonToEnableOrDisable());
                Die();
            }
        }
        void Die()
        {
            Destroy(healthBar);
            Destroy(gameObject);
            gameManager.gold += coinsAfterDeath;
            gameManager.DisplayGoldText();
        }
    }
}
