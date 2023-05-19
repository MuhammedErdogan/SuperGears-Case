using Car;
using Car.Display;
using Car.Other;
using Engine;
using Engine.Car;
using UnityEngine;

namespace Controller
{
    public class GameController : MonoBehaviour
    {
        #region Singleton
        public static GameController Instance { get; private set; }
        #endregion

        [SerializeField] private Transform targetLocation;
        private BaseCar _car;
        private DriveState _driveState;

        private IAudioService _carAudioService;
        private IEngine _engine;
        private ICar _carInterface;

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

            LoadGame();
            StartGame();
        }

        private void LoadGame()
        {
            _car = FindObjectOfType<BaseCar>();
            _carInterface = _car;

            var sedan = _car as Sedan;
            sedan.Initialize();

            //******************************************************
            var container = new DependencyContainer();

            //camera init
            var followCarController = FindObjectOfType<CameraController>();
            container.Register(sedan.transform);
            container.InjectDependencies(followCarController);

            //engine creation
            _engine = new StandardEngine(100f, 10f, 20f, 5f, 5, 7000);
            container.Register(_engine);
            container.InjectDependencies(_carInterface);

            //display init
            var speedoMeter = FindObjectOfType<Speedometer>();
            var rpmMeter = FindObjectOfType<RpmMeter>();
            container.InjectDependencies(speedoMeter);
            container.InjectDependencies(rpmMeter);

            //******************************************************
            container = new DependencyContainer();

            //audio init
            _carAudioService = new CarAudioService();
            var audioSource = sedan.GetComponent<AudioSource>();
            var carAudioClip = audioSource.clip;

            container.Register(audioSource);
            container.Register(carAudioClip);
            container.InjectDependencies(_carAudioService);

            //******************************************************
            EventManager.TriggerEvent(EventManager.GameLoaded);
        }

        private void StartGame()
        {
            _carAudioService.PlaySound();
            EventManager.TriggerEvent(EventManager.TourStarted);
        }

        private void OnEnable()
        {
            EventManager.StartListening(EventManager.OnCountdownEnded, () => _isTourStarted = true);
        }

        private void OnDisable()
        {
            EventManager.StopListening(EventManager.OnCountdownEnded, () => _isTourStarted = true);
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
            IsArrivedToTargetLocation();
        }

        private void IsArrivedToTargetLocation()
        {
            Debug.Log((_car.transform.position - targetLocation.position).sqrMagnitude);
            if ((_car.transform.position - targetLocation.position).sqrMagnitude < 10f)
            {
                EventManager.TriggerEvent(EventManager.OnTourCompleted);
            }
        }
    }
}