using UnityEngine;

public class WireScript : MonoBehaviour
{
    public string snapTag = "Default"; 

    private Vector3 offsetFromMouse;
    private Vector3 startPosition;

    private float objectZCoordinate;

    private Camera puzzleCamera;
    
    void Start()
    {
        puzzleCamera = Camera.main;
        startPosition = transform.position;
    }

    void OnMouseDown()
    {
        objectZCoordinate = puzzleCamera.WorldToScreenPoint(gameObject.transform.position).z;

        offsetFromMouse = gameObject.transform.position - GetMouseAsWorldPoint();
    }

    void OnMouseDrag()
    {
        transform.position = GetMouseAsWorldPoint() + offsetFromMouse;
    }

    void OnMouseUp()
    {
        RaycastHit hit;
        Ray ray = puzzleCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag(snapTag))
            {
                transform.position = hit.collider.transform.position;
                return;
            }
        }

        transform.position = startPosition;
    }

    private Vector3 GetMouseAsWorldPoint()
    {
        Vector3 mouseScreenPoint = Input.mousePosition;
        mouseScreenPoint.z = objectZCoordinate;
        return puzzleCamera.ScreenToWorldPoint(mouseScreenPoint);
    }
}