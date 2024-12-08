using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MobileTowerDefense.PathWay;
using static Path;

namespace MobileTowerDefense
{
    public class PathWay : MonoBehaviour
    {
        [System.Serializable]
        public class Paths
        {
            public Transform spawnPoint;
            public Path path;
        }     

        public Paths[] paths;
    }

}

