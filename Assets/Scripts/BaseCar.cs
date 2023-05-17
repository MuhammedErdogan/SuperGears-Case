using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCar : MonoBehaviour
{
    [SerializeField] private WheelCollider frontLeftWheel, frontRightWheel, rearLeftWheel, rearRightWheel;
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform, rearLeftWheelTransform, rearRightWheelTransform;

    private Rigidbody rb;
    protected IEngine engine;
    private Vector3 startingPosition;

    public virtual void Initialize(DependencyContainer container)
    {
        this.engine = container.Resolve<IEngine>();
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        startingPosition = transform.position;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            engine.Accelerate(rb);
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            engine.Brake(rb);
        }

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
