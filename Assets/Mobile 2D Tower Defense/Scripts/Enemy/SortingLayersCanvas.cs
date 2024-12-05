using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MobileTowerDefense
{
    public class SortingLayersCanvas : MonoBehaviour
    {
        private Canvas canvas;
        
        void Start()
        {
            canvas = GetComponent<Canvas>();
        }

        void Update()
        {
            canvas.sortingOrder = (int)(transform.position.y * (-100));
        }
    }
}

