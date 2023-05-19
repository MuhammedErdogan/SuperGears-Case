using Car;
using Car.Display;
using Car.Other;
using Engine;
using Engine.Car;
using Manager;
using Others;
using UnityEngine;

namespace Controller
{
    [DefaultExecutionOrder(-90)]
    public class GameController : MonoBehaviour
    {
        #region Singleton
        public static GameController Instance { get; private set; }
        #endregion

        #region Serialized Fields
        [SerializeField] private Transform targetLocation, startLocation;
        #endregion

        #region Private Fields
        private BaseCar _car;
        private DriveState _driveState;
        private IAudioService _carAudioService;
        private IEngine _engine;
        private ICar _carInterface;
        private bool _isTourStarted, _isTourCompleted;
        private Coroutine _ChangePitchOverTimeCoroutine;
        private float _maxSpeedKmh;
        #endregion

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            LoadGame();
            Initialize();
        }

        private void OnEnable()
        {
            EventManager.StartListening(EventManager.OnCountdownEnded, StartTour);
            EventManager.StartListening(EventManager.OnTourRestart, ResetController);
        }

        private void OnDisable()
        {
            EventManager.StopListening(EventManager.OnCountdownEnded, StartTour);
            EventManager.StopListening(EventManager.OnTourRestart, ResetController);
        }

        private void FixedUpdate()
        {
            if (!_isTourStarted || _isTourCompleted) return;
            UpdateDrivingState();
            UpdateCarAndAudio();
            CheckArrival();
            CalculateMaxSpeed();
        }

        private void LoadGame()
        {
            _car = FindObjectOfType<BaseCar>();
            _carInterface = _car;

            if (_car is Sedan sedan)
            {
                sedan.Initialize();
            }

            DependencyManager container = new DependencyManager();

            var followCarController = FindObjectOfType<CameraController>();
            container.Register(_car.transform);
            container.InjectDependencies(followCarController);

            _engine = new StandardEngine(100f, 10f, 20f, 5f, 5, 7000);
            container.Register(_engine);
            container.InjectDependencies(_carInterface);

            var speedometer = FindObjectOfType<Speedometer>(true);
            var rpmMeter = FindObjectOfType<RpmMeter>(true);
            container.InjectDependencies(speedometer);
            container.InjectDependencies(rpmMeter);

            container = new DependencyManager();

            _carAudioService = new CarAudioService();
            var audioSource = _car.GetComponent<AudioSource>();
            var carAudioClip = audioSource.clip;

            container.Register(audioSource);
            container.Register(carAudioClip);
            container.InjectDependencies(_carAudioService);

            EventManager.TriggerEvent(EventManager.GameLoaded);
        }

        private void Initialize()
        {
            SetCarPosition(startLocation.position);
            _car.Initialize();
            _carAudioService.PlaySound();
        }

        private void StartTour()
        {
            EventManager.TriggerEvent(EventManager.OnTourStarted);
            _isTourStarted = true;
        }

        private void ResetController()
        {
            _isTourStarted = false;
            _isTourCompleted = false;
            _maxSpeedKmh = 0;

            if (_ChangePitchOverTimeCoroutine != null)
                StopCoroutine(_ChangePitchOverTimeCoroutine);

            _carAudioService.UpdatePitch(0, _engine.MaxRPM);

            Initialize();
        }

        private bool IsArrivedToTargetLocation => (_car.transform.position - targetLocation.position).sqrMagnitude < 10f;

        private void Arrived()
        {
            _isTourCompleted = true;
            ChangePitchOverTime();

            EventManager.TriggerEvent(EventManager.OnTourCompleted, TimerController.Instance.Timer, _car.TotalDistanceTravelled, _maxSpeedKmh);
        }

        private void ChangePitchOverTime()
        {
            this.ChangeTo(_car.CurrentRpm, 0, 3, value =>
            {
                _carAudioService.UpdatePitch(value, _engine.MaxRPM);
            }, out _ChangePitchOverTimeCoroutine, _ =>
            {
                _carAudioService.StopSound();
            });
        }

        private void SetCarPosition(Vector3 position)
        {
            _car.transform.position = position;
        }

        private void UpdateDrivingState()
        {
            if (Input.GetKey(KeyCode.UpArrow))
                _driveState = DriveState.Accelerate;
            else if (Input.GetKey(KeyCode.Space))
                _driveState = DriveState.Brake;
            else
                _driveState = DriveState.Decelerate;
        }

        private void UpdateCarAndAudio()
        {
            _car.Drive(_driveState);
            _carAudioService.UpdatePitch(_car.CurrentRpm, _engine.MaxRPM);
        }

        private void CheckArrival()
        {
            if (IsArrivedToTargetLocation)
            {
                Arrived();
            }
        }

        private void CalculateMaxSpeed()
        {
            _maxSpeedKmh = _car.CurrentSpeedMs > _maxSpeedKmh ? _car.CurrentSpeedMs : _maxSpeedKmh;
        }
    }
}
