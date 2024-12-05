using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MobileTowerDefense
{
    public class SortingLayers : MonoBehaviour
    {
        private SpriteRenderer sprite;
        
        void Start()
        {
            sprite = GetComponent<SpriteRenderer>();
        }
        
        void Update()
        {
            sprite.sortingOrder = (int)(transform.position.y * (-100));
        }
    }
}