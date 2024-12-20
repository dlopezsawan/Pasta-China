using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MobileTowerDefense
{
    public class CatopultTower : MonoBehaviour
    {
        [HideInInspector]public Transform target;

        [Header("Attributes")]

        public float range = 1f;
        public float towerDamage = 80f;
        [HideInInspector]public float fireRate = 1f;
        [HideInInspector]public  float fireCountDown = 0f;

        [Header("Unity Setup Fields")]

        public string enemyTag = "Enemy";

        public GameObject bulletPrefab;
        public Transform firepoint;

        private Animator catopultAnim;
        AudioManager audioManager;
        ObjectPool objectPool;
        void Start()
        {
            objectPool = GameObject.Find("ObjectPool").GetComponent<ObjectPool>();
            audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
            catopultAnim = GetComponent<Animator>();
        }

        void Update()
        {
            UpdateTarget();
            if(fireCountDown <= 0f && target != null)
            {
                catopultAnim.SetTrigger("Shoot");
                fireCountDown = 5f;
            }
            fireCountDown -= Time.deltaTime;
        }
        
        void UpdateTarget ()
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
            float shortestDistance = Mathf.Infinity;
            GameObject nearestEnemy = null;

            foreach(GameObject enemy in enemies)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if(distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = enemy;
                }
            }

            if(nearestEnemy != null && shortestDistance <= range)
            {
                target = nearestEnemy.transform;
            }
            else
            {
                target = null;
            }
        }

        public void Shoot()
        {
            GameObject bulletGO = objectPool.GetFromPool(bulletPrefab, firepoint);
            Stone bullet = bulletGO.GetComponent<Stone>();
            if (audioManager != null) {audioManager.PlaySound("Action", "CatapultFire", transform.position, false); }

            if (bullet != null)
            {
                bullet.currentPrefab = bulletGO;
                bullet.bulletPrefab = bulletPrefab;
                bullet.Seek(target);
                bullet.damage = towerDamage;
            }
        }

        public void NewFireCountDown()
        {
            fireCountDown = 1f / fireRate;
            catopultAnim.SetTrigger("Idle");  
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, range);
        }
    }

}
