using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{


    public float rotationSpeed = 5f;
    public static FollowMouse Instance;
    public Vector3 facingDirection { get; private set; }
    public float spingg;
    public float yOfset;

    void Start()
    {
        facingDirection = transform.forward;
    }
    void Awake()
    {
        Instance = this;
    }
    void FixedUpdate()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;

        // Set a distance from the camera to the object
        mouseScreenPosition.z = 10f;
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        mouseWorldPosition.y = transform.position.y;
        mouseWorldPosition.x -= yOfset;

        Vector3 direction = mouseWorldPosition - transform.position;

        // Calculate the rotation angle in degrees
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        // Create a rotation based on the angle, only around the Y-axis
        Quaternion targetRotation = Quaternion.Euler(-90f, angle - spingg, 0f);

        // Rotate the object smoothly toward the mouse position
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);


    }


}