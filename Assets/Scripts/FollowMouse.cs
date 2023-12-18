using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{

  
        public float rotationSpeed = 5f; // Adjust the rotation speed as needed
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
        mouseScreenPosition.z = 10f; // Adjust the distance based on your scene

        // Convert the screen position to a world point
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

        // Ensure the object only rotates on the Y-axis
        mouseWorldPosition.y = transform.position.y; // Keep the current Y position
         mouseWorldPosition.x -=yOfset;

        // Calculate the direction from the object to the mouse position
        Vector3 direction = mouseWorldPosition - transform.position;

        // Calculate the rotation angle in degrees
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        // Create a rotation based on the angle, only around the Y-axis
        Quaternion targetRotation = Quaternion.Euler(-90f, angle - spingg, 0f);

        // Rotate the object smoothly toward the mouse position
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
       

    }
    
    
    }   
    
