using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public EventSystem eventSystem;
    public GraphicRaycaster graphicRaycaster;

    void Start()
    {
        // Get the required components if they are not set
        if (eventSystem == null)
            eventSystem = FindObjectOfType<EventSystem>();

        if (graphicRaycaster == null)
            graphicRaycaster = FindObjectOfType<GraphicRaycaster>();
    }

    void Update()
    {
        GameObject uiElement = GetUIElementUnderPointer();

        if (uiElement != null)
        {
            Debug.Log("Mouse is over UI element: " + uiElement.name);
        }
        else
        {
            Debug.Log("Mouse is not over any UI element.");
        }
    }

    private GameObject GetUIElementUnderPointer()
    {
        PointerEventData eventData = new PointerEventData(eventSystem)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        graphicRaycaster.Raycast(eventData, results);

        if (results.Count > 0)
        {
            // Return the first result's GameObject
            return results[0].gameObject;
        }

        return null;
    }
}
