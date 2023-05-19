using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Car.Display
{
    public class Speedometer : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] private TextMeshProUGUI _distanceTravelledText;
        [SerializeField] private RectTransform _needle;
        #endregion

        #region Private Fields
        private GameObject _numberPrefab;  // A Text UI prefab that will be instantiated for each number.
        private float _maxSpeedKmh = 200f;  // max speed for your speedometer
        private readonly int _numberOfMarks = 10;  // The number of marks on your speedometer.
        private readonly float _maxAngle = -130f;  // the maximum rotation of the needle
        private readonly float _minAngle = 130f;  // the minimum rotation of the needle
        #endregion

        #region Injected Fields
        [Inject] private Engine.IEngine _engine;
        #endregion

        #region Actions
        private Action<float> UpdateSpeed;
        #endregion

        private void Awake()
        {
            _numberPrefab = UnityEngine.Resources.Load<GameObject>("Prefabs/Number");
        }

        private void OnEnable()
        {
            EventManager.StartListening(EventManager.GameLoaded, Initialize);
            EventManager.StartListening(EventManager.OnTourRestart, ResetSpeedometer);
            EventManager.StartListening(EventManager.OnCarMove, UpdateSpeedometer);
        }

        private void OnDisable()
        {
            EventManager.StopListening(EventManager.GameLoaded, Initialize);
            EventManager.StopListening(EventManager.OnTourRestart, ResetSpeedometer);
            EventManager.StopListening(EventManager.OnCarMove, UpdateSpeedometer);
        }

        private void Initialize()
        {
            _maxSpeedKmh = _engine.MaxSpeed * 3.6f;
            //_numberOfMarks = _engine.NumberOfGears;

            GenerateNumbers();
            UpdateSpeedometer(0);
        }

        private void ResetSpeedometer()
        {
            _distanceTravelledText.text = "0 KM";

            UpdateSpeedometer(0);
        }

        private void UpdateSpeedometer(float currentSpeedMs = 0, float _ = 0, float travelledDistance = 0)
        {
            var speedFraction = (currentSpeedMs * 3.6f) / _maxSpeedKmh; // currentSpeedMs * 3.6f converts m/s to km/h
            var angle = Mathf.Lerp(_minAngle, _maxAngle, speedFraction);

            _needle.rotation = Quaternion.Euler(0, 0, angle);
            _distanceTravelledText.text = $"{Math.Round(travelledDistance / 1000, 2)} KM";
        }

        private void GenerateNumbers()
        {
            float range = _maxAngle - _minAngle;
            float increment = range / _numberOfMarks;

            for (int i = 0; i <= _numberOfMarks; i++)
            {
                GameObject numberObject = Instantiate(_numberPrefab, transform);
                RectTransform rectTransform = numberObject.GetComponent<RectTransform>();

                float angle = _minAngle + increment * i;
                rectTransform.localRotation = Quaternion.Euler(0, 0, angle);

                var text = numberObject.GetComponent<TextMeshProUGUI>();
                text.text = (_maxSpeedKmh / _numberOfMarks * i).ToString();

                numberObject.transform.SetSiblingIndex(0);
            }

            //var maxSpeedMph = Math.Round(maxSpeedKmh * 0.62137, 2);
        }
    }
}
