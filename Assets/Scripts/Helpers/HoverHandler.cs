using UnityEngine;
using UnityEngine.EventSystems;

public class HoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Declare an event to braodcast the name of the UI element
    public delegate void HoverEventHandler(string elementName);
    public static event HoverEventHandler OnHoverEnter;
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Called when the mouse enters the UI element
        // Broadcast the name of the UI element
        string elementName = gameObject.name;
        // Debug.Log("Mouse entered: " + elementName);
        
        // Broadcast the name using the event
        OnHoverEnter?.Invoke(elementName);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Called when the mouse exits the UI element
        // You can perform actions when the pointer is no longer hovering over the UI element
        
        // Debug.Log("Mouse exited: " + gameObject.name);
    }
}
