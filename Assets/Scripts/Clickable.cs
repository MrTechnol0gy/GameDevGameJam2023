using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clickable : MonoBehaviour
{
    Camera[] cameras;

    void Start()
    {
        cameras = MainCamera.get.cameraList();
        //Debug.Log("Camera list is this long " + cameras.Length);
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            foreach (Camera camera in cameras)
            {
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Debug.Log("Raycast hit: " + hit.collider.gameObject.name);
                    Debug.DrawRay(ray.origin, ray.direction * 1000f, Color.red);

                    // Check if the clicked object has the desired script/component
                    AIMugger aIMugger = hit.collider.GetComponent<AIMugger>();
                    if (aIMugger != null)
                    {
                        // Invoke the method on the clicked object
                        aIMugger.Gottem();
                    }
                }
            }
        }
    }
}