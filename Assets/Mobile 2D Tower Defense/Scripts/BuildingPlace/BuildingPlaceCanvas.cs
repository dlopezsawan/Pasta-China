using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MobileTowerDefense
{
    public class BuildingPlaceCanvas : MonoBehaviour
    {
        public GameObject[] buttonGameObjects;
        public GameObject[] buildingDisplays;
        public GameObject[] attackZones;

        [HideInInspector]public GameObject selectedButton;
        [HideInInspector]public GameObject selectedAtackZone;
        private int nextAttackZone = 1;
        public GameObject buildPanel;
        public GameObject updatePanel; 
        

        private GameManager gameManager;
        public BuildingPlace buildingPlace;
        public GameObject updateButton;
        public int numberOfBuiltTower;

        void Start()
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            
            buildPanel.SetActive(true);
            updatePanel.SetActive(false);
            
            ResetButtons();
        }

        void Update()
        {
            //
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

            CheckEnoghtCountOfMoney update = updateButton.GetComponent<CheckEnoghtCountOfMoney>();
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

            for(int i = 0; i < 3; i++)
            {
                if(attackZones[i] == selectedAtackZone) {continue;}
                attackZones[i].SetActive(false);
            }

            if(buildingPlace.level == buildingPlace.towers[numberOfBuiltTower].levels.Length) {updateButton.SetActive(false);}

            nextAttackZone = 2;
        }

        public void SoldButtonSecondClickEvent()
        {
            buildingPlace.DestroyTower(numberOfBuiltTower);
            buildPanel.SetActive(true);
            updatePanel.SetActive(false);
            updateButton.SetActive(true);

            for(int i = 0; i < 3; i++)
            {
                attackZones[i].SetActive(false);
            }

            nextAttackZone = 1;
            numberOfBuiltTower = 0;
        }

        
        public void ResetButtons()
        {
            foreach(GameObject button in buttonGameObjects)
            {
                if(selectedButton != null && button == selectedButton) {continue;}
                
                TapButton tapButtonScript = button.GetComponent<TapButton>();
                tapButtonScript.alreadyClicked = false;

                Image buttonImage = button.GetComponent<Image>();
                buttonImage.sprite = tapButtonScript.buyButtonDefault;
            }

            for(int i = 0; i < 3; i++)
            {
                if(selectedButton != null && buttonGameObjects[i] == selectedButton) {continue;}
                buildingDisplays[i].SetActive(false);
            }   
        }
    }
}

