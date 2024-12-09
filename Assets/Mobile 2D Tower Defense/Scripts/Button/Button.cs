using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MobileTowerDefense
{
    public class Button : MonoBehaviour
    {
        public BuildingPlaceCanvas buildingPlaceCanvas; // Building Place Canvas script
        [HideInInspector] public bool alreadyClicked = false;
        public Sprite buyButtonSelected; //Sprite for selected button
        public Sprite buyButtonDefault; //Sprite of default button

        public UnityEvent firstClickButtonEvent; // Event when first click on the button
        public UnityEvent secondClickButtonEvent; // Event when second click on the button

        [HideInInspector] public Image btnImage; // image of button
        [HideInInspector] public UnityEngine.UI.Button btnButton;

        [HideInInspector]public Text btnText;

        [SerializeField] public int numberOfTower = 0;

        private void Awake()
        {       
            btnImage = GetComponent<Image>();
            btnButton = GetComponent<UnityEngine.UI.Button>();
            btnText = GetComponentInChildren<Text>();
        }

        protected void ChooseButtonOneAfterAnother()
        {
            if (alreadyClicked)
            {
                alreadyClicked = false;
                buildingPlaceCanvas.selectedButton = this;
                secondClickButtonEvent.Invoke();
            }
            else
            {
                alreadyClicked = true;
                buildingPlaceCanvas.selectedButton = this;

                buildingPlaceCanvas.ResetButtons();
                firstClickButtonEvent.Invoke();

                btnImage.sprite = buyButtonSelected;
            }
        }
 
        private void OnEnable()
        {
            if (btnText == null) { return; }
            buildingPlaceCanvas.UpdateButtonmsText(btnText, numberOfTower);
        }
    }
}