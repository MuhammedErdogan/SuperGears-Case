using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Speedometer : MonoBehaviour
{
    [SerializeField] private RectTransform needle;
    [SerializeField] private GameObject numberPrefab;  // A Text UI prefab that will be instantiated for each number.

    private float maxSpeedKmh = 200f;  // max speed for your speedometer
    private int numberOfMarks = 10;  // The number of marks on your speedometer.

    private float maxAngle = -130f;  // the maximum rotation of the needle
    private float minAngle = 130f;  // the minimum rotation of the needle

    [Inject] private IEngine engine;
    //[Inject] private ICar car;

    private void OnEnable()
    {
        EventManager.StartListening(EventManager.CarInitialized, Initialize);
        EventManager.StartListening(EventManager.NotifySpeed, UpdateSpeedometer);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventManager.CarInitialized, Initialize);
        EventManager.StopListening(EventManager.NotifySpeed, UpdateSpeedometer);
    }

    private void Initialize()
    {
        maxSpeedKmh = engine.MaxSpeed;
        numberOfMarks = engine.NumberOfGears;

        GenerateNumbers();
        UpdateSpeedometer(0);
    }

    private void UpdateSpeedometer(float currentSpeedKmh)
    {
        float speedFraction = currentSpeedKmh / maxSpeedKmh;
        float angle = Mathf.Lerp(minAngle, maxAngle, speedFraction);
        needle.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void GenerateNumbers()
    {
        float range = maxAngle - minAngle;
        float increment = range / numberOfMarks;

        for (int i = 0; i <= numberOfMarks; i++)
        {
            GameObject numberObject = Instantiate(numberPrefab, transform);
            RectTransform rectTransform = numberObject.GetComponent<RectTransform>();

            float angle = minAngle + increment * i;
            rectTransform.localRotation = Quaternion.Euler(0, 0, angle);

            var text = numberObject.GetComponent<TextMeshProUGUI>();
            text.text = ((maxSpeedKmh / numberOfMarks) * i).ToString();

            numberObject.transform.SetSiblingIndex(0);
        }
    }
}
