using UnityEngine;

public class CameraFollow : MonoBehaviour

{
        [HideInInspector]
    public Vector3 fixedRotation;

    private void Awake()
    {
        // Store the initial rotation of the camera
        fixedRotation = transform.rotation.eulerAngles;
    }

    private void FixedUpdate()
    {
        // Apply the fixed rotation to the camera
        transform.rotation = Quaternion.Euler(fixedRotation);
    }
}
