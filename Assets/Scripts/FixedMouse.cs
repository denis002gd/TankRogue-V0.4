using UnityEngine;

public class FixedMouse : MonoBehaviour
{
    public float yAxisInteractOffset;
    public Transform body;
    public Transform pointer; // Define the pointer variable
    public float rotationSpeed = 5f; // Define the rotation speed

    void Start()
    {
        // Initialize variables or perform any necessary setup
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 mousePositionScreen = Input.mousePosition;
        MovePointer(mousePositionScreen);
    }

    public void MovePointer(Vector3 mousePos)
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        Plane groundPlane = new Plane(Vector3.up, new Vector3(0, transform.position.y + yAxisInteractOffset, 0));

        float distance;
        if (groundPlane.Raycast(ray, out distance))
        {
            Vector3 targetPosition = ray.GetPoint(distance);
            pointer.position = targetPosition;

            // Calculate the direction to the pointer
            Vector3 directionToPointer = targetPosition - transform.position;

            Quaternion targetRotation = Quaternion.LookRotation(directionToPointer);
            body.rotation = Quaternion.Slerp(body.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }
}
