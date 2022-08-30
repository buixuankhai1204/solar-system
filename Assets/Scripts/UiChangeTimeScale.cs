using UnityEngine.EventSystems;
using UnityEngine;

public class UiChangeTimeScale : MonoBehaviour, IPointerClickHandler, IEndDragHandler, IBeginDragHandler,IPointerExitHandler,IPointerEnterHandler
{
    public GameManager gameManager;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        foreach (var circleMovation in FindObjectsOfType<CircleMovation>())
        {
            circleMovation.indexMax = 2000;
            circleMovation.firstDraw = true;
        }

    }
    public void OnEndDrag(PointerEventData eventData)
    {
        foreach (var circleMovation in FindObjectsOfType<CircleMovation>())
        {
            circleMovation.indexMax = 2000;
            circleMovation.firstDraw = true;

        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        gameManager.countPlanetDrew = 0;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
    }
}
