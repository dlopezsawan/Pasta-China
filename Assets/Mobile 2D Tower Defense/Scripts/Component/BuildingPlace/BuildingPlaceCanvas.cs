using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MobileTowerDefense
{
    public class BuildingPlaceCanvas : MonoBehaviour
    {
        public CustomButton[] buttonGameObjects;
        public GameObject[] buildingDisplays;
        public GameObject[] attackZones;

        [HideInInspector] public GameObject selectedButton;
        [HideInInspector] public GameObject selectedAtackZone;
        private int nextAttackZone = 1;
        public GameObject buildPanel;
        public GameObject updatePanel;

        public BuildingPlace buildingPlace;
        public GameObject updateButton;
        public int numberOfBuiltTower;

        void Start()
        {
            buildPanel.SetActive(true);
            updatePanel.SetActive(false);

            ResetButtons();
        }

        private void TurnOffCanvas()
        {
            gameObject.SetActive(false);
        }

        public void BuildButtonFirstClickEvent(int numberOfTower)
        {
            buildingPlace.spriteRenderer.sprite = buildingPlace.placeForBuildingNotFree;
            buildingDisplays[numberOfTower].SetActive(true);
        }

        public void BuildButtonSecondClickEvent(int numberOfTower)
        {
            buildingPlace.BuildTheTower(numberOfTower);

            buildPanel.SetActive(false);
            updatePanel.SetActive(true);
            attackZones[0].SetActive(true);
            selectedAtackZone = attackZones[0];

            numberOfBuiltTower = numberOfTower;

            CheckEnoghMoney update = updateButton.GetComponent<CheckEnoghMoney>();
            update.numberOfTower = numberOfBuiltTower;
        }

        public void UpgradeButtonFirstClickEvent()
        {
            attackZones[nextAttackZone].SetActive(true);
        }

        public void UpgradeButtonSecondClickEvent()
        {
            buildingPlace.UpdateTower(numberOfBuiltTower, buildingPlace.level);
            selectedAtackZone = attackZones[nextAttackZone];

            for (int i = 0; i < 3; i++)
            {
                if (attackZones[i] == selectedAtackZone) { continue; }
                attackZones[i].SetActive(false);
            }

            if (buildingPlace.level == buildingPlace.towers[numberOfBuiltTower].levels.Length) { updateButton.SetActive(false); }

            nextAttackZone = 2;
        }

        public void SoldButtonSecondClickEvent()
        {
            buildingPlace.DestroyTower(numberOfBuiltTower);
            buildPanel.SetActive(true);
            updatePanel.SetActive(false);
            updateButton.SetActive(true);

            for (int i = 0; i < 3; i++)
            {
                attackZones[i].SetActive(false);
            }

            nextAttackZone = 1;
            numberOfBuiltTower = 0;
        }


        public void ResetButtons()
        {
            foreach (CustomButton button in buttonGameObjects)
            {
                if (selectedButton != null && button.gameObject == selectedButton) { continue; }
                button.alreadyClicked = false;
                Image buttonImage = button.GetComponent<Image>();
                buttonImage.sprite = button.buyButtonDefault;
            }

            for (int i = 0; i < 3; i++)
            {
                if (selectedButton != null && buttonGameObjects[i].gameObject == selectedButton) { continue; }
                buildingDisplays[i].SetActive(false);
            }
        }
    }
}

