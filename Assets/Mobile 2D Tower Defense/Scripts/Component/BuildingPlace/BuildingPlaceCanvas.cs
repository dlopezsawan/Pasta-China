using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static MobileTowerDefense.BuildingPlace;

namespace MobileTowerDefense
{
    public class BuildingPlaceCanvas : MonoBehaviour
    {
        public Button[] buttonGameObjects;
        public GameObject[] buildingTransparentDisplays;
        public GameObject[] attackZones;

        [HideInInspector]public Button selectedButton;
        public Button updateButton;

        [HideInInspector]public GameObject selectedAtackZone;
        private int nextAttackZone = 1;
        public GameObject buildPanel;
        public GameObject updatePanel; 
        
        private GameManager gameManager;
        public BuildingPlace buildingPlace;
        public int numberOfBuiltTower;

        void Start()
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
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
            buildingPlace.currentIcon.sprite = buildingPlace.placeForBuildingNotFree;
            buildingTransparentDisplays[numberOfTower].SetActive(true);

            updateButton.numberOfTower = numberOfTower;
            gameManager.currentBuildingPlace = buildingPlace;
        }

        public void BuildButtonSecondClickEvent(int numberOfTower) // Also update upgrade button
        {
            buildingPlace.BuildTheTower(numberOfTower);

            gameManager.DisplayGoldText();

            buildPanel.SetActive(false);
            updatePanel.SetActive(true);
            attackZones[0].SetActive(true);
            selectedAtackZone = attackZones[0];

            numberOfBuiltTower = numberOfTower;
        }

        //Removing the class and adding here script of checkmoney class
        ///----------------------------------------------------------------------------------------------------------
        public void UpdateButtonmsText(Text btnText, int numberOfTower)
        {
            btnText.text = buildingPlace.towers[numberOfTower].levels[buildingPlace.level].cost.ToString();
        }

        public bool CheckIsMoneyEnough()
        {
            //if(buildingPlace.level >= 2) return true;
            if (gameManager.gold >= buildingPlace.towers[numberOfBuiltTower].levels[buildingPlace.level].cost)
            {            
                return true;
            }        
            return false;
        }
        //----------------------------------------------------------------------------------------------------------------------

        public void UpgradeButtonFirstClickEvent()
        {
            attackZones[nextAttackZone].SetActive(true);
        }

        public void UpgradeButtonSecondClickEvent()
        {
            buildingPlace.UpdateTower(numberOfBuiltTower, buildingPlace.level);
            selectedAtackZone = attackZones[nextAttackZone];

            gameManager.DisplayGoldText();

            for (int i = 0; i < 3; i++)
            {
                if(attackZones[i] == selectedAtackZone) {continue;}
                attackZones[i].SetActive(false);
            }

            if(buildingPlace.level == buildingPlace.towers[numberOfBuiltTower].levels.Length-1) {updateButton.gameObject.SetActive(false);}
            //Make it dynamic
            nextAttackZone = 2;
        }

        public void SoldButtonSecondClickEvent()
        {
            buildingPlace.DestroyTower(numberOfBuiltTower);
            buildPanel.SetActive(true);
            updatePanel.SetActive(false);
            updateButton.gameObject.SetActive(true);

            gameManager.DisplayGoldText();
            StartCoroutine(CheckButtonToEnableOrDisable());

            for (int i = 0; i < 3; i++)
            {
                attackZones[i].SetActive(false);
            }

            nextAttackZone = 1;
            numberOfBuiltTower = 0;
        }

        
        public void ResetButtons()
        {
            foreach(Button button in buttonGameObjects)
            {
                if(selectedButton != null && button == selectedButton) {continue;}
                button.alreadyClicked = false;
                if(button.btnImage != null)
                {
                    button.btnImage.sprite = button.buyButtonDefault;
                }              
            }

            for(int i = 0; i < 3; i++)
            {
                if(selectedButton != null && buttonGameObjects[i] == selectedButton) {continue;}
                buildingTransparentDisplays[i].SetActive(false);
            }   
        }

        public IEnumerator CheckButtonToEnableOrDisable()
        {
            yield return new WaitForSeconds(0.2f);
            foreach (var item in buttonGameObjects)
            {
                if (item.btnText == null) continue;
                Debug.Log("yes: " + item.btnText.text);
                if (gameManager.gold >= int.Parse(item.btnText.text))
                {
                    item.btnButton.interactable = true;
                }
                else
                {
                    item.btnButton.interactable = false;
                }
            }
        }

        public void ResetCurentButon()
        {
            selectedButton.btnImage.sprite = selectedButton.buyButtonDefault;
        }
    }
}

