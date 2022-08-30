using UnityEngine.EventSystems; // 1
using UnityEngine;

public class UiChangePosition : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IEndDragHandler, IDragHandler
    , IPointerExitHandler
{
    public GameManager gameManager;
    // Start is called before the first frame update
    public void OnPointerClick(PointerEventData eventData) // 3
    {
        gameManager.isDrawAgain = true;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
    }
 
    public void OnPointerExit(PointerEventData eventData)
    {
        gameManager.isDrawAgain = false;
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        gameManager.isDrawAgain = true;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        gameManager.isDrawAgain = true;
    }
}
