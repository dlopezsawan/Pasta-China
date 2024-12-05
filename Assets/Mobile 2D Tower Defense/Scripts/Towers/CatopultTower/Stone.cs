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
                Destroy(gameObject);
                return;
            }

            if(timePast >= duration)
            {
                HitTarget(target, damage);
                return;
            }
        }

        void HitTarget(Transform enemy, float hitDamage)
        {
            Enemy e = enemy.GetComponent<Enemy>();

            e.TakeDamage(hitDamage);
            GameObject effect = (GameObject)Instantiate(hitEffect, transform.position, transform.rotation);
            Destroy(effect, 1f);

            Destroy(gameObject);
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

