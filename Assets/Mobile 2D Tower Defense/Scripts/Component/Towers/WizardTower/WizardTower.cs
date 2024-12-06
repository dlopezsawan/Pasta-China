using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MobileTowerDefense
{
    public class WizardTower : MonoBehaviour
    {
        [HideInInspector]public Transform target;

        [Header("Attributes")]

        public float range = 1f;
        public float towerDamage = 100f;
        [HideInInspector]public float fireRate = 1f;
        [HideInInspector]public  float fireCountDown = 0f;

        [Header("Unity Setup Fields")]

        public string enemyTag = "Enemy";
        public Transform partToRotate;
        [HideInInspector]public float turnSpeed = 10f;

        public GameObject bulletPrefab;
        public Transform firepoint;

        public Animator activeWizardAnim;

        void Update()
        {
            UpdateTarget();

            if(fireCountDown <= 0f && target != null)
            {
                activeWizardAnim.SetTrigger("Shoot");
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
            GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firepoint.position, firepoint.rotation);
            Bullet bullet = bulletGO.GetComponent<Bullet>();

            if(bullet != null)
            {
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
