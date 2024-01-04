using UnityEngine;

public class ClickAndDrag : MonoBehaviour
{
    private Vector3 mouseOffset;
    private bool isDragging = false;

    void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Set the flag to indicate that dragging has started
            isDragging = true;

            // Store the offset between the object's position and the mouse position
            mouseOffset = transform.position - GetMouseWorldPosition();
        }
    }

    void OnMouseUp()
    {
        // Reset the dragging flag when the mouse button is released
        isDragging = false;
    }

    void Update()
    {
        // Scale the object with the mouse scroll wheel
        float scrollWheelInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollWheelInput != 0)
        {
            // Adjust the scale factor as needed
            float scaleFactor = 1.0f + scrollWheelInput;
            transform.localScale *= scaleFactor;
        }

        // Move the object while dragging
        if (isDragging)
        {
            Vector3 targetPosition = GetMouseWorldPosition() + mouseOffset;
            transform.position = targetPosition;
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        // Get the mouse position in screen coordinates
        Vector3 mouseScreenPosition = Input.mousePosition;

        // Convert the screen position to a ray and cast it into the scene
        Ray ray = Camera.main.ScreenPointToRay(mouseScreenPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Return the hit point in world coordinates
            return hit.point;
        }

        // If no collision, return a default position
        return Vector3.zero;
    }
}

