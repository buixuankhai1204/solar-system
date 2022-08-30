using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems; // 1
 
public class DetectUI : MonoBehaviour
        , IPointerClickHandler // 2
        , IDragHandler
        , IEndDragHandler
        , IPointerEnterHandler
        , IPointerExitHandler
    // ... And many more available!
{
    public GameManager gameManager;
 

 
    void Update()
    {
        
    }
 
    public void OnPointerClick(PointerEventData eventData) // 3
    {
        print("I was clicked");
        gameManager.checkClickUi = true;
        gameManager.isDrawAgain = true;

    }
 
    public void OnDrag(PointerEventData eventData)
    {
        gameManager.checkClickUi = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // gameManager.isDrawAgain = true;

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