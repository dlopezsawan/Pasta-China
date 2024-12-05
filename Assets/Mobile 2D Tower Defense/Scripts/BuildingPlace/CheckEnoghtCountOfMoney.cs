using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MobileTowerDefense
{
    public class CheckEnoghtCountOfMoney : MonoBehaviour
    {
        public Text costDisplay;
        public int numberOfTower;
        
        public BuildingPlace buildingPlace;
        private GameManager gameManager;
        private Image image;
        private Button button;

        void Start()
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            image = GetComponent<Image>();
            button = GetComponent<Button>();
        }

        void Update()
        {
            if(buildingPlace.level < buildingPlace.towers[numberOfTower].levels.Length)
            {
                if(gameManager.gold >= buildingPlace.towers[numberOfTower].levels[buildingPlace.level].cost)
                {
                    button.interactable = true;
                }
                else
                {
                    button.interactable = false;
                }
                costDisplay.text = buildingPlace.towers[numberOfTower].levels[buildingPlace.level].cost.ToString();
            }
        }
    }
}

