using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine;

public class DetectUiForSpeedPlanet : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IEndDragHandler, IDragHandler
    , IPointerExitHandler
{
    public GameManager gameManager;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject.Find(gameManager.nameActive).GetComponent<CircleMovation>().indexMax = 2000;
        GameObject.Find(gameManager.nameActive).GetComponent<CircleMovation>().firstDraw = true;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
    }
 
    public void OnPointerExit(PointerEventData eventData)
    {
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject.Find(gameManager.nameActive).GetComponent<CircleMovation>().indexMax = 2000;
        GameObject.Find(gameManager.nameActive).GetComponent<CircleMovation>().firstDraw = true;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        
    }
}
