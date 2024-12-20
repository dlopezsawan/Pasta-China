using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MobileTowerDefense
{
    public class Stone : MonoBehaviour
    {
        public AnimationCurve curve;

        public float duration = 2.0f;
        public  float maxHeightY = 3.0f;

        private Transform target;
        private float timePast = 0f;
        [HideInInspector]public float damage = 0.0f;

        public GameObject hitEffect;
        AudioManager audioManager;
        ObjectPool objectPool;

        [HideInInspector] public GameObject currentPrefab;
        [HideInInspector] public GameObject bulletPrefab;
        private void Start()
        {
            audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
            objectPool = GameObject.Find("ObjectPool").GetComponent<ObjectPool>();
        }

        public void Seek(Transform _target)
        {
            target = _target;
            if(target != null)
            {
                StartCoroutine(Curve(transform.position, target.position));
            }
        }

        void Update()
        {
            if(target == null)
            {
                objectPool.ReturnToPool(bulletPrefab, currentPrefab);
                return;
            }

            if (timePast >= duration)
            {
                HitTarget(target, damage);
                return;
            }
        }

        void HitTarget(Transform enemy, float hitDamage)
        {
            Enemy e = enemy.GetComponent<Enemy>();
            if (audioManager != null) { audioManager.PlaySound("Action", "CatapultHit", transform.position, false); }
            timePast = 0;
            e.TakeDamage(hitDamage);
            objectPool.ReturnToPool(bulletPrefab, currentPrefab);
        }

        public IEnumerator Curve(Vector3 start, Vector3 finish)
        {
            while (timePast < duration)
            {
                timePast += Time.deltaTime;

                float linearTime = timePast / duration;
                float heightTime = curve.Evaluate(linearTime);

                float height = Mathf.Lerp(0f, maxHeightY, heightTime);

                transform.position = Vector2.Lerp(start, finish, linearTime) + new Vector2(0f, height);

                yield return null;
            }
        }
    }
}

