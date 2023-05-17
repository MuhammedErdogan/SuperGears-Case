using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCar : MonoBehaviour
{
    public WheelCollider frontLeftWheel, frontRightWheel, rearLeftWheel, rearRightWheel;
    public Transform frontLeftWheelTransform, frontRightWheelTransform, rearLeftWheelTransform, rearRightWheelTransform;

    private Rigidbody rb;
    private IEngine engine;
    private Vector3 startingPosition;

    public BaseCar(IEngine engine)
    {
        this.engine = engine;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        startingPosition = transform.position;
    }

    private void Update()
    {
        engine.Accelerate(rb.velocity.magnitude, rb);
        engine.Brake(rb.velocity.magnitude, rb);
        UpdateWheelPoses();
    }

    private void UpdateWheelPoses()
    {
        UpdateWheelPoseAndRot(frontLeftWheel, frontLeftWheelTransform);
        UpdateWheelPoseAndRot(frontRightWheel, frontRightWheelTransform);
        UpdateWheelPoseAndRot(rearLeftWheel, rearLeftWheelTransform);
        UpdateWheelPoseAndRot(rearRightWheel, rearRightWheelTransform);
    }

    private void UpdateWheelPoseAndRot(WheelCollider wheelCollider, Transform wheelTransform)
    {
        wheelCollider.GetWorldPose(out Vector3 position, out _);

        wheelTransform.position = position;

        float wheelRadius = wheelCollider.radius;
        float wheelCircumference = 2 * Mathf.PI * wheelRadius;

        float distanceTravelled = Vector3.Distance(startingPosition, transform.position);
        float rotationAngle = 360 * (distanceTravelled / wheelCircumference);

        wheelTransform.rotation = Quaternion.Euler(rotationAngle, 0, 0);
    }
}
