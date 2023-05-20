using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Car.Display
{
    public class RpmMeter : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] private RectTransform _needle;
        [SerializeField] private TextMeshProUGUI _currentGearText;
        #endregion

        #region Private Fields
        private GameObject _numberPrefab;
        private readonly int _numberOfMarks = 10;  // The number of marks on your RPM meter.
        private readonly float _maxAngle = -130f;  // The maximum rotation of the needle
        private readonly float _minAngle = 130f;  // The minimum rotation of the needle
        #endregion

        #region Injected Fields
        [Inject] private int _maxRPM = 7000;
        #endregion

        private void Awake()
        {
            _numberPrefab = Resources.Load<GameObject>("Prefabs/Number");
        }

        private void OnEnable()
        {
            EventManager.StartListening(EventManager.GameLoaded, Initialize);
            EventManager.StartListening(EventManager.OnTourRestart, ResetRpmMeter);
            EventManager.StartListening(EventManager.OnCarMove, UpdateRPMMeter);
            EventManager.StartListening(EventManager.OnGearChange, UpdateGear);
        }

        private void OnDisable()
        {
            EventManager.StopListening(EventManager.GameLoaded, Initialize);
            EventManager.StopListening(EventManager.OnTourRestart, ResetRpmMeter);
            EventManager.StopListening(EventManager.OnCarMove, UpdateRPMMeter);
            EventManager.StopListening(EventManager.OnGearChange, UpdateGear);
        }

        private void Initialize()
        {
            GenerateNumbers();
            UpdateRPMMeter(0);
        }

        private void ResetRpmMeter()
        {
            UpdateRPMMeter(0);
        }

        private void UpdateGear(int currentGear)
        {
            _currentGearText.text = currentGear.ToString();
        }

        private void UpdateRPMMeter(float _ = 0, float currentRPM = 0, float __ = 0)
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
