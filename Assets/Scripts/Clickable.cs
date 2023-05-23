using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clickable : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
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
