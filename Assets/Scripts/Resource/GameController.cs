using Car;
using Car.Display;
using Car.Engine;
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
        private IAudioService _carAudioService;
        private IEngine _engine;

        public float SpeedMs { get; private set; }
        public float SpeedKmh { get; private set; }
        public float SpeedMph { get; private set; }

        private bool _isTourStarted;

        private void Start()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            _car = FindObjectOfType<BaseCar>();

            var container = new DependencyContainer();

            _engine = new StandardEngine(100f, 10f, 20f, 5f, 5, 7000);
            container.Register(_engine);

            var sedan = _car as Sedan;
            container.InjectDependencies(_car);

            var speedoMeter = FindObjectOfType<Speedometer>();
            container.InjectDependencies(speedoMeter);

            var rpmMeter = FindObjectOfType<RpmMeter>();
            container.InjectDependencies(rpmMeter);

            container = new DependencyContainer();
            _carAudioService = new CarAudioService();
            var audioSource = sedan.GetComponent<AudioSource>();
            var carAudioClip = audioSource.clip;

            container.Register(audioSource);
            container.Register(carAudioClip);
            container.InjectDependencies(_carAudioService);

            _carAudioService.PlaySound();
            EventManager.TriggerEvent(EventManager.GameLoaded);
        }

        private void OnEnable()
        {
            EventManager.StartListening(EventManager.OnCountdownEnded, StartTour);
        }

        private void OnDisable()
        {
            EventManager.StopListening(EventManager.OnCountdownEnded, StartTour);
        }

        private void FixedUpdate()
        {
            if (!_isTourStarted) return;

            if (Input.GetKey(KeyCode.UpArrow))
                _driveState = DriveState.Accelerate;
            else if (Input.GetKey(KeyCode.Space))
                _driveState = DriveState.Brake;
            else
                _driveState = DriveState.Decelerate;

            _car.Drive(_driveState);
            _carAudioService.UpdatePitch(_car.CurrentRpm, _engine.MaxRPM);
        }

        private void StartTour()
        {
            _isTourStarted = true;
        }
    }
}