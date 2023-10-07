using UnityEngine;

public class SmoothRotation : MonoBehaviour
{
    [Header("Camera Stats")]
    [SerializeField] private float rotationSpeed = 2f;
    [SerializeField] private float minRotationAngle = 0f;
    [SerializeField] private float maxRotationAngle = 180f;

    private Quaternion targetRotation;

    private void Start()
    {
        // Set the initial target rotation based on the starting rotation of the GameObject
        targetRotation = transform.rotation;
    }

    private void Update()
    {
        // Calculate the target rotation based on the current time and rotation range
        float t = Mathf.PingPong(Time.time * rotationSpeed, 1f);
        float angle = Mathf.Lerp(minRotationAngle, maxRotationAngle, t);

        // Set the target rotation to rotate only around the y-axis
        targetRotation = Quaternion.Euler(0f, angle, 0f);

        // Smoothly rotate towards the target rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
}
