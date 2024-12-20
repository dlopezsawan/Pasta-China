using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MobileTowerDefense
{
    public class ArcherTower : MonoBehaviour
    {
        [HideInInspector]public Transform target;

        [Header("Attributes")]

        public float range = 1f;
        public float fireRate = 1f;
        public float towerDamage = 50f;
        [HideInInspector]public  float fireCountDown = 0f;

        [Header("Unity Setup Fields")]

        public string enemyTag = "Enemy";
        public Transform partToRotate;
        public float turnSpeed = 10f;

        public GameObject bulletPrefab;
        public Transform firepoint;

        public Animator bowAnim;
        AudioManager audioManager;
        ObjectPool objectPool;
        private void Start()
        {
            objectPool = GameObject.Find("ObjectPool").GetComponent<ObjectPool>();
            audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        }

        void Update()
        {
            UpdateTarget();
            
            if(fireCountDown <= 0f && target != null)
            {
                bowAnim.SetTrigger("Shoot");
                fireCountDown = 5f;
            }
            fireCountDown -= Time.deltaTime;

            if(target != null)
            {
                Vector3 dir = target.position - transform.position;
                Vector3 rotatedVectorDir = Quaternion.Euler(0, 0, -90) * dir;
                Quaternion lookRotation  = Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorDir);
                Quaternion rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed);
                partToRotate.rotation = rotation;
            }
            
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
            Arrow bullet = bulletGO.GetComponent<Arrow>();
            if (audioManager != null) { audioManager.PlaySound("Action", "Bow", transform.position, false); }

            if (bullet != null)
            {
                bullet.currentPrefab = bulletGO;
                bullet.bulletPrefab = bulletPrefab;
                bullet.Seek(target);
                bullet.damage = towerDamage;
            }
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, range);
        }
    }
}
