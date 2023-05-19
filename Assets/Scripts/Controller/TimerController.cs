using TMPro;
using UnityEngine;
using System.Collections;
using Others;

namespace Controller
{
    public class TimerController : MonoBehaviour
    {
        #region Singleton
        public static TimerController Instance { get; private set; }
        #endregion

        #region Serialized Fields
        [SerializeField] private TextMeshProUGUI _countdownText;
        [SerializeField] private TextMeshProUGUI _timerText;
        #endregion

        #region Countdown Variables
        public int CountdownTime = 5;
        private float _timer;
        private Coroutine _countdownCoroutine, _startTimerCoroutine;
        #endregion

        #region Properties
        public float Timer => _timer;
        #endregion

        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            EventManager.StartListening(EventManager.GameStarted, StartCountdown);
            EventManager.StartListening(EventManager.OnTourRestart, ResetTimer);
            EventManager.StartListening(EventManager.OnTourCompleted, (float _, float __, float ___) =>
            {
                if (_startTimerCoroutine != null)
                    StopCoroutine(_startTimerCoroutine);
            });
        }

        private void OnDisable()
        {
            EventManager.StopListening(EventManager.GameStarted, StartCountdown);
            EventManager.StopListening(EventManager.OnTourRestart, ResetTimer);
            EventManager.StopListening(EventManager.OnTourCompleted, (float _, float __, float ___) =>
            {
                if (_startTimerCoroutine != null)
                    StopCoroutine(_startTimerCoroutine);
            });
        }

        private void StartCountdown()
        {
            _countdownCoroutine = StartCoroutine(CountdownCoroutine());
        }

        private void ResetTimer()
        {
            _countdownText.gameObject.SetActive(true);

            _countdownText.text = CountdownTime.ToString();
            _timerText.text = "0";
            _timer = 0;

            if (_countdownCoroutine != null)
                StopCoroutine(_countdownCoroutine);

            if (_startTimerCoroutine != null)
                StopCoroutine(_startTimerCoroutine);

            StartCountdown();
        }

        private IEnumerator CountdownCoroutine()
        {
            for (int timer = CountdownTime; timer > 0; timer--)
            {
                _countdownText.text = timer.ToString();
                _countdownText.transform.ScaleTo(Vector3.one * 1.5f, 0.25f, () =>
                {
                    _countdownText.transform.ScaleTo(Vector3.one, 0.25f);
                });
                yield return new WaitForSeconds(1f);
            }

            _countdownText.text = "GO!";
            _countdownText.transform.ScaleTo(Vector3.one * 1.5f, 0.5f, () =>
            {
                _countdownText.gameObject.SetActive(false);
            });

            _startTimerCoroutine = StartCoroutine(StartTimer());
            EventManager.TriggerEvent(EventManager.OnCountdownEnded);
        }

        private IEnumerator StartTimer()
        {
            while (true)
            {
                _timer += Time.deltaTime;
                _timerText.text = _timer.ToString("F2");
                yield return null;
            }
        }
    }
}
