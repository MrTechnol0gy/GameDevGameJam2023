using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public static MainCamera get;
    // Array of cameras in the scene
    private Camera[] cameras;
    // List of cameras in the scene
    private List<GameObject> cameraList = new List<GameObject>();

    void Awake()
    {
        get = this;
    }
    void Start()
    {
        GetCamerasInScene();
        SetCameraViewports();        
    }

    private void GetCamerasInScene()
    {
        // Get the size of the level from the LevelManager
        int amountOfCameras = LevelManager.instance.DetermineCameraAmount();

        // Create an array of cameras
        cameras = new Camera[amountOfCameras];

        // Find all cameras in the scene by tag "SecurityCamera"
        GameObject[] cameraObjects = GameObject.FindGameObjectsWithTag("SecurityCamera");

        // Loop through the cameras and add them to the array
        for (int i = 0; i < amountOfCameras; i++)
        {
            cameras[i] = cameraObjects[i].GetComponentInChildren<Camera>();
        }
    }

    private void SetCameraViewports()
    {
        // Set the viewport rect for each camera view
        int numCameras = cameras.Length;
        int numRows = Mathf.CeilToInt(Mathf.Sqrt(numCameras));
        int numCols = Mathf.CeilToInt((float)numCameras / numRows);
        int p = 0;

        float viewWidth = 1f / numCols;
        float viewHeight = 1f / numRows;

        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numCols; j++)
            {
                float x = j * viewWidth;
                float y = i * viewHeight;

                // Adjust the last row and last column if the number of cameras is not a perfect square
                if (i == numRows - 1 && numCameras % numRows != 0)
                {
                    viewWidth = 1f / (numCameras % numRows);
                }
                if (j == numCols - 1 && numCameras % numCols != 0)
                {
                    viewHeight = 1f / (numCameras % numCols);
                }

                SetViewportRect(p, x, y, viewWidth, viewHeight);
                p++;
            }
        }
    }

    void SetViewportRect(int cameraIndex, float x, float y, float width, float height)
    {
        Rect viewportRect = new Rect(x, y, width, height);
        cameras[cameraIndex].rect = viewportRect;
    }

    // return the list of cameras
    public List<GameObject> GetCamerasList()
    {
        return cameraList;
    }

    // return the array of cameras
    public Camera[] GetCamerasArray()
    {
        return cameras;
    }
}
