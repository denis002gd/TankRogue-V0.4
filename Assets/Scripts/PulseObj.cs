using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseObj : MonoBehaviour
{
 public Material targetMaterial; // The material to switch to
    private float switchDuration = 0.05f; // Duration of the material switch in seconds

    private Renderer objectRenderer;    // The renderer component of the object
    private Material originalMaterial;  // The original material of the object

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        originalMaterial = objectRenderer.material;
    }


    public void SwitchMaterial()
    {
        StartCoroutine(SwitchAndReset());
    }

    private System.Collections.IEnumerator SwitchAndReset()
    {
        objectRenderer.material = targetMaterial;
        yield return new WaitForSeconds(switchDuration);
        objectRenderer.material = originalMaterial;
    }

    private bool IsSwitching()
    {
        return objectRenderer.material == targetMaterial;
    }
}