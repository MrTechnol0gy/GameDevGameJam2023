using UnityEngine;
using System.Collections;

public class MoveAndLerpBack : MonoBehaviour
{
    [SerializeField] private float moveDistance = 5f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float lerpSpeed = 2f;

    private Vector3 originalPosition;
    private Vector3 targetPosition;
    public bool isMoving = false;

    private void Start()
    {
        originalPosition = transform.position;
        targetPosition = originalPosition + Vector3.up * moveDistance;
    }

    private void Update()
    {
        if (isMoving)
        {
            // Move the GameObject forcefully towards the target position on the y-axis
            Vector3 newPosition = transform.position + Vector3.up * moveSpeed * Time.deltaTime;
            if (newPosition.y >= targetPosition.y)
            {
                isMoving = false;
                newPosition = targetPosition;
                StartCoroutine(LerpBack());
            }
            transform.position = newPosition;
        }
    }

    public void StartMovement()
    {
        isMoving = true;
    }

    private IEnumerator LerpBack()
    {
        float t = 0f;
        Vector3 startPosition = transform.position;

        while (t < 1f)
        {
            t += Time.deltaTime * lerpSpeed;
            transform.position = Vector3.Lerp(startPosition, originalPosition, t);
            yield return null;
        }

        // Ensure the GameObject's position is exactly the original position
        transform.position = originalPosition;
    }
}
