using MobileTowerDefense;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SoldButton : Button, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        ChooseButtonOneAfterAnother();
    }

}
