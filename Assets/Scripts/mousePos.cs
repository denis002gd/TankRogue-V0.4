using UnityEngine;

public class mousePos : MonoBehaviour
{
    void Update()
    {
       Vector3 mousePosition = Input.mousePosition;
       Vector3 fixRotation = new Vector3(-90,0,90);

        // Convert the mouse position from screen space to world space
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 10f));

        // Calculate the direction from the object to the mouse position
        Vector3 lookDirection = worldMousePosition - transform.position;
        lookDirection.y = 0f; // Ignore the Y-axis to preserve rotation on the Y-axis

        // Rotate the object to face the mouse position
        transform.rotation = Quaternion.LookRotation(lookDirection.normalized, Vector3.up);

        // Invert the rotation on the Y-axis
        transform.Rotate(fixRotation);
    }
}

