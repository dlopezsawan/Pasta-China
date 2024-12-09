using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MobileTowerDefense
{
    public class Bow : MonoBehaviour
    {
        public ArcherTower archerTower;

        public void ShootAnimationEnded()
        {
            archerTower.Shoot();
        }

        public void NewFireCountDown()
        {
            archerTower.fireCountDown = 1f / archerTower.fireRate;
            archerTower.bowAnim.SetTrigger("Idle");
        }
    }
}

