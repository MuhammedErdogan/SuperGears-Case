using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCar : MonoBehaviour
{
    [SerializeField] private WheelCollider frontLeftWheel, frontRightWheel, rearLeftWheel, rearRightWheel;
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform, rearLeftWheelTransform, rearRightWheelTransform;

    [Inject] protected IEngine engine;
    private Rigidbody rb;
    private Vector3 startingPosition;

    public virtual void Initialize(DependencyContainer container)
    {
        //this.engine = container.Resolve<IEngine>();
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        startingPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            engine.Accelerate(rb);
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            engine.Brake(rb);
        }
        else
        {
            engine.Decelerate(rb);
        }

        EventManager.TriggerEvent(EventManager.OnCarMove, rb);
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

        // 2 * Mathf.PI * wheelCollider.radius = circumference of the wheel and Vector3.Distance(startingPosition, transform.position) = distance travelled by the car.
        float rotationAngle = 360 * (Vector3.Distance(startingPosition, transform.position) / 2 * Mathf.PI * wheelCollider.radius);
        wheelTransform.rotation = Quaternion.Euler(rotationAngle, 0, 0);
    }
}
