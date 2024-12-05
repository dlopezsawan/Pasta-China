using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

namespace MobileTowerDefense
{
    public class TapButton : MonoBehaviour , IPointerClickHandler, IPointerExitHandler
    {
        public BuildingPlaceCanvas buildingPlaceCanvas; // Building Place Canvas script
        [HideInInspector]public bool alreadyClicked = false; 
        public Sprite buyButtonSelected; //Sprite for selected button
        public Sprite buyButtonDefault; //Sprite of default button

        public UnityEvent firstClickButtonEvent; // Event when first click on the button
        public UnityEvent secondClickButtonEvent; // Event when second click on the button

        [HideInInspector]public Image image; // image of button
        private Button button;

        private void Start()
        {
            image = GetComponent<Image>();
            button = GetComponent<Button>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(button.interactable == true)
            {
                if(alreadyClicked)
                {
                    alreadyClicked = false;
                    buildingPlaceCanvas.selectedButton = null;
            
                    secondClickButtonEvent.Invoke();
                }
                else
                {
                    alreadyClicked = true;
                    buildingPlaceCanvas.selectedButton = gameObject;

                    buildingPlaceCanvas.ResetButtons();
                    firstClickButtonEvent.Invoke();
                
                    image.sprite = buyButtonSelected;
                }
            }
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            buildingPlaceCanvas.ResetButtons();
        }
    }
}