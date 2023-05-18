using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Car.Display
{
    public class RpmMeter : MonoBehaviour
    {
        [SerializeField] private RectTransform _needle;
        [SerializeField] private GameObject _numberPrefab;
        [SerializeField] private TextMeshProUGUI _currentGearText;

        private float _maxRPM = 7000f;  // Maximum RPM for your RPM meter
        private int _numberOfMarks = 10;  // The number of marks on your RPM meter.

        private float _maxAngle = -130f;  // The maximum rotation of the needle
        private float _minAngle = 130f;  // The minimum rotation of the needle

        [Inject] private Engine.IEngine engine;

        private void OnEnable()
        {
            EventManager.StartListening(EventManager.CarInitialized, Initialize);
            EventManager.StartListening(EventManager.OnCarMove, UpdateRPMMeter);
            EventManager.StartListening(EventManager.OnGearChange, UpdateGear);
        }

        private void OnDisable()
        {
            EventManager.StopListening(EventManager.CarInitialized, Initialize);
            EventManager.StopListening(EventManager.OnCarMove, UpdateRPMMeter);
            EventManager.StopListening(EventManager.OnGearChange, UpdateGear);
        }

        private void Initialize()
        {
            _maxRPM = engine.MaxRPM;
            _numberOfMarks = engine.NumberOfGears;

            GenerateNumbers();
            UpdateRPMMeter(0, 0);
        }

        private void UpdateGear(int currentGear)
        {
            _currentGearText.text = currentGear.ToString();
        }

        private void UpdateRPMMeter(float _, float currentRPM)
        {
            // currentRPM / _maxRPM is the fraction of the current RPM to the maximum RPM
            float angle = Mathf.Lerp(_minAngle, _maxAngle, currentRPM / _maxRPM);
            _needle.rotation = Quaternion.Lerp(_needle.rotation, Quaternion.Euler(0, 0, angle), 0.15f);
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
                text.text = ((_maxRPM / _numberOfMarks) * i).ToString();

                numberObject.transform.SetSiblingIndex(0);
            }
        }
    }
}
