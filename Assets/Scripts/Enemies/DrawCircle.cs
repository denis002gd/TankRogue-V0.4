using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(LineRenderer))]
public class DrawCircle : MonoBehaviour
{
   



    [SerializeField] private int numPoints = 100; // Number of points around the circle
    [SerializeField] private float radius = 5f;   // Radius of the circle

    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        
    }

    void Update()
    {
        Circle();
    }

    void Circle()
    {
        lineRenderer.positionCount = numPoints + 1;

        float deltaTheta = (2f * Mathf.PI) / numPoints;
        float theta = 0f;

        for (int i = 0; i <= numPoints; i++)
        {
            float x = radius * Mathf.Cos(theta);
            float y = radius * Mathf.Sin(theta);

            // Rotate the position 90 degrees on the Z-axis
            Vector3 rotatedPos = Quaternion.Euler(90f, 0f, 0f) * new Vector3(x, y, 0f);

            // Add the position of the GameObject the script is attached to
            Vector3 pos = rotatedPos + transform.position + new Vector3(0f, -0.99f, 0f);
            lineRenderer.SetPosition(i, pos);

            theta += deltaTheta;
        }
    }
    


}
