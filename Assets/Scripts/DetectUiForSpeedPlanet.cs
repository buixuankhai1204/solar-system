using System.Collections;
using UnityEngine.EventSystems; // 1
using UnityEngine;

public class DetectUiForSpeedPlanet : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
    , IPointerExitHandler
{
    public GameManager gameManager;
    
    public void OnPointerClick(PointerEventData eventData) // 3
    {
        gameManager.checkClickUi = true;
        Debug.Log(gameManager.listPlanetInformations[gameManager.nameActive].speed);
        
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        gameManager.checkClickUi = true;
    }
 
    public void OnPointerExit(PointerEventData eventData)
    {
        gameManager.checkClickUi = false;
    }
}
