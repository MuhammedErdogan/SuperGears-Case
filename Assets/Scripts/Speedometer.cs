using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Car.Display
{
    public class Speedometer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _distanceTravelledText;
        [SerializeField] private RectTransform needle;
        [SerializeField] private GameObject numberPrefab;  // A Text UI prefab that will be instantiated for each number.

        private float maxSpeedKmh = 200f;  // max speed for your speedometer
        private int numberOfMarks = 10;  // The number of marks on your speedometer.

        private float maxAngle = -130f;  // the maximum rotation of the needle
        private float minAngle = 130f;  // the minimum rotation of the needle

        [Inject] private Engine.IEngine engine;

        private void OnEnable()
        {
            EventManager.StartListening(EventManager.CarInitialized, Initialize);
            EventManager.StartListening(EventManager.OnCarMove, UpdateSpeedometer);
        }

        private void OnDisable()
        {
            EventManager.StopListening(EventManager.CarInitialized, Initialize);
            EventManager.StopListening(EventManager.OnCarMove, UpdateSpeedometer);
        }

        private void Initialize()
        {
            maxSpeedKmh = engine.MaxSpeed;
            numberOfMarks = engine.NumberOfGears;

            GenerateNumbers();
            UpdateSpeedometer(0, 0);
        }

        private void UpdateSpeedometer(float currentSpeedMs, float _)
        {
            var currentSpeedKmh = currentSpeedMs * 3.6f;
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
                text.text = (maxSpeedKmh / numberOfMarks * i).ToString();

                numberObject.transform.SetSiblingIndex(0);
            }

            //var maxSpeedMph = Math.Round(maxSpeedKmh * 0.62137, 2);
        }
    }
}
