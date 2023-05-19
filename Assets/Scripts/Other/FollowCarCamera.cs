using UnityEngine;

public class FollowCarCamera : MonoBehaviour
{
    [SerializeField] private GameObject car;  // The car that the camera will follow.
    [SerializeField] private float distance = 10.0f;  // The distance the camera will keep from the car.
    [SerializeField] private float rotationSpeed = 5.0f;  // The speed at which the camera will rotate around the car.
    [SerializeField] private float pitchSpeed = 2.0f;  // The speed at which the camera will pitch up and down.
    private float currentHorizontalAngle = 0.0f;  // The current horizontal angle of rotation.
    private float currentVerticalAngle = 0.0f;  // The current vertical angle of rotation.

    private void LateUpdate()
    {
        HandleRotation();

        // Adjust the distance based on the mouse scroll wheel input.
        distance -= Input.GetAxis("Mouse ScrollWheel") * 5.0f;
        distance = Mathf.Clamp(distance, 5.0f, 20.0f); // Optional: Clamp the distance to a desired range.

        // Calculate the new position of the camera.
        Vector3 direction = new Vector3(
            Mathf.Sin(currentHorizontalAngle),
            Mathf.Sin(currentVerticalAngle),
            Mathf.Cos(currentHorizontalAngle)
        ).normalized;

        transform.position = car.transform.position + direction * distance;

        transform.LookAt(car.transform);
    }


    private void HandleRotation()
    {
        if (Input.GetMouseButton(0))
        {
            // Rotate horizontally.
            currentHorizontalAngle += Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            // Pitch vertically, with a clamp to avoid flipping the camera.
            currentVerticalAngle = Mathf.Clamp(currentVerticalAngle - Input.GetAxis("Mouse Y") * pitchSpeed * Time.deltaTime, 0, Mathf.PI / 2);
        }
    }
}
