using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MobileTowerDefense
{
    public class BuildingPlace : MonoBehaviour
    {
        [System.Serializable]
        public class Level
        {
            public int cost; 
            public GameObject towerPrefab;
        }

        [System.Serializable]
        public class Tower
        {
            public Level[] levels;
            public GameObject buildingSpawnPoint;
        }
        public Tower[] towers;

        [HideInInspector]public int level = 0;

        [HideInInspector]public GameObject builtTower;

        public GameObject childCanvas;
        public bool checkClicking; 
        private bool towerWasPlaced = false;
        
        public Sprite placeForBuildingFree;
        public Sprite placeForBuildingNotFree;

        private GameManager gameManager;
        [HideInInspector]public SpriteRenderer currentIcon;
        public Animator canvasAnimator;
        public BuildingPlaceCanvas buildingPlaceCanvas;

        void Start()
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            currentIcon = GetComponent<SpriteRenderer>();

            childCanvas.SetActive(false);
            checkClicking = false; 
            currentIcon.sprite = placeForBuildingFree;
        }

        public void ClosedOpenBuildingCanvas()
        {      
            //Disable button area on click side of building place
            if (checkClicking)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

                    if (hit.transform != null)
                    {
                        if (hit.transform.gameObject != gameObject && hit.transform.gameObject.layer != LayerMask.NameToLayer("UI"))
                        {
                            checkClicking = false;
                            currentIcon.sprite = placeForBuildingFree;
                            canvasAnimator.SetTrigger("CloseCanvas");
                        }
                    }
                }
            }
        }
   
        private void OnMouseDown()
        {          
            gameManager.ResetBuildingPlaces();
            checkClicking = true;
            buildingPlaceCanvas.ResetButtons();
            childCanvas.SetActive(true);

            StartCoroutine(buildingPlaceCanvas.CheckButtonToEnableOrDisable());
        }

        private void OnMouseExit()
        {
            if(buildingPlaceCanvas != null && checkClicking)
            {
                gameManager.currentBuildingPlace = buildingPlaceCanvas.buildingPlace;               
            }          
        }

        public void BuildTheTower(int numberOfTower)
        { 
            currentIcon.sprite = placeForBuildingNotFree;

            if(gameManager.gold >= towers[numberOfTower].levels[level].cost)
            {
                childCanvas.SetActive(false);
                checkClicking = false;
                towerWasPlaced = true;
                gameManager.gold -= towers[numberOfTower].levels[level].cost;

                GameObject tower = (GameObject)Instantiate(towers[numberOfTower].levels[level].towerPrefab, towers[numberOfTower].buildingSpawnPoint.transform.position, towers[numberOfTower].buildingSpawnPoint.transform.rotation);
                builtTower = tower;
            }
        }

        public void UpdateTower(int numberOfTower, int levelOfTower)
        {
            if(gameManager.gold >= towers[numberOfTower].levels[levelOfTower].cost)
            {              

                buildingPlaceCanvas.ResetCurentButon();

                childCanvas.SetActive(false);
                checkClicking = false;
                gameManager.gold -= towers[numberOfTower].levels[levelOfTower].cost;

                Destroy(builtTower.gameObject);
                GameObject tower = (GameObject)Instantiate(towers[numberOfTower].levels[levelOfTower].towerPrefab, builtTower.transform.position, builtTower.transform.rotation);
                builtTower = tower;

                level++;         
            }
        }

        public void DestroyTower(int numberOfTower)
        {
            for(int i = 0; i < level+1; i++)
            {
                Debug.Log("yess");
                gameManager.gold += (int)(towers[numberOfTower].levels[i].cost * 0.7f);
            }
            
            Destroy(builtTower.gameObject);
            level = 0;
            towerWasPlaced = false;
            currentIcon.sprite = placeForBuildingFree;
        }

        public void ResetThisPlace()
        {
            checkClicking = false;
            childCanvas.SetActive(false);
            if(towerWasPlaced == true)
            {
                currentIcon.sprite = placeForBuildingNotFree;
            }
            else
            {
                currentIcon.sprite = placeForBuildingFree;
            }
        }
        
    }
}
