using Others;
using UnityEngine;

namespace Controller
{
    public class CameraController : MonoBehaviour
    {
        #region Singleton
        public static GameController Instance { get; private set; }
        #endregion

        #region Serialized Fields
        [SerializeField] private Transform targetLocation, startLocation;
        #endregion

        #region Private Fields
        [Inject] private Transform _car;  // The car that the camera will follow.
        private float _distance = 10.0f;  // The distance the camera will keep from the car.
        private float _rotationSpeed = 5.0f;  // The speed at which the camera will rotate around the car.
        private float _pitchSpeed = 2.0f;  // The speed at which the camera will pitch up and down.
        private float _currentHorizontalAngle = 0.0f;  // The current horizontal angle of rotation.
        private float _currentVerticalAngle = 0.0f;  // The current vertical angle of rotation.
        #endregion

        #region Tour Flags
        private bool _isTourStarted, _isTourCompleted;
        #endregion

        private void OnEnable()
        {
            EventManager.StartListening(EventManager.GameLoaded, Init);
            EventManager.StartListening(EventManager.OnTourRestart, ResetController);

            EventManager.StartListening(EventManager.OnCountdownEnded, () => _isTourStarted = true);
            EventManager.StartListening(EventManager.OnTourCompleted, (float _, float __,float ___) => _isTourCompleted = true);
        }

        private void OnDisable()
        {
            EventManager.StopListening(EventManager.GameLoaded, Init);
            EventManager.StopListening(EventManager.OnTourRestart, ResetController);

            EventManager.StopListening(EventManager.OnCountdownEnded, () => _isTourStarted = true);
            EventManager.StopListening(EventManager.OnTourCompleted, (float _, float __, float ___) => _isTourCompleted = true);
        }

        private void Init()
        {
            _isTourStarted = false;
            _isTourCompleted = false;

            PlayStartAnimation();
        }

        private void ResetController()
        {
            _isTourStarted = false;
            _isTourCompleted = false;
            
            _currentHorizontalAngle = 0.0f;
            _currentVerticalAngle = 0.0f;

            PlayStartAnimation();
        }

        private void PlayStartAnimation()
        {
            this.ChangeTo(0f, .5f, 5f, currentValue =>
            {
                _currentHorizontalAngle = -currentValue * 5;
                _currentVerticalAngle = currentValue;
            }, out _);
        }

        private void LateUpdate()
        {
            if (_isTourCompleted) return;

            HandleRotation();

            // Adjust the distance based on the mouse scroll wheel input.
            _distance -= Input.GetAxis("Mouse ScrollWheel") * 5.0f;
            _distance = Mathf.Clamp(_distance, 5.0f, 20.0f); // Optional: Clamp the distance to a desired range.

            // Calculate the new position of the camera.
            Vector3 direction = new Vector3(
                Mathf.Sin(_currentHorizontalAngle),
                Mathf.Sin(_currentVerticalAngle),
                Mathf.Cos(_currentHorizontalAngle)
            ).normalized;

            transform.position = _car.position + direction * _distance;

            transform.LookAt(_car);
        }


        private void HandleRotation()
        {
            if (!_isTourStarted) return;

            if (Input.GetMouseButton(0))
            {
                // Rotate horizontally.
                _currentHorizontalAngle += Input.GetAxis("Mouse X") * _rotationSpeed * Time.deltaTime;
                // Pitch vertically, with a clamp to avoid flipping the camera.
                _currentVerticalAngle = Mathf.Clamp(_currentVerticalAngle - Input.GetAxis("Mouse Y") * _pitchSpeed * Time.deltaTime, 0, Mathf.PI / 2);
            }
        }
    }
}