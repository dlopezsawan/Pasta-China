using System.Collections.Generic;
using UnityEngine;

namespace MobileTowerDefense
{
    public class ObjectPool : MonoBehaviour
    {
        private Dictionary<GameObject, Queue<GameObject>> poolDictionary = new Dictionary<GameObject, Queue<GameObject>>();

        public void PreloadObjects(GameObject prefab, int count)
        {
            if (!poolDictionary.ContainsKey(prefab))
            {
                poolDictionary[prefab] = new Queue<GameObject>();
            }

            for (int i = 0; i < count; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.transform.parent = transform;
                obj.SetActive(false);
                poolDictionary[prefab].Enqueue(obj);
            }
        }

        public GameObject GetFromPool(GameObject prefab, Transform transform)
        {
            if (poolDictionary.ContainsKey(prefab) && poolDictionary[prefab].Count > 0)
            {
                GameObject obj = poolDictionary[prefab].Dequeue();
                obj.transform.SetLocalPositionAndRotation(transform.position, transform.rotation);               
                obj.SetActive(true);
                return obj;
            }
            GameObject newObj = Instantiate(prefab,transform);
            newObj.transform.parent = this.transform;
            poolDictionary[prefab].Enqueue(newObj);
            return newObj;
        }

        public void ReturnToPool(GameObject prefab, GameObject obj)
        {
            obj.SetActive(false);
            if (!poolDictionary.ContainsKey(prefab))
            {
                poolDictionary[prefab] = new Queue<GameObject>();
            }
            poolDictionary[prefab].Enqueue(obj);
        }
    }
}
