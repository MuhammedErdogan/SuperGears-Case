using Others;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Controller
{
    [DefaultExecutionOrder(-80)]
    public class UIController : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] private GameObject _onStart, _onDrive, _onFinish;
        [SerializeField] private TextMeshProUGUI _timeText;
        [SerializeField] private TextMeshProUGUI _averageSpeedText;
        [SerializeField] private TextMeshProUGUI _maxSpeedText;
        #endregion

        private void OnEnable()
        {
            EventManager.StartListening(EventManager.GameLoaded, OnStart);
            EventManager.StartListening(EventManager.OnTourRestart, OnStart);
            EventManager.StartListening(EventManager.OnTourStarted, TourStarted);
            EventManager.StartListening(EventManager.OnTourCompleted, TourCompleted);
        }

        private void OnDisable()
        {
            EventManager.StopListening(EventManager.GameLoaded, OnStart);
            EventManager.StopListening(EventManager.OnTourRestart, OnStart);
            EventManager.StopListening(EventManager.OnTourStarted, TourStarted);
            EventManager.StopListening(EventManager.OnTourCompleted, TourCompleted);
        }

        private void OnStart()
        {
            _onStart.SetActive(true);
            _onDrive.SetActive(false);
            _onFinish.SetActive(false);
        }

        private void TourStarted()
        {
            _onStart.SetActive(false);
            _onDrive.SetActive(true);
            _onFinish.SetActive(false);
        }

        private void TourCompleted(float time, float distance, float maxSpeed)
        {
            this.DelayedAction(1.5f, () =>
            {
                _onStart.SetActive(false);
                _onDrive.SetActive(false);
                _onFinish.SetActive(true);

                _timeText.text = $"Time: {time:F2} s";
                _averageSpeedText.text = $"Average Speed: {(distance / time) * 3.6f:F2} KmH";
                _maxSpeedText.text = $"Max Speed: {maxSpeed * 3.6f:F2} m/s";
            }, out _);
        }
    }
}