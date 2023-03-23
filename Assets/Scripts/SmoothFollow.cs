using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    public Transform cameraTransform;
    public bool isCentered;

    private void Update()
    {
        // Calculate the target position for the canvas
        Vector3 targetPosition = isCentered ? cameraTransform.position + cameraTransform.forward * 0.9f : cameraTransform.position + cameraTransform.forward * 0.9f + cameraTransform.up * 0.3f + cameraTransform.right * 0.3f;
        
        // Smoothly interpolate between the current position and the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 7f);

        transform.rotation = Quaternion.LookRotation(transform.position - cameraTransform.position);

    }
}