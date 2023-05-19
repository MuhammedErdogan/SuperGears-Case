using TMPro;
using UnityEngine;
using System.Collections;
using Resource;

public class TourTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _countdownText;
    [SerializeField] private TextMeshProUGUI _timerText;

    public int CountdownTime = 5;
    private float _timer;
    private Coroutine _countdownCoroutine;

    private void OnEnable()
    {
        EventManager.StartListening(EventManager.GameLoaded, StartCountdown);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventManager.GameLoaded, StartCountdown);
    }

    private void StartCountdown()
    {
        if (_countdownCoroutine != null)
        {
            StopCoroutine(_countdownCoroutine);
        }
        _countdownCoroutine = StartCoroutine(CountdownCoroutine());
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

        StartCoroutine(StartTimer());
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
