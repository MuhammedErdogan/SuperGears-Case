using Car.Engine;
using UnityEngine;

namespace Car
{
    public enum DriveState
    {
        Accelerate,
        Brake,
        Decelerate
    }

    public abstract class BaseCar : MonoBehaviour, ICar
    {
        [SerializeField] private WheelCollider frontLeftWheel, frontRightWheel, rearLeftWheel, rearRightWheel;
        [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform, rearLeftWheelTransform, rearRightWheelTransform;

        [Inject] protected IEngine engine;
        private Rigidbody rb;
        private Vector3 _lastPosition;
        private float _totalDistanceTravelled;

        public float CurrentSpeedMs => rb.velocity.magnitude;
        public float MaxSpeedKmh => engine.MaxSpeed;
        public int CurrentRpm => engine.RPM;
        public int NumberOfGears => engine.NumberOfGears;

        public virtual void Initialize(DependencyContainer container)
        {

        }

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            _lastPosition = transform.position;
        }

        public void Drive(DriveState driveState)
        {
            switch (driveState)
            {
                case DriveState.Accelerate:
                    engine.Accelerate(rb);
                    break;
                case DriveState.Brake:
                    engine.Brake(rb);
                    break;
                case DriveState.Decelerate:
                    engine.Decelerate(rb);
                    break;
            }

            CalculateTravelledDistance();
            UpdateWheelPoses();
        }

        private void CalculateTravelledDistance()
        {
            _totalDistanceTravelled += Vector3.Distance(transform.position, _lastPosition);
            _lastPosition = transform.position;
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

            // 2 * Mathf.PI * wheelCollider.radius = circumference of the wheel and Vector3.Distance(_lastPosition, transform.position) = distance travelled by the car.
            float rotationAngle = 360 * (_totalDistanceTravelled / 2 * Mathf.PI * wheelCollider.radius);
            wheelTransform.rotation = Quaternion.Euler(rotationAngle, 0, 0);
        }
    }
}
