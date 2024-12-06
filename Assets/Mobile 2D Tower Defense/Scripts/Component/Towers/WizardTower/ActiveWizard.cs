using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MobileTowerDefense
{
    public class ActiveWizard : MonoBehaviour
    {
        public WizardTower wizardTower;

        public void ShootAnimationEnded()
        {
            wizardTower.Shoot();
        }

        public void NewFireCountDown()
        {
            wizardTower.fireCountDown = 1f / wizardTower.fireRate;
            wizardTower.activeWizardAnim.SetTrigger("Idle");
        }
    }
}

