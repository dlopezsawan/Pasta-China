using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MobileTowerDefense
{
    public class Bullet : MonoBehaviour
    {
        private Transform target;
        public float speed = 10f;
        public float turnSpeed = 10f;
        [HideInInspector]public float damage = 0.0f;

        public void Seek(Transform _target)
        {
            target = _target;
        }

        void Update()
        {
            if(target == null)
            {
                Destroy(gameObject);
                return;
            }

            Vector3 dir = target.position - transform.position;
            float distanceThisFrame = speed * Time.deltaTime;

            if(dir.magnitude <= distanceThisFrame)
            {
                HitTarget(target, damage);
                return;
            }
            transform.Translate(dir.normalized * distanceThisFrame, Space.World);

            Vector3 direction = target.position - transform.position;
            Vector3 rotatedVectorDir = Quaternion.Euler(0, 0, -90) * direction;
            Quaternion lookRotation  = Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorDir);
            Quaternion rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
            transform.rotation = rotation;

        }

        void HitTarget(Transform enemy, float hitDamage)
        {
            Enemy e = enemy.GetComponent<Enemy>();

            e.TakeDamage(hitDamage);
            Destroy(gameObject);
        }
    }
}


