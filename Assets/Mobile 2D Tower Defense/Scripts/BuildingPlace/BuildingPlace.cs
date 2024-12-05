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
        private bool checkClicking; 
        private bool towerWasPlaced = false;
        
        public Sprite placeForBuildingFree;
        public Sprite placeForBuildingNotFree;

        private GameManager gameManager;
        [HideInInspector]public SpriteRenderer spriteRenderer;
        public Animator canvasAnimator;
        public BuildingPlaceCanvas buildingPlaceCanvas;

        void Start()
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            spriteRenderer = GetComponent<SpriteRenderer>();

            childCanvas.SetActive(false);
            checkClicking = false; 
            spriteRenderer.sprite = placeForBuildingFree;
        }

        
        void Update()
        {
            if(checkClicking)
            {
                if(Input.GetMouseButtonDown(0))
                {
                    Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

                    if(hit.transform != null)
                    {   
                        if(hit.transform.gameObject != gameObject && hit.transform.gameObject.layer != LayerMask.NameToLayer("UI"))
                        {
                            checkClicking = false;
                            spriteRenderer.sprite = placeForBuildingFree;
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
        }

        public void BuildTheTower(int numberOfTower)
        { 
            spriteRenderer.sprite = placeForBuildingNotFree;

            if(gameManager.gold >= towers[numberOfTower].levels[level].cost)
            {
                childCanvas.SetActive(false);
                checkClicking = false;
                towerWasPlaced = true;
                gameManager.gold -= towers[numberOfTower].levels[level].cost;

                GameObject tower = (GameObject)Instantiate(towers[numberOfTower].levels[level].towerPrefab, towers[numberOfTower].buildingSpawnPoint.transform.position, towers[numberOfTower].buildingSpawnPoint.transform.rotation);
                builtTower = tower;

                level+=1;
            }
        }

        public void UpdateTower(int numberOfTower, int levelOfTower)
        {
            if(gameManager.gold >= towers[numberOfTower].levels[levelOfTower].cost)
            {
                childCanvas.SetActive(false);
                checkClicking = false;
                gameManager.gold -= towers[numberOfTower].levels[levelOfTower].cost;

                Destroy(builtTower.gameObject);
                GameObject tower = (GameObject)Instantiate(towers[numberOfTower].levels[levelOfTower].towerPrefab, builtTower.transform.position, builtTower.transform.rotation);
                builtTower = tower;

                level+=1;
            }
        }

        public void DestroyTower(int numberOfTower)
        {
            int getMoneyback;
            for(int i = 0; i < level; i++)
            {
                gameManager.gold += (int)(towers[numberOfTower].levels[i].cost * 0.7f);
            }
            
            Destroy(builtTower.gameObject);
            level = 0;
            towerWasPlaced = false;
            spriteRenderer.sprite = placeForBuildingFree;
        }

        public void ResetThisPlace()
        {
            checkClicking = false;
            childCanvas.SetActive(false);
            if(towerWasPlaced == true)
            {
                spriteRenderer.sprite = placeForBuildingNotFree;
            }
            else
            {
                spriteRenderer.sprite = placeForBuildingFree;
            }
        }
        
    }
}
