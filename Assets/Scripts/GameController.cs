using Car;
using UnityEngine;

namespace Controller
{
    public class GameController : MonoBehaviour
    {
        #region Singleton
        public static GameController Instance { get; private set; }
        #endregion

        private BaseCar _car;
        private DriveState _driveState;

        public float SpeedMs { get; private set; }
        public float SpeedKmh { get; private set; }
        public float SpeedMph { get; private set; }

        private void Start()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            _car = FindObjectOfType<BaseCar>();

            var container = new DependencyContainer();

            IEngine standardEngine = new StandardEngine(100f, 10f, 20f, 5f, 5, 7000);
            container.Register(standardEngine);

            var sedan = _car as Sedan;
            container.InjectDependencies(_car);

            var speedoMeter = FindObjectOfType<Speedometer>();
            container.InjectDependencies(speedoMeter);

            var rpmMeter = FindObjectOfType<RpmMeter>();
            container.InjectDependencies(rpmMeter);

            EventManager.TriggerEvent(EventManager.CarInitialized);
        }

        private void OnEnable()
        {
        }

        private void OnDisable()
        {
        }

        private void FixedUpdate()
        {
            if (Input.GetKey(KeyCode.UpArrow))
                _driveState = DriveState.Accelerate;
            else if (Input.GetKey(KeyCode.Space))
                _driveState = DriveState.Brake;
            else
                _driveState = DriveState.Decelerate;

            _car.Drive(_driveState);

            EventManager.TriggerEvent(EventManager.OnCarMove, _car.CurrentSpeedMs, _car.CurrentRpm);
        }
    }
}